#nullable enable

using static OpenAL.Bindings;

namespace CKGL
{
	public class AudioSource
	{
		internal uint ID { get; private set; }
		internal bool DestroyOnStop = false;

		public readonly AudioChannel[] Channels = new AudioChannel[Audio.ChannelCount];

		public bool Active => ID != default;

		internal AudioStream? Stream;

		private AudioBuffer? buffer;
		public AudioBuffer? Buffer
		{
			get => buffer;
			set
			{
				// This property handles resetting AL_SOURCE_TYPE to AL_UNDETERMINED, so we can swap between AL_STATIC and AL_STREAMING by simply setting the Buffer
				Stop();

				alSourcei(ID, alSourceiParameter.Buffer, AL_NONE);
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

				updateLooping(); // Looping depends on if the Buffer is streamed - see below
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

		/// <summary>
		/// (0f, 0f, 0f) ( - )
		/// </summary>
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

		/// <summary>
		/// (0f, 0f, 0f) ( - )
		/// </summary>
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

		/// <summary>
		/// (0f, 0f, 0f) ( - )
		/// </summary>
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

		/// <summary>
		/// 1f (0f - )
		/// </summary>
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

		/// <summary>
		/// 1f (float.Epsilon - )
		/// </summary>
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
				Audio.CheckRange("AudioSource.Pitch", value, float.Epsilon, float.MaxValue);
				alSourcef(ID, alSourcefParameter.Pitch, value);
				Audio.CheckALError("Could not update AudioSource.Pitch");
			}
		}

		public void Rewind()
		{
			if (buffer?.Streamed ?? false)
			{
				bool wasPlaying = IsPlaying;
				Stop();
				Stream?.Rewind();
				if (wasPlaying)
					Play();
			}
			else
			{
				alSourcef(ID, alSourcefParameter.ByteOffset, 0);
				Audio.CheckALError("Could not update AudioSource.ByteOffset");
			}
		}

		/// <summary>
		/// n/a (0f - )
		/// </summary>
		public float SecondOffset
		{
			get
			{
				// TODO - Streamed Offset Gets
				if (buffer?.Streamed ?? false)
					return 0f;

				alGetSourcef(ID, alSourcefParameter.SecondOffset, out float secondOffset);
				Audio.CheckALError("Could not read AudioSource.SecondOffset");
				return secondOffset;
			}
			set
			{
				Audio.CheckRange("AudioSource.SecondOffset", value, 0f, buffer?.Length ?? float.MaxValue);

				Output.WriteLine($"AudioSource.SecondOffset: {value}");

				if (buffer?.Streamed ?? false)
				{
					bool wasPlaying = IsPlaying;
					Stop();
					Stream?.SeekSeconds(value);
					if (wasPlaying)
						Play();
				}
				else
				{
					alSourcef(ID, alSourcefParameter.SecondOffset, value);
					Audio.CheckALError("Could not update AudioSource.SecondOffset");
				}
			}
		}

		/// <summary>
		/// n/a (0f - 1f)
		/// </summary>
		public float PercentOffset
		{
			get => SecondOffset / (buffer?.Length ?? 1f);
			set
			{
				Audio.CheckRange("AudioSource.PercentOffset", value, 0f, 1f);
				SecondOffset = (buffer?.Length ?? 1f) * value;
			}
		}

		// Looping a streamed Buffer requires the Source.Looping state to be false, yet still return true for the Codec
		private bool looping = false;
		/// <summary>
		/// false (false/true)
		/// </summary>
		public bool Looping
		{
			get
			{
				if (buffer?.Streamed ?? false)
				{
					return looping;
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
				looping = value;
				updateLooping();
			}
		}
		private void updateLooping()
		{
			if (buffer?.Streamed ?? false)
			{
				alSourcei(ID, alSourceiParameter.Looping, 0);
				Audio.CheckALError("Could not update AudioSource.Looping");
			}
			else
			{
				alSourcei(ID, alSourceiParameter.Looping, looping ? 1 : 0);
				Audio.CheckALError("Could not update AudioSource.Looping");
			}
		}

		/// <summary>
		/// false (false/true)
		/// </summary>
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

		internal int BuffersQueued
		{
			get
			{
				alGetSourcei(ID, alGetSourceiParameter.BuffersQueued, out int buffersQueued);
				Audio.CheckALError("Could not read AudioSource.BuffersQueued");
				return buffersQueued;
			}
		}

		internal int BuffersProcessed
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