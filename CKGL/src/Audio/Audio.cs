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
						alcIsExtensionPresent(device, "ALC_SOFT_pause_device") &&
						alIsExtensionPresent("AL_EXT_float32") &&
						alIsExtensionPresent("AL_EXT_double") &&
						alIsExtensionPresent("AL_EXT_MCFORMATS") &&
						alIsExtensionPresent("AL_SOFTX_effect_chain"))
					{
						alcGetIntegerv(device, ALC_MAX_AUXILIARY_SENDS, out int channelCount);
						ChannelCount = channelCount;

						Active = true;

						// Debug
						Output.WriteLine("OpenAL Initialized");
						Output.WriteLine($"OpenAL Source Channel Count: {ChannelCount}");
						Output.WriteLine("OpenAL Devices:");
						string[] audioDevices = alcGetStringArray(null, alcString.AllDevicesSpecifier);
						foreach (string audioDevice in audioDevices)
							Output.WriteLine($"    - {audioDevice}");
						Output.WriteLine("OpenAL Capture Devices:");
						string[] audioCaptureDevices = alcGetStringArray(null, alcString.CaptureDeviceSpecifier);
						foreach (string audioCaptureDevice in audioCaptureDevices)
							Output.WriteLine($"    - {audioCaptureDevice}");
						//Output.WriteLine($"OpenAL alcExtensions: {alcGetString(null, alcString.Extensions)}");
						//Output.WriteLine($"OpenAL alExtensions: {alGetString(alString.Extensions)}");
					}
					else
					{
						Destroy();
						Output.WriteLine("OpenAL Error: ALC_EXT_EFX, ALC_EXT_disconnect, ALC_SOFT_pause_device, AL_EXT_float32, AL_EXT_double, AL_EXT_MCFORMATS, and AL_SOFTX_effect_chain are required extensions");
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

		public static void Play()
		{
			alcDeviceResumeSOFT(device);
			CheckALCError("Could not play Device");
		}

		public static void Pause()
		{
			alcDevicePauseSOFT(device);
			CheckALCError("Could not pause Device");
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

		internal static void CheckRange(string name, Vector3 value, float min, float max)
		{
			if (value.X < min || value.X > max)
				throw new CKGLException($"OpenAL Error: Illegal Value for \"{name}.X\" = {value} | Range: ({min} - {max})");
			if (value.Y < min || value.Y > max)
				throw new CKGLException($"OpenAL Error: Illegal Value for \"{name}.Y\" = {value} | Range: ({min} - {max})");
			if (value.Z < min || value.Z > max)
				throw new CKGLException($"OpenAL Error: Illegal Value for \"{name}.Z\" = {value} | Range: ({min} - {max})");
		}

		internal static void CheckRange(string name, int value, int min, int max)
		{
			if (value < min || value > max)
				throw new CKGLException($"OpenAL Error: Illegal Value for \"{name}\" = {value} | Range: ({min} - {max})");
		}
	}
}