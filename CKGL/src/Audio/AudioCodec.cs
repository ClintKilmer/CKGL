/*
 * WAVE PCM Reference: http://soundfile.sapp.org/doc/WaveFormat/
 */

using System;
using System.IO;

namespace CKGL
{
	internal class AudioCodec
	{
		/// <summary>Length in seconds</summary>
		internal readonly float Length;
		/// <summary>Size in bytes</summary>
		internal readonly int Size;
		internal readonly int Channels;
		internal readonly int Samples;
		/// <summary>Sample frequency in Hz</summary>
		internal readonly int SampleRate;
		/// <summary>Size of a sample in bytes</summary>
		internal readonly int SampleSize;
		internal readonly int BitDepth;

		internal int Position => dataPosition;
		internal bool IsFinished => dataPosition >= Size;

		private readonly BinaryReader reader;
		private readonly int dataStart;
		private int dataPosition => (int)reader.BaseStream.Position - dataStart;
		private int dataLeft => Size - dataPosition;

		internal AudioCodec(string file)
		{
			try
			{
				if (!File.Exists(file))
					throw new FileNotFoundException("Audio file not found.", file);

				string extension = Path.GetExtension(file).ToLower();
				if (extension != ".wav")
					throw new CKGLException($"Invalid file extension: {extension} - only \".wav\" accepted");

				reader = new BinaryReader(File.Open(file, FileMode.Open, FileAccess.Read, FileShare.Read));

				// "RIFF" chunk
				string riffID = new string(reader.ReadChars(4)); // "RIFF"
				if (riffID != "RIFF")
					throw new CKGLException($"Invalid riffID: {riffID} - only \"RIFF\" accepted");
				int fileSize = reader.ReadInt32(); // Size of file less 8 bytes
				string typeID = new string(reader.ReadChars(4)); // "WAVE"
				if (typeID != "WAVE")
					throw new CKGLException($"Invalid RIFF typeID: {typeID} - only \"WAVE\" accepted");

				// "fmt " chunk
				string fmtID = new string(reader.ReadChars(4)); // "fmt "
				if (fmtID != "fmt ")
					throw new CKGLException($"Invalid fmtID: {fmtID} - only \"fmt \" accepted");
				int fmtSize = reader.ReadInt32(); // bytes for this chunk
				int fmtCode = reader.ReadInt16(); // 1 (PCM)
				if (fmtCode != 1)
					throw new CKGLException($"Invalid fmtCode: {fmtCode} - Only 1 (PCM) accepted");
				Channels = reader.ReadInt16(); // 1 (mono) or 2 (stereo)
				if (Channels < 1 || Channels > 2)
					throw new CKGLException($"Invalid channel count: {Channels} - Only 1 (mono) or 2 (stereo) accepted");
				SampleRate = reader.ReadInt32();
				if (SampleRate != 8000 && SampleRate != 11025 && SampleRate != 22050 && SampleRate != 44100 && SampleRate != 48000 && SampleRate != 88200 && SampleRate != 96000 && SampleRate != 192000)
					throw new CKGLException($"Invalid sampleRate: {SampleRate} - Only 8000, 11025, 22050, 44100, 48000, 88200, 96000, or 192000 accepted");
				int byteRate = reader.ReadInt32();
				SampleSize = reader.ReadInt16(); // Sample size
				BitDepth = reader.ReadInt16(); // 8-bit or 16-bit
				if (BitDepth != 8 && BitDepth != 16)
					throw new CKGLException($"Invalid bitDepth: {BitDepth} - Only 8 or 16 accepted");

				if (fmtSize == 18)
				{
					int fmtExtraSize = reader.ReadInt16(); // size of extra chunk
					reader.ReadBytes(fmtExtraSize); // Skip over "INFO" chunk
				}

				// "data" chunk - Some DAWs will insert extra chunks before the "data" chunk, so skip over them
				while (new string(reader.ReadChars(4)) != "data")
				{
					int chunkSize = reader.ReadInt32();
					reader.ReadBytes(chunkSize);
				}
				Size = reader.ReadInt32(); // Size of "data" chunk

				dataStart = dataPosition;
				Samples = Size / SampleSize;
				Length = (float)Samples / SampleRate;

				//Output.WriteLine($"AudioCodec: File: {file} Size: {Size} Length: {Length}s Channels: {Channels} Samples: {Samples} SampleRate: {SampleRate} SampleSize: {SampleSize} BitDepth: {BitDepth}"); // Debug
			}
			catch (Exception e)
			{
				throw new CKGLException($"AudioCodec Error: {e.Message}");
			}
		}

		internal byte[] GetData(int size, bool looping = false)
		{
			if (looping && IsFinished)
				Rewind();

			// Most likely early out where the data buffer contains at least the requested size
			if (size <= dataLeft)
				return reader.ReadBytes(size);

			// Data buffer holds less data than requested size
			// If we're not looping (likely), then just return the remainder
			if (!looping)
			{
				if (IsFinished)
					return new byte[0];
				return reader.ReadBytes(dataLeft);
			}
			// If we're looping, fill the buffer as many times as required to fill the requested size
			else
			{
				byte[] bytes = new byte[size];
				int bytesConsumed = 0;

				while (bytesConsumed < size)
				{
					if (IsFinished)
						Rewind();
					byte[] newBytes = reader.ReadBytes(Math.Min(size - bytesConsumed, dataLeft));
					Array.Copy(newBytes, 0, bytes, bytesConsumed, newBytes.Length);
					bytesConsumed += newBytes.Length;
				}

				return bytes;
			}
		}

		internal void Rewind()
		{
			reader.BaseStream.Seek(dataStart, SeekOrigin.Begin);
		}

		internal void SeekBytes(int offsetBytes)
		{
			if (offsetBytes < 0 || offsetBytes > Size)
				throw new CKGLException($"AudioCodec Error: Illegal Value for \"SeekBytes(int offsetBytes)\" = {offsetBytes} | Range: ({0} - {Size})");
			offsetBytes -= offsetBytes % SampleSize; // Align to start of block
			reader.BaseStream.Seek(dataStart + offsetBytes, SeekOrigin.Begin);
		}

		internal void SeekSamples(int offsetSamples)
		{
			if (offsetSamples < 0f || offsetSamples > Samples)
				throw new CKGLException($"AudioCodec Error: Illegal Value for \"SeekSamples(float offsetSamples)\" = {offsetSamples} | Range: ({0f} - {Samples})");
			SeekBytes(SampleSize * offsetSamples);
		}

		internal void SeekSeconds(float offsetSeconds)
		{
			if (offsetSeconds < 0f || offsetSeconds > Length)
				throw new CKGLException($"AudioCodec Error: Illegal Value for \"SeekSeconds(float offsetSeconds)\" = {offsetSeconds} | Range: ({0f} - {Length})");
			SeekBytes((int)((float)SampleSize * SampleRate * offsetSeconds));
		}

		internal void SeekPercent(float offsetPercent)
		{
			if (offsetPercent < 0f || offsetPercent > 1f)
				throw new CKGLException($"AudioCodec Error: Illegal Value for \"SeekPercent(float offsetPercent)\" = {offsetPercent} | Range: ({0f} - {1f})");
			SeekBytes((int)(Size * offsetPercent));
		}

		internal void Destroy() => reader.Dispose();

		internal static byte[] Decode(string file, out float length, out int channels, out int samples, out int sampleRate, out int bitDepth)
		{
			AudioCodec codec = new AudioCodec(file);
			length = codec.Length;
			channels = codec.Channels;
			samples = codec.Samples;
			sampleRate = codec.SampleRate;
			bitDepth = codec.BitDepth;
			byte[] bytes = codec.GetData(codec.Size);
			codec.Destroy();
			return bytes;
		}
	}
}