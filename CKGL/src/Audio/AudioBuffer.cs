using System;
using System.IO;
using static OpenAL.Bindings;
using static SDL2.SDL;

namespace CKGL
{
	public class AudioBuffer
	{
		public uint id;

		public AudioBuffer(string file)
		{
			if (!File.Exists(file))
				throw new FileNotFoundException(file);

			id = alGenBuffer();
			if (Audio.CheckALError())
				throw new CKGLException("OpenAL Error: Could not create Buffer");

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

			alBufferData(id, format, audioBuffer, (int)audioLength, wavspec.freq);

			SDL_FreeWAV(audioBuffer);

			if (Audio.CheckALError())
				throw new CKGLException($"OpenAL could not load \"{file}\"");

			Audio.Buffers.Add(this);
		}

		public void Destroy()
		{
			alDeleteBuffer(id);
			Audio.Buffers.Remove(this);
		}

		public void Play()
		{
			Audio.PlayBuffer(this);
		}
	}
}
