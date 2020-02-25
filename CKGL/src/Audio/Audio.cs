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

		private static IntPtr device = IntPtr.Zero;
		private static IntPtr context = IntPtr.Zero;

		private static IntPtr currentDevice { get { return alcGetContextsDevice(currentContext); } }
		private static IntPtr currentContext { get { return alcGetCurrentContext(); } }

		internal static List<AudioBuffer> Buffers = new List<AudioBuffer>();
		internal static List<AudioSource> Sources = new List<AudioSource>();

		internal static int BufferCount { get { return Buffers.Count; } }
		internal static int SourceCount { get { return Sources.Count; } }

		public static DistortionEffect distortionEffect; // TEMP

		internal static void Init()
		{
			device = alcOpenDevice(null);
			if (device != IntPtr.Zero)
			{
				context = alcCreateContext(device, new int[] { ALC_MAX_AUXILIARY_SENDS, 4 });
				if (context != IntPtr.Zero && alcMakeContextCurrent(context))
				{
					if (alcIsExtensionPresent(device, "ALC_EXT_EFX") && alIsExtensionPresent("AL_SOFTX_effect_chain")) // TEMP
					{
						Active = true;

						// Debug
						Output.WriteLine($"OpenAL Initialized");

						alcGetIntegerv(device, ALC_MAX_AUXILIARY_SENDS, out int sourceMax);
						Output.WriteLine($"OpenAL ALC_MAX_AUXILIARY_SENDS: {sourceMax}");

						//Output.WriteLine($"OpenAL alcExtensions: {alcGetString(null, alcString.Extensions)}");
						//Output.WriteLine($"OpenAL alExtensions: {alGetString(alString.Extensions)}");

						AudioListener.Reset();

						distortionEffect = new DistortionEffect();
						distortionEffect.Edge = 1f;
						distortionEffect.Gain = 1f;
						distortionEffect.Apply();
					}
					else
					{
						alcDestroyContext(context);
						alcCloseDevice(device);

						Output.WriteLine("OpenAL Error: ALC_EXT_EFX and AL_SOFTX_effect_chain are required extensions");
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

		internal static void Destroy()
		{
			Active = false;

			for (int i = 0; i < Sources.Count; i++)
				Sources[i].Destroy();

			for (int i = 0; i < Buffers.Count; i++)
				Buffers[i].Destroy();

			alcMakeContextCurrent(IntPtr.Zero);

			if (context != IntPtr.Zero)
				alcDestroyContext(context);

			if (device != IntPtr.Zero)
				alcCloseDevice(device);

			context = IntPtr.Zero;
			device = IntPtr.Zero;
		}

		internal static void Update()
		{
			// Handle device disconnect with ALC_EXT_disconnect
			alcGetIntegerv(device, ALC_CONNECTED, out int connected);
			if (Active && connected == 0)
			{
				Destroy();
				Output.WriteLine($"OpenAL audio device has been disconnected. Please restart application to reenable audio.");
			}

			for (int i = 0; i < Sources.Count; i++)
			{
				if (Sources[i].IsStopped())
					Sources[i].Destroy();
			}
		}

		internal static void PlayBuffer(AudioBuffer audioBuffer)
		{
			try
			{
				AudioSource audioSource = new AudioSource();
				audioSource.BindAudioBuffer(audioBuffer);
				audioSource.Play();
			}
			catch { }
		}

		internal static bool CheckALCError(string message = "")
		{
			if (currentDevice != IntPtr.Zero)
			{
				alcErrorCode error = alcGetError(device);
				if (error != alcErrorCode.NoError)
				{
					message = message != "" ? " - " + message : "";
					switch (error)
					{
						case alcErrorCode.InvalidDevice:
							Output.WriteLine($"OpenAL ALC Error: ALC_INVALID_DEVICE{message}");
							break;
						case alcErrorCode.InvalidContext:
							Output.WriteLine($"OpenAL ALC Error: ALC_INVALID_CONTEXT{message}");
							break;
						case alcErrorCode.InvalidEnum:
							Output.WriteLine($"OpenAL ALC Error: ALC_INVALID_ENUM{message}");
							break;
						case alcErrorCode.InvalidValue:
							Output.WriteLine($"OpenAL ALC Error: ALC_INVALID_VALUE{message}");
							break;
						case alcErrorCode.OutOfMemory:
							Output.WriteLine($"OpenAL ALC Error: ALC_OUT_OF_MEMORY{message}");
							break;
					}
					return true;
				}
			}
			return false;
		}

		internal static bool CheckALError(string message = "")
		{
			if (currentContext != IntPtr.Zero)
			{
				alErrorCode error = alGetError();
				if (error != alErrorCode.NoError)
				{
					message = message != "" ? " - " + message : "";
					switch (error)
					{
						case alErrorCode.InvalidName:
							Output.WriteLine($"OpenAL AL Error: AL_INVALID_NAME{message}");
							break;
						case alErrorCode.InvalidEnum:
							Output.WriteLine($"OpenAL AL Error: AL_INVALID_ENUM{message}");
							break;
						case alErrorCode.InvalidValue:
							Output.WriteLine($"OpenAL AL Error: AL_INVALID_VALUE{message}");
							break;
						case alErrorCode.InvalidOperation:
							Output.WriteLine($"OpenAL AL Error: AL_INVALID_OPERATION{message}");
							break;
						case alErrorCode.OutOfMemory:
							Output.WriteLine($"OpenAL AL Error: AL_OUT_OF_MEMORY{message}");
							break;
					}
					return true;
				}
			}
			return false;
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