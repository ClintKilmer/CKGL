#nullable enable

using static OpenAL.Bindings;

namespace CKGL
{
	public class AudioSource
	{
		internal uint ID;

		public bool DestroyOnStop = false;

		private AudioFilter? directFilter;
		public AudioFilter? DirectFilter
		{
			get
			{
				return directFilter;
			}
			set
			{
				alSourcei(ID, AL_DIRECT_FILTER, (int)(value?.ID ?? AL_FILTER_NULL));
				Audio.CheckALError("Could not set AudioSource.Filter");
				directFilter = value;
			}
		}

		public readonly AudioChannel[] Channels = new AudioChannel[Audio.ChannelCount];

		public Vector3 Position
		{
			get
			{
				alGetSource3f(ID, alSource3fParameter.Position, out float x, out float y, out float z);
				Audio.CheckALError("Could not read AudioSource.Position");
				return new Vector3(x, y, -z);
			}
			set
			{
				alSource3f(ID, alSource3fParameter.Position, value.X, value.Y, -value.Z);
				Audio.CheckALError("Could not update AudioSource.Position");
			}
		}

		public Vector3 Velocity
		{
			get
			{
				alGetSource3f(ID, alSource3fParameter.Velocity, out float x, out float y, out float z);
				Audio.CheckALError("Could not read AudioSource.Velocity");
				return new Vector3(x, y, -z);
			}
			set
			{
				alSource3f(ID, alSource3fParameter.Velocity, value.X, value.Y, -value.Z);
				Audio.CheckALError("Could not update AudioSource.Velocity");
			}
		}

		public Vector3 Direction
		{
			get
			{
				alGetSource3f(ID, alSource3fParameter.Direction, out float x, out float y, out float z);
				Audio.CheckALError("Could not read AudioSource.Direction");
				return new Vector3(x, y, -z);
			}
			set
			{
				alSource3f(ID, alSource3fParameter.Direction, value.X, value.Y, -value.Z);
				Audio.CheckALError("Could not update AudioSource.Direction");
			}
		}

		public float Gain
		{
			get
			{
				alGetSourcef(ID, alSourcefParameter.Gain, out float gain);
				Audio.CheckALError("Could not read AudioSource.Gain");
				return gain;
			}
			set
			{
				Audio.CheckRange("AudioSource.Gain", value, 0f, float.MaxValue);
				alSourcef(ID, alSourcefParameter.Gain, value);
				Audio.CheckALError("Could not update AudioSource.Gain");
			}
		}

		public float Pitch
		{
			get
			{
				alGetSourcef(ID, alSourcefParameter.Pitch, out float gain);
				Audio.CheckALError("Could not read AudioSource.Pitch");
				return gain;
			}
			set
			{
				Audio.CheckRange("AudioSource.Pitch", value, 0.5f, 2f);
				alSourcef(ID, alSourcefParameter.Pitch, value);
				Audio.CheckALError("Could not update AudioSource.Pitch");
			}
		}

		public bool Looping
		{
			get
			{
				alGetSourcei(ID, alGetSourceiParameter.Looping, out int gain);
				Audio.CheckALError("Could not read AudioSource.Looping");
				if (gain == 0)
					return false;
				else if (gain == 1)
					return true;
				throw new CKGLException($"OpenAL Error: AudioSource.Looping returned an invalid value ({gain}) | Range (0 - 1)");
			}
			set
			{
				alSourcei(ID, alSourceiParameter.Looping, value ? 1 : 0);
				Audio.CheckALError("Could not update AudioSource.Looping");
			}
		}

		public bool Relative
		{
			get
			{
				alGetSourcei(ID, alGetSourceiParameter.SourceRelative, out int relative);
				Audio.CheckALError("Could not read AudioSource.SourceRelative");
				if (relative == 0)
					return false;
				else if (relative == 1)
					return true;
				throw new CKGLException($"OpenAL Error: AudioSource.SourceRelative returned an invalid value ({relative}) | Range (0 - 1)");
			}
			set
			{
				alSourcei(ID, alSourceiParameter.SourceRelative, value ? 1 : 0);
				Audio.CheckALError("Could not update AudioSource.SourceRelative");
			}
		}

		public AudioBuffer AudioBuffer
		{
			set
			{
				alSourcei(ID, alSourceiParameter.Buffer, (int)value.ID);
				Audio.CheckALError("Could not set AudioSource.AudioBuffer");
			}
		}

		public int BuffersQueued
		{
			get
			{
				alGetSourcei(ID, alGetSourceiParameter.BuffersQueued, out int buffersQueued);
				Audio.CheckALError("Could not read AudioSource.BuffersQueued");
				return buffersQueued;
			}
		}

		public int BuffersProcessed
		{
			get
			{
				alGetSourcei(ID, alGetSourceiParameter.BuffersProcessed, out int buffersProcessed);
				Audio.CheckALError("Could not read AudioSource.BuffersProcessed");
				return buffersProcessed;
			}
		}

		private alSourceState State
		{
			get
			{
				alGetSourcei(ID, alGetSourceiParameter.SourceState, out int state);
				Audio.CheckALError("Could not read Source State");
				return (alSourceState)state;
			}
		}

		public bool IsPlaying => State == alSourceState.Playing;

		public bool IsPaused => State == alSourceState.Paused;

		public bool IsStopped => State == alSourceState.Stopped;

		public AudioSource()
		{
			ID = alGenSource();
			Audio.CheckALError("Could not create Source");

			Audio.Sources.Add(this);

			for (int i = 0; i < Audio.ChannelCount; i++)
				Channels[i] = new AudioChannel(this, i);
		}

		public void Destroy()
		{
			alDeleteSource(ID);
			Audio.CheckALError("Could not destroy Source");

			Audio.Sources.Remove(this);
		}

		public void Play()
		{
			alSourcePlay(ID);
			Audio.CheckALError("Could not play Source");
		}

		public void Pause()
		{
			alSourcePause(ID);
			Audio.CheckALError("Could not pause Source");
		}

		public void Stop()
		{
			alSourceStop(ID);
			Audio.CheckALError("Could not stop Source");
		}
	}
}