#nullable enable

using System.Collections.Generic;
using System.IO;
using static OpenAL.Bindings;

namespace CKGL
{
	public class AudioBuffer
	{
		public readonly string File;
		public readonly bool Streamed;

		/// <summary>Size in bytes</summary>
		public readonly int Size;
		public readonly int Samples;
		/// <summary>Length in seconds</summary>
		public readonly float Length;

		internal uint ID { get; private set; }

		internal readonly List<AudioSource> Sources = new List<AudioSource>();

		private bool destroyed = false;

		public AudioBuffer(string file, bool streamed = false)
		{
			if (!System.IO.File.Exists(file))
				throw new FileNotFoundException("Audio file not found.", file);

			File = file;
			Streamed = streamed;

			if (Streamed)
			{
				AudioCodec codec = new AudioCodec(file);
				Size = codec.Size;
				Samples = codec.Samples;
				Length = codec.Length;
				codec.Destroy();
			}
			else
			{
				ID = alGenBuffer();
				Audio.CheckALError("Could not create Buffer");

				byte[] bytes = AudioCodec.Decode(file, out float length, out int channels, out int samples, out int sampleRate, out int bitDepth);
				Size = bytes.Length;
				Samples = samples;
				Length = length;

				alBufferData(ID, Audio.GetalBufferFormat(channels, bitDepth), bytes, bytes.Length, sampleRate);
				Audio.CheckALError("Could not set Buffer Data");
			}

			Audio.Buffers.Add(this);
		}

		public void Destroy()
		{
			for (int i = Sources.Count - 1; i >= 0; i--)
				Sources[i].Buffer = null;

			if (!Streamed)
			{
				alDeleteBuffer(ID);
				Audio.CheckALError("Could not destroy Buffer");
				ID = default;
			}

			Audio.Buffers.Remove(this);

			destroyed = true;
		}

		private AudioSource play(bool looping, AudioFilter? directFilter = null, params (AudioFilter? filter, AudioEffect? effect)?[]? channels)
		{
			if (Audio.Active && destroyed)
				throw new CKGLException("OpenAL Error: AudioBuffer has been destroyed");

			AudioSource source = new AudioSource();
			source.DestroyOnStop = true;
			source.Looping = looping;
			source.Buffer = this;

			if (directFilter != null)
				source.Filter = directFilter;

			if (channels != null)
			{
				if (channels.Length > source.Channels.Length)
					Output.WriteLine($"OpenAL Error: Tried to set {channels.Length} channels which exceeds the maximum number of {source.Channels.Length} (ignoring excess channels)");

				for (int i = 0; i < channels.Length && i < source.Channels.Length; i++)
					source.Channels[i].FilterEffect = (channels[i]?.filter, channels[i]?.effect);
			}

			source.Play();

			return source;
		}

		/// <summary>Only use the returned AudioSource immediately after instantiation. Do not store a reference for later use, as it will be automatically destroyed upon audio completion.</summary>
		public AudioSource Play(AudioFilter? directFilter = null, params (AudioFilter? filter, AudioEffect? effect)?[]? channels) => play(false, directFilter, channels);

		/// <summary>Only use the returned AudioSource immediately after instantiation. Do not store a reference for later use, as it will be automatically destroyed upon audio completion.</summary>
		public AudioSource PlayLooped(AudioFilter? directFilter = null, params (AudioFilter? filter, AudioEffect? effect)?[]? channels) => play(true, directFilter, channels);
	}
}