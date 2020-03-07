#nullable enable

using static OpenAL.Bindings;

namespace CKGL
{
	public class AudioSource
	{
		internal uint ID { get; private set; }
		internal bool DestroyOnStop = false;

		public readonly AudioChannel[] Channels = new AudioChannel[Audio.ChannelCount];

		internal AudioStream? Stream;

		private AudioBuffer? buffer;
		public AudioBuffer? Buffer
		{
			get => buffer;
			set
			{
				// This property handles resetting AL_SOURCE_TYPE to AL_UNDETERMINED, so we can swap between AL_STATIC and AL_STREAMING by simply setting the Buffer
				Stop();

				alSourcei(ID, alSourceiParameter.Buffer, (int)AL_NONE);
				Audio.CheckALError("Could not set AudioSource.Buffer");

				if (buffer?.Streamed ?? false)
				{
					Stream?.Destroy();
					Stream = null;
				}

				if (value != null)
				{
					if (value.Streamed)
					{
						Stream = new AudioStream(value, this);
					}
					else
					{
						alSourcei(ID, alSourceiParameter.Buffer, (int)value.ID);
						Audio.CheckALError("Could not set AudioSource.Buffer");
					}
				}

				buffer?.Sources.Remove(this);
				value?.Sources.Add(this);
				buffer = value;
			}
		}

		private AudioFilter? filter;
		public AudioFilter? Filter
		{
			get => filter;
			set
			{
				alSourcei(ID, AL_DIRECT_FILTER, (int)(value?.ID ?? AL_FILTER_NULL));
				Audio.CheckALError("Could not set AudioSource.Filter");
				filter?.Sources.Remove(this);
				value?.Sources.Add(this);
				filter = value;
			}
		}

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
				alGetSourcef(ID, alSourcefParameter.Pitch, out float pitch);
				Audio.CheckALError("Could not read AudioSource.Pitch");
				return pitch;
			}
			set
			{
				Audio.CheckRange("AudioSource.Pitch", value, 0.5f, 2f);
				alSourcef(ID, alSourcefParameter.Pitch, value);
				Audio.CheckALError("Could not update AudioSource.Pitch");
			}
		}

		private bool streamedLooping = false;
		public bool Looping
		{
			get
			{
				if (buffer?.Streamed ?? false)
				{
					return streamedLooping;
				}
				else
				{
					alGetSourcei(ID, alGetSourceiParameter.Looping, out int looping);
					Audio.CheckALError("Could not read AudioSource.Looping");
					return looping == 1;
				}
			}
			set
			{
				if (buffer?.Streamed ?? false)
				{
					streamedLooping = value;
				}
				else
				{
					alSourcei(ID, alSourceiParameter.Looping, value ? 1 : 0);
					Audio.CheckALError("Could not update AudioSource.Looping");
				}
			}
		}

		public bool Relative
		{
			get
			{
				alGetSourcei(ID, alGetSourceiParameter.SourceRelative, out int relative);
				Audio.CheckALError("Could not read AudioSource.SourceRelative");
				return relative == 1;
			}
			set
			{
				alSourcei(ID, alSourceiParameter.SourceRelative, value ? 1 : 0);
				Audio.CheckALError("Could not update AudioSource.SourceRelative");
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

		public bool IsStopped => State == alSourceState.Stopped || !Audio.Active;

		public AudioSource()
		{
			ID = alGenSource();
			Audio.CheckALError("Could not create Source");

			for (int i = 0; i < Channels.Length; i++)
				Channels[i] = new AudioChannel(this, i);

			if (Audio.Active)
				Audio.Sources.Add(this);
		}

		public void Update()
		{
			if (DestroyOnStop && IsStopped)
				Destroy();
		}

		public void Destroy()
		{
			Stop();

			Filter = null;

			for (int i = 0; i < Channels.Length; i++)
				Channels[i].FilterEffect = (null, null);

			Buffer = null;

			alDeleteSource(ID);
			Audio.CheckALError("Could not destroy Source");
			ID = default;

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