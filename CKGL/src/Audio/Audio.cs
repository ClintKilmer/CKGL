/*
 * Converts the right-handedness of OpenAL Soft to the left-handedness of CKGL.
 */

using System;
using System.Collections.Generic;
using System.IO;
using static OpenAL.Bindings;
using static SDL2.SDL;

namespace CKGL
{
	public static class Audio
	{
		public static bool Active { get; private set; } = false;

		private static IntPtr device = IntPtr.Zero;
		private static IntPtr context = IntPtr.Zero;

		private static IntPtr currentDevice { get { return alcGetContextsDevice(currentContext); } }
		private static IntPtr currentContext { get { return alcGetCurrentContext(); } }

		private static List<Buffer> buffers = new List<Buffer>();
		private static List<Source> sources = new List<Source>();

		public static int BufferCount { get { return buffers.Count; } }
		public static int SourceCount { get { return sources.Count; } }

		public static DistortionEffect distortionEffect;
		public static uint slot;

		public static void Init()
		{
			device = alcOpenDevice(null);
			if (device != IntPtr.Zero)
			{
				context = alcCreateContext(device, null);
				if (context != IntPtr.Zero && alcMakeContextCurrent(context))
				{
					if (alcIsExtensionPresent(device, "ALC_EXT_EFX")) // TEMP
					{
						Active = true;

						// Debug
						Output.WriteLine($"OpenAL Initialized");

						Listener.Reset();

						distortionEffect = new DistortionEffect();
						distortionEffect.Edge = 1f;
						distortionEffect.Gain = 1f;

						slot = alGenAuxiliaryEffectSlot();
						if (CheckALError())
							Output.WriteLine("3");

						alAuxiliaryEffectSloti(slot, AL_EFFECTSLOT_EFFECT, (int)distortionEffect.ID);
						if (CheckALError())
							Output.WriteLine("4");

						var distEffect = new DistortionEffect();
					}
					else
					{
						alcDestroyContext(context);
						alcCloseDevice(device);

						Output.WriteLine("OpenAL Error: ALC_EXT_EFX Extension not found");
						return;
					}
				}
				else
				{
					alcDestroyContext(context);
					alcCloseDevice(device);

					Output.WriteLine("OpenAL Error: Could not create an audio context");
					return;
				}
			}
			else
			{
				Output.WriteLine("OpenAL Error: Could not open an audio device");
				return;
			}
		}

		public static void Destroy()
		{
			Active = false;

			for (int i = 0; i < sources.Count; i++)
				sources[i].Destroy();

			for (int i = 0; i < buffers.Count; i++)
				buffers[i].Destroy();

			alcMakeContextCurrent(IntPtr.Zero);

			if (context != IntPtr.Zero)
				alcDestroyContext(context);

			if (device != IntPtr.Zero)
				alcCloseDevice(device);

			context = IntPtr.Zero;
			device = IntPtr.Zero;
		}

		public static void Update()
		{
			// Handle device disconnect with ALC_EXT_disconnect
			alcGetIntegerv(device, ALC_CONNECTED, out int connected);
			if (Active && connected == 0)
			{
				Destroy();
				Output.WriteLine($"OpenAL audio device has been disconnected. Please restart application to reenable audio.");
			}

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

		public static bool CheckALCError()
		{
			if (currentDevice != IntPtr.Zero)
			{
				alcErrorCode error = alcGetError(device);
				if (error != alcErrorCode.NoError)
				{
					switch (error)
					{
						case alcErrorCode.InvalidDevice:
							Output.WriteLine("OpenAL alcError: ALC_INVALID_DEVICE");
							break;
						case alcErrorCode.InvalidContext:
							Output.WriteLine("OpenAL alcError: ALC_INVALID_CONTEXT");
							break;
						case alcErrorCode.InvalidEnum:
							Output.WriteLine("OpenAL alcError: ALC_INVALID_ENUM");
							break;
						case alcErrorCode.InvalidValue:
							Output.WriteLine("OpenAL alcError: ALC_INVALID_VALUE");
							break;
						case alcErrorCode.OutOfMemory:
							Output.WriteLine("OpenAL alcError: ALC_OUT_OF_MEMORY");
							break;
					}
					return true;
				}
			}
			return false;
		}

		public static bool CheckALError()
		{
			if (currentContext != IntPtr.Zero)
			{
				alErrorCode error = alGetError();
				if (error != alErrorCode.NoError)
				{
					switch (error)
					{
						case alErrorCode.InvalidName:
							Output.WriteLine("OpenAL alError: AL_INVALID_NAME");
							break;
						case alErrorCode.InvalidEnum:
							Output.WriteLine("OpenAL alError: AL_INVALID_ENUM");
							break;
						case alErrorCode.InvalidValue:
							Output.WriteLine("OpenAL alError: AL_INVALID_VALUE");
							break;
						case alErrorCode.InvalidOperation:
							Output.WriteLine("OpenAL alError: AL_INVALID_OPERATION");
							break;
						case alErrorCode.OutOfMemory:
							Output.WriteLine("OpenAL alError: AL_OUT_OF_MEMORY");
							break;
					}
					return true;
				}
			}
			return false;
		}

		public static class Listener
		{
			public static Vector3 Position
			{
				get
				{
					alGetListener3f(alListener3fParameter.Position, out float x, out float y, out float z);
					if (CheckALError())
						Output.WriteLine("OpenAL Error: Could not read Listener Position");
					return new Vector3(x, y, -z);
				}
				set
				{
					alListener3f(alListener3fParameter.Position, value.X, value.Y, -value.Z);
					if (CheckALError())
						Output.WriteLine("OpenAL Error: Could not update Listener Position");
				}
			}

			public static Vector3 Velocity
			{
				get
				{
					alGetListener3f(alListener3fParameter.Velocity, out float x, out float y, out float z);
					if (CheckALError())
						Output.WriteLine("OpenAL Error: Could not read Listener Velocity");
					return new Vector3(x, y, -z);
				}
				set
				{
					alListener3f(alListener3fParameter.Velocity, value.X, value.Y, -value.Z);
					if (CheckALError())
						Output.WriteLine("OpenAL Error: Could not update Listener Velocity");
				}
			}

			public static (Vector3 Forward, Vector3 Up) Orientation
			{
				get
				{
					float[] orientation = new float[6];
					alGetListenerfv(alListenerfvParameter.Orientation, orientation);
					if (CheckALError())
						Output.WriteLine("OpenAL Error: Could not read Listener Orientation");
					return (new Vector3(orientation[0], orientation[1], -orientation[2]), new Vector3(orientation[3], orientation[4], -orientation[5]));
				}
				set
				{
					alListenerfv(alListenerfvParameter.Orientation, new float[] { value.Forward.X, value.Forward.Y, -value.Forward.Z, value.Up.X, value.Up.Y, -value.Up.Z });
					if (CheckALError())
						Output.WriteLine("OpenAL Error: Could not update Listener Orientation");
				}
			}

			public static float Gain
			{
				get
				{
					alGetListenerf(alListenerfParameter.Gain, out float gain);
					if (CheckALError())
						Output.WriteLine("OpenAL Error: Could not read Listener Gain");
					return gain;
				}
				set
				{
					alListenerf(alListenerfParameter.Gain, value);
					if (CheckALError())
						Output.WriteLine("OpenAL Error: Could not update Listener Gain");
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

				id = alGenBuffer();
				if (CheckALError())
					throw new CKGLException("OpenAL Error: Could not create Buffer");

				SDL_AudioSpec wavspec = new SDL_AudioSpec();
				SDL_LoadWAV(file, ref wavspec, out IntPtr audioBuffer, out uint audioLength);

				// map wav header to openal format
				alBufferFormat format;
				switch (wavspec.format)
				{
					case AUDIO_U8:
					case AUDIO_S8:
						format = wavspec.channels == 2 ? alBufferFormat.Stereo8 : alBufferFormat.Mono8;
						break;
					case AUDIO_U16:
					case AUDIO_S16:
						format = wavspec.channels == 2 ? alBufferFormat.Stereo16 : alBufferFormat.Mono16;
						break;
					default:
						SDL_FreeWAV(audioBuffer);
						throw new CKGLException($"SDL failed parsing wav: {file}");
				}

				alBufferData(id, format, audioBuffer, (int)audioLength, wavspec.freq);

				SDL_FreeWAV(audioBuffer);

				if (CheckALError())
					throw new CKGLException($"OpenAL could not load \"{file}\"");

				buffers.Add(this);
			}

			public void Destroy()
			{
				alDeleteBuffer(id);
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
				id = alGenSource();
				if (CheckALError())
					throw new CKGLException("Could not make OpenAL Source");

				alSource3f(id, alSource3fParameter.Position, 0, 0, 0);
				alSource3f(id, alSource3fParameter.Velocity, 0, 0, 0);
				alSourcef(id, alSourcefParameter.Gain, 1f);
				alSourcef(id, alSourcefParameter.Pitch, 1f);
				alSourcei(id, alSourceiParameter.Looping, 0);
				//alSourcei(id, alSourceiParameter.SourceRelative, 0);
				alSource3i(id, AL_AUXILIARY_SEND_FILTER, (int)slot, 0, AL_FILTER_NULL);
				if (CheckALError())
					throw new CKGLException("Could set OpenAL Source properties");

				sources.Add(this);
			}

			public void Destroy()
			{
				alDeleteSource(id);
				sources.Remove(this);
			}

			public void BindBuffer(Buffer buffer)
			{
				alSourcei(id, alSourceiParameter.Buffer, (int)buffer.id);
			}

			public void Play()
			{
				alSourcePlay(id);

				if (CheckALError())
					throw new CKGLException("OpenAL Error: Source.Play()");
			}

			public void Pause()
			{
				alSourcePause(id);
			}

			public void Stop()
			{
				alSourceStop(id);
			}

			private alSourceState GetState()
			{
				alGetSourcei(id, alGetSourceiParameter.SourceState, out int state);
				if (CheckALError())
					throw new CKGLException("Could set OpenAL Source properties");
				return (alSourceState)state;
			}

			public bool IsPlaying()
			{
				return GetState() == alSourceState.Playing;
			}

			public bool IsPaused()
			{
				return GetState() == alSourceState.Paused;
			}

			public bool IsStopped()
			{
				return GetState() == alSourceState.Stopped;
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