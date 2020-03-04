#nullable enable

using System.Collections.Generic;
using System.IO;
using static OpenAL.Bindings;

namespace CKGL
{
	public class AudioBuffer
	{
		internal uint ID;

		internal readonly List<AudioSource> Sources = new List<AudioSource>();

		public AudioBuffer(string file)
		{
			if (!File.Exists(file))
				throw new FileNotFoundException("Audio file not found.", file);

			ID = alGenBuffer();
			Audio.CheckALError("Could not create Buffer");

			byte[] bytes = AudioDecoder.Load(file, out int channels, out int sampleRate, out int bitdepth);

			alBufferFormat format = channels switch
			{
				1 => bitdepth switch
				{
					8 => alBufferFormat.Mono8,
					16 => alBufferFormat.Mono16,
					32 => alBufferFormat.Mono32,
					64 => alBufferFormat.Mono64,
					_ => throw new CKGLException("OpenAL Error: Invalid bit depth")
				},
				2 => bitdepth switch
				{
					8 => alBufferFormat.Stereo8,
					16 => alBufferFormat.Stereo16,
					32 => alBufferFormat.Stereo32,
					64 => alBufferFormat.Stereo64,
					_ => throw new CKGLException("OpenAL Error: Invalid bit depth")
				},
				_ => throw new CKGLException("OpenAL Error: Invalid channel count")
			};

			alBufferData(ID, format, bytes, bytes.Length, sampleRate);
			Audio.CheckALError("Could not set Buffer Data");

			Audio.Buffers.Add(this);
		}

		public void Destroy()
		{
			for (int i = Sources.Count - 1; i >= 0; i--)
				Sources[i].Buffer = null;

			alDeleteBuffer(ID);
			Audio.CheckALError("Could not destroy Buffer");
			ID = default;

			Audio.Buffers.Remove(this);
		}

		public void Play(AudioFilter? directFilter = null, params (AudioFilter? filter, AudioEffect? effect)?[]? channels)
		{
			AudioSource source = new AudioSource();
			source.DestroyOnStop = true;
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
		}
	}
}