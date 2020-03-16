#nullable enable

using System;
using static OpenAL.Bindings;

namespace CKGL
{
	public class AudioCaptureDevice
	{
		public readonly string? DeviceName;
		public readonly int Channels;
		public readonly int BitDepth;
		public readonly int SampleRate;

		public bool Active => ptr != IntPtr.Zero;
		public bool IsRecording { get; private set; } = false;

		public int SamplesAvailable
		{
			get
			{
				if (!Active)
					return 0;
				alcGetIntegerv(ptr, ALC_CAPTURE_SAMPLES, out int samplesAvailable);
				Audio.CheckALCError(ptr, "Could not get CaptureDevice.SamplesAvailable");
				return samplesAvailable;
			}
		}

		public float SecondsAvailable => (float)SamplesAvailable / SampleRate;

		private IntPtr ptr;
		private readonly int bufferSizeInSamples;

		/// <param name="deviceName">Name of capture device or null for system default</param>
		/// <param name="length">Minimum length of recording time in seconds</param>
		public AudioCaptureDevice(string? deviceName, int channels, int bitDepth, int sampleRate, float length)
		{
			DeviceName = deviceName;
			Channels = channels;
			BitDepth = bitDepth;
			SampleRate = sampleRate;
			bufferSizeInSamples = (length * sampleRate * (sampleRate / 44100f)).CeilToInt(); // For some reason, changing sampleRate isn't calculated right, as it seems to always calculate with 44100

			ptr = alcCaptureOpenDevice(DeviceName, (uint)SampleRate, Audio.GetalBufferFormat(Channels, BitDepth), bufferSizeInSamples);

			if (Active)
			{
				Audio.CaptureDevices.Add(this);

				// Debug
				Output.WriteLine($"OpenAL Capture Device Initialized:\n    - \"{alcGetString(ptr, alcString.CaptureDeviceSpecifier)}\"");
			}
		}

		public void Destroy()
		{
			if (Active)
			{
				Stop();

				alcCaptureCloseDevice(ptr);
				Audio.CheckALError("Could not close CaptureDevice");
				ptr = IntPtr.Zero;

				Audio.CaptureDevices.Remove(this);
			}
		}

		public void Start()
		{
			if (Active)
			{
				alcCaptureStart(ptr);
				Audio.CheckALError("Could not start capture for CaptureDevice");

				IsRecording = true;
			}
		}

		public void Stop()
		{
			if (Active)
			{
				alcCaptureStop(ptr);
				Audio.CheckALError("Could not stop capture for CaptureDevice");

				IsRecording = false;
			}
		}

		public byte[] GetData()
		{
			if (!Active)
				return new byte[0];

			byte[] data = new byte[SamplesAvailable * Channels * (BitDepth / 8)];
			alcCaptureSamples(ptr, data, SamplesAvailable);
			Audio.CheckALCError(ptr, "Could not get data from CaptureDevice");
			return data;
		}
	}
}