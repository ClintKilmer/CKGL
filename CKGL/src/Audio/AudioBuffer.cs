#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using static OpenAL.Bindings;
using static SDL2.SDL;

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

			SDL_AudioSpec wavspec = new SDL_AudioSpec();
			SDL_LoadWAV(file, ref wavspec, out IntPtr audioBuffer, out uint audioLength);

			// map wav header to openal format
			alBufferFormat format;
			switch (wavspec.format)
			{
				case AUDIO_U8:
				case AUDIO_S8:
					format = wavspec.channels == 2 ? alBufferFormat.Stereo8 : alBufferFormat.Mono8;
					break;
				case AUDIO_U16:
				case AUDIO_S16:
					format = wavspec.channels == 2 ? alBufferFormat.Stereo16 : alBufferFormat.Mono16;
					break;
				default:
					SDL_FreeWAV(audioBuffer);
					throw new CKGLException($"SDL failed parsing wav: {file}");
			}

			alBufferData(ID, format, audioBuffer, (int)audioLength, wavspec.freq);
			Audio.CheckALError("Could not set Buffer Data");

			SDL_FreeWAV(audioBuffer);

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