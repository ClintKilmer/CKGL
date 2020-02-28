/*
 * Converts the right-handedness of OpenAL Soft to the left-handedness of CKGL.
 */

using System;
using System.Collections.Generic;
using static OpenAL.Bindings;

namespace CKGL
{
	public static class Audio
	{
		public static bool Active { get; private set; } = false;
		public static int ChannelCount { get; private set; } = 4;

		private static IntPtr device = IntPtr.Zero;
		private static IntPtr context = IntPtr.Zero;

		internal static List<AudioBuffer> Buffers = new List<AudioBuffer>();
		internal static List<AudioSource> Sources = new List<AudioSource>();
		internal static List<AudioEffect> Effects = new List<AudioEffect>();
		internal static List<AudioFilter> Filters = new List<AudioFilter>();

		internal static int BufferCount { get { return Buffers.Count; } }
		internal static int SourceCount { get { return Sources.Count; } }

		public static AudioDistanceModel DistanceModel
		{
			get
			{
				float distanceModel = alGetInteger(AL_DISTANCE_MODEL);
				CheckALError("Could not read Audio.DistanceModel");
				return (AudioDistanceModel)distanceModel;
			}
			set
			{
				alDistanceModel((int)value);
				CheckALError("Could not update Audio.DistanceModel");
			}
		}
		public static readonly AudioDistanceModel DistanceModelDefault = AudioDistanceModel.InverseDistanceClamped;

		/// <summary>
		/// 1f (0f - )
		/// </summary>
		public static float DopplerFactor
		{
			get
			{
				float dopplerFactor = alGetFloat(alStatefParameter.DopplerFactor);
				CheckALError("Could not read Audio.DopplerFactor");
				return dopplerFactor;
			}
			set
			{
				CheckRange("Audio.DopplerFactor", value, 0f, float.MaxValue);
				alDopplerFactor(value);
				CheckALError("Could not update Audio.DopplerFactor");
			}
		}
		public static readonly float DopplerFactorDefault = 1f;

		/// <summary>
		/// 343.3f (0.0001f - )
		/// </summary>
		public static float SpeedOfSound
		{
			get
			{
				float speedOfSound = alGetFloat(alStatefParameter.SpeedOfSound);
				CheckALError("Could not read Audio.SpeedOfSound");
				return speedOfSound;
			}
			set
			{
				CheckRange("Audio.SpeedOfSound", value, 0.0001f, float.MaxValue);
				alSpeedOfSound(value);
				CheckALError("Could not update Audio.SpeedOfSound");
			}
		}
		public static readonly float SpeedOfSoundDefault = 343.3f;

		internal static void Init()
		{
			device = alcOpenDevice(null);
			if (device != IntPtr.Zero)
			{
				context = alcCreateContext(device, new int[] { ALC_MAX_AUXILIARY_SENDS, ChannelCount });
				if (context != IntPtr.Zero && alcMakeContextCurrent(context))
				{
					if (alcIsExtensionPresent(device, "ALC_EXT_EFX") &&
						alcIsExtensionPresent(device, "ALC_EXT_disconnect") &&
						alIsExtensionPresent("AL_SOFTX_effect_chain"))
					{
						alcGetIntegerv(device, ALC_MAX_AUXILIARY_SENDS, out int channelCount);
						ChannelCount = channelCount;

						Active = true;

						// Debug
						Output.WriteLine($"OpenAL Initialized - Channel Count: {ChannelCount}");

						//Output.WriteLine($"OpenAL alcExtensions: {alcGetString(null, alcString.Extensions)}");
						//Output.WriteLine($"OpenAL alExtensions: {alGetString(alString.Extensions)}");
					}
					else
					{
						Destroy();
						Output.WriteLine("OpenAL Error: ALC_EXT_EFX, ALC_EXT_disconnect, and AL_SOFTX_effect_chain are required extensions");
						return;
					}
				}
				else
				{
					Destroy();
					Output.WriteLine("OpenAL Error: Could not create an audio context");
					return;
				}
			}
			else
			{
				Destroy();
				Output.WriteLine("OpenAL Error: Could not open an audio device");
				return;
			}
		}

		internal static void Update()
		{
			if (Active)
			{
				// Handle device disconnect with ALC_EXT_disconnect
				alcGetIntegerv(device, alcInteger.Connected, out int connected);
				if (connected == 0)
				{
					Destroy();
					Output.WriteLine($"OpenAL Error: Audio device has been disconnected, restart the application to enable audio");
				}

				for (int i = Sources.Count - 1; i >= 0; i--)
					Sources[i].Update();
			}
		}

		internal static void Destroy()
		{
			Active = false;

			for (int i = Effects.Count - 1; i >= 0; i--)
				Effects[i].Destroy();

			for (int i = Filters.Count - 1; i >= 0; i--)
				Filters[i].Destroy();

			for (int i = Sources.Count - 1; i >= 0; i--)
				Sources[i].Destroy();

			for (int i = Buffers.Count - 1; i >= 0; i--)
				Buffers[i].Destroy();

			alcMakeContextCurrent(null);

			if (context != IntPtr.Zero)
				alcDestroyContext(context);

			if (device != IntPtr.Zero)
				alcCloseDevice(device);

			context = IntPtr.Zero;
			device = IntPtr.Zero;
		}

		internal static bool CheckALCError(string message = "")
		{
			if (device != IntPtr.Zero)
			{
				alcErrorCode error = (alcErrorCode)alcGetError(device);
				if (error != alcErrorCode.NoError)
				{
					message = message != "" ? " - " + message : "";
					switch (error)
					{
						case alcErrorCode.InvalidDevice:
							throw new CKGLException($"OpenAL ALC Error: ALC_INVALID_DEVICE{message}");
						case alcErrorCode.InvalidContext:
							throw new CKGLException($"OpenAL ALC Error: ALC_INVALID_CONTEXT{message}");
						case alcErrorCode.InvalidEnum:
							throw new CKGLException($"OpenAL ALC Error: ALC_INVALID_ENUM{message}");
						case alcErrorCode.InvalidValue:
							throw new CKGLException($"OpenAL ALC Error: ALC_INVALID_VALUE{message}");
						case alcErrorCode.OutOfMemory:
							throw new CKGLException($"OpenAL ALC Error: ALC_OUT_OF_MEMORY{message}");
					}
					return true;
				}
			}
			return false;
		}

		internal static bool CheckALError(string message = "")
		{
			if (context != IntPtr.Zero)
			{
				alErrorCode error = (alErrorCode)alGetError();
				if (error != alErrorCode.NoError)
				{
					message = message != "" ? " - " + message : "";
					switch (error)
					{
						case alErrorCode.InvalidName:
							throw new CKGLException($"OpenAL AL Error: AL_INVALID_NAME{message}");
						case alErrorCode.InvalidEnum:
							throw new CKGLException($"OpenAL AL Error: AL_INVALID_ENUM{message}");
						case alErrorCode.InvalidValue:
							throw new CKGLException($"OpenAL AL Error: AL_INVALID_VALUE{message}");
						case alErrorCode.InvalidOperation:
							throw new CKGLException($"OpenAL AL Error: AL_INVALID_OPERATION{message}");
						case alErrorCode.OutOfMemory:
							throw new CKGLException($"OpenAL AL Error: AL_OUT_OF_MEMORY{message}");
					}
					return true;
				}
			}
			return false;
		}

		internal static void CheckRange(string name, float value, float min, float max)
		{
			if (value < min || value > max)
				throw new CKGLException($"OpenAL Error: Illegal Value for \"{name}\" = {value} | Range: ({min} - {max})");
		}

		internal static void CheckRange(string name, int value, int min, int max)
		{
			if (value < min || value > max)
				throw new CKGLException($"OpenAL Error: Illegal Value for \"{name}\" = {value} | Range: ({min} - {max})");
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