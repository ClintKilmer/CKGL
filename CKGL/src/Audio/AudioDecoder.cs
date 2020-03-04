using System;
using System.IO;

namespace CKGL
{
	public class AudioDecoder
	{
		public static byte[] Load(string file, out int channels, out int sampleRate, out int bitDepth)
		{
			try
			{
				string extension = Path.GetExtension(file).ToLower();
				if (extension != ".wav")
					throw new CKGLException($"Invalid file extension: {extension} - only \".wav\" accepted");

				using (FileStream fs = File.Open(file, FileMode.Open))
				{
					using (BinaryReader reader = new BinaryReader(fs))
					{
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
						channels = reader.ReadInt16(); // 1 (mono) or 2 (stereo)
						if (channels < 1 || channels > 2)
							throw new CKGLException($"Invalid channel count: {channels} - Only 1 (mono) or 2 (stereo) accepted");
						sampleRate = reader.ReadInt32();
						if (sampleRate != 44100 && sampleRate != 48000)
							throw new CKGLException($"Invalid sampleRate: {sampleRate} - Only 44100 or 48000 accepted");
						int byteRate = reader.ReadInt32();
						int fmtBlockAlign = reader.ReadInt16();
						bitDepth = reader.ReadInt16(); // 8, 16, 24, 32, or 64 bits
						if (bitDepth != 8 && bitDepth != 16 && bitDepth != 24 && bitDepth != 32 && bitDepth != 64)
							throw new CKGLException($"Invalid bitDepth: {bitDepth} - Only 8, 16, 24, 32, or 64 accepted");

						if (fmtSize == 18)
						{
							int fmtExtraSize = reader.ReadInt16(); // size of extra chunk
							reader.ReadBytes(fmtExtraSize); // Skip over "INFO" chunk
						}

						// chunk 2 - 8 bytes
						// Some DAWs will instert extra chunks before the "data" chunk, so skip over them
						//int chunksSkipped = 0;
						while (new string(reader.ReadChars(4)) != "data")
						{
							int chunkSize = reader.ReadInt32();
							reader.ReadBytes(chunkSize);
							//chunksSkipped++;
							//Output.WriteLine($"Skipping chunk {chunksSkipped} (size: {chunkSize})");
						}
						//Output.WriteLine($"Skipping {chunksSkipped} chunks to find \"data\" chunk");
						int dataSize = reader.ReadInt32();
						//Output.WriteLine($"dataSize: {dataSize}");

						// Return PCM data
						return reader.ReadBytes(dataSize);
					}
				}
			}
			catch (Exception e)
			{
				throw new CKGLException($"AudioDecoder Error: {e.Message}");
			}
		}
	}
}