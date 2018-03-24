using System;
using System.Collections.Generic;
using System.IO;
using OpenAL;

namespace CKGL
{
	public static class Audio
	{
		public static IntPtr Device = IntPtr.Zero;
		public static IntPtr Context = IntPtr.Zero;

		private static List<uint> buffers = new List<uint>();

		public static void Init()
		{
			// Create Device
			Device = ALC10.alcOpenDevice(string.Empty);
			if (CheckALCError() || Device == IntPtr.Zero)
			{
				throw new InvalidOperationException("Could not open OpenAL audio device");
			}


			int[] contextAttributes = new int[0];
			Context = ALC10.alcCreateContext(Device, contextAttributes);
			if (CheckALCError() || Context == IntPtr.Zero)
			{
				Destroy();
				throw new InvalidOperationException("Could not create OpenAL context");
			}

			ALC10.alcMakeContextCurrent(Context);
			if (CheckALCError())
			{
				Destroy();
				throw new InvalidOperationException("Could not make OpenAL context current");
			}

			AL10.alListenerfv(AL10.AL_ORIENTATION, new float[] { 0.0f, 0.0f, -1.0f, 0.0f, 1.0f, 0.0f });
			AL10.alListener3f(AL10.AL_POSITION, 0.0f, 0.0f, 0.0f);
			AL10.alListener3f(AL10.AL_VELOCITY, 0.0f, 0.0f, 0.0f);
			AL10.alListenerf(AL10.AL_GAIN, 1.0f);
			if (CheckALError())
			{
				Destroy();
				throw new InvalidOperationException("Could not make OpenAL Listener");
			}

			//uint[] source = new uint[1];
			//AL10.alGenSources(1, source);
			//AL10.alSource3f(source[0], AL10.AL_POSITION, 0, 0, 0);
			//AL10.alSource3f(source[0], AL10.AL_VELOCITY, 0, 0, 0);
			//AL10.alSourcef(source[0], AL10.AL_GAIN, 1f);
			//AL10.alSourcef(source[0], AL10.AL_PITCH, 1f);
			//AL10.alSourcei(source[0], AL10.AL_LOOPING, 0);
			//if (CheckALError())
			//{
			//	Close();
			//	throw new InvalidOperationException("Could not make OpenAL Listener");
			//}
		}

		public static void Destroy()
		{
			ALC10.alcMakeContextCurrent(IntPtr.Zero);

			if (Context != IntPtr.Zero)
				ALC10.alcDestroyContext(Context);

			if (Device != IntPtr.Zero)
				ALC10.alcCloseDevice(Device);

			Context = IntPtr.Zero;
			Device = IntPtr.Zero;
		}

		public static void LoadAudio(string filename)
		{
			uint[] buffer = new uint[1];
			AL10.alGenBuffers(1, buffer);
			buffers.Add(buffer[0]);

			AL10.alBufferData(buffer, AL10.AL_FORMAT_STEREO16, readWav(filename, ), 0, 0);

			if (CheckALError())
			{
				Destroy();
				throw new InvalidOperationException($"OpenAL could not load \"{filename}\"");
			}
		}

		public static bool CheckALCError()
		{
			int error = ALC10.alcGetError(Device);
			if (error != ALC10.ALC_NO_ERROR)
			{
				switch (error)
				{
					case ALC10.ALC_INVALID_DEVICE:
						Console.WriteLine("OpenAL ALC error: ALC_INVALID_DEVICE");
						break;
					case ALC10.ALC_INVALID_CONTEXT:
						Console.WriteLine("OpenAL ALC error: ALC_INVALID_CONTEXT");
						break;
					case ALC10.ALC_INVALID_ENUM:
						Console.WriteLine("OpenAL ALC error: ALC_INVALID_ENUM");
						break;
					case ALC10.ALC_INVALID_VALUE:
						Console.WriteLine("OpenAL ALC error: ALC_INVALID_VALUE");
						break;
					case ALC10.ALC_OUT_OF_MEMORY:
						Console.WriteLine("OpenAL ALC error: ALC_OUT_OF_MEMORY");
						break;
				}
				return true;
			}
			return false;
		}

		public static bool CheckALError()
		{
			int error = AL10.alGetError();
			if (error != AL10.AL_NO_ERROR)
			{
				switch (error)
				{
					case AL10.AL_INVALID_NAME:
						Console.WriteLine("OpenAL AL error: AL_INVALID_NAME");
						break;
					case AL10.AL_INVALID_ENUM:
						Console.WriteLine("OpenAL AL error: AL_INVALID_ENUM");
						break;
					case AL10.AL_INVALID_VALUE:
						Console.WriteLine("OpenAL AL error: AL_INVALID_VALUE");
						break;
					case AL10.AL_INVALID_OPERATION:
						Console.WriteLine("OpenAL AL error: AL_INVALID_OPERATION");
						break;
					case AL10.AL_OUT_OF_MEMORY:
						Console.WriteLine("OpenAL AL error: AL_OUT_OF_MEMORY");
						break;
				}
				return true;
			}
			return false;
		}

		private static float[] readWav(string filename)
		{
			float[] L = null;

			try
			{
				using (FileStream fs = File.Open(filename, FileMode.Open))
				{
					BinaryReader reader = new BinaryReader(fs);

					// chunk 0
					int chunkID = reader.ReadInt32();
					int fileSize = reader.ReadInt32();
					int riffType = reader.ReadInt32();


					// chunk 1
					int fmtID = reader.ReadInt32();
					int fmtSize = reader.ReadInt32(); // bytes for this chunk
					int fmtCode = reader.ReadInt16();
					int channels = reader.ReadInt16();
					int sampleRate = reader.ReadInt32();
					int byteRate = reader.ReadInt32();
					int fmtBlockAlign = reader.ReadInt16();
					int bitDepth = reader.ReadInt16();

					if (fmtSize == 18)
					{
						// Read any extra values
						int fmtExtraSize = reader.ReadInt16();
						reader.ReadBytes(fmtExtraSize);
					}

					// chunk 2
					int dataID = reader.ReadInt32();
					int bytes = reader.ReadInt32();

					// DATA!
					byte[] byteArray = reader.ReadBytes(bytes);

					int bytesForSamp = bitDepth / 8;
					int samps = bytes / bytesForSamp;


					float[] asFloat = null;
					switch (bitDepth)
					{
						case 64:
							double[]
							asDouble = new double[samps];
							Buffer.BlockCopy(byteArray, 0, asDouble, 0, bytes);
							asFloat = Array.ConvertAll(asDouble, e => (float)e);
							break;
						case 32:
							asFloat = new float[samps];
							Buffer.BlockCopy(byteArray, 0, asFloat, 0, bytes);
							break;
						case 16:
							Int16[]
							asInt16 = new Int16[samps];
							Buffer.BlockCopy(byteArray, 0, asInt16, 0, bytes);
							asFloat = Array.ConvertAll(asInt16, e => e / (float)Int16.MaxValue);
							break;
							//default:
							//return false;
					}

					L = asFloat;

					//switch (channels)
					//{
					//	case 1:
					//		L = asFloat;
					//		R = null;
					//		return true;
					//	case 2:
					//		L = new float[samps];
					//		R = new float[samps];
					//		for (int i = 0, s = 0; i < samps; i++)
					//		{
					//			L[i] = asFloat[s++];
					//			R[i] = asFloat[s++];
					//		}
					//		return true;
					//	default:
					//		return false;
					//}
				}
			}
			catch
			{
				Console.WriteLine("...Failed to load note: " + filename);
				//return false;
				//left = new float[ 1 ]{ 0f };
			}

			//return false;
		}
	}
}