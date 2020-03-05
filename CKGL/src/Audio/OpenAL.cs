/*
 * C# bindings of OpenAL Soft: https://github.com/kcat/openal-soft
 * Specifically supplies bindings that will only work with v1.20.1 OpenAL Soft implementation of OpenAL.
 * 
 * Headers: https://github.com/kcat/openal-soft/tree/master/include/AL
 * Documentation:
 *		https://github.com/kcat/openal-soft/wiki/Programmer's-Guide
 *		https://kcat.strangesoft.net/misc-downloads/Effects%20Extension%20Guide.pdf
 *		https://github.com/kcat/openal-soft/issues/57 // AL_SOFTX_effect_chain
 *		https://www.openal.org/documentation/
 */

#nullable enable

using System;
using System.Runtime.InteropServices;

namespace OpenAL
{
	internal static class Bindings
	{
		private const string DLL = "soft_oal";

		#region ALC
		#region Tokens
		//public const int ALC_FALSE = 0x0000; // Equivalent to bool false
		//public const int ALC_TRUE = 0x0001; // Equivalent to bool true

		public const int ALC_FREQUENCY = 0x1007;
		public const int ALC_REFRESH = 0x1008;
		public const int ALC_SYNC = 0x1009;
		public const int ALC_MONO_SOURCES = 0x1010;
		public const int ALC_STEREO_SOURCES = 0x1011;

		public const int ALC_NO_ERROR = 0x0000;
		public const int ALC_INVALID_DEVICE = 0xA001;
		public const int ALC_INVALID_CONTEXT = 0xA002;
		public const int ALC_INVALID_ENUM = 0xA003;
		public const int ALC_INVALID_VALUE = 0xA004;
		public const int ALC_OUT_OF_MEMORY = 0xA005;

		public const int ALC_MAJOR_VERSION = 0x1000;
		public const int ALC_MINOR_VERSION = 0x1001;
		public const int ALC_ATTRIBUTES_SIZE = 0x1002;
		public const int ALC_ALL_ATTRIBUTES = 0x1003;

		public const int ALC_DEFAULT_DEVICE_SPECIFIER = 0x1004;
		public const int ALC_DEVICE_SPECIFIER = 0x1005;
		public const int ALC_EXTENSIONS = 0x1006;

		public const int ALC_CAPTURE_DEVICE_SPECIFIER = 0x0310;
		public const int ALC_CAPTURE_DEFAULT_DEVICE_SPECIFIER = 0x0311;
		public const int ALC_CAPTURE_SAMPLES = 0x0312;

		public const int ALC_DEFAULT_ALL_DEVICES_SPECIFIER = 0x1012;
		public const int ALC_ALL_DEVICES_SPECIFIER = 0x1013;
		#endregion

		#region Enums
		public enum alcErrorCode : int
		{
			NoError = ALC_NO_ERROR,
			InvalidDevice = ALC_INVALID_DEVICE,
			InvalidContext = ALC_INVALID_CONTEXT,
			InvalidEnum = ALC_INVALID_ENUM,
			InvalidValue = ALC_INVALID_VALUE,
			OutOfMemory = ALC_OUT_OF_MEMORY
		}

		public enum alcString : int
		{
			DefaultDeviceSpecifier = ALC_DEFAULT_DEVICE_SPECIFIER,
			CaptureDefaultDeviceSpecifier = ALC_CAPTURE_DEFAULT_DEVICE_SPECIFIER,
			DeviceSpecifier = ALC_DEVICE_SPECIFIER,
			CaptureDeviceSpecifier = ALC_CAPTURE_DEVICE_SPECIFIER,
			Extensions = ALC_EXTENSIONS,
			DefaultAllDevicesSpecifier = ALC_DEFAULT_ALL_DEVICES_SPECIFIER,
			AllDevicesSpecifier = ALC_ALL_DEVICES_SPECIFIER
		}

		public enum alcInteger : int
		{
			MajorVersion = ALC_MAJOR_VERSION,
			MinorVersion = ALC_MINOR_VERSION,
			AttributesSize = ALC_ATTRIBUTES_SIZE,
			AllAttributes = ALC_ALL_ATTRIBUTES,
			CaptureSamples = ALC_CAPTURE_SAMPLES,
			Connected = ALC_CONNECTED // ALC_EXT_disconnect
		}

		public enum alcAttribute : int
		{
			Frequency = ALC_FREQUENCY,
			Refresh = ALC_REFRESH,
			Sync = ALC_SYNC,
			MonoSources = ALC_MONO_SOURCES,
			StereoSources = ALC_STEREO_SOURCES
		}
		#endregion

		#region Functions
		// Context Error Functions
		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern int alcGetError(IntPtr device);

		// Context State Functions
		[DllImport(DLL, EntryPoint = "alcGetString", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr INTERNAL_alcGetString(IntPtr device, int param);
		public static string alcGetString(IntPtr? device, int param) => Marshal.PtrToStringAnsi(INTERNAL_alcGetString(device ?? IntPtr.Zero, param));
		public static string[] alcGetStringArray(IntPtr? device, int param) => PtrToStringArray(INTERNAL_alcGetString(device ?? IntPtr.Zero, param));
		[DllImport(DLL, EntryPoint = "alcGetString", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr INTERNAL_alcGetString(IntPtr device, alcString param);
		public static string alcGetString(IntPtr? device, alcString param) => Marshal.PtrToStringAnsi(INTERNAL_alcGetString(device ?? IntPtr.Zero, param));
		public static string[] alcGetStringArray(IntPtr? device, alcString param) => PtrToStringArray(INTERNAL_alcGetString(device ?? IntPtr.Zero, param));
		private static string[] PtrToStringArray(IntPtr stringPtr)
		{
			System.Collections.Generic.List<string> strings = new System.Collections.Generic.List<string>();

			bool lastNull = false;
			int i = -1;
			byte c;
			while (!((c = Marshal.ReadByte(stringPtr, ++i)) == '\0' && lastNull))
			{
				if (c == '\0')
				{
					lastNull = true;

					strings.Add(Marshal.PtrToStringAnsi(stringPtr, i));
					stringPtr = new IntPtr((long)stringPtr + i + 1);
					i = -1;
				}
				else
					lastNull = false;
			}

			return strings.ToArray();
		}

		[DllImport(DLL, EntryPoint = "alcGetIntegerv", CallingConvention = CallingConvention.Cdecl)]
		private static extern void INTERNAL_alcGetIntegerv(IntPtr device, int param, int size, int[] values);
		public static void alcGetIntegerv(IntPtr? device, int param, int size, int[] values) => INTERNAL_alcGetIntegerv(device ?? IntPtr.Zero, param, size, values);
		[DllImport(DLL, EntryPoint = "alcGetIntegerv", CallingConvention = CallingConvention.Cdecl)]
		private static extern void INTERNAL_alcGetIntegerv(IntPtr device, alcInteger param, int size, int[] values);
		public static void alcGetIntegerv(IntPtr? device, alcInteger param, int size, int[] values) => INTERNAL_alcGetIntegerv(device ?? IntPtr.Zero, param, size, values);

		[DllImport(DLL, EntryPoint = "alcGetIntegerv", CallingConvention = CallingConvention.Cdecl)]
		private static extern void INTERNAL_alcGetIntegerv(IntPtr device, int param, int size, out int values);
		public static void alcGetIntegerv(IntPtr? device, int param, out int value) => INTERNAL_alcGetIntegerv(device ?? IntPtr.Zero, param, 1, out value);
		[DllImport(DLL, EntryPoint = "alcGetIntegerv", CallingConvention = CallingConvention.Cdecl)]
		private static extern void INTERNAL_alcGetIntegerv(IntPtr device, alcInteger param, int size, out int values);
		public static void alcGetIntegerv(IntPtr? device, alcInteger param, out int value) => INTERNAL_alcGetIntegerv(device ?? IntPtr.Zero, param, 1, out value);

		// Context Device Functions
		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr alcOpenDevice([In()][MarshalAs(UnmanagedType.LPStr)]string? devicename);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool alcCloseDevice(IntPtr device);

		// Context Management Functions
		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr alcCreateContext(IntPtr device, int[]? attrlist);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alcDestroyContext(IntPtr context);

		[DllImport(DLL, EntryPoint = "alcMakeContextCurrent", CallingConvention = CallingConvention.Cdecl)]
		private static extern bool INTERNAL_alcMakeContextCurrent(IntPtr context);
		public static bool alcMakeContextCurrent(IntPtr? context) => INTERNAL_alcMakeContextCurrent(context ?? IntPtr.Zero);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alcProcessContext(IntPtr context); // Possible NOP

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alcSuspendContext(IntPtr context); // Possible NOP

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr alcGetCurrentContext();

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr alcGetContextsDevice(IntPtr context);

		// Context Capture Functions
		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr alcCaptureOpenDevice([In()][MarshalAs(UnmanagedType.LPStr)]string devicename, uint frequency, int format, int buffersize);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool alcCaptureCloseDevice(IntPtr device);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alcCaptureStart(IntPtr device);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alcCaptureStop(IntPtr device);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alcCaptureSamples(IntPtr device, IntPtr buffer, int samples);

		// Context Extension Functions
		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool alcIsExtensionPresent(IntPtr device, [In()][MarshalAs(UnmanagedType.LPStr)]string extname);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr alcGetProcAddress(IntPtr device, [In()][MarshalAs(UnmanagedType.LPStr)]string funcname);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern int alcGetEnumValue(IntPtr device, [In()][MarshalAs(UnmanagedType.LPStr)]string enumname);
		#endregion
		#endregion

		#region AL
		#region Tokens
		public const int AL_NONE = 0x0000;
		//public const int AL_FALSE = 0x0000; // Equivalent to bool false
		//public const int AL_TRUE = 0x0001; // Equivalent to bool true

		public const int AL_SOURCE_RELATIVE = 0x0202;
		public const int AL_CONE_INNER_ANGLE = 0x1001;
		public const int AL_CONE_OUTER_ANGLE = 0x1002;
		public const int AL_PITCH = 0x1003;
		public const int AL_POSITION = 0x1004;
		public const int AL_DIRECTION = 0x1005;
		public const int AL_VELOCITY = 0x1006;
		public const int AL_LOOPING = 0x1007;
		public const int AL_BUFFER = 0x1009;
		public const int AL_GAIN = 0x100A;
		public const int AL_MIN_GAIN = 0x100D;
		public const int AL_MAX_GAIN = 0x100E;
		public const int AL_ORIENTATION = 0x100F;

		public const int AL_SOURCE_STATE = 0x1010;
		public const int AL_INITIAL = 0x1011;
		public const int AL_PLAYING = 0x1012;
		public const int AL_PAUSED = 0x1013;
		public const int AL_STOPPED = 0x1014;

		public const int AL_BUFFERS_QUEUED = 0x1015;
		public const int AL_BUFFERS_PROCESSED = 0x1016;
		public const int AL_REFERENCE_DISTANCE = 0x1020;
		public const int AL_ROLLOFF_FACTOR = 0x1021;
		public const int AL_CONE_OUTER_GAIN = 0x1022;
		public const int AL_MAX_DISTANCE = 0x1023;
		public const int AL_SEC_OFFSET = 0x1024;
		public const int AL_SAMPLE_OFFSET = 0x1025;
		public const int AL_BYTE_OFFSET = 0x1026;
		public const int AL_SOURCE_TYPE = 0x1027;
		public const int AL_STATIC = 0x1028;
		public const int AL_STREAMING = 0x1029;
		public const int AL_UNDETERMINED = 0x1030;

		public const int AL_FORMAT_MONO8 = 0x1100;
		public const int AL_FORMAT_MONO16 = 0x1101;
		public const int AL_FORMAT_STEREO8 = 0x1102;
		public const int AL_FORMAT_STEREO16 = 0x1103;

		public const int AL_FREQUENCY = 0x2001;
		public const int AL_BITS = 0x2002;
		public const int AL_CHANNELS = 0x2003;
		public const int AL_SIZE = 0x2004;

		public const int AL_NO_ERROR = 0x0000;
		public const int AL_INVALID_NAME = 0xA001;
		public const int AL_INVALID_ENUM = 0xA002;
		public const int AL_INVALID_VALUE = 0xA003;
		public const int AL_INVALID_OPERATION = 0xA004;
		public const int AL_OUT_OF_MEMORY = 0xA005;

		public const int AL_VENDOR = 0xB001;
		public const int AL_VERSION = 0xB002;
		public const int AL_RENDERER = 0xB003;
		public const int AL_EXTENSIONS = 0xB004;

		public const int AL_DOPPLER_FACTOR = 0xC000;
		//public const int AL_DOPPLER_VELOCITY = 0xC001; // Deprecated
		public const int AL_SPEED_OF_SOUND = 0xC003;

		public const int AL_DISTANCE_MODEL = 0xD000;
		public const int AL_INVERSE_DISTANCE = 0xD001;
		public const int AL_INVERSE_DISTANCE_CLAMPED = 0xD002;
		public const int AL_LINEAR_DISTANCE = 0xD003;
		public const int AL_LINEAR_DISTANCE_CLAMPED = 0xD004;
		public const int AL_EXPONENT_DISTANCE = 0xD005;
		public const int AL_EXPONENT_DISTANCE_CLAMPED = 0xD006;
		#endregion

		#region Enums
		public enum alErrorCode : int
		{
			NoError = AL_NO_ERROR,
			InvalidName = AL_INVALID_NAME,
			InvalidEnum = AL_INVALID_ENUM,
			InvalidValue = AL_INVALID_VALUE,
			InvalidOperation = AL_INVALID_OPERATION,
			OutOfMemory = AL_OUT_OF_MEMORY
		}

		public enum alCapability : int
		{
		}

		public enum alStatefParameter : int
		{
			DopplerFactor = AL_DOPPLER_FACTOR,
			SpeedOfSound = AL_SPEED_OF_SOUND, // Default 343.3f
			DistanceModel = AL_DISTANCE_MODEL // Default AL_INVERSE_DISTANCE_CLAMPED
		}

		public enum alString : int
		{
			Vendor = AL_VENDOR,
			Version = AL_VERSION,
			Renderer = AL_RENDERER,
			Extensions = AL_EXTENSIONS
		}

		public enum alDistanceModelType : int
		{
			InverseDistance = AL_INVERSE_DISTANCE,
			InverseDistanceClamped = AL_INVERSE_DISTANCE_CLAMPED, // Default
			LinearDistance = AL_LINEAR_DISTANCE,
			LinearDistanceClamped = AL_LINEAR_DISTANCE_CLAMPED,
			ExponentDistance = AL_EXPONENT_DISTANCE,
			ExponentDistanceClamped = AL_EXPONENT_DISTANCE_CLAMPED,
			None = AL_NONE
		}

		public enum alListenerfParameter : int
		{
			Gain = AL_GAIN
		}

		public enum alListener3fParameter : int
		{
			Position = AL_POSITION,
			Velocity = AL_VELOCITY
		}

		public enum alListenerfvParameter : int
		{
			Orientation = AL_ORIENTATION
		}

		public enum alBufferFormat : int
		{
			Mono8 = AL_FORMAT_MONO8,
			Mono16 = AL_FORMAT_MONO16,
			Mono32 = AL_FORMAT_MONO_FLOAT32,
			Mono64 = AL_FORMAT_MONO_DOUBLE_EXT,
			Stereo8 = AL_FORMAT_STEREO8,
			Stereo16 = AL_FORMAT_STEREO16,
			Stereo32 = AL_FORMAT_STEREO_FLOAT32,
			Stereo64 = AL_FORMAT_STEREO_DOUBLE_EXT
		}

		public enum alGetBufferiParameter : int
		{
			Frequency = AL_FREQUENCY,
			Bits = AL_BITS,
			Channels = AL_CHANNELS,
			Size = AL_SIZE
		}

		public enum alSourcefParameter : int
		{
			Pitch = AL_PITCH,
			Gain = AL_GAIN,
			MinGain = AL_MIN_GAIN,
			MaxGain = AL_MAX_GAIN,
			MaxDistance = AL_MAX_DISTANCE,
			RolloffFactor = AL_ROLLOFF_FACTOR,
			ConeOuterGain = AL_CONE_OUTER_GAIN,
			ConeInnerAngle = AL_CONE_INNER_ANGLE,
			ConeOuterAngle = AL_CONE_OUTER_ANGLE,
			ReferenceDistance = AL_REFERENCE_DISTANCE
		}

		public enum alSource3fParameter : int
		{
			Position = AL_POSITION,
			Velocity = AL_VELOCITY,
			Direction = AL_DIRECTION
		}

		public enum alSourceiParameter : int
		{
			SourceRelative = AL_SOURCE_RELATIVE,
			Looping = AL_LOOPING,
			Buffer = AL_BUFFER,
			SourceState = AL_SOURCE_STATE
		}

		public enum alGetSourceiParameter : int
		{
			SourceRelative = AL_SOURCE_RELATIVE,
			Looping = AL_LOOPING,
			Buffer = AL_BUFFER,
			SourceState = AL_SOURCE_STATE,
			BuffersQueued = AL_BUFFERS_QUEUED,
			BuffersProcessed = AL_BUFFERS_PROCESSED
		}

		public enum alSourceState : int
		{
			Initial = AL_INITIAL,
			Playing = AL_PLAYING,
			Paused = AL_PAUSED,
			Stopped = AL_STOPPED
		}
		#endregion

		#region Functions
		// Error Functions
		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern int alGetError();

		// State Functions
		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alEnable(int capability);
		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alEnable(alCapability capability);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alDisable(int capability);
		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alDisable(alCapability capability);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool alIsEnabled(int capability);
		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool alIsEnabled(alCapability capability);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool alGetBoolean(int param);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alGetBooleanv(int param, bool[] values);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern double alGetDouble(int param);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alGetDoublev(int param, double[] values);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern float alGetFloat(int param);
		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern float alGetFloat(alStatefParameter param);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alGetFloatv(int param, float[] values);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern int alGetInteger(int param);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alGetIntegerv(int param, int[] values);

		[DllImport(DLL, EntryPoint = "alGetString", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr INTERNAL_alGetString(int param);
		public static string alGetString(int param) => Marshal.PtrToStringAnsi(INTERNAL_alGetString(param));
		[DllImport(DLL, EntryPoint = "alGetString", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr INTERNAL_alGetString(alString param);
		public static string alGetString(alString param) => Marshal.PtrToStringAnsi(INTERNAL_alGetString(param));

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alDistanceModel(int distanceModel);
		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alDistanceModel(alDistanceModelType distanceModel);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alDopplerFactor(float value);

		//[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		//public static extern void alDopplerVelocity(float value);  // Deprecated

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alSpeedOfSound(float value);

		// Extension Functions
		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool alIsExtensionPresent([In()][MarshalAs(UnmanagedType.LPStr)]string extname);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr alGetProcAddress([In()][MarshalAs(UnmanagedType.LPStr)]string fname);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern int alGetEnumValue([In()][MarshalAs(UnmanagedType.LPStr)]string ename);

		// Listener Functions
		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alListenerf(int param, float value);
		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alListenerf(alListenerfParameter param, float value);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alListener3f(int param, float value1, float value2, float value3);
		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alListener3f(alListener3fParameter param, float value1, float value2, float value3);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alListenerfv(int param, float[] values);
		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alListenerfv(alListenerfvParameter param, float[] values);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alListeneri(int param, int value);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alListener3i(int param, int value1, int value2, int value3);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alListeneriv(int param, int[] values);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alGetListenerf(int param, out float value);
		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alGetListenerf(alListenerfParameter param, out float value);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alGetListener3f(int param, out float value1, out float value2, out float value3);
		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alGetListener3f(alListener3fParameter param, out float value1, out float value2, out float value3);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alGetListenerfv(int param, float[] values);
		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alGetListenerfv(alListenerfvParameter param, float[] values);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alGetListeneri(int param, out int value);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alGetListener3i(int param, out int value1, out int value2, out int value3);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alGetListeneriv(int param, int[] values);

		// Buffer Functions
		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alGenBuffers(int n, uint[] buffers);

		[DllImport(DLL, EntryPoint = "alGenBuffers", CallingConvention = CallingConvention.Cdecl)]
		private static extern void INTERNAL_alGenBuffers(int n, out uint buffers);
		public static uint alGenBuffer()
		{
			INTERNAL_alGenBuffers(1, out uint buffer);
			return buffer;
		}

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alDeleteBuffers(int n, uint[] buffers);

		[DllImport(DLL, EntryPoint = "alDeleteBuffers", CallingConvention = CallingConvention.Cdecl)]
		private static extern void INTERNAL_alDeleteBuffers(int n, ref uint buffers);
		public static void alDeleteBuffer(uint buffer) => INTERNAL_alDeleteBuffers(1, ref buffer);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool alIsBuffer(uint buffer);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alBufferData(uint buffer, int format, IntPtr data, int size, int freq);
		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alBufferData(uint buffer, alBufferFormat format, IntPtr data, int size, int freq);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alBufferData(uint buffer, int format, byte[] data, int size, int freq);
		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alBufferData(uint buffer, alBufferFormat format, byte[] data, int size, int freq);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alBufferf(uint buffer, int param, float value);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alBuffer3f(uint buffer, int param, float value1, float value2, float value3);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alBufferfv(uint buffer, int param, float[] values);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alBufferi(uint buffer, int param, int value);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alBuffer3i(uint buffer, int param, int value1, int value2, int value3);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alBufferiv(uint buffer, int param, int[] values);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alGetBufferf(uint buffer, int param, out float value);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alGetBuffer3f(uint buffer, int param, out float value1, out float value2, out float value3);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alGetBufferfv(uint buffer, int param, float[] values);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alGetBufferi(uint buffer, int param, out int value);
		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alGetBufferi(uint buffer, alGetBufferiParameter param, out int value);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alGetBuffer3i(uint buffer, int param, out int value1, out int value2, out int value3);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alGetBufferiv(uint buffer, int param, int[] values);

		// Source Functions
		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alGenSources(int n, uint[] sources);

		[DllImport(DLL, EntryPoint = "alGenSources", CallingConvention = CallingConvention.Cdecl)]
		private static extern void INTERNAL_alGenSources(int n, out uint sources);
		public static uint alGenSource()
		{
			INTERNAL_alGenSources(1, out uint source);
			return source;
		}

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alDeleteSources(int n, uint[] sources);

		[DllImport(DLL, EntryPoint = "alDeleteSources", CallingConvention = CallingConvention.Cdecl)]
		private static extern void INTERNAL_alDeleteSources(int n, ref uint sources);
		public static void alDeleteSource(uint source) => INTERNAL_alDeleteSources(1, ref source);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool alIsSource(uint source);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alSourcef(uint source, int param, float value);
		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alSourcef(uint source, alSourcefParameter param, float value);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alSource3f(uint source, int param, float value1, float value2, float value3);
		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alSource3f(uint source, alSource3fParameter param, float value1, float value2, float value3);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alSourcefv(uint source, int param, float[] values);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alSourcei(uint source, int param, int value);
		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alSourcei(uint source, alSourceiParameter param, int value);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alSource3i(uint source, int param, int value1, int value2, int value3);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alSourceiv(uint source, int param, int[] values);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alGetSourcef(uint source, int param, out float value);
		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alGetSourcef(uint source, alSourcefParameter param, out float value);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alGetSource3f(uint source, int param, out float value1, out float value2, out float value3);
		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alGetSource3f(uint source, alSource3fParameter param, out float value1, out float value2, out float value3);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alGetSourcefv(uint source, int param, float[] values);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alGetSourcei(uint source, int param, out int value);
		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alGetSourcei(uint source, alGetSourceiParameter param, out int value);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alGetSource3i(uint source, int param, out int value1, out int value2, out int value3);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alGetSourceiv(uint source, int param, int[] values);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alSourcePlay(uint source);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alSourcePlayv(int n, uint[] sources);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alSourcePause(uint source);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alSourcePausev(int n, uint[] sources);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alSourceStop(uint source);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alSourceStopv(int n, uint[] sources);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alSourceRewind(uint source);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alSourceRewindv(int n, uint[] sources);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alSourceQueueBuffers(uint source, int nb, uint[] buffers);

		[DllImport(DLL, EntryPoint = "alSourceQueueBuffers", CallingConvention = CallingConvention.Cdecl)]
		private static extern void INTERNAL_alSourceQueueBuffers(uint source, int nb, ref uint buffers);
		public static void alSourceQueueBuffers(uint source, uint buffer) => INTERNAL_alSourceQueueBuffers(source, 1, ref buffer);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alSourceUnqueueBuffers(uint source, int nb, uint[] buffers);

		[DllImport(DLL, EntryPoint = "alSourceUnqueueBuffers", CallingConvention = CallingConvention.Cdecl)]
		private static extern void INTERNAL_alSourceUnqueueBuffers(uint source, int nb, out uint buffers);
		public static void alSourceUnqueueBuffers(uint source, out uint buffer) => INTERNAL_alSourceUnqueueBuffers(source, 1, out buffer);
		#endregion
		#endregion

		#region ALEXT
		#region Tokens
		// ALC_EXT_disconnect
		public const int ALC_CONNECTED = 0x313;

		// AL_EXT_float32
		public const int AL_FORMAT_MONO_FLOAT32 = 0x10010;
		public const int AL_FORMAT_STEREO_FLOAT32 = 0x10011;

		// AL_EXT_double
		public const int AL_FORMAT_MONO_DOUBLE_EXT = 0x10012;
		public const int AL_FORMAT_STEREO_DOUBLE_EXT = 0x10013;

		// AL_EXT_MCFORMATS
		public const int AL_FORMAT_QUAD8 = 0x1204;
		public const int AL_FORMAT_QUAD16 = 0x1205;
		public const int AL_FORMAT_QUAD32 = 0x1206;
		public const int AL_FORMAT_REAR8 = 0x1207;
		public const int AL_FORMAT_REAR16 = 0x1208;
		public const int AL_FORMAT_REAR32 = 0x1209;
		public const int AL_FORMAT_51CHN8 = 0x120A;
		public const int AL_FORMAT_51CHN16 = 0x120B;
		public const int AL_FORMAT_51CHN32 = 0x120C;
		public const int AL_FORMAT_61CHN8 = 0x120D;
		public const int AL_FORMAT_61CHN16 = 0x120E;
		public const int AL_FORMAT_61CHN32 = 0x120F;
		public const int AL_FORMAT_71CHN8 = 0x1210;
		public const int AL_FORMAT_71CHN16 = 0x1211;
		public const int AL_FORMAT_71CHN32 = 0x1212;

		// AL_SOFTX_effect_chain
		public const int AL_EFFECTSLOT_TARGET_SOFT = 0xf000;
		#endregion

		#region Functions
		// ALC_SOFT_pause_device
		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool alcDevicePauseSOFT(IntPtr device);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool alcDeviceResumeSOFT(IntPtr device);
		#endregion
		#endregion

		#region EFX
		#region Tokens
		public const string ALC_EXT_EFX_NAME = "ALC_EXT_EFX";

		public const int ALC_EFX_MAJOR_VERSION = 0x20001;
		public const int ALC_EFX_MINOR_VERSION = 0x20002;
		public const int ALC_MAX_AUXILIARY_SENDS = 0x20003; // Default 2, Max clamped to 16


		// Listener properties
		public const int AL_METERS_PER_UNIT = 0x20004;

		// Source properties
		public const int AL_DIRECT_FILTER = 0x20005;
		public const int AL_AUXILIARY_SEND_FILTER = 0x20006;
		public const int AL_AIR_ABSORPTION_FACTOR = 0x20007;
		public const int AL_ROOM_ROLLOFF_FACTOR = 0x20008;
		public const int AL_CONE_OUTER_GAINHF = 0x20009;
		public const int AL_DIRECT_FILTER_GAINHF_AUTO = 0x2000A;
		public const int AL_AUXILIARY_SEND_FILTER_GAIN_AUTO = 0x2000B;
		public const int AL_AUXILIARY_SEND_FILTER_GAINHF_AUTO = 0x2000C;


		// Effect properties

		// Reverb effect parameters
		public const int AL_REVERB_DENSITY = 0x0001;
		public const int AL_REVERB_DIFFUSION = 0x0002;
		public const int AL_REVERB_GAIN = 0x0003;
		public const int AL_REVERB_GAINHF = 0x0004;
		public const int AL_REVERB_DECAY_TIME = 0x0005;
		public const int AL_REVERB_DECAY_HFRATIO = 0x0006;
		public const int AL_REVERB_REFLECTIONS_GAIN = 0x0007;
		public const int AL_REVERB_REFLECTIONS_DELAY = 0x0008;
		public const int AL_REVERB_LATE_REVERB_GAIN = 0x0009;
		public const int AL_REVERB_LATE_REVERB_DELAY = 0x000A;
		public const int AL_REVERB_AIR_ABSORPTION_GAINHF = 0x000B;
		public const int AL_REVERB_ROOM_ROLLOFF_FACTOR = 0x000C;
		public const int AL_REVERB_DECAY_HFLIMIT = 0x000D;

		// EAX Reverb effect parameters
		public const int AL_EAXREVERB_DENSITY = 0x0001;
		public const int AL_EAXREVERB_DIFFUSION = 0x0002;
		public const int AL_EAXREVERB_GAIN = 0x0003;
		public const int AL_EAXREVERB_GAINHF = 0x0004;
		public const int AL_EAXREVERB_GAINLF = 0x0005;
		public const int AL_EAXREVERB_DECAY_TIME = 0x0006;
		public const int AL_EAXREVERB_DECAY_HFRATIO = 0x0007;
		public const int AL_EAXREVERB_DECAY_LFRATIO = 0x0008;
		public const int AL_EAXREVERB_REFLECTIONS_GAIN = 0x0009;
		public const int AL_EAXREVERB_REFLECTIONS_DELAY = 0x000A;
		public const int AL_EAXREVERB_REFLECTIONS_PAN = 0x000B;
		public const int AL_EAXREVERB_LATE_REVERB_GAIN = 0x000C;
		public const int AL_EAXREVERB_LATE_REVERB_DELAY = 0x000D;
		public const int AL_EAXREVERB_LATE_REVERB_PAN = 0x000E;
		public const int AL_EAXREVERB_ECHO_TIME = 0x000F;
		public const int AL_EAXREVERB_ECHO_DEPTH = 0x0010;
		public const int AL_EAXREVERB_MODULATION_TIME = 0x0011;
		public const int AL_EAXREVERB_MODULATION_DEPTH = 0x0012;
		public const int AL_EAXREVERB_AIR_ABSORPTION_GAINHF = 0x0013;
		public const int AL_EAXREVERB_HFREFERENCE = 0x0014;
		public const int AL_EAXREVERB_LFREFERENCE = 0x0015;
		public const int AL_EAXREVERB_ROOM_ROLLOFF_FACTOR = 0x0016;
		public const int AL_EAXREVERB_DECAY_HFLIMIT = 0x0017;

		// Chorus effect parameters
		public const int AL_CHORUS_WAVEFORM = 0x0001;
		public const int AL_CHORUS_PHASE = 0x0002;
		public const int AL_CHORUS_RATE = 0x0003;
		public const int AL_CHORUS_DEPTH = 0x0004;
		public const int AL_CHORUS_FEEDBACK = 0x0005;
		public const int AL_CHORUS_DELAY = 0x0006;

		// Distortion effect parameters
		public const int AL_DISTORTION_EDGE = 0x0001;
		public const int AL_DISTORTION_GAIN = 0x0002;
		public const int AL_DISTORTION_LOWPASS_CUTOFF = 0x0003;
		public const int AL_DISTORTION_EQCENTER = 0x0004;
		public const int AL_DISTORTION_EQBANDWIDTH = 0x0005;

		// Echo effect parameters
		public const int AL_ECHO_DELAY = 0x0001;
		public const int AL_ECHO_LRDELAY = 0x0002;
		public const int AL_ECHO_DAMPING = 0x0003;
		public const int AL_ECHO_FEEDBACK = 0x0004;
		public const int AL_ECHO_SPREAD = 0x0005;

		// Flanger effect parameters
		public const int AL_FLANGER_WAVEFORM = 0x0001;
		public const int AL_FLANGER_PHASE = 0x0002;
		public const int AL_FLANGER_RATE = 0x0003;
		public const int AL_FLANGER_DEPTH = 0x0004;
		public const int AL_FLANGER_FEEDBACK = 0x0005;
		public const int AL_FLANGER_DELAY = 0x0006;

		// Frequency shifter effect parameters
		public const int AL_FREQUENCY_SHIFTER_FREQUENCY = 0x0001;
		public const int AL_FREQUENCY_SHIFTER_LEFT_DIRECTION = 0x0002;
		public const int AL_FREQUENCY_SHIFTER_RIGHT_DIRECTION = 0x0003;

		// Vocal morpher effect parameters
		public const int AL_VOCAL_MORPHER_PHONEMEA = 0x0001;
		public const int AL_VOCAL_MORPHER_PHONEMEA_COARSE_TUNING = 0x0002;
		public const int AL_VOCAL_MORPHER_PHONEMEB = 0x0003;
		public const int AL_VOCAL_MORPHER_PHONEMEB_COARSE_TUNING = 0x0004;
		public const int AL_VOCAL_MORPHER_WAVEFORM = 0x0005;
		public const int AL_VOCAL_MORPHER_RATE = 0x0006;

		// Pitchshifter effect parameters
		public const int AL_PITCH_SHIFTER_COARSE_TUNE = 0x0001;
		public const int AL_PITCH_SHIFTER_FINE_TUNE = 0x0002;

		// Ringmodulator effect parameters
		public const int AL_RING_MODULATOR_FREQUENCY = 0x0001;
		public const int AL_RING_MODULATOR_HIGHPASS_CUTOFF = 0x0002;
		public const int AL_RING_MODULATOR_WAVEFORM = 0x0003;

		// Autowah effect parameters
		public const int AL_AUTOWAH_ATTACK_TIME = 0x0001;
		public const int AL_AUTOWAH_RELEASE_TIME = 0x0002;
		public const int AL_AUTOWAH_RESONANCE = 0x0003;
		public const int AL_AUTOWAH_PEAK_GAIN = 0x0004;

		// Compressor effect parameters
		public const int AL_COMPRESSOR_ONOFF = 0x0001;

		// Equalizer effect parameters
		public const int AL_EQUALIZER_LOW_GAIN = 0x0001;
		public const int AL_EQUALIZER_LOW_CUTOFF = 0x0002;
		public const int AL_EQUALIZER_MID1_GAIN = 0x0003;
		public const int AL_EQUALIZER_MID1_CENTER = 0x0004;
		public const int AL_EQUALIZER_MID1_WIDTH = 0x0005;
		public const int AL_EQUALIZER_MID2_GAIN = 0x0006;
		public const int AL_EQUALIZER_MID2_CENTER = 0x0007;
		public const int AL_EQUALIZER_MID2_WIDTH = 0x0008;
		public const int AL_EQUALIZER_HIGH_GAIN = 0x0009;
		public const int AL_EQUALIZER_HIGH_CUTOFF = 0x000A;

		// Effect type
		public const int AL_EFFECT_FIRST_PARAMETER = 0x0000;
		public const int AL_EFFECT_LAST_PARAMETER = 0x8000;
		public const int AL_EFFECT_TYPE = 0x8001;

		// Effect types, used with the AL_EFFECT_TYPE property
		public const int AL_EFFECT_NULL = 0x0000;
		public const int AL_EFFECT_REVERB = 0x0001;
		public const int AL_EFFECT_CHORUS = 0x0002;
		public const int AL_EFFECT_DISTORTION = 0x0003;
		public const int AL_EFFECT_ECHO = 0x0004;
		public const int AL_EFFECT_FLANGER = 0x0005;
		public const int AL_EFFECT_FREQUENCY_SHIFTER = 0x0006;
		public const int AL_EFFECT_VOCAL_MORPHER = 0x0007;
		public const int AL_EFFECT_PITCH_SHIFTER = 0x0008;
		public const int AL_EFFECT_RING_MODULATOR = 0x0009;
		public const int AL_EFFECT_AUTOWAH = 0x000A;
		public const int AL_EFFECT_COMPRESSOR = 0x000B;
		public const int AL_EFFECT_EQUALIZER = 0x000C;
		public const int AL_EFFECT_EAXREVERB = 0x8000;

		// Auxiliary Effect Slot properties
		public const int AL_EFFECTSLOT_EFFECT = 0x0001;
		public const int AL_EFFECTSLOT_GAIN = 0x0002;
		public const int AL_EFFECTSLOT_AUXILIARY_SEND_AUTO = 0x0003;

		// NULL Auxiliary Slot ID to disable a source send
		public const int AL_EFFECTSLOT_NULL = 0x0000;


		// Filter properties

		// Lowpass filter parameters
		public const int AL_LOWPASS_GAIN = 0x0001;
		public const int AL_LOWPASS_GAINHF = 0x0002;

		// Highpass filter parameters
		public const int AL_HIGHPASS_GAIN = 0x0001;
		public const int AL_HIGHPASS_GAINLF = 0x0002;

		// Bandpass filter parameters
		public const int AL_BANDPASS_GAIN = 0x0001;
		public const int AL_BANDPASS_GAINLF = 0x0002;
		public const int AL_BANDPASS_GAINHF = 0x0003;

		// Filter type
		public const int AL_FILTER_FIRST_PARAMETER = 0x0000;
		public const int AL_FILTER_LAST_PARAMETER = 0x8000;
		public const int AL_FILTER_TYPE = 0x8001;

		// Filter types, used with the AL_FILTER_TYPE property
		public const int AL_FILTER_NULL = 0x0000;
		public const int AL_FILTER_LOWPASS = 0x0001;
		public const int AL_FILTER_HIGHPASS = 0x0002;
		public const int AL_FILTER_BANDPASS = 0x0003;
		#endregion

		#region Filter/Effect ranges and defaults
		// Filter ranges and defaults

		// Lowpass filter
		public const float AL_LOWPASS_MIN_GAIN = 0.0f;
		public const float AL_LOWPASS_MAX_GAIN = 1.0f;
		public const float AL_LOWPASS_DEFAULT_GAIN = 1.0f;

		public const float AL_LOWPASS_MIN_GAINHF = 0.0f;
		public const float AL_LOWPASS_MAX_GAINHF = 1.0f;
		public const float AL_LOWPASS_DEFAULT_GAINHF = 1.0f;

		// Highpass filter
		public const float AL_HIGHPASS_MIN_GAIN = 0.0f;
		public const float AL_HIGHPASS_MAX_GAIN = 1.0f;
		public const float AL_HIGHPASS_DEFAULT_GAIN = 1.0f;

		public const float AL_HIGHPASS_MIN_GAINLF = 0.0f;
		public const float AL_HIGHPASS_MAX_GAINLF = 1.0f;
		public const float AL_HIGHPASS_DEFAULT_GAINLF = 1.0f;

		// Bandpass filter
		public const float AL_BANDPASS_MIN_GAIN = 0.0f;
		public const float AL_BANDPASS_MAX_GAIN = 1.0f;
		public const float AL_BANDPASS_DEFAULT_GAIN = 1.0f;

		public const float AL_BANDPASS_MIN_GAINHF = 0.0f;
		public const float AL_BANDPASS_MAX_GAINHF = 1.0f;
		public const float AL_BANDPASS_DEFAULT_GAINHF = 1.0f;

		public const float AL_BANDPASS_MIN_GAINLF = 0.0f;
		public const float AL_BANDPASS_MAX_GAINLF = 1.0f;
		public const float AL_BANDPASS_DEFAULT_GAINLF = 1.0f;


		// Effect parameter ranges and defaults

		// Standard reverb effect
		public const float AL_REVERB_MIN_DENSITY = 0.0f;
		public const float AL_REVERB_MAX_DENSITY = 1.0f;
		public const float AL_REVERB_DEFAULT_DENSITY = 1.0f;

		public const float AL_REVERB_MIN_DIFFUSION = 0.0f;
		public const float AL_REVERB_MAX_DIFFUSION = 1.0f;
		public const float AL_REVERB_DEFAULT_DIFFUSION = 1.0f;

		public const float AL_REVERB_MIN_GAIN = 0.0f;
		public const float AL_REVERB_MAX_GAIN = 1.0f;
		public const float AL_REVERB_DEFAULT_GAIN = 0.32f;

		public const float AL_REVERB_MIN_GAINHF = 0.0f;
		public const float AL_REVERB_MAX_GAINHF = 1.0f;
		public const float AL_REVERB_DEFAULT_GAINHF = 0.89f;

		public const float AL_REVERB_MIN_DECAY_TIME = 0.1f;
		public const float AL_REVERB_MAX_DECAY_TIME = 20.0f;
		public const float AL_REVERB_DEFAULT_DECAY_TIME = 1.49f;

		public const float AL_REVERB_MIN_DECAY_HFRATIO = 0.1f;
		public const float AL_REVERB_MAX_DECAY_HFRATIO = 2.0f;
		public const float AL_REVERB_DEFAULT_DECAY_HFRATIO = 0.83f;

		public const float AL_REVERB_MIN_REFLECTIONS_GAIN = 0.0f;
		public const float AL_REVERB_MAX_REFLECTIONS_GAIN = 3.16f;
		public const float AL_REVERB_DEFAULT_REFLECTIONS_GAIN = 0.05f;

		public const float AL_REVERB_MIN_REFLECTIONS_DELAY = 0.0f;
		public const float AL_REVERB_MAX_REFLECTIONS_DELAY = 0.3f;
		public const float AL_REVERB_DEFAULT_REFLECTIONS_DELAY = 0.007f;

		public const float AL_REVERB_MIN_LATE_REVERB_GAIN = 0.0f;
		public const float AL_REVERB_MAX_LATE_REVERB_GAIN = 10.0f;
		public const float AL_REVERB_DEFAULT_LATE_REVERB_GAIN = 1.26f;

		public const float AL_REVERB_MIN_LATE_REVERB_DELAY = 0.0f;
		public const float AL_REVERB_MAX_LATE_REVERB_DELAY = 0.1f;
		public const float AL_REVERB_DEFAULT_LATE_REVERB_DELAY = 0.011f;

		public const float AL_REVERB_MIN_AIR_ABSORPTION_GAINHF = 0.892f;
		public const float AL_REVERB_MAX_AIR_ABSORPTION_GAINHF = 1.0f;
		public const float AL_REVERB_DEFAULT_AIR_ABSORPTION_GAINHF = 0.994f;

		public const float AL_REVERB_MIN_ROOM_ROLLOFF_FACTOR = 0.0f;
		public const float AL_REVERB_MAX_ROOM_ROLLOFF_FACTOR = 10.0f;
		public const float AL_REVERB_DEFAULT_ROOM_ROLLOFF_FACTOR = 0.0f;

		public const bool AL_REVERB_MIN_DECAY_HFLIMIT = false;
		public const bool AL_REVERB_MAX_DECAY_HFLIMIT = true;
		public const bool AL_REVERB_DEFAULT_DECAY_HFLIMIT = true;

		// EAX reverb effect
		public const float AL_EAXREVERB_MIN_DENSITY = 0.0f;
		public const float AL_EAXREVERB_MAX_DENSITY = 1.0f;
		public const float AL_EAXREVERB_DEFAULT_DENSITY = 1.0f;

		public const float AL_EAXREVERB_MIN_DIFFUSION = 0.0f;
		public const float AL_EAXREVERB_MAX_DIFFUSION = 1.0f;
		public const float AL_EAXREVERB_DEFAULT_DIFFUSION = 1.0f;

		public const float AL_EAXREVERB_MIN_GAIN = 0.0f;
		public const float AL_EAXREVERB_MAX_GAIN = 1.0f;
		public const float AL_EAXREVERB_DEFAULT_GAIN = 0.32f;

		public const float AL_EAXREVERB_MIN_GAINHF = 0.0f;
		public const float AL_EAXREVERB_MAX_GAINHF = 1.0f;
		public const float AL_EAXREVERB_DEFAULT_GAINHF = 0.89f;

		public const float AL_EAXREVERB_MIN_GAINLF = 0.0f;
		public const float AL_EAXREVERB_MAX_GAINLF = 1.0f;
		public const float AL_EAXREVERB_DEFAULT_GAINLF = 1.0f;

		public const float AL_EAXREVERB_MIN_DECAY_TIME = 0.1f;
		public const float AL_EAXREVERB_MAX_DECAY_TIME = 20.0f;
		public const float AL_EAXREVERB_DEFAULT_DECAY_TIME = 1.49f;

		public const float AL_EAXREVERB_MIN_DECAY_HFRATIO = 0.1f;
		public const float AL_EAXREVERB_MAX_DECAY_HFRATIO = 2.0f;
		public const float AL_EAXREVERB_DEFAULT_DECAY_HFRATIO = 0.83f;

		public const float AL_EAXREVERB_MIN_DECAY_LFRATIO = 0.1f;
		public const float AL_EAXREVERB_MAX_DECAY_LFRATIO = 2.0f;
		public const float AL_EAXREVERB_DEFAULT_DECAY_LFRATIO = 1.0f;

		public const float AL_EAXREVERB_MIN_REFLECTIONS_GAIN = 0.0f;
		public const float AL_EAXREVERB_MAX_REFLECTIONS_GAIN = 3.16f;
		public const float AL_EAXREVERB_DEFAULT_REFLECTIONS_GAIN = 0.05f;

		public const float AL_EAXREVERB_MIN_REFLECTIONS_DELAY = 0.0f;
		public const float AL_EAXREVERB_MAX_REFLECTIONS_DELAY = 0.3f;
		public const float AL_EAXREVERB_DEFAULT_REFLECTIONS_DELAY = 0.007f;

		public const float AL_EAXREVERB_DEFAULT_REFLECTIONS_PAN_XYZ = 0.0f;

		public const float AL_EAXREVERB_MIN_LATE_REVERB_GAIN = 0.0f;
		public const float AL_EAXREVERB_MAX_LATE_REVERB_GAIN = 10.0f;
		public const float AL_EAXREVERB_DEFAULT_LATE_REVERB_GAIN = 1.26f;

		public const float AL_EAXREVERB_MIN_LATE_REVERB_DELAY = 0.0f;
		public const float AL_EAXREVERB_MAX_LATE_REVERB_DELAY = 0.1f;
		public const float AL_EAXREVERB_DEFAULT_LATE_REVERB_DELAY = 0.011f;

		public const float AL_EAXREVERB_DEFAULT_LATE_REVERB_PAN_XYZ = 0.0f;

		public const float AL_EAXREVERB_MIN_ECHO_TIME = 0.075f;
		public const float AL_EAXREVERB_MAX_ECHO_TIME = 0.25f;
		public const float AL_EAXREVERB_DEFAULT_ECHO_TIME = 0.25f;

		public const float AL_EAXREVERB_MIN_ECHO_DEPTH = 0.0f;
		public const float AL_EAXREVERB_MAX_ECHO_DEPTH = 1.0f;
		public const float AL_EAXREVERB_DEFAULT_ECHO_DEPTH = 0.0f;

		public const float AL_EAXREVERB_MIN_MODULATION_TIME = 0.04f;
		public const float AL_EAXREVERB_MAX_MODULATION_TIME = 4.0f;
		public const float AL_EAXREVERB_DEFAULT_MODULATION_TIME = 0.25f;

		public const float AL_EAXREVERB_MIN_MODULATION_DEPTH = 0.0f;
		public const float AL_EAXREVERB_MAX_MODULATION_DEPTH = 1.0f;
		public const float AL_EAXREVERB_DEFAULT_MODULATION_DEPTH = 0.0f;

		public const float AL_EAXREVERB_MIN_AIR_ABSORPTION_GAINHF = 0.892f;
		public const float AL_EAXREVERB_MAX_AIR_ABSORPTION_GAINHF = 1.0f;
		public const float AL_EAXREVERB_DEFAULT_AIR_ABSORPTION_GAINHF = 0.994f;

		public const float AL_EAXREVERB_MIN_HFREFERENCE = 1000.0f;
		public const float AL_EAXREVERB_MAX_HFREFERENCE = 20000.0f;
		public const float AL_EAXREVERB_DEFAULT_HFREFERENCE = 5000.0f;

		public const float AL_EAXREVERB_MIN_LFREFERENCE = 20.0f;
		public const float AL_EAXREVERB_MAX_LFREFERENCE = 1000.0f;
		public const float AL_EAXREVERB_DEFAULT_LFREFERENCE = 250.0f;

		public const float AL_EAXREVERB_MIN_ROOM_ROLLOFF_FACTOR = 0.0f;
		public const float AL_EAXREVERB_MAX_ROOM_ROLLOFF_FACTOR = 10.0f;
		public const float AL_EAXREVERB_DEFAULT_ROOM_ROLLOFF_FACTOR = 0.0f;

		public const bool AL_EAXREVERB_MIN_DECAY_HFLIMIT = false;
		public const bool AL_EAXREVERB_MAX_DECAY_HFLIMIT = true;
		public const bool AL_EAXREVERB_DEFAULT_DECAY_HFLIMIT = true;

		// Chorus effect
		public const int AL_CHORUS_WAVEFORM_SINUSOID = 0;
		public const int AL_CHORUS_WAVEFORM_TRIANGLE = 1;

		public const int AL_CHORUS_MIN_WAVEFORM = 0;
		public const int AL_CHORUS_MAX_WAVEFORM = 1;
		public const int AL_CHORUS_DEFAULT_WAVEFORM = 1;

		public const float AL_CHORUS_MIN_PHASE = -180;
		public const float AL_CHORUS_MAX_PHASE = 180;
		public const float AL_CHORUS_DEFAULT_PHASE = 90;

		public const float AL_CHORUS_MIN_RATE = 0.0f;
		public const float AL_CHORUS_MAX_RATE = 10.0f;
		public const float AL_CHORUS_DEFAULT_RATE = 1.1f;

		public const float AL_CHORUS_MIN_DEPTH = 0.0f;
		public const float AL_CHORUS_MAX_DEPTH = 1.0f;
		public const float AL_CHORUS_DEFAULT_DEPTH = 0.1f;

		public const float AL_CHORUS_MIN_FEEDBACK = -1.0f;
		public const float AL_CHORUS_MAX_FEEDBACK = 1.0f;
		public const float AL_CHORUS_DEFAULT_FEEDBACK = 0.25f;

		public const float AL_CHORUS_MIN_DELAY = 0.0f;
		public const float AL_CHORUS_MAX_DELAY = 0.016f;
		public const float AL_CHORUS_DEFAULT_DELAY = 0.016f;

		// Distortion effect
		public const float AL_DISTORTION_MIN_EDGE = 0.0f;
		public const float AL_DISTORTION_MAX_EDGE = 1.0f;
		public const float AL_DISTORTION_DEFAULT_EDGE = 0.2f;

		public const float AL_DISTORTION_MIN_GAIN = 0.01f;
		public const float AL_DISTORTION_MAX_GAIN = 1.0f;
		public const float AL_DISTORTION_DEFAULT_GAIN = 0.05f;

		public const float AL_DISTORTION_MIN_LOWPASS_CUTOFF = 80.0f;
		public const float AL_DISTORTION_MAX_LOWPASS_CUTOFF = 24000.0f;
		public const float AL_DISTORTION_DEFAULT_LOWPASS_CUTOFF = 8000.0f;

		public const float AL_DISTORTION_MIN_EQCENTER = 80.0f;
		public const float AL_DISTORTION_MAX_EQCENTER = 24000.0f;
		public const float AL_DISTORTION_DEFAULT_EQCENTER = 3600.0f;

		public const float AL_DISTORTION_MIN_EQBANDWIDTH = 80.0f;
		public const float AL_DISTORTION_MAX_EQBANDWIDTH = 24000.0f;
		public const float AL_DISTORTION_DEFAULT_EQBANDWIDTH = 3600.0f;

		// Echo effect
		public const float AL_ECHO_MIN_DELAY = 0.0f;
		public const float AL_ECHO_MAX_DELAY = 0.207f;
		public const float AL_ECHO_DEFAULT_DELAY = 0.1f;

		public const float AL_ECHO_MIN_LRDELAY = 0.0f;
		public const float AL_ECHO_MAX_LRDELAY = 0.404f;
		public const float AL_ECHO_DEFAULT_LRDELAY = 0.1f;

		public const float AL_ECHO_MIN_DAMPING = 0.0f;
		public const float AL_ECHO_MAX_DAMPING = 0.99f;
		public const float AL_ECHO_DEFAULT_DAMPING = 0.5f;

		public const float AL_ECHO_MIN_FEEDBACK = 0.0f;
		public const float AL_ECHO_MAX_FEEDBACK = 1.0f;
		public const float AL_ECHO_DEFAULT_FEEDBACK = 0.5f;

		public const float AL_ECHO_MIN_SPREAD = -1.0f;
		public const float AL_ECHO_MAX_SPREAD = 1.0f;
		public const float AL_ECHO_DEFAULT_SPREAD = -1.0f;

		// Flanger effect
		public const int AL_FLANGER_WAVEFORM_SINUSOID = 0;
		public const int AL_FLANGER_WAVEFORM_TRIANGLE = 1;

		public const int AL_FLANGER_MIN_WAVEFORM = 0;
		public const int AL_FLANGER_MAX_WAVEFORM = 1;
		public const int AL_FLANGER_DEFAULT_WAVEFORM = 1;

		public const int AL_FLANGER_MIN_PHASE = -180;
		public const int AL_FLANGER_MAX_PHASE = 180;
		public const int AL_FLANGER_DEFAULT_PHASE = 0;

		public const float AL_FLANGER_MIN_RATE = 0.0f;
		public const float AL_FLANGER_MAX_RATE = 10.0f;
		public const float AL_FLANGER_DEFAULT_RATE = 0.27f;

		public const float AL_FLANGER_MIN_DEPTH = 0.0f;
		public const float AL_FLANGER_MAX_DEPTH = 1.0f;
		public const float AL_FLANGER_DEFAULT_DEPTH = 1.0f;

		public const float AL_FLANGER_MIN_FEEDBACK = -1.0f;
		public const float AL_FLANGER_MAX_FEEDBACK = 1.0f;
		public const float AL_FLANGER_DEFAULT_FEEDBACK = -0.5f;

		public const float AL_FLANGER_MIN_DELAY = 0.0f;
		public const float AL_FLANGER_MAX_DELAY = 0.004f;
		public const float AL_FLANGER_DEFAULT_DELAY = 0.002f;

		// Frequency shifter effect
		public const float AL_FREQUENCY_SHIFTER_MIN_FREQUENCY = 0.0f;
		public const float AL_FREQUENCY_SHIFTER_MAX_FREQUENCY = 24000.0f;
		public const float AL_FREQUENCY_SHIFTER_DEFAULT_FREQUENCY = 0.0f;

		public const int AL_FREQUENCY_SHIFTER_MIN_LEFT_DIRECTION = 0;
		public const int AL_FREQUENCY_SHIFTER_MAX_LEFT_DIRECTION = 2;
		public const int AL_FREQUENCY_SHIFTER_DEFAULT_LEFT_DIRECTION = 0;

		public const int AL_FREQUENCY_SHIFTER_DIRECTION_DOWN = 0;
		public const int AL_FREQUENCY_SHIFTER_DIRECTION_UP = 1;
		public const int AL_FREQUENCY_SHIFTER_DIRECTION_OFF = 2;

		public const int AL_FREQUENCY_SHIFTER_MIN_RIGHT_DIRECTION = 0;
		public const int AL_FREQUENCY_SHIFTER_MAX_RIGHT_DIRECTION = 2;
		public const int AL_FREQUENCY_SHIFTER_DEFAULT_RIGHT_DIRECTION = 0;

		// Vocal morpher effect
		public const int AL_VOCAL_MORPHER_MIN_PHONEMEA = 0;
		public const int AL_VOCAL_MORPHER_MAX_PHONEMEA = 29;
		public const int AL_VOCAL_MORPHER_DEFAULT_PHONEMEA = 0;

		public const int AL_VOCAL_MORPHER_MIN_PHONEMEA_COARSE_TUNING = -24;
		public const int AL_VOCAL_MORPHER_MAX_PHONEMEA_COARSE_TUNING = 24;
		public const int AL_VOCAL_MORPHER_DEFAULT_PHONEMEA_COARSE_TUNING = 0;

		public const int AL_VOCAL_MORPHER_MIN_PHONEMEB = 0;
		public const int AL_VOCAL_MORPHER_MAX_PHONEMEB = 29;
		public const int AL_VOCAL_MORPHER_DEFAULT_PHONEMEB = 10;

		public const int AL_VOCAL_MORPHER_MIN_PHONEMEB_COARSE_TUNING = -24;
		public const int AL_VOCAL_MORPHER_MAX_PHONEMEB_COARSE_TUNING = 24;
		public const int AL_VOCAL_MORPHER_DEFAULT_PHONEMEB_COARSE_TUNING = 0;

		public const int AL_VOCAL_MORPHER_PHONEME_A = 0;
		public const int AL_VOCAL_MORPHER_PHONEME_E = 1;
		public const int AL_VOCAL_MORPHER_PHONEME_I = 2;
		public const int AL_VOCAL_MORPHER_PHONEME_O = 3;
		public const int AL_VOCAL_MORPHER_PHONEME_U = 4;
		public const int AL_VOCAL_MORPHER_PHONEME_AA = 5;
		public const int AL_VOCAL_MORPHER_PHONEME_AE = 6;
		public const int AL_VOCAL_MORPHER_PHONEME_AH = 7;
		public const int AL_VOCAL_MORPHER_PHONEME_AO = 8;
		public const int AL_VOCAL_MORPHER_PHONEME_EH = 9;
		public const int AL_VOCAL_MORPHER_PHONEME_ER = 10;
		public const int AL_VOCAL_MORPHER_PHONEME_IH = 11;
		public const int AL_VOCAL_MORPHER_PHONEME_IY = 12;
		public const int AL_VOCAL_MORPHER_PHONEME_UH = 13;
		public const int AL_VOCAL_MORPHER_PHONEME_UW = 14;
		public const int AL_VOCAL_MORPHER_PHONEME_B = 15;
		public const int AL_VOCAL_MORPHER_PHONEME_D = 16;
		public const int AL_VOCAL_MORPHER_PHONEME_F = 17;
		public const int AL_VOCAL_MORPHER_PHONEME_G = 18;
		public const int AL_VOCAL_MORPHER_PHONEME_J = 19;
		public const int AL_VOCAL_MORPHER_PHONEME_K = 20;
		public const int AL_VOCAL_MORPHER_PHONEME_L = 21;
		public const int AL_VOCAL_MORPHER_PHONEME_M = 22;
		public const int AL_VOCAL_MORPHER_PHONEME_N = 23;
		public const int AL_VOCAL_MORPHER_PHONEME_P = 24;
		public const int AL_VOCAL_MORPHER_PHONEME_R = 25;
		public const int AL_VOCAL_MORPHER_PHONEME_S = 26;
		public const int AL_VOCAL_MORPHER_PHONEME_T = 27;
		public const int AL_VOCAL_MORPHER_PHONEME_V = 28;
		public const int AL_VOCAL_MORPHER_PHONEME_Z = 29;

		public const int AL_VOCAL_MORPHER_WAVEFORM_SINUSOID = 0;
		public const int AL_VOCAL_MORPHER_WAVEFORM_TRIANGLE = 1;
		public const int AL_VOCAL_MORPHER_WAVEFORM_SAWTOOTH = 2;

		public const int AL_VOCAL_MORPHER_MIN_WAVEFORM = 0;
		public const int AL_VOCAL_MORPHER_MAX_WAVEFORM = 2;
		public const int AL_VOCAL_MORPHER_DEFAULT_WAVEFORM = 0;

		public const float AL_VOCAL_MORPHER_MIN_RATE = 0.0f;
		public const float AL_VOCAL_MORPHER_MAX_RATE = 10.0f;
		public const float AL_VOCAL_MORPHER_DEFAULT_RATE = 1.41f;

		// Pitch shifter effect
		public const int AL_PITCH_SHIFTER_MIN_COARSE_TUNE = -12;
		public const int AL_PITCH_SHIFTER_MAX_COARSE_TUNE = 12;
		public const int AL_PITCH_SHIFTER_DEFAULT_COARSE_TUNE = 12;

		public const int AL_PITCH_SHIFTER_MIN_FINE_TUNE = -50;
		public const int AL_PITCH_SHIFTER_MAX_FINE_TUNE = 50;
		public const int AL_PITCH_SHIFTER_DEFAULT_FINE_TUNE = 0;

		// Ring modulator effect
		public const float AL_RING_MODULATOR_MIN_FREQUENCY = 0.0f;
		public const float AL_RING_MODULATOR_MAX_FREQUENCY = 8000.0f;
		public const float AL_RING_MODULATOR_DEFAULT_FREQUENCY = 440.0f;

		public const float AL_RING_MODULATOR_MIN_HIGHPASS_CUTOFF = 0.0f;
		public const float AL_RING_MODULATOR_MAX_HIGHPASS_CUTOFF = 24000.0f;
		public const float AL_RING_MODULATOR_DEFAULT_HIGHPASS_CUTOFF = 800.0f;

		public const int AL_RING_MODULATOR_SINUSOID = 0;
		public const int AL_RING_MODULATOR_SAWTOOTH = 1;
		public const int AL_RING_MODULATOR_SQUARE = 2;

		public const int AL_RING_MODULATOR_MIN_WAVEFORM = 0;
		public const int AL_RING_MODULATOR_MAX_WAVEFORM = 2;
		public const int AL_RING_MODULATOR_DEFAULT_WAVEFORM = 0;

		// Autowah effect
		public const float AL_AUTOWAH_MIN_ATTACK_TIME = 0.0001f;
		public const float AL_AUTOWAH_MAX_ATTACK_TIME = 1.0f;
		public const float AL_AUTOWAH_DEFAULT_ATTACK_TIME = 0.06f;

		public const float AL_AUTOWAH_MIN_RELEASE_TIME = 0.0001f;
		public const float AL_AUTOWAH_MAX_RELEASE_TIME = 1.0f;
		public const float AL_AUTOWAH_DEFAULT_RELEASE_TIME = 0.06f;

		public const float AL_AUTOWAH_MIN_RESONANCE = 2.0f;
		public const float AL_AUTOWAH_MAX_RESONANCE = 1000.0f;
		public const float AL_AUTOWAH_DEFAULT_RESONANCE = 1000.0f;

		public const float AL_AUTOWAH_MIN_PEAK_GAIN = 0.00003f;
		public const float AL_AUTOWAH_MAX_PEAK_GAIN = 31621.0f;
		public const float AL_AUTOWAH_DEFAULT_PEAK_GAIN = 11.22f;

		// Compressor effect
		public const int AL_COMPRESSOR_MIN_ONOFF = 0;
		public const int AL_COMPRESSOR_MAX_ONOFF = 1;
		public const int AL_COMPRESSOR_DEFAULT_ONOFF = 1;

		// Equalizer effect
		public const float AL_EQUALIZER_MIN_LOW_GAIN = 0.126f;
		public const float AL_EQUALIZER_MAX_LOW_GAIN = 7.943f;
		public const float AL_EQUALIZER_DEFAULT_LOW_GAIN = 1.0f;

		public const float AL_EQUALIZER_MIN_LOW_CUTOFF = 50.0f;
		public const float AL_EQUALIZER_MAX_LOW_CUTOFF = 800.0f;
		public const float AL_EQUALIZER_DEFAULT_LOW_CUTOFF = 200.0f;

		public const float AL_EQUALIZER_MIN_MID1_GAIN = 0.126f;
		public const float AL_EQUALIZER_MAX_MID1_GAIN = 7.943f;
		public const float AL_EQUALIZER_DEFAULT_MID1_GAIN = 1.0f;

		public const float AL_EQUALIZER_MIN_MID1_CENTER = 200.0f;
		public const float AL_EQUALIZER_MAX_MID1_CENTER = 3000.0f;
		public const float AL_EQUALIZER_DEFAULT_MID1_CENTER = 500.0f;

		public const float AL_EQUALIZER_MIN_MID1_WIDTH = 0.01f;
		public const float AL_EQUALIZER_MAX_MID1_WIDTH = 1.0f;
		public const float AL_EQUALIZER_DEFAULT_MID1_WIDTH = 1.0f;

		public const float AL_EQUALIZER_MIN_MID2_GAIN = 0.126f;
		public const float AL_EQUALIZER_MAX_MID2_GAIN = 7.943f;
		public const float AL_EQUALIZER_DEFAULT_MID2_GAIN = 1.0f;

		public const float AL_EQUALIZER_MIN_MID2_CENTER = 1000.0f;
		public const float AL_EQUALIZER_MAX_MID2_CENTER = 8000.0f;
		public const float AL_EQUALIZER_DEFAULT_MID2_CENTER = 3000.0f;

		public const float AL_EQUALIZER_MIN_MID2_WIDTH = 0.01f;
		public const float AL_EQUALIZER_MAX_MID2_WIDTH = 1.0f;
		public const float AL_EQUALIZER_DEFAULT_MID2_WIDTH = 1.0f;

		public const float AL_EQUALIZER_MIN_HIGH_GAIN = 0.126f;
		public const float AL_EQUALIZER_MAX_HIGH_GAIN = 7.943f;
		public const float AL_EQUALIZER_DEFAULT_HIGH_GAIN = 1.0f;

		public const float AL_EQUALIZER_MIN_HIGH_CUTOFF = 4000.0f;
		public const float AL_EQUALIZER_MAX_HIGH_CUTOFF = 16000.0f;
		public const float AL_EQUALIZER_DEFAULT_HIGH_CUTOFF = 6000.0f;


		// Source parameter value ranges and defaults
		public const float AL_MIN_AIR_ABSORPTION_FACTOR = 0.0f;
		public const float AL_MAX_AIR_ABSORPTION_FACTOR = 10.0f;
		public const float AL_DEFAULT_AIR_ABSORPTION_FACTOR = 0.0f;

		public const float AL_MIN_ROOM_ROLLOFF_FACTOR = 0.0f;
		public const float AL_MAX_ROOM_ROLLOFF_FACTOR = 10.0f;
		public const float AL_DEFAULT_ROOM_ROLLOFF_FACTOR = 0.0f;

		public const float AL_MIN_CONE_OUTER_GAINHF = 0.0f;
		public const float AL_MAX_CONE_OUTER_GAINHF = 1.0f;
		public const float AL_DEFAULT_CONE_OUTER_GAINHF = 1.0f;

		public const bool AL_MIN_DIRECT_FILTER_GAINHF_AUTO = false;
		public const bool AL_MAX_DIRECT_FILTER_GAINHF_AUTO = true;
		public const bool AL_DEFAULT_DIRECT_FILTER_GAINHF_AUTO = true;

		public const bool AL_MIN_AUXILIARY_SEND_FILTER_GAIN_AUTO = false;
		public const bool AL_MAX_AUXILIARY_SEND_FILTER_GAIN_AUTO = true;
		public const bool AL_DEFAULT_AUXILIARY_SEND_FILTER_GAIN_AUTO = true;

		public const bool AL_MIN_AUXILIARY_SEND_FILTER_GAINHF_AUTO = false;
		public const bool AL_MAX_AUXILIARY_SEND_FILTER_GAINHF_AUTO = true;
		public const bool AL_DEFAULT_AUXILIARY_SEND_FILTER_GAINHF_AUTO = true;


		// Listener parameter value ranges and defaults
		public const float AL_MIN_METERS_PER_UNIT = float.MinValue;
		public const float AL_MAX_METERS_PER_UNIT = float.MaxValue;
		public const float AL_DEFAULT_METERS_PER_UNIT = 1.0f;
		#endregion

		#region Reverb presets for EFX
		public struct alEFXReverbPreset
		{
			public float Density;
			public float Diffusion;
			public float Gain;
			public float GainHF;
			public float GainLF;
			public float DecayTime;
			public float DecayHFRatio;
			public float DecayLFRatio;
			public float ReflectionsGain;
			public float ReflectionsDelay;
			public float[] ReflectionsPan; // float[3]
			public float LateReverbGain;
			public float LateReverbDelay;
			public float[] LateReverbPan; // float[3]
			public float EchoTime;
			public float EchoDepth;
			public float ModulationTime;
			public float ModulationDepth;
			public float AirAbsorptionGainHF;
			public float HFReference;
			public float LFReference;
			public float RoomRolloffFactor;
			public int DecayHFLimit;

			public alEFXReverbPreset(float Density, float Diffusion, float Gain, float GainHF, float GainLF, float DecayTime, float DecayHFRatio, float DecayLFRatio, float ReflectionsGain, float ReflectionsDelay, float[] ReflectionsPan, float LateReverbGain, float LateReverbDelay, float[] LateReverbPan, float EchoTime, float EchoDepth, float ModulationTime, float ModulationDepth, float AirAbsorptionGainHF, float HFReference, float LFReference, float RoomRolloffFactor, int DecayHFLimit)
			{
				this.Density = Density;
				this.Diffusion = Diffusion;
				this.Gain = Gain;
				this.GainHF = GainHF;
				this.GainLF = GainLF;
				this.DecayTime = DecayTime;
				this.DecayHFRatio = DecayHFRatio;
				this.DecayLFRatio = DecayLFRatio;
				this.ReflectionsGain = ReflectionsGain;
				this.ReflectionsDelay = ReflectionsDelay;
				this.ReflectionsPan = ReflectionsPan;
				this.LateReverbGain = LateReverbGain;
				this.LateReverbDelay = LateReverbDelay;
				this.LateReverbPan = LateReverbPan;
				this.EchoTime = EchoTime;
				this.EchoDepth = EchoDepth;
				this.ModulationTime = ModulationTime;
				this.ModulationDepth = ModulationDepth;
				this.AirAbsorptionGainHF = AirAbsorptionGainHF;
				this.HFReference = HFReference;
				this.LFReference = LFReference;
				this.RoomRolloffFactor = RoomRolloffFactor;
				this.DecayHFLimit = DecayHFLimit;
			}
		}

		// Default Presets
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_GENERIC = new alEFXReverbPreset(1.0000f, 1.0000f, 0.3162f, 0.8913f, 1.0000f, 1.4900f, 0.8300f, 1.0000f, 0.0500f, 0.0070f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 1.2589f, 0.0110f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_PADDEDCELL = new alEFXReverbPreset(0.1715f, 1.0000f, 0.3162f, 0.0010f, 1.0000f, 0.1700f, 0.1000f, 1.0000f, 0.2500f, 0.0010f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 1.2691f, 0.0020f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_ROOM = new alEFXReverbPreset(0.4287f, 1.0000f, 0.3162f, 0.5929f, 1.0000f, 0.4000f, 0.8300f, 1.0000f, 0.1503f, 0.0020f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 1.0629f, 0.0030f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_BATHROOM = new alEFXReverbPreset(0.1715f, 1.0000f, 0.3162f, 0.2512f, 1.0000f, 1.4900f, 0.5400f, 1.0000f, 0.6531f, 0.0070f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 3.2734f, 0.0110f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_LIVINGROOM = new alEFXReverbPreset(0.9766f, 1.0000f, 0.3162f, 0.0010f, 1.0000f, 0.5000f, 0.1000f, 1.0000f, 0.2051f, 0.0030f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2805f, 0.0040f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_STONEROOM = new alEFXReverbPreset(1.0000f, 1.0000f, 0.3162f, 0.7079f, 1.0000f, 2.3100f, 0.6400f, 1.0000f, 0.4411f, 0.0120f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 1.1003f, 0.0170f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_AUDITORIUM = new alEFXReverbPreset(1.0000f, 1.0000f, 0.3162f, 0.5781f, 1.0000f, 4.3200f, 0.5900f, 1.0000f, 0.4032f, 0.0200f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.7170f, 0.0300f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_CONCERTHALL = new alEFXReverbPreset(1.0000f, 1.0000f, 0.3162f, 0.5623f, 1.0000f, 3.9200f, 0.7000f, 1.0000f, 0.2427f, 0.0200f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.9977f, 0.0290f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_CAVE = new alEFXReverbPreset(1.0000f, 1.0000f, 0.3162f, 1.0000f, 1.0000f, 2.9100f, 1.3000f, 1.0000f, 0.5000f, 0.0150f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.7063f, 0.0220f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x0);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_ARENA = new alEFXReverbPreset(1.0000f, 1.0000f, 0.3162f, 0.4477f, 1.0000f, 7.2400f, 0.3300f, 1.0000f, 0.2612f, 0.0200f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 1.0186f, 0.0300f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_HANGAR = new alEFXReverbPreset(1.0000f, 1.0000f, 0.3162f, 0.3162f, 1.0000f, 10.0500f, 0.2300f, 1.0000f, 0.5000f, 0.0200f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 1.2560f, 0.0300f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_CARPETEDHALLWAY = new alEFXReverbPreset(0.4287f, 1.0000f, 0.3162f, 0.0100f, 1.0000f, 0.3000f, 0.1000f, 1.0000f, 0.1215f, 0.0020f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.1531f, 0.0300f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_HALLWAY = new alEFXReverbPreset(0.3645f, 1.0000f, 0.3162f, 0.7079f, 1.0000f, 1.4900f, 0.5900f, 1.0000f, 0.2458f, 0.0070f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 1.6615f, 0.0110f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_STONECORRIDOR = new alEFXReverbPreset(1.0000f, 1.0000f, 0.3162f, 0.7612f, 1.0000f, 2.7000f, 0.7900f, 1.0000f, 0.2472f, 0.0130f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 1.5758f, 0.0200f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_ALLEY = new alEFXReverbPreset(1.0000f, 0.3000f, 0.3162f, 0.7328f, 1.0000f, 1.4900f, 0.8600f, 1.0000f, 0.2500f, 0.0070f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.9954f, 0.0110f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.1250f, 0.9500f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_FOREST = new alEFXReverbPreset(1.0000f, 0.3000f, 0.3162f, 0.0224f, 1.0000f, 1.4900f, 0.5400f, 1.0000f, 0.0525f, 0.1620f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.7682f, 0.0880f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.1250f, 1.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_CITY = new alEFXReverbPreset(1.0000f, 0.5000f, 0.3162f, 0.3981f, 1.0000f, 1.4900f, 0.6700f, 1.0000f, 0.0730f, 0.0070f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.1427f, 0.0110f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_MOUNTAINS = new alEFXReverbPreset(1.0000f, 0.2700f, 0.3162f, 0.0562f, 1.0000f, 1.4900f, 0.2100f, 1.0000f, 0.0407f, 0.3000f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.1919f, 0.1000f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 1.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x0);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_QUARRY = new alEFXReverbPreset(1.0000f, 1.0000f, 0.3162f, 0.3162f, 1.0000f, 1.4900f, 0.8300f, 1.0000f, 0.0000f, 0.0610f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 1.7783f, 0.0250f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.1250f, 0.7000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_PLAIN = new alEFXReverbPreset(1.0000f, 0.2100f, 0.3162f, 0.1000f, 1.0000f, 1.4900f, 0.5000f, 1.0000f, 0.0585f, 0.1790f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.1089f, 0.1000f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 1.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_PARKINGLOT = new alEFXReverbPreset(1.0000f, 1.0000f, 0.3162f, 1.0000f, 1.0000f, 1.6500f, 1.5000f, 1.0000f, 0.2082f, 0.0080f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2652f, 0.0120f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x0);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_SEWERPIPE = new alEFXReverbPreset(0.3071f, 0.8000f, 0.3162f, 0.3162f, 1.0000f, 2.8100f, 0.1400f, 1.0000f, 1.6387f, 0.0140f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 3.2471f, 0.0210f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_UNDERWATER = new alEFXReverbPreset(0.3645f, 1.0000f, 0.3162f, 0.0100f, 1.0000f, 1.4900f, 0.1000f, 1.0000f, 0.5963f, 0.0070f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 7.0795f, 0.0110f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.0000f, 1.1800f, 0.3480f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_DRUGGED = new alEFXReverbPreset(0.4287f, 0.5000f, 0.3162f, 1.0000f, 1.0000f, 8.3900f, 1.3900f, 1.0000f, 0.8760f, 0.0020f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 3.1081f, 0.0300f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.0000f, 0.2500f, 1.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x0);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_DIZZY = new alEFXReverbPreset(0.3645f, 0.6000f, 0.3162f, 0.6310f, 1.0000f, 17.2300f, 0.5600f, 1.0000f, 0.1392f, 0.0200f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.4937f, 0.0300f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 1.0000f, 0.8100f, 0.3100f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x0);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_PSYCHOTIC = new alEFXReverbPreset(0.0625f, 0.5000f, 0.3162f, 0.8404f, 1.0000f, 7.5600f, 0.9100f, 1.0000f, 0.4864f, 0.0200f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 2.4378f, 0.0300f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.0000f, 4.0000f, 1.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x0);

		// Castle Presets
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_CASTLE_SMALLROOM = new alEFXReverbPreset(1.0000f, 0.8900f, 0.3162f, 0.3981f, 0.1000f, 1.2200f, 0.8300f, 0.3100f, 0.8913f, 0.0220f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 1.9953f, 0.0110f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.1380f, 0.0800f, 0.2500f, 0.0000f, 0.9943f, 5168.6001f, 139.5000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_CASTLE_SHORTPASSAGE = new alEFXReverbPreset(1.0000f, 0.8900f, 0.3162f, 0.3162f, 0.1000f, 2.3200f, 0.8300f, 0.3100f, 0.8913f, 0.0070f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 1.2589f, 0.0230f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.1380f, 0.0800f, 0.2500f, 0.0000f, 0.9943f, 5168.6001f, 139.5000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_CASTLE_MEDIUMROOM = new alEFXReverbPreset(1.0000f, 0.9300f, 0.3162f, 0.2818f, 0.1000f, 2.0400f, 0.8300f, 0.4600f, 0.6310f, 0.0220f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 1.5849f, 0.0110f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.1550f, 0.0300f, 0.2500f, 0.0000f, 0.9943f, 5168.6001f, 139.5000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_CASTLE_LARGEROOM = new alEFXReverbPreset(1.0000f, 0.8200f, 0.3162f, 0.2818f, 0.1259f, 2.5300f, 0.8300f, 0.5000f, 0.4467f, 0.0340f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 1.2589f, 0.0160f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.1850f, 0.0700f, 0.2500f, 0.0000f, 0.9943f, 5168.6001f, 139.5000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_CASTLE_LONGPASSAGE = new alEFXReverbPreset(1.0000f, 0.8900f, 0.3162f, 0.3981f, 0.1000f, 3.4200f, 0.8300f, 0.3100f, 0.8913f, 0.0070f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 1.4125f, 0.0230f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.1380f, 0.0800f, 0.2500f, 0.0000f, 0.9943f, 5168.6001f, 139.5000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_CASTLE_HALL = new alEFXReverbPreset(1.0000f, 0.8100f, 0.3162f, 0.2818f, 0.1778f, 3.1400f, 0.7900f, 0.6200f, 0.1778f, 0.0560f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 1.1220f, 0.0240f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5168.6001f, 139.5000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_CASTLE_CUPBOARD = new alEFXReverbPreset(1.0000f, 0.8900f, 0.3162f, 0.2818f, 0.1000f, 0.6700f, 0.8700f, 0.3100f, 1.4125f, 0.0100f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 3.5481f, 0.0070f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.1380f, 0.0800f, 0.2500f, 0.0000f, 0.9943f, 5168.6001f, 139.5000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_CASTLE_COURTYARD = new alEFXReverbPreset(1.0000f, 0.4200f, 0.3162f, 0.4467f, 0.1995f, 2.1300f, 0.6100f, 0.2300f, 0.2239f, 0.1600f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.7079f, 0.0360f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.3700f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x0);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_CASTLE_ALCOVE = new alEFXReverbPreset(1.0000f, 0.8900f, 0.3162f, 0.5012f, 0.1000f, 1.6400f, 0.8700f, 0.3100f, 1.0000f, 0.0070f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 1.4125f, 0.0340f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.1380f, 0.0800f, 0.2500f, 0.0000f, 0.9943f, 5168.6001f, 139.5000f, 0.0000f, 0x1);

		// Factory Presets
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_FACTORY_SMALLROOM = new alEFXReverbPreset(0.3645f, 0.8200f, 0.3162f, 0.7943f, 0.5012f, 1.7200f, 0.6500f, 1.3100f, 0.7079f, 0.0100f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 1.7783f, 0.0240f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.1190f, 0.0700f, 0.2500f, 0.0000f, 0.9943f, 3762.6001f, 362.5000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_FACTORY_SHORTPASSAGE = new alEFXReverbPreset(0.3645f, 0.6400f, 0.2512f, 0.7943f, 0.5012f, 2.5300f, 0.6500f, 1.3100f, 1.0000f, 0.0100f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 1.2589f, 0.0380f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.1350f, 0.2300f, 0.2500f, 0.0000f, 0.9943f, 3762.6001f, 362.5000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_FACTORY_MEDIUMROOM = new alEFXReverbPreset(0.4287f, 0.8200f, 0.2512f, 0.7943f, 0.5012f, 2.7600f, 0.6500f, 1.3100f, 0.2818f, 0.0220f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 1.4125f, 0.0230f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.1740f, 0.0700f, 0.2500f, 0.0000f, 0.9943f, 3762.6001f, 362.5000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_FACTORY_LARGEROOM = new alEFXReverbPreset(0.4287f, 0.7500f, 0.2512f, 0.7079f, 0.6310f, 4.2400f, 0.5100f, 1.3100f, 0.1778f, 0.0390f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 1.1220f, 0.0230f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2310f, 0.0700f, 0.2500f, 0.0000f, 0.9943f, 3762.6001f, 362.5000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_FACTORY_LONGPASSAGE = new alEFXReverbPreset(0.3645f, 0.6400f, 0.2512f, 0.7943f, 0.5012f, 4.0600f, 0.6500f, 1.3100f, 1.0000f, 0.0200f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 1.2589f, 0.0370f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.1350f, 0.2300f, 0.2500f, 0.0000f, 0.9943f, 3762.6001f, 362.5000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_FACTORY_HALL = new alEFXReverbPreset(0.4287f, 0.7500f, 0.3162f, 0.7079f, 0.6310f, 7.4300f, 0.5100f, 1.3100f, 0.0631f, 0.0730f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.8913f, 0.0270f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.0700f, 0.2500f, 0.0000f, 0.9943f, 3762.6001f, 362.5000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_FACTORY_CUPBOARD = new alEFXReverbPreset(0.3071f, 0.6300f, 0.2512f, 0.7943f, 0.5012f, 0.4900f, 0.6500f, 1.3100f, 1.2589f, 0.0100f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 1.9953f, 0.0320f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.1070f, 0.0700f, 0.2500f, 0.0000f, 0.9943f, 3762.6001f, 362.5000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_FACTORY_COURTYARD = new alEFXReverbPreset(0.3071f, 0.5700f, 0.3162f, 0.3162f, 0.6310f, 2.3200f, 0.2900f, 0.5600f, 0.2239f, 0.1400f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.3981f, 0.0390f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.2900f, 0.2500f, 0.0000f, 0.9943f, 3762.6001f, 362.5000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_FACTORY_ALCOVE = new alEFXReverbPreset(0.3645f, 0.5900f, 0.2512f, 0.7943f, 0.5012f, 3.1400f, 0.6500f, 1.3100f, 1.4125f, 0.0100f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 1.0000f, 0.0380f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.1140f, 0.1000f, 0.2500f, 0.0000f, 0.9943f, 3762.6001f, 362.5000f, 0.0000f, 0x1);

		// Ice Palace Presets
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_ICEPALACE_SMALLROOM = new alEFXReverbPreset(1.0000f, 0.8400f, 0.3162f, 0.5623f, 0.2818f, 1.5100f, 1.5300f, 0.2700f, 0.8913f, 0.0100f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 1.4125f, 0.0110f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.1640f, 0.1400f, 0.2500f, 0.0000f, 0.9943f, 12428.5000f, 99.6000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_ICEPALACE_SHORTPASSAGE = new alEFXReverbPreset(1.0000f, 0.7500f, 0.3162f, 0.5623f, 0.2818f, 1.7900f, 1.4600f, 0.2800f, 0.5012f, 0.0100f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 1.1220f, 0.0190f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.1770f, 0.0900f, 0.2500f, 0.0000f, 0.9943f, 12428.5000f, 99.6000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_ICEPALACE_MEDIUMROOM = new alEFXReverbPreset(1.0000f, 0.8700f, 0.3162f, 0.5623f, 0.4467f, 2.2200f, 1.5300f, 0.3200f, 0.3981f, 0.0390f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 1.1220f, 0.0270f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.1860f, 0.1200f, 0.2500f, 0.0000f, 0.9943f, 12428.5000f, 99.6000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_ICEPALACE_LARGEROOM = new alEFXReverbPreset(1.0000f, 0.8100f, 0.3162f, 0.5623f, 0.4467f, 3.1400f, 1.5300f, 0.3200f, 0.2512f, 0.0390f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 1.0000f, 0.0270f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2140f, 0.1100f, 0.2500f, 0.0000f, 0.9943f, 12428.5000f, 99.6000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_ICEPALACE_LONGPASSAGE = new alEFXReverbPreset(1.0000f, 0.7700f, 0.3162f, 0.5623f, 0.3981f, 3.0100f, 1.4600f, 0.2800f, 0.7943f, 0.0120f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 1.2589f, 0.0250f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.1860f, 0.0400f, 0.2500f, 0.0000f, 0.9943f, 12428.5000f, 99.6000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_ICEPALACE_HALL = new alEFXReverbPreset(1.0000f, 0.7600f, 0.3162f, 0.4467f, 0.5623f, 5.4900f, 1.5300f, 0.3800f, 0.1122f, 0.0540f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.6310f, 0.0520f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2260f, 0.1100f, 0.2500f, 0.0000f, 0.9943f, 12428.5000f, 99.6000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_ICEPALACE_CUPBOARD = new alEFXReverbPreset(1.0000f, 0.8300f, 0.3162f, 0.5012f, 0.2239f, 0.7600f, 1.5300f, 0.2600f, 1.1220f, 0.0120f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 1.9953f, 0.0160f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.1430f, 0.0800f, 0.2500f, 0.0000f, 0.9943f, 12428.5000f, 99.6000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_ICEPALACE_COURTYARD = new alEFXReverbPreset(1.0000f, 0.5900f, 0.3162f, 0.2818f, 0.3162f, 2.0400f, 1.2000f, 0.3800f, 0.3162f, 0.1730f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.3162f, 0.0430f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2350f, 0.4800f, 0.2500f, 0.0000f, 0.9943f, 12428.5000f, 99.6000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_ICEPALACE_ALCOVE = new alEFXReverbPreset(1.0000f, 0.8400f, 0.3162f, 0.5623f, 0.2818f, 2.7600f, 1.4600f, 0.2800f, 1.1220f, 0.0100f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.8913f, 0.0300f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.1610f, 0.0900f, 0.2500f, 0.0000f, 0.9943f, 12428.5000f, 99.6000f, 0.0000f, 0x1);

		// Space Station Presets
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_SPACESTATION_SMALLROOM = new alEFXReverbPreset(0.2109f, 0.7000f, 0.3162f, 0.7079f, 0.8913f, 1.7200f, 0.8200f, 0.5500f, 0.7943f, 0.0070f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 1.4125f, 0.0130f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.1880f, 0.2600f, 0.2500f, 0.0000f, 0.9943f, 3316.1001f, 458.2000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_SPACESTATION_SHORTPASSAGE = new alEFXReverbPreset(0.2109f, 0.8700f, 0.3162f, 0.6310f, 0.8913f, 3.5700f, 0.5000f, 0.5500f, 1.0000f, 0.0120f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 1.1220f, 0.0160f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.1720f, 0.2000f, 0.2500f, 0.0000f, 0.9943f, 3316.1001f, 458.2000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_SPACESTATION_MEDIUMROOM = new alEFXReverbPreset(0.2109f, 0.7500f, 0.3162f, 0.6310f, 0.8913f, 3.0100f, 0.5000f, 0.5500f, 0.3981f, 0.0340f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 1.1220f, 0.0350f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2090f, 0.3100f, 0.2500f, 0.0000f, 0.9943f, 3316.1001f, 458.2000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_SPACESTATION_LARGEROOM = new alEFXReverbPreset(0.3645f, 0.8100f, 0.3162f, 0.6310f, 0.8913f, 3.8900f, 0.3800f, 0.6100f, 0.3162f, 0.0560f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.8913f, 0.0350f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2330f, 0.2800f, 0.2500f, 0.0000f, 0.9943f, 3316.1001f, 458.2000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_SPACESTATION_LONGPASSAGE = new alEFXReverbPreset(0.4287f, 0.8200f, 0.3162f, 0.6310f, 0.8913f, 4.6200f, 0.6200f, 0.5500f, 1.0000f, 0.0120f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 1.2589f, 0.0310f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.2300f, 0.2500f, 0.0000f, 0.9943f, 3316.1001f, 458.2000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_SPACESTATION_HALL = new alEFXReverbPreset(0.4287f, 0.8700f, 0.3162f, 0.6310f, 0.8913f, 7.1100f, 0.3800f, 0.6100f, 0.1778f, 0.1000f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.6310f, 0.0470f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.2500f, 0.2500f, 0.0000f, 0.9943f, 3316.1001f, 458.2000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_SPACESTATION_CUPBOARD = new alEFXReverbPreset(0.1715f, 0.5600f, 0.3162f, 0.7079f, 0.8913f, 0.7900f, 0.8100f, 0.5500f, 1.4125f, 0.0070f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 1.7783f, 0.0180f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.1810f, 0.3100f, 0.2500f, 0.0000f, 0.9943f, 3316.1001f, 458.2000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_SPACESTATION_ALCOVE = new alEFXReverbPreset(0.2109f, 0.7800f, 0.3162f, 0.7079f, 0.8913f, 1.1600f, 0.8100f, 0.5500f, 1.4125f, 0.0070f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 1.0000f, 0.0180f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.1920f, 0.2100f, 0.2500f, 0.0000f, 0.9943f, 3316.1001f, 458.2000f, 0.0000f, 0x1);

		// Wooden Galleon Presets
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_WOODEN_SMALLROOM = new alEFXReverbPreset(1.0000f, 1.0000f, 0.3162f, 0.1122f, 0.3162f, 0.7900f, 0.3200f, 0.8700f, 1.0000f, 0.0320f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.8913f, 0.0290f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 4705.0000f, 99.6000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_WOODEN_SHORTPASSAGE = new alEFXReverbPreset(1.0000f, 1.0000f, 0.3162f, 0.1259f, 0.3162f, 1.7500f, 0.5000f, 0.8700f, 0.8913f, 0.0120f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.6310f, 0.0240f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 4705.0000f, 99.6000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_WOODEN_MEDIUMROOM = new alEFXReverbPreset(1.0000f, 1.0000f, 0.3162f, 0.1000f, 0.2818f, 1.4700f, 0.4200f, 0.8200f, 0.8913f, 0.0490f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.8913f, 0.0290f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 4705.0000f, 99.6000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_WOODEN_LARGEROOM = new alEFXReverbPreset(1.0000f, 1.0000f, 0.3162f, 0.0891f, 0.2818f, 2.6500f, 0.3300f, 0.8200f, 0.8913f, 0.0660f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.7943f, 0.0490f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 4705.0000f, 99.6000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_WOODEN_LONGPASSAGE = new alEFXReverbPreset(1.0000f, 1.0000f, 0.3162f, 0.1000f, 0.3162f, 1.9900f, 0.4000f, 0.7900f, 1.0000f, 0.0200f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.4467f, 0.0360f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 4705.0000f, 99.6000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_WOODEN_HALL = new alEFXReverbPreset(1.0000f, 1.0000f, 0.3162f, 0.0794f, 0.2818f, 3.4500f, 0.3000f, 0.8200f, 0.8913f, 0.0880f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.7943f, 0.0630f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 4705.0000f, 99.6000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_WOODEN_CUPBOARD = new alEFXReverbPreset(1.0000f, 1.0000f, 0.3162f, 0.1413f, 0.3162f, 0.5600f, 0.4600f, 0.9100f, 1.1220f, 0.0120f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 1.1220f, 0.0280f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 4705.0000f, 99.6000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_WOODEN_COURTYARD = new alEFXReverbPreset(1.0000f, 0.6500f, 0.3162f, 0.0794f, 0.3162f, 1.7900f, 0.3500f, 0.7900f, 0.5623f, 0.1230f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.1000f, 0.0320f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 4705.0000f, 99.6000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_WOODEN_ALCOVE = new alEFXReverbPreset(1.0000f, 1.0000f, 0.3162f, 0.1259f, 0.3162f, 1.2200f, 0.6200f, 0.9100f, 1.1220f, 0.0120f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.7079f, 0.0240f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 4705.0000f, 99.6000f, 0.0000f, 0x1);

		// Sports Presets
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_SPORT_EMPTYSTADIUM = new alEFXReverbPreset(1.0000f, 1.0000f, 0.3162f, 0.4467f, 0.7943f, 6.2600f, 0.5100f, 1.1000f, 0.0631f, 0.1830f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.3981f, 0.0380f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_SPORT_SQUASHCOURT = new alEFXReverbPreset(1.0000f, 0.7500f, 0.3162f, 0.3162f, 0.7943f, 2.2200f, 0.9100f, 1.1600f, 0.4467f, 0.0070f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.7943f, 0.0110f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.1260f, 0.1900f, 0.2500f, 0.0000f, 0.9943f, 7176.8999f, 211.2000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_SPORT_SMALLSWIMMINGPOOL = new alEFXReverbPreset(1.0000f, 0.7000f, 0.3162f, 0.7943f, 0.8913f, 2.7600f, 1.2500f, 1.1400f, 0.6310f, 0.0200f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.7943f, 0.0300f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.1790f, 0.1500f, 0.8950f, 0.1900f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x0);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_SPORT_LARGESWIMMINGPOOL = new alEFXReverbPreset(1.0000f, 0.8200f, 0.3162f, 0.7943f, 1.0000f, 5.4900f, 1.3100f, 1.1400f, 0.4467f, 0.0390f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.5012f, 0.0490f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2220f, 0.5500f, 1.1590f, 0.2100f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x0);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_SPORT_GYMNASIUM = new alEFXReverbPreset(1.0000f, 0.8100f, 0.3162f, 0.4467f, 0.8913f, 3.1400f, 1.0600f, 1.3500f, 0.3981f, 0.0290f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.5623f, 0.0450f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.1460f, 0.1400f, 0.2500f, 0.0000f, 0.9943f, 7176.8999f, 211.2000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_SPORT_FULLSTADIUM = new alEFXReverbPreset(1.0000f, 1.0000f, 0.3162f, 0.0708f, 0.7943f, 5.2500f, 0.1700f, 0.8000f, 0.1000f, 0.1880f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2818f, 0.0380f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_SPORT_STADIUMTANNOY = new alEFXReverbPreset(1.0000f, 0.7800f, 0.3162f, 0.5623f, 0.5012f, 2.5300f, 0.8800f, 0.6800f, 0.2818f, 0.2300f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.5012f, 0.0630f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.2000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x1);

		// Prefab Presets
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_PREFAB_WORKSHOP = new alEFXReverbPreset(0.4287f, 1.0000f, 0.3162f, 0.1413f, 0.3981f, 0.7600f, 1.0000f, 1.0000f, 1.0000f, 0.0120f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 1.1220f, 0.0120f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x0);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_PREFAB_SCHOOLROOM = new alEFXReverbPreset(0.4022f, 0.6900f, 0.3162f, 0.6310f, 0.5012f, 0.9800f, 0.4500f, 0.1800f, 1.4125f, 0.0170f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 1.4125f, 0.0150f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.0950f, 0.1400f, 0.2500f, 0.0000f, 0.9943f, 7176.8999f, 211.2000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_PREFAB_PRACTISEROOM = new alEFXReverbPreset(0.4022f, 0.8700f, 0.3162f, 0.3981f, 0.5012f, 1.1200f, 0.5600f, 0.1800f, 1.2589f, 0.0100f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 1.4125f, 0.0110f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.0950f, 0.1400f, 0.2500f, 0.0000f, 0.9943f, 7176.8999f, 211.2000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_PREFAB_OUTHOUSE = new alEFXReverbPreset(1.0000f, 0.8200f, 0.3162f, 0.1122f, 0.1585f, 1.3800f, 0.3800f, 0.3500f, 0.8913f, 0.0240f, new float[] { 0.0000f, 0.0000f, -0.0000f }, 0.6310f, 0.0440f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.1210f, 0.1700f, 0.2500f, 0.0000f, 0.9943f, 2854.3999f, 107.5000f, 0.0000f, 0x0);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_PREFAB_CARAVAN = new alEFXReverbPreset(1.0000f, 1.0000f, 0.3162f, 0.0891f, 0.1259f, 0.4300f, 1.5000f, 1.0000f, 1.0000f, 0.0120f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 1.9953f, 0.0120f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x0);

		// Dome and Pipe Presets
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_DOME_TOMB = new alEFXReverbPreset(1.0000f, 0.7900f, 0.3162f, 0.3548f, 0.2239f, 4.1800f, 0.2100f, 0.1000f, 0.3868f, 0.0300f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 1.6788f, 0.0220f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.1770f, 0.1900f, 0.2500f, 0.0000f, 0.9943f, 2854.3999f, 20.0000f, 0.0000f, 0x0);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_PIPE_SMALL = new alEFXReverbPreset(1.0000f, 1.0000f, 0.3162f, 0.3548f, 0.2239f, 5.0400f, 0.1000f, 0.1000f, 0.5012f, 0.0320f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 2.5119f, 0.0150f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 2854.3999f, 20.0000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_DOME_SAINTPAULS = new alEFXReverbPreset(1.0000f, 0.8700f, 0.3162f, 0.3548f, 0.2239f, 10.4800f, 0.1900f, 0.1000f, 0.1778f, 0.0900f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 1.2589f, 0.0420f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.1200f, 0.2500f, 0.0000f, 0.9943f, 2854.3999f, 20.0000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_PIPE_LONGTHIN = new alEFXReverbPreset(0.2560f, 0.9100f, 0.3162f, 0.4467f, 0.2818f, 9.2100f, 0.1800f, 0.1000f, 0.7079f, 0.0100f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.7079f, 0.0220f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 2854.3999f, 20.0000f, 0.0000f, 0x0);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_PIPE_LARGE = new alEFXReverbPreset(1.0000f, 1.0000f, 0.3162f, 0.3548f, 0.2239f, 8.4500f, 0.1000f, 0.1000f, 0.3981f, 0.0460f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 1.5849f, 0.0320f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 2854.3999f, 20.0000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_PIPE_RESONANT = new alEFXReverbPreset(0.1373f, 0.9100f, 0.3162f, 0.4467f, 0.2818f, 6.8100f, 0.1800f, 0.1000f, 0.7079f, 0.0100f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 1.0000f, 0.0220f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 2854.3999f, 20.0000f, 0.0000f, 0x0);

		// Outdoors Presets
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_OUTDOORS_BACKYARD = new alEFXReverbPreset(1.0000f, 0.4500f, 0.3162f, 0.2512f, 0.5012f, 1.1200f, 0.3400f, 0.4600f, 0.4467f, 0.0690f, new float[] { 0.0000f, 0.0000f, -0.0000f }, 0.7079f, 0.0230f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2180f, 0.3400f, 0.2500f, 0.0000f, 0.9943f, 4399.1001f, 242.9000f, 0.0000f, 0x0);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_OUTDOORS_ROLLINGPLAINS = new alEFXReverbPreset(1.0000f, 0.0000f, 0.3162f, 0.0112f, 0.6310f, 2.1300f, 0.2100f, 0.4600f, 0.1778f, 0.3000f, new float[] { 0.0000f, 0.0000f, -0.0000f }, 0.4467f, 0.0190f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 1.0000f, 0.2500f, 0.0000f, 0.9943f, 4399.1001f, 242.9000f, 0.0000f, 0x0);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_OUTDOORS_DEEPCANYON = new alEFXReverbPreset(1.0000f, 0.7400f, 0.3162f, 0.1778f, 0.6310f, 3.8900f, 0.2100f, 0.4600f, 0.3162f, 0.2230f, new float[] { 0.0000f, 0.0000f, -0.0000f }, 0.3548f, 0.0190f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 1.0000f, 0.2500f, 0.0000f, 0.9943f, 4399.1001f, 242.9000f, 0.0000f, 0x0);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_OUTDOORS_CREEK = new alEFXReverbPreset(1.0000f, 0.3500f, 0.3162f, 0.1778f, 0.5012f, 2.1300f, 0.2100f, 0.4600f, 0.3981f, 0.1150f, new float[] { 0.0000f, 0.0000f, -0.0000f }, 0.1995f, 0.0310f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2180f, 0.3400f, 0.2500f, 0.0000f, 0.9943f, 4399.1001f, 242.9000f, 0.0000f, 0x0);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_OUTDOORS_VALLEY = new alEFXReverbPreset(1.0000f, 0.2800f, 0.3162f, 0.0282f, 0.1585f, 2.8800f, 0.2600f, 0.3500f, 0.1413f, 0.2630f, new float[] { 0.0000f, 0.0000f, -0.0000f }, 0.3981f, 0.1000f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.3400f, 0.2500f, 0.0000f, 0.9943f, 2854.3999f, 107.5000f, 0.0000f, 0x0);

		// Mood Presets
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_MOOD_HEAVEN = new alEFXReverbPreset(1.0000f, 0.9400f, 0.3162f, 0.7943f, 0.4467f, 5.0400f, 1.1200f, 0.5600f, 0.2427f, 0.0200f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 1.2589f, 0.0290f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.0800f, 2.7420f, 0.0500f, 0.9977f, 5000.0000f, 250.0000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_MOOD_HELL = new alEFXReverbPreset(1.0000f, 0.5700f, 0.3162f, 0.3548f, 0.4467f, 3.5700f, 0.4900f, 2.0000f, 0.0000f, 0.0200f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 1.4125f, 0.0300f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.1100f, 0.0400f, 2.1090f, 0.5200f, 0.9943f, 5000.0000f, 139.5000f, 0.0000f, 0x0);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_MOOD_MEMORY = new alEFXReverbPreset(1.0000f, 0.8500f, 0.3162f, 0.6310f, 0.3548f, 4.0600f, 0.8200f, 0.5600f, 0.0398f, 0.0000f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 1.1220f, 0.0000f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.0000f, 0.4740f, 0.4500f, 0.9886f, 5000.0000f, 250.0000f, 0.0000f, 0x0);

		// Driving Presets
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_DRIVING_COMMENTATOR = new alEFXReverbPreset(1.0000f, 0.0000f, 0.3162f, 0.5623f, 0.5012f, 2.4200f, 0.8800f, 0.6800f, 0.1995f, 0.0930f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2512f, 0.0170f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 1.0000f, 0.2500f, 0.0000f, 0.9886f, 5000.0000f, 250.0000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_DRIVING_PITGARAGE = new alEFXReverbPreset(0.4287f, 0.5900f, 0.3162f, 0.7079f, 0.5623f, 1.7200f, 0.9300f, 0.8700f, 0.5623f, 0.0000f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 1.2589f, 0.0160f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.1100f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x0);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_DRIVING_INCAR_RACER = new alEFXReverbPreset(0.0832f, 0.8000f, 0.3162f, 1.0000f, 0.7943f, 0.1700f, 2.0000f, 0.4100f, 1.7783f, 0.0070f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.7079f, 0.0150f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 10268.2002f, 251.0000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_DRIVING_INCAR_SPORTS = new alEFXReverbPreset(0.0832f, 0.8000f, 0.3162f, 0.6310f, 1.0000f, 0.1700f, 0.7500f, 0.4100f, 1.0000f, 0.0100f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.5623f, 0.0000f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 10268.2002f, 251.0000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_DRIVING_INCAR_LUXURY = new alEFXReverbPreset(0.2560f, 1.0000f, 0.3162f, 0.1000f, 0.5012f, 0.1300f, 0.4100f, 0.4600f, 0.7943f, 0.0100f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 1.5849f, 0.0100f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 10268.2002f, 251.0000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_DRIVING_FULLGRANDSTAND = new alEFXReverbPreset(1.0000f, 1.0000f, 0.3162f, 0.2818f, 0.6310f, 3.0100f, 1.3700f, 1.2800f, 0.3548f, 0.0900f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.1778f, 0.0490f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 10420.2002f, 250.0000f, 0.0000f, 0x0);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_DRIVING_EMPTYGRANDSTAND = new alEFXReverbPreset(1.0000f, 1.0000f, 0.3162f, 1.0000f, 0.7943f, 4.6200f, 1.7500f, 1.4000f, 0.2082f, 0.0900f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2512f, 0.0490f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 10420.2002f, 250.0000f, 0.0000f, 0x0);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_DRIVING_TUNNEL = new alEFXReverbPreset(1.0000f, 0.8100f, 0.3162f, 0.3981f, 0.8913f, 3.4200f, 0.9400f, 1.3100f, 0.7079f, 0.0510f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.7079f, 0.0470f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2140f, 0.0500f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 155.3000f, 0.0000f, 0x1);

		// City Presets
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_CITY_STREETS = new alEFXReverbPreset(1.0000f, 0.7800f, 0.3162f, 0.7079f, 0.8913f, 1.7900f, 1.1200f, 0.9100f, 0.2818f, 0.0460f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.1995f, 0.0280f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.2000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_CITY_SUBWAY = new alEFXReverbPreset(1.0000f, 0.7400f, 0.3162f, 0.7079f, 0.8913f, 3.0100f, 1.2300f, 0.9100f, 0.7079f, 0.0460f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 1.2589f, 0.0280f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.1250f, 0.2100f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_CITY_MUSEUM = new alEFXReverbPreset(1.0000f, 0.8200f, 0.3162f, 0.1778f, 0.1778f, 3.2800f, 1.4000f, 0.5700f, 0.2512f, 0.0390f, new float[] { 0.0000f, 0.0000f, -0.0000f }, 0.8913f, 0.0340f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.1300f, 0.1700f, 0.2500f, 0.0000f, 0.9943f, 2854.3999f, 107.5000f, 0.0000f, 0x0);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_CITY_LIBRARY = new alEFXReverbPreset(1.0000f, 0.8200f, 0.3162f, 0.2818f, 0.0891f, 2.7600f, 0.8900f, 0.4100f, 0.3548f, 0.0290f, new float[] { 0.0000f, 0.0000f, -0.0000f }, 0.8913f, 0.0200f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.1300f, 0.1700f, 0.2500f, 0.0000f, 0.9943f, 2854.3999f, 107.5000f, 0.0000f, 0x0);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_CITY_UNDERPASS = new alEFXReverbPreset(1.0000f, 0.8200f, 0.3162f, 0.4467f, 0.8913f, 3.5700f, 1.1200f, 0.9100f, 0.3981f, 0.0590f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.8913f, 0.0370f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.1400f, 0.2500f, 0.0000f, 0.9920f, 5000.0000f, 250.0000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_CITY_ABANDONED = new alEFXReverbPreset(1.0000f, 0.6900f, 0.3162f, 0.7943f, 0.8913f, 3.2800f, 1.1700f, 0.9100f, 0.4467f, 0.0440f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2818f, 0.0240f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.2000f, 0.2500f, 0.0000f, 0.9966f, 5000.0000f, 250.0000f, 0.0000f, 0x1);

		// Misc. Presets
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_DUSTYROOM = new alEFXReverbPreset(0.3645f, 0.5600f, 0.3162f, 0.7943f, 0.7079f, 1.7900f, 0.3800f, 0.2100f, 0.5012f, 0.0020f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 1.2589f, 0.0060f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2020f, 0.0500f, 0.2500f, 0.0000f, 0.9886f, 13046.0000f, 163.3000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_CHAPEL = new alEFXReverbPreset(1.0000f, 0.8400f, 0.3162f, 0.5623f, 1.0000f, 4.6200f, 0.6400f, 1.2300f, 0.4467f, 0.0320f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.7943f, 0.0490f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.2500f, 0.0000f, 0.2500f, 0.1100f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, 0x1);
		public static readonly alEFXReverbPreset EFX_REVERB_PRESET_SMALLWATERROOM = new alEFXReverbPreset(1.0000f, 0.7000f, 0.3162f, 0.4477f, 1.0000f, 1.5100f, 1.2500f, 1.1400f, 0.8913f, 0.0200f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 1.4125f, 0.0300f, new float[] { 0.0000f, 0.0000f, 0.0000f }, 0.1790f, 0.1500f, 0.8950f, 0.1900f, 0.9920f, 5000.0000f, 250.0000f, 0.0000f, 0x0);
		#endregion

		#region Functions
		// Effect Functions
		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alGenEffects(int n, uint[] effects);

		[DllImport(DLL, EntryPoint = "alGenEffects", CallingConvention = CallingConvention.Cdecl)]
		private static extern void INTERNAL_alGenEffects(int n, out uint effects);
		public static uint alGenEffect()
		{
			INTERNAL_alGenEffects(1, out uint effect);
			return effect;
		}

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alDeleteEffects(int n, uint[] effects);

		[DllImport(DLL, EntryPoint = "alDeleteEffects", CallingConvention = CallingConvention.Cdecl)]
		private static extern void INTERNAL_alDeleteEffects(int n, ref uint effects);
		public static void alDeleteEffect(uint effect) => INTERNAL_alDeleteEffects(1, ref effect);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool alIsEffect(uint effect);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alEffecti(uint effect, int param, int iValue);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alEffectiv(uint effect, int param, int[] piValues);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alEffectf(uint effect, int param, float flValue);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alEffectfv(uint effect, int param, float[] pflValues);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alGetEffecti(uint effect, int param, out int piValue);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alGetEffectiv(uint effect, int param, int[] piValues);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alGetEffectf(uint effect, int param, out float pflValue);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alGetEffectfv(uint effect, int param, float[] pflValues);

		// Filter Functions
		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alGenFilters(int n, uint[] filters);

		[DllImport(DLL, EntryPoint = "alGenFilters", CallingConvention = CallingConvention.Cdecl)]
		private static extern void INTERNAL_alGenFilters(int n, out uint filters);
		public static uint alGenFilter()
		{
			INTERNAL_alGenFilters(1, out uint filter);
			return filter;
		}

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alDeleteFilters(int n, uint[] filters);

		[DllImport(DLL, EntryPoint = "alDeleteFilters", CallingConvention = CallingConvention.Cdecl)]
		private static extern void INTERNAL_alDeleteFilters(int n, ref uint filters);
		public static void alDeleteFilter(uint filter) => INTERNAL_alDeleteFilters(1, ref filter);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool alIsFilter(uint filter);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alFilteri(uint filter, int param, int iValue);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alFilteriv(uint filter, int param, int[] piValues);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alFilterf(uint filter, int param, float flValue);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alFilterfv(uint filter, int param, float[] pflValues);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alGetFilteri(uint filter, int param, out int piValue);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alGetFilteriv(uint filter, int param, int[] piValues);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alGetFilterf(uint filter, int param, out float pflValue);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alGetFilterfv(uint filter, int param, float[] pflValues);

		// Auxiliary Effect Slot Functions
		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alGenAuxiliaryEffectSlots(int n, uint[] effectslots);

		[DllImport(DLL, EntryPoint = "alGenAuxiliaryEffectSlots", CallingConvention = CallingConvention.Cdecl)]
		private static extern void INTERNAL_alGenAuxiliaryEffectSlots(int n, out uint effectslots);
		public static uint alGenAuxiliaryEffectSlot()
		{
			INTERNAL_alGenAuxiliaryEffectSlots(1, out uint effectslot);
			return effectslot;
		}

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alDeleteAuxiliaryEffectSlots(int n, uint[] effectslots);

		[DllImport(DLL, EntryPoint = "alDeleteAuxiliaryEffectSlots", CallingConvention = CallingConvention.Cdecl)]
		private static extern void INTERNAL_alDeleteAuxiliaryEffectSlots(int n, ref uint effectslots);
		public static void alDeleteAuxiliaryEffectSlot(uint effectslot) => INTERNAL_alDeleteAuxiliaryEffectSlots(1, ref effectslot);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool alIsAuxiliaryEffectSlot(uint effectslot);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alAuxiliaryEffectSloti(uint effectslot, int param, int iValue);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alAuxiliaryEffectSlotiv(uint effectslot, int param, int[] piValues);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alAuxiliaryEffectSlotf(uint effectslot, int param, float flValue);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alAuxiliaryEffectSlotfv(uint effectslot, int param, float[] pflValues);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alGetAuxiliaryEffectSloti(uint effectslot, int param, out int piValue);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alGetAuxiliaryEffectSlotiv(uint effectslot, int param, int[] piValues);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alGetAuxiliaryEffectSlotf(uint effectslot, int param, out float pflValue);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alGetAuxiliaryEffectSlotfv(uint effectslot, int param, float[] pflValues);
		#endregion
		#endregion
	}
}