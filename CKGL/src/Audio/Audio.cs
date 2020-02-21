/*
 * Converts the right-handedness of OpenAL Soft to the left-handedness of CKGL.
 */

using System;
using System.Collections.Generic;
using System.IO;
using OpenAL;
using static SDL2.SDL;

namespace CKGL
{
	public static class Audio
	{
		private static IntPtr Device = IntPtr.Zero;
		private static IntPtr Context = IntPtr.Zero;

		private static List<Buffer> buffers = new List<Buffer>();
		private static List<Source> sources = new List<Source>();

		public static int BufferCount { get { return buffers.Count; } }
		public static int SourceCount { get { return sources.Count; } }

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

			Listener.Reset();

			// Debug
			Output.WriteLine($"OpenAL Initialized");
		}

		public static void Destroy()
		{
			for (int i = 0; i < sources.Count; i++)
				sources[i].Destroy();

			for (int i = 0; i < buffers.Count; i++)
				buffers[i].Destroy();

			ALC10.alcMakeContextCurrent(IntPtr.Zero);

			if (Context != IntPtr.Zero)
				ALC10.alcDestroyContext(Context);

			if (Device != IntPtr.Zero)
				ALC10.alcCloseDevice(Device);

			Context = IntPtr.Zero;
			Device = IntPtr.Zero;
		}

		public static void Update()
		{
			for (int i = 0; i < sources.Count; i++)
			{
				if (sources[i].IsStopped())
					sources[i].Destroy();
			}
		}

		private static void PlayBuffer(Buffer buffer)
		{
			try
			{
				Source source = new Source();
				source.BindBuffer(buffer);
				source.Play();
			}
			catch { }
		}

		private static bool CheckALCError()
		{
			int error = ALC10.alcGetError(Device);
			if (error != ALC10.ALC_NO_ERROR)
			{
				switch (error)
				{
					case ALC10.ALC_INVALID_DEVICE:
						Output.WriteLine("OpenAL ALC error: ALC_INVALID_DEVICE");
						break;
					case ALC10.ALC_INVALID_CONTEXT:
						Output.WriteLine("OpenAL ALC error: ALC_INVALID_CONTEXT");
						break;
					case ALC10.ALC_INVALID_ENUM:
						Output.WriteLine("OpenAL ALC error: ALC_INVALID_ENUM");
						break;
					case ALC10.ALC_INVALID_VALUE:
						Output.WriteLine("OpenAL ALC error: ALC_INVALID_VALUE");
						break;
					case ALC10.ALC_OUT_OF_MEMORY:
						Output.WriteLine("OpenAL ALC error: ALC_OUT_OF_MEMORY");
						break;
				}
				return true;
			}
			return false;
		}

		private static bool CheckALError()
		{
			int error = AL10.alGetError();
			if (error != AL10.AL_NO_ERROR)
			{
				switch (error)
				{
					case AL10.AL_INVALID_NAME:
						Output.WriteLine("OpenAL AL error: AL_INVALID_NAME");
						break;
					case AL10.AL_INVALID_ENUM:
						Output.WriteLine("OpenAL AL error: AL_INVALID_ENUM");
						break;
					case AL10.AL_INVALID_VALUE:
						Output.WriteLine("OpenAL AL error: AL_INVALID_VALUE");
						break;
					case AL10.AL_INVALID_OPERATION:
						Output.WriteLine("OpenAL AL error: AL_INVALID_OPERATION");
						break;
					case AL10.AL_OUT_OF_MEMORY:
						Output.WriteLine("OpenAL AL error: AL_OUT_OF_MEMORY");
						break;
				}
				return true;
			}
			return false;
		}

		public static class Listener
		{
			public static Vector3 Position
			{
				get
				{
					AL10.alGetListener3f(AL10.AL_POSITION, out float x, out float y, out float z);
					if (CheckALError())
						throw new InvalidOperationException("OpenAL Error: Could not read Listener Position");
					return new Vector3(x, y, -z);
				}
				set
				{
					AL10.alListener3f(AL10.AL_POSITION, value.X, value.Y, -value.Z);
					if (CheckALError())
						throw new InvalidOperationException("OpenAL Error: Could not update Listener Position");
				}
			}

			public static Vector3 Velocity
			{
				get
				{
					AL10.alGetListener3f(AL10.AL_VELOCITY, out float x, out float y, out float z);
					if (CheckALError())
						throw new InvalidOperationException("OpenAL Error: Could not read Listener Velocity");
					return new Vector3(x, y, -z);
				}
				set
				{
					AL10.alListener3f(AL10.AL_VELOCITY, value.X, value.Y, -value.Z);
					if (CheckALError())
						throw new InvalidOperationException("OpenAL Error: Could not update Listener Velocity");
				}
			}

			public static (Vector3 Forward, Vector3 Up) Orientation
			{
				get
				{
					float[] orientation = new float[6];
					AL10.alGetListenerfv(AL10.AL_ORIENTATION, orientation);
					if (CheckALError())
						throw new InvalidOperationException("OpenAL Error: Could not read Listener Velocity");
					return (new Vector3(orientation[0], orientation[1], -orientation[2]), new Vector3(orientation[3], orientation[4], -orientation[5]));
				}
				set
				{
					AL10.alListenerfv(AL10.AL_ORIENTATION, new float[] { value.Forward.X, value.Forward.Y, -value.Forward.Z, value.Up.X, value.Up.Y, -value.Up.Z });
					if (CheckALError())
						throw new InvalidOperationException("OpenAL Error: Could not update Listener Orientation");
				}
			}

			public static float Gain
			{
				get
				{
					AL10.alGetListenerf(AL10.AL_POSITION, out float gain);
					if (CheckALError())
						throw new InvalidOperationException("OpenAL Error: Could not read Listener Position");
					return gain;
				}
				set
				{
					AL10.alListenerf(AL10.AL_GAIN, value);
					if (CheckALError())
						throw new InvalidOperationException("OpenAL Error: Could not update Listener Gain");
				}
			}

			public static void Reset()
			{
				Position = Vector3.Zero;
				Velocity = Vector3.Zero;
				Orientation = (Vector3.Forward, Vector3.Up);
				Gain = 1f;
			}
		}

		public class Buffer
		{
			public uint id;

			public Buffer(string file)
			{
				if (!File.Exists(file))
					throw new FileNotFoundException(file);

				AL10.alGenBuffers(1, out id);
				if (CheckALError())
					throw new InvalidOperationException("Could not make OpenAL Buffer");

				SDL_AudioSpec wavspec = new SDL_AudioSpec();
				SDL_LoadWAV(file, ref wavspec, out IntPtr audioBuffer, out uint audioLength);

				// map wav header to openal format
				int format;
				switch (wavspec.format)
				{
					case AUDIO_U8:
					case AUDIO_S8:
						format = wavspec.channels == 2 ? AL10.AL_FORMAT_STEREO8 : AL10.AL_FORMAT_MONO8;
						break;
					case AUDIO_U16:
					case AUDIO_S16:
						format = wavspec.channels == 2 ? AL10.AL_FORMAT_STEREO16 : AL10.AL_FORMAT_MONO16;
						break;
					default:
						SDL_FreeWAV(audioBuffer);
						throw new InvalidOperationException($"SDL failed parsing wav: {file}");
				}

				AL10.alBufferData(id, format, audioBuffer, (int)audioLength, wavspec.freq);

				SDL_FreeWAV(audioBuffer);

				if (CheckALError())
					throw new InvalidOperationException($"OpenAL could not load \"{file}\"");

				buffers.Add(this);
			}

			public void Destroy()
			{
				AL10.alDeleteBuffers(1, ref id);
				buffers.Remove(this);
			}

			public void Play()
			{
				PlayBuffer(this);
			}
		}

		private class Source
		{
			public uint id;

			public Source()
			{
				AL10.alGenSources(1, out id);
				if (CheckALError())
					throw new InvalidOperationException("Could not make OpenAL Source");

				AL10.alSource3f(id, AL10.AL_POSITION, 0, 0, 0);
				AL10.alSource3f(id, AL10.AL_VELOCITY, 0, 0, 0);
				AL10.alSourcef(id, AL10.AL_GAIN, 1f);
				AL10.alSourcef(id, AL10.AL_PITCH, 1f);
				AL10.alSourcei(id, AL10.AL_LOOPING, 0);
				//AL10.alSourcei(id, AL10.AL_SOURCE_RELATIVE, 0);
				if (CheckALError())
					throw new InvalidOperationException("Could set OpenAL Source properties");

				sources.Add(this);
			}

			public void Destroy()
			{
				AL10.alDeleteSources(1, ref id);
				sources.Remove(this);
			}

			public void BindBuffer(Buffer buffer)
			{
				AL10.alSourcei(id, AL10.AL_BUFFER, (int)buffer.id);
			}

			public void Play()
			{
				AL10.alSourcePlay(id);

				if (CheckALError())
					throw new InvalidOperationException("OpenAL Error: Source.Play()");
			}

			public void Pause()
			{
				AL10.alSourcePause(id);
			}

			public void Stop()
			{
				AL10.alSourceStop(id);
			}

			private int GetState()
			{
				AL10.alGetSourcei(id, AL10.AL_SOURCE_STATE, out int state);
				if (CheckALError())
					throw new InvalidOperationException("Could set OpenAL Source properties");
				return state;
			}

			public bool IsPlaying()
			{
				return GetState() == AL10.AL_PLAYING;
			}

			public bool IsPaused()
			{
				return GetState() == AL10.AL_PAUSED;
			}

			public bool IsStopped()
			{
				return GetState() == AL10.AL_STOPPED;
			}
		}

		#region Custom WAV parser - not used, SDL provides this functionality
		//private static float[] readWAV(string filename)
		//{
		//	float[] L = null;

		//	try
		//	{
		//		using (FileStream fs = File.Open(filename, FileMode.Open))
		//		{
		//			BinaryReader reader = new BinaryReader(fs);

		//			// chunk 0
		//			int chunkID = reader.ReadInt32();
		//			int fileSize = reader.ReadInt32();
		//			int riffType = reader.ReadInt32();


		//			// chunk 1
		//			int fmtID = reader.ReadInt32();
		//			int fmtSize = reader.ReadInt32(); // bytes for this chunk
		//			int fmtCode = reader.ReadInt16();
		//			int channels = reader.ReadInt16();
		//			int sampleRate = reader.ReadInt32();
		//			int byteRate = reader.ReadInt32();
		//			int fmtBlockAlign = reader.ReadInt16();
		//			int bitDepth = reader.ReadInt16();

		//			if (fmtSize == 18)
		//			{
		//				// Read any extra values
		//				int fmtExtraSize = reader.ReadInt16();
		//				reader.ReadBytes(fmtExtraSize);
		//			}

		//			// chunk 2
		//			int dataID = reader.ReadInt32();
		//			int bytes = reader.ReadInt32();

		//			// DATA!
		//			byte[] byteArray = reader.ReadBytes(bytes);

		//			int bytesForSamp = bitDepth / 8;
		//			int samps = bytes / bytesForSamp;


		//			float[] asFloat = null;
		//			switch (bitDepth)
		//			{
		//				case 64:
		//					double[]
		//					asDouble = new double[samps];
		//					Buffer.BlockCopy(byteArray, 0, asDouble, 0, bytes);
		//					asFloat = Array.ConvertAll(asDouble, e => (float)e);
		//					break;
		//				case 32:
		//					asFloat = new float[samps];
		//					Buffer.BlockCopy(byteArray, 0, asFloat, 0, bytes);
		//					break;
		//				case 16:
		//					Int16[]
		//					asInt16 = new Int16[samps];
		//					Buffer.BlockCopy(byteArray, 0, asInt16, 0, bytes);
		//					asFloat = Array.ConvertAll(asInt16, e => e / (float)Int16.MaxValue);
		//					break;
		//					//default:
		//					//return false;
		//			}

		//			L = asFloat;

		//			//switch (channels)
		//			//{
		//			//	case 1:
		//			//		L = asFloat;
		//			//		R = null;
		//			//		return true;
		//			//	case 2:
		//			//		L = new float[samps];
		//			//		R = new float[samps];
		//			//		for (int i = 0, s = 0; i < samps; i++)
		//			//		{
		//			//			L[i] = asFloat[s++];
		//			//			R[i] = asFloat[s++];
		//			//		}
		//			//		return true;
		//			//	default:
		//			//		return false;
		//			//}
		//		}
		//	}
		//	catch
		//	{
		//		Output.WriteLine("...Failed to load note: " + filename);
		//		//return false;
		//		//left = new float[ 1 ]{ 0f };
		//	}

		//	//return false;
		//} 
		#endregion
	}
}