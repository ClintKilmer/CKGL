#nullable enable

using static OpenAL.Bindings;

namespace CKGL
{
	internal class AudioStream
	{
		private static readonly int bufferSize = 65536;

		private readonly AudioSource source;
		private readonly AudioCodec codec;
		private readonly alBufferFormat format;
		private readonly uint[] bufferIDs = new uint[3];

		internal AudioStream(AudioBuffer buffer, AudioSource source)
		{
			this.source = source;

			codec = new AudioCodec(buffer.File);
			format = Audio.GetalBufferFormat(codec.Channels, codec.BitDepth);

			alGenBuffers(bufferIDs.Length, bufferIDs);
			Audio.CheckALError("Could not create Buffers");

			fillInitialBuffers();

			Audio.Streams.Add(this);
		}

		private void fillInitialBuffers()
		{
			for (int i = 0; i < bufferIDs.Length; i++)
			{
				byte[] bytes = codec.GetData(bufferSize, source.Looping);

				if (bytes.Length == 0)
					return;

				alBufferData(bufferIDs[i], format, bytes, bytes.Length, codec.SampleRate);
				Audio.CheckALError("Could not set Buffer Data");

				alSourceQueueBuffers(source.ID, bufferIDs[i]);
				Audio.CheckALError("Could not queue Buffer");
			}
		}

		internal void Update()
		{
			while (source.BuffersProcessed > 1)
			{
				alSourceUnqueueBuffers(source.ID, out uint bufferID);
				Audio.CheckALError("Could not unqueue Buffer");

				byte[] bytes = codec.GetData(bufferSize, source.Looping);

				if (bytes.Length == 0)
					continue;

				alBufferData(bufferID, format, bytes, bytes.Length, codec.SampleRate);
				Audio.CheckALError("Could not set Buffer Data");

				alSourceQueueBuffers(source.ID, bufferID);
				Audio.CheckALError("Could not queue Buffer");
			}
		}

		internal void Destroy()
		{
			codec.Destroy();

			alDeleteBuffers(bufferIDs.Length, bufferIDs);
			Audio.CheckALError("Could not destroy Buffers");

			for (int i = 0; i < bufferIDs.Length; i++)
				bufferIDs[i] = default;

			Audio.Streams.Remove(this);
		}

		internal void Rewind()
		{
			UnqueueBuffers();
			codec.Rewind();
			fillInitialBuffers();
		}

		internal void SeekBytes(int offsetBytes)
		{
			UnqueueBuffers();
			codec.SeekBytes(offsetBytes);
			fillInitialBuffers();
		}

		internal void SeekSamples(int offsetSamples)
		{
			UnqueueBuffers();
			codec.SeekSamples(offsetSamples);
			fillInitialBuffers();
		}

		internal void SeekSeconds(float offsetSeconds)
		{
			UnqueueBuffers();
			codec.SeekSeconds(offsetSeconds);
			fillInitialBuffers();
		}

		internal void SeekPercent(float offsetPercent)
		{
			UnqueueBuffers();
			codec.SeekPercent(offsetPercent);
			fillInitialBuffers();
		}

		private void UnqueueBuffers()
		{
			// This is more robust than alSourceUnqueueBuffers
			alSourcei(source.ID, alSourceiParameter.Buffer, AL_NONE);
			Audio.CheckALError("Could not set AudioSource.Buffer in AudioStream.UnqueueBuffers()");
		}
	}
}