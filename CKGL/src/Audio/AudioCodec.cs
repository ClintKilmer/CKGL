/*
 * WAVE PCM Reference: http://soundfile.sapp.org/doc/WaveFormat/
 */

using System;
using System.IO;

namespace CKGL
{
	internal class AudioCodec
	{
		internal readonly int Channels;
		internal readonly int SampleRate;
		internal readonly int BitDepth;

		private readonly BinaryReader reader;
		private readonly int dataStart;
		private readonly int dataSize;
		private int dataPosition => (int)reader.BaseStream.Position - dataStart;

		internal bool IsFinished => dataPosition >= dataSize;

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

				// chunk 0 - 8 + 4 bytes
				string riffID = new string(reader.ReadChars(4)); // "RIFF"
				if (riffID != "RIFF")
					throw new CKGLException($"Invalid riffID: {riffID} - only \"RIFF\" accepted");
				int fileSize = reader.ReadInt32(); // Size of file less 8 bytes
				string typeID = new string(reader.ReadChars(4)); // "WAVE"
				if (typeID != "WAVE")
					throw new CKGLException($"Invalid RIFF typeID: {typeID} - only \"WAVE\" accepted");

				// chunk 1 - 8 + 16 / 8 + 18 bytes
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
				if (SampleRate != 44100 && SampleRate != 48000)
					throw new CKGLException($"Invalid sampleRate: {SampleRate} - Only 44100 or 48000 accepted");
				int byteRate = reader.ReadInt32();
				int fmtBlockAlign = reader.ReadInt16();
				BitDepth = reader.ReadInt16(); // 8-bit or 16-bit
				if (BitDepth != 8 && BitDepth != 16)
					throw new CKGLException($"Invalid bitDepth: {BitDepth} - Only 8 or 16 accepted");

				if (fmtSize == 18)
				{
					int fmtExtraSize = reader.ReadInt16(); // size of extra chunk
					reader.ReadBytes(fmtExtraSize); // Skip over "INFO" chunk
				}

				// chunk 2 - 8 bytes
				// Some DAWs will instert extra chunks before the "data" chunk, so skip over them
				//int chunksSkipped = 0; // Debug
				while (new string(reader.ReadChars(4)) != "data")
				{
					int chunkSize = reader.ReadInt32();
					reader.ReadBytes(chunkSize);
					//chunksSkipped++; // Debug
					//Output.WriteLine($"Skipping chunk {chunksSkipped} (size: {chunkSize})"); // Debug
				}
				//Output.WriteLine($"Skipping {chunksSkipped} chunks to find \"data\" chunk"); // Debug
				dataSize = reader.ReadInt32();
				//Output.WriteLine($"dataSize: {dataSize}"); // Debug

				dataStart = dataPosition;
			}
			catch (Exception e)
			{
				throw new CKGLException($"AudioCodec Error: {e.Message}");
			}
		}

		internal byte[] GetData(int size)
		{
			if (IsFinished)
				return new byte[0];

			// Trim size to match buffer length
			if (size > dataSize - dataPosition)
				size = dataSize - dataPosition;

			//Output.WriteLine($"dataSize: {dataSize}, dataPosition: {dataPosition}, playback%: {(float)dataPosition / (float)dataSize:P2}"); // Debug

			// Return PCM data
			byte[] bytes = reader.ReadBytes(size);
			//Output.WriteLine($"After Read - dataSize: {dataSize}, dataPosition: {dataPosition}, playback%: {(float)dataPosition / (float)dataSize:P2}"); // Debug
			return bytes;
		}

		internal byte[] GetData(int offset, int size)
		{
			Seek(offset);
			return GetData(size);
		}

		internal void Seek(int offset)
		{
			if (offset % BitDepth > 0)
				offset = offset / BitDepth * BitDepth;
			reader.BaseStream.Seek(dataStart + offset, SeekOrigin.Begin);
		}

		internal void Seek(float percent)
		{
			Seek((int)(dataSize * percent.Clamp(0f, 1f)));
		}

		internal void Destroy() => reader.Dispose();

		internal static byte[] Decode(string file, out int channels, out int sampleRate, out int bitDepth)
		{
			AudioCodec codec = new AudioCodec(file);
			channels = codec.Channels;
			sampleRate = codec.SampleRate;
			bitDepth = codec.BitDepth;
			byte[] bytes = codec.GetData(codec.dataSize);
			codec.Destroy();
			return bytes;
		}
	}
}