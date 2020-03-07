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
		private uint[] bufferIDs = new uint[3];

		internal AudioStream(AudioBuffer buffer, AudioSource source)
		{
			this.source = source;

			codec = new AudioCodec(buffer.File);
			format = Audio.GetalBufferFormat(codec.Channels, codec.BitDepth);

			alGenBuffers(bufferIDs.Length, bufferIDs);
			Audio.CheckALError("Could not create Buffers");

			for (int i = 0; i < bufferIDs.Length; i++)
			{
				if (codec.IsFinished && source.Looping)
					codec.Seek(0);

				if (!codec.IsFinished)
				{
					byte[] bytes = codec.GetData(bufferSize);
					alBufferData(bufferIDs[i], format, bytes, bytes.Length, codec.SampleRate);
					Audio.CheckALError("Could not set Buffer Data");

					alSourceQueueBuffers(source.ID, bufferIDs[i]);
					Audio.CheckALError("Could not queue Buffer");
				}
			}

			Audio.Streams.Add(this);
		}

		internal void Update()
		{
			while (source.BuffersProcessed > 1)
			{
				alSourceUnqueueBuffers(source.ID, out uint bufferID);
				Audio.CheckALError("Could not unqueue Buffer");

				if (codec.IsFinished && source.Looping)
					codec.Seek(0);

				if (!codec.IsFinished)
				{
					byte[] bytes = codec.GetData(bufferSize);
					alBufferData(bufferID, format, bytes, bytes.Length, codec.SampleRate);
					Audio.CheckALError("Could not set Buffer Data");

					alSourceQueueBuffers(source.ID, bufferID);
					Audio.CheckALError("Could not queue Buffer");
				}
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
	}
}