using static OpenAL.Bindings;

namespace CKGL
{
	public class AudioSource
	{
		public uint ID;

		public AudioSource()
		{
			ID = alGenSource();
			if (Audio.CheckALError())
				throw new CKGLException("Could not make OpenAL Source");

			alSource3f(ID, alSource3fParameter.Position, 0, 0, 0);
			alSource3f(ID, alSource3fParameter.Velocity, 0, 0, 0);
			alSourcef(ID, alSourcefParameter.Gain, 1f);
			alSourcef(ID, alSourcefParameter.Pitch, 1f);
			alSourcei(ID, alSourceiParameter.Looping, 0);
			//alSourcei(id, alSourceiParameter.SourceRelative, 0);
			alSource3i(ID, AL_AUXILIARY_SEND_FILTER, (int)Audio.slot, 0, AL_FILTER_NULL);
			if (Audio.CheckALError())
				throw new CKGLException("Could set OpenAL Source properties");

			Audio.Sources.Add(this);
		}

		public void Destroy()
		{
			alDeleteSource(ID);
			Audio.Sources.Remove(this);
		}

		public void BindAudioBuffer(AudioBuffer audioBuffer)
		{
			alSourcei(ID, alSourceiParameter.Buffer, (int)audioBuffer.id);
		}

		public void Play()
		{
			alSourcePlay(ID);

			if (Audio.CheckALError())
				throw new CKGLException("OpenAL Error: Source.Play()");
		}

		public void Pause()
		{
			alSourcePause(ID);
		}

		public void Stop()
		{
			alSourceStop(ID);
		}

		private alSourceState GetState()
		{
			alGetSourcei(ID, alGetSourceiParameter.SourceState, out int state);
			if (Audio.CheckALError())
				throw new CKGLException("Could set OpenAL Source properties");
			return (alSourceState)state;
		}

		public bool IsPlaying()
		{
			return GetState() == alSourceState.Playing;
		}

		public bool IsPaused()
		{
			return GetState() == alSourceState.Paused;
		}

		public bool IsStopped()
		{
			return GetState() == alSourceState.Stopped;
		}
	}
}
