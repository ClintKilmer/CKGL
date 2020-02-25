﻿/*
 * C# bindings of OpenAL Soft: https://github.com/kcat/openal-soft
 * Specifically supplies bindings that will only work with the OpenAL Soft implementation of OpenAL.
 * 
 * Documentation: https://github.com/kcat/openal-soft/wiki/Programmer's-Guide
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
		#region Raw Enums
		//private const int ALC_FALSE = 0x0000; // Equivalent to bool false
		//private const int ALC_TRUE = 0x0001; // Equivalent to bool true

		private const int ALC_NO_ERROR = 0x0000;
		private const int ALC_INVALID_DEVICE = 0xA001;
		private const int ALC_INVALID_CONTEXT = 0xA002;
		private const int ALC_INVALID_ENUM = 0xA003;
		private const int ALC_INVALID_VALUE = 0xA004;
		private const int ALC_OUT_OF_MEMORY = 0xA005;

		private const int ALC_DEFAULT_DEVICE_SPECIFIER = 0x1004;
		private const int ALC_CAPTURE_DEFAULT_DEVICE_SPECIFIER = 0x0311;
		private const int ALC_DEVICE_SPECIFIER = 0x1005;
		private const int ALC_CAPTURE_DEVICE_SPECIFIER = 0x0310;
		private const int ALC_EXTENSIONS = 0x1006;
		private const int ALC_DEFAULT_ALL_DEVICES_SPECIFIER = 0x1012;
		private const int ALC_ALL_DEVICES_SPECIFIER = 0x1013;

		private const int ALC_MAJOR_VERSION = 0x1000;
		private const int ALC_MINOR_VERSION = 0x1001;
		private const int ALC_ATTRIBUTES_SIZE = 0x1002;
		private const int ALC_ALL_ATTRIBUTES = 0x1003;
		private const int ALC_CAPTURE_SAMPLES = 0x0312;

		private const int ALC_FREQUENCY = 0x1007;
		private const int ALC_REFRESH = 0x1008;
		private const int ALC_SYNC = 0x1009;
		private const int ALC_MONO_SOURCES = 0x1010;
		private const int ALC_STEREO_SOURCES = 0x1011;
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
			//Connected = ALC_CONNECTED // ALC_EXT_disconnect // TEMP
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

		#region DLL Imports
		// Context Error Functions
		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern alcErrorCode alcGetError(IntPtr device);

		// Context State Functions
		[DllImport(DLL, EntryPoint = "alGetString", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr INTERNAL_alcGetString(IntPtr device, alcString param);
		public static string alcGetString(IntPtr? device, alcString param)
		{
			return Marshal.PtrToStringAnsi(INTERNAL_alcGetString(device ?? IntPtr.Zero, param));
		}
		public static string[] alcGetStringArray(IntPtr? device, alcString param)
		{
			IntPtr location = INTERNAL_alcGetString(device ?? IntPtr.Zero, param);
			System.Collections.Generic.List<string> strings = new System.Collections.Generic.List<string>();

			bool lastNull = false;
			int i = -1;
			byte c;
			while (!((c = Marshal.ReadByte(location, ++i)) == '\0' && lastNull))
			{
				if (c == '\0')
				{
					lastNull = true;

					strings.Add(Marshal.PtrToStringAnsi(location, i));
					location = new IntPtr((long)location + i + 1);
					i = -1;
				}
				else
					lastNull = false;
			}

			return strings.ToArray();
		}

		[DllImport(DLL, EntryPoint = "alcGetIntegerv", CallingConvention = CallingConvention.Cdecl)]
		private static extern void INTERNAL_alcGetIntegerv(IntPtr device, alcInteger param, int size, int[] values);
		public static void alcGetIntegerv(IntPtr? device, alcInteger param, int size, int[] values) => INTERNAL_alcGetIntegerv(device ?? IntPtr.Zero, param, size, values);

		[DllImport(DLL, EntryPoint = "alcGetIntegerv", CallingConvention = CallingConvention.Cdecl)]
		private static extern void INTERNAL_alcGetIntegerv(IntPtr device, int param, int size, out int values); // TEMP
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

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool alcMakeContextCurrent(IntPtr context);

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
		public static extern bool alcIsExtensionPresent(IntPtr device, [In()][MarshalAs(UnmanagedType.LPStr)]string extName);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr alcGetProcAddress(IntPtr device, [In()][MarshalAs(UnmanagedType.LPStr)]string funcName);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern int alcGetEnumValue(IntPtr device, [In()][MarshalAs(UnmanagedType.LPStr)]string enumName);
		#endregion
		#endregion

		#region AL
		#region Raw Enums
		private const int AL_NONE = 0x0000;
		//private const int AL_FALSE = 0x0000; // Equivalent to bool false
		//private const int AL_TRUE = 0x0001; // Equivalent to bool true

		private const int AL_NO_ERROR = 0x0000;
		private const int AL_INVALID_NAME = 0xA001;
		private const int AL_INVALID_ENUM = 0xA002;
		private const int AL_INVALID_VALUE = 0xA003;
		private const int AL_INVALID_OPERATION = 0xA004;
		private const int AL_OUT_OF_MEMORY = 0xA005;

		private const int AL_VENDOR = 0xB001;
		private const int AL_VERSION = 0xB002;
		private const int AL_RENDERER = 0xB003;
		private const int AL_EXTENSIONS = 0xB004;

		private const int AL_DOPPLER_FACTOR = 0xC000;
		//private const int AL_DOPPLER_VELOCITY = 0xC001; // Deprecated
		private const int AL_SPEED_OF_SOUND = 0xC003;

		private const int AL_DISTANCE_MODEL = 0xD000;
		private const int AL_INVERSE_DISTANCE = 0xD001;
		private const int AL_INVERSE_DISTANCE_CLAMPED = 0xD002;
		private const int AL_LINEAR_DISTANCE = 0xD003;
		private const int AL_LINEAR_DISTANCE_CLAMPED = 0xD004;
		private const int AL_EXPONENT_DISTANCE = 0xD005;
		private const int AL_EXPONENT_DISTANCE_CLAMPED = 0xD006;

		//private const int AL_POSITION = 0x1004; // Listener enums are poached from Source enums below
		//private const int AL_VELOCITY = 0x1006;
		//private const int AL_ORIENTATION = 0x100F;
		//private const int AL_GAIN = 0x100A;

		private const int AL_FORMAT_MONO8 = 0x1100;
		private const int AL_FORMAT_MONO16 = 0x1101;
		private const int AL_FORMAT_STEREO8 = 0x1102;
		private const int AL_FORMAT_STEREO16 = 0x1103;

		private const int AL_FREQUENCY = 0x2001;
		private const int AL_BITS = 0x2002;
		private const int AL_CHANNELS = 0x2003;
		private const int AL_SIZE = 0x2004;

		private const int AL_PITCH = 0x1003;
		private const int AL_POSITION = 0x1004;
		private const int AL_DIRECTION = 0x1005;
		private const int AL_VELOCITY = 0x1006;
		private const int AL_LOOPING = 0x1007;
		private const int AL_BUFFER = 0x1009;
		private const int AL_GAIN = 0x100A;
		private const int AL_MIN_GAIN = 0x100D;
		private const int AL_MAX_GAIN = 0x100E;
		private const int AL_ORIENTATION = 0x100F;
		private const int AL_SOURCE_RELATIVE = 0x0202;
		private const int AL_CONE_INNER_ANGLE = 0x1001;
		private const int AL_CONE_OUTER_ANGLE = 0x1002;
		private const int AL_SOURCE_STATE = 0x1010;
		private const int AL_INITIAL = 0x1011;
		private const int AL_PLAYING = 0x1012;
		private const int AL_PAUSED = 0x1013;
		private const int AL_STOPPED = 0x1014;
		private const int AL_BUFFERS_QUEUED = 0x1015;
		private const int AL_BUFFERS_PROCESSED = 0x1016;
		private const int AL_REFERENCE_DISTANCE = 0x1020;
		private const int AL_ROLLOFF_FACTOR = 0x1021;
		private const int AL_CONE_OUTER_GAIN = 0x1022;
		private const int AL_MAX_DISTANCE = 0x1023;
		private const int AL_SEC_OFFSET = 0x1024;
		private const int AL_SAMPLE_OFFSET = 0x1025;
		private const int AL_BYTE_OFFSET = 0x1026;
		private const int AL_SOURCE_TYPE = 0x1027;
		private const int AL_STATIC = 0x1028;
		private const int AL_STREAMING = 0x1029;
		private const int AL_UNDETERMINED = 0x1030;
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
			DistanceModel = AL_DISTANCE_MODEL
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
			Stereo8 = AL_FORMAT_STEREO8,
			Stereo16 = AL_FORMAT_STEREO16
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

		#region DLL Imports
		// Error Functions
		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern alErrorCode alGetError();

		// State Functions
		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alEnable(alCapability capability);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alDisable(alCapability capability);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool alIsEnabled(alCapability capability);

		//[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		//public static extern bool alGetBoolean(int param);

		//[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		//public static extern void alGetBooleanv(int param, bool[] values);

		//[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		//public static extern double alGetDouble(int param);

		//[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		//public static extern void alGetDoublev(int param, double[] values);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern float alGetFloat(alStatefParameter param);

		//[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		//public static extern void alGetFloatv(int param, float[] values);

		//[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		//public static extern int alGetInteger(int param);

		//[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		//public static extern void alGetIntegerv(int param, int[] values);

		[DllImport(DLL, EntryPoint = "alGetString", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr INTERNAL_alGetString(alString param);
		public static string alGetString(alString param) => Marshal.PtrToStringAnsi(INTERNAL_alGetString(param));

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alDistanceModel(alDistanceModelType value);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alDopplerFactor(float value);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alSpeedOfSound(float value); // Default 343.3

		// Extension Functions
		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool alIsExtensionPresent([In()][MarshalAs(UnmanagedType.LPStr)]string extname);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr alGetProcAddress([In()][MarshalAs(UnmanagedType.LPStr)]string fname);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern int alGetEnumValue([In()][MarshalAs(UnmanagedType.LPStr)]string ename);

		// Listener Functions
		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alListenerf(alListenerfParameter param, float value);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alListener3f(alListener3fParameter param, float v1, float v2, float v3);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alListenerfv(alListenerfvParameter param, float[] values);

		//[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		//public static extern void alListeneri(int param, int value);

		//[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		//public static extern void alListener3i(int param, int v1, int v2, int v3);

		//[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		//public static extern void alListeneriv(int param, int[] values);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alGetListenerf(alListenerfParameter param, out float value);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alGetListener3f(alListener3fParameter param, out float v1, out float v2, out float v3);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alGetListenerfv(alListenerfvParameter param, float[] values);

		//[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		//public static extern void alGetListeneri(int param, out int value);

		//[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		//public static extern void alGetListener3i(int param, out int v1, out int v2, out int v3);

		//[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		//public static extern void alGetListeneriv(int param, int[] values);

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
		public static extern void alBufferData(uint buffer, alBufferFormat format, IntPtr data, int size, int freq);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alBufferData(uint buffer, alBufferFormat format, byte[] data, int size, int freq);

		//[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		//public static extern void alBufferf(uint buffer, int param, float value);

		//[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		//public static extern void alBuffer3f(uint buffer, int param, float v1, float v2, float v3);

		//[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		//public static extern void alBufferfv(uint buffer, int param, float[] values);

		//[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		//public static extern void alBufferi(uint buffer, int param, int value);

		//[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		//public static extern void alBuffer3i(uint buffer, int param, int v1, int v2, int v3);

		//[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		//public static extern void alBufferiv(uint buffer, int param, int[] values);

		//[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		//public static extern void alGetBufferf(uint buffer, int param, out float value);

		//[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		//public static extern void alGetBuffer3f(uint buffer, int param, out float v1, out float v2, out float v3);

		//[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		//public static extern void alGetBufferfv(uint buffer, int param, float[] values);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alGetBufferi(uint buffer, alGetBufferiParameter param, out int value);

		//[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		//public static extern void alGetBuffer3i(uint buffer, int param, out int v1, out int v2, out int v3);

		//[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		//public static extern void alGetBufferiv(uint buffer, int param, int[] values);

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
		public static extern void alSourcef(uint source, alSourcefParameter param, float value);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alSource3f(uint source, alSource3fParameter param, float v1, float v2, float v3);

		//[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		//public static extern void alSourcefv(uint source, int param, float[] values);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alSourcei(uint source, alSourceiParameter param, int value);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alSource3i(uint source, int param, int v1, int v2, int v3); // TEMP

		//[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		//public static extern void alSourceiv(uint source, int param, int[] values);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alGetSourcef(uint source, alSourcefParameter param, out float value);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alGetSource3f(uint source, alSource3fParameter param, out float v1, out float v2, out float v3);

		//[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		//public static extern void alGetSourcefv(uint source, int param, float[] values);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alGetSourcei(uint source, alGetSourceiParameter param, out int value);

		//[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		//public static extern void alGetSource3i(uint source, int param, out int v1, out int v2, out int v3);

		//[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		//public static extern void alGetSourceiv(uint source, int param, int[] values);

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
		public static extern void alSourceQueueBuffers(uint source, int n, uint[] buffers);

		[DllImport(DLL, EntryPoint = "alSourceQueueBuffers", CallingConvention = CallingConvention.Cdecl)]
		private static extern void INTERNAL_alSourceQueueBuffers(uint source, int n, ref uint buffers);
		public static void alSourceQueueBuffers(uint source, uint buffer) => INTERNAL_alSourceQueueBuffers(source, 1, ref buffer);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alSourceUnqueueBuffers(uint source, int n, uint[] buffers);

		[DllImport(DLL, EntryPoint = "alSourceUnqueueBuffers", CallingConvention = CallingConvention.Cdecl)]
		private static extern void INTERNAL_alSourceUnqueueBuffers(uint source, int n, ref uint buffers);
		public static void alSourceUnqueueBuffers(uint source, uint buffer) => INTERNAL_alSourceUnqueueBuffers(source, 1, ref buffer);
		#endregion
		#endregion

		#region ALEXT
		#region Raw Enums
		// ALC_EXT_disconnect
		public const int ALC_CONNECTED = 0x313; // TEMP
		#endregion

		#region Enums
		#endregion

		#region DLL Imports
		#endregion
		#endregion

		#region EFX
		#region Raw Enums
		public const int AL_METERS_PER_UNIT = 0x20004;

		public const int AL_DIRECT_FILTER = 0x20005;
		public const int AL_AUXILIARY_SEND_FILTER = 0x20006;
		public const int AL_AIR_ABSORPTION_FACTOR = 0x20007;
		public const int AL_ROOM_ROLLOFF_FACTOR = 0x20008;
		public const int AL_CONE_OUTER_GAINHF = 0x20009;
		public const int AL_DIRECT_FILTER_GAINHF_AUTO = 0x2000A;
		public const int AL_AUXILIARY_SEND_FILTER_GAIN_AUTO = 0x2000B;
		public const int AL_AUXILIARY_SEND_FILTER_GAINHF_AUTO = 0x2000C;

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
		public const int AL_REVERB_AIR_ABSORPTION_FACTOR = 0x000B;
		public const int AL_REVERB_ROOM_ROLLOFF_FACTOR = 0x000C;
		public const int AL_REVERB_DECAY_HFLIMIT = 0x000D;

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
		public const int AL_EAXREVERB_LATE_REVERT_PAN = 0x000E;
		public const int AL_EAXREVERB_ECHO_TIME = 0x000F;
		public const int AL_EAXREVERB_ECHO_DEPTH = 0x0010;
		public const int AL_EAXREVERB_MODULATION_TIME = 0x0011;
		public const int AL_EAXREVERB_MODULATION_DEPTH = 0x0012;
		public const int AL_EAXREVERB_AIR_ABSORPTION_GAINHF = 0x0013;
		public const int AL_EAXREVERB_HFREFERENCE = 0x0014;
		public const int AL_EAXREVERB_LFREFERENCE = 0x0015;
		public const int AL_EAXREVERB_ROOM_ROLLOFF_FACTOR = 0x0016;
		public const int AL_EAXREVERB_DECAY_HFLIMIT = 0x0017;

		public const int AL_CHORUS_WAVEFORM = 0x0001;
		public const int AL_CHORUS_PHASE = 0x0002;
		public const int AL_CHORUS_RATE = 0x0003;
		public const int AL_CHORUS_DEPTH = 0x0004;
		public const int AL_CHORUS_FEEDBACK = 0x0005;
		public const int AL_CHORUS_DELAY = 0x0006;

		public const int AL_DISTORTION_EDGE = 0x0001;
		public const int AL_DISTORTION_GAIN = 0x0002;
		public const int AL_DISTORTION_LOWPASS_CUTOFF = 0x0003;
		public const int AL_DISTORTION_EQCENTER = 0x0004;
		public const int AL_DISTORTION_EQBANDWIDTH = 0x0005;

		public const int AL_ECHO_DELAY = 0x0001;
		public const int AL_ECHO_LRDELAY = 0x0002;
		public const int AL_ECHO_DAMPING = 0x0003;
		public const int AL_ECHO_FEEDBACK = 0x0004;
		public const int AL_ECHO_SPREAD = 0x0005;

		public const int AL_FLANGER_WAVEFORM = 0x0001;
		public const int AL_FLANGER_PHASE = 0x0002;
		public const int AL_FLANGER_RATE = 0x0003;
		public const int AL_FLANGER_DEPTH = 0x0004;
		public const int AL_FLANGER_FEEDBACK = 0x0005;
		public const int AL_FLANGER_DELAY = 0x0006;

		public const int AL_FREQUENCY_SHIFTER_FREQUENCY = 0x0001;
		public const int AL_FREQUENCY_SHIFTER_LEFT_DIRECTION = 0x0002;
		public const int AL_FREQUENCY_SHIFTER_RIGHT_DIRECTION = 0x0003;

		public const int AL_VOCAL_MORPHER_PHONEMEA = 0x0001;
		public const int AL_VOCAL_MORPHER_PHONEMEA_COARSE_TUNING = 0x0002;
		public const int AL_VOCAL_MORPHER_PHONEMEB = 0x0003;
		public const int AL_VOCAL_MORPHER_PHONEMEB_COARSE_TUNING = 0x0004;
		public const int AL_VOCAL_MORPHER_WAVEFORM = 0x0005;
		public const int AL_VOCAL_MORPHER_RATE = 0x0006;

		public const int AL_PITCH_SHIFTER_COARSE_TUNE = 0x0001;
		public const int AL_PITCH_SHIFTER_FINE_TUNE = 0x0002;

		public const int AL_RING_MODULATOR_FREQUENCY = 0x0001;
		public const int AL_RING_MODULATOR_HIGHPASS_CUTOFF = 0x0002;
		public const int AL_RING_MODULATOR_WAVEFORM = 0x0003;

		public const int AL_AUTOWAH_ATTACK_TIME = 0x0001;
		public const int AL_AUTOWAH_RELEASE_TIME = 0x0002;
		public const int AL_AUTOWAH_RESONANCE = 0x0003;
		public const int AL_AUTOWAH_PEAK_GAIN = 0x0004;

		public const int AL_COMPRESSOR_ONOFF = 0x0001;

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

		public const int AL_EFFECT_FIRST_PARAMETER = 0x0000;
		public const int AL_EFFECT_LAST_PARAMETER = 0x8000;
		public const int AL_EFFECT_TYPE = 0x8001;

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

		public const int AL_EFFECTSLOT_EFFECT = 0x0001;
		public const int AL_EFFECTSLOT_GAIN = 0x0002;
		public const int AL_EFFECTSLOT_AUXILIARY_SEND_AUTO = 0x0003;

		public const int AL_EFFECTSLOT_NULL = 0x0000;

		public const int AL_LOWPASS_GAIN = 0x0001;
		public const int AL_LOWPASS_GAINHF = 0x0002;

		public const int AL_HIGHPASS_GAIN = 0x0001;
		public const int AL_HIGHPASS_GAINLF = 0x0002;

		public const int AL_BANDPASS_GAIN = 0x0001;
		public const int AL_BANDPASS_GAINLF = 0x0002;
		public const int AL_BANDPASS_GAINHF = 0x0003;

		public const int AL_FILTER_FIRST_PARAMETER = 0x0000;
		public const int AL_FILTER_LAST_PARAMETER = 0x8000;
		public const int AL_FILTER_TYPE = 0x8001;

		public const int AL_FILTER_NULL = 0x0000;
		public const int AL_FILTER_LOWPASS = 0x0001;
		public const int AL_FILTER_HIGHPASS = 0x0002;
		public const int AL_FILTER_BANDPASS = 0x0003;
		#endregion

		#region Enums
		#endregion

		#region Parameter ranges and defaults
		// Effect parameter ranges and defaults

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
		#endregion

		#region DLL Imports
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
		public static extern void alEffectiv(uint effect, int param, int[] iValues);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alEffectf(uint effect, int param, float flValue);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alEffectfv(uint effect, int param, float[] flValues);

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
		public static extern void alFilteriv(uint filter, int param, int[] iValues);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alFilterf(uint filter, int param, float flValue);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alFilterfv(uint filter, int param, float[] flValues);

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
		public static extern void alAuxiliaryEffectSlotiv(uint effectslot, int param, int[] iValues);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alAuxiliaryEffectSlotf(uint effectslot, int param, float flValue);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void alAuxiliaryEffectSlotfv(uint effectslot, int param, float[] flValues);

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