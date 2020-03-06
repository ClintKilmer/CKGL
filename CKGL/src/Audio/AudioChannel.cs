#nullable enable

using static OpenAL.Bindings;

namespace CKGL
{
	public class AudioChannel
	{
		private readonly AudioSource source;
		private readonly int number;

		private AudioFilter? filter;
		private AudioEffect? effect;

		public AudioFilter? Filter
		{
			get => filter;
			set
			{
				filter?.Channels.Remove(this);
				value?.Channels.Add(this);
				filter = value;
				Apply();
			}
		}

		public AudioEffect? Effect
		{
			get => effect;
			set
			{
				effect?.Channels.Remove(this);
				value?.Channels.Add(this);
				effect = value;
				Apply();
			}
		}

		public (AudioFilter? Filter, AudioEffect? Effect)? FilterEffect
		{
			set
			{
				filter?.Channels.Remove(this);
				value?.Filter?.Channels.Add(this);
				filter = value?.Filter;

				effect?.Channels.Remove(this);
				value?.Effect?.Channels.Add(this);
				effect = value?.Effect;

				Apply();
			}
		}

		internal AudioChannel(AudioSource source, int number)
		{
			this.source = source;
			this.number = number;
		}

		private void Apply()
		{
			alSource3i(source.ID, AL_AUXILIARY_SEND_FILTER, (int)(effect?.ID ?? AL_EFFECTSLOT_NULL), number, (int)(filter?.ID ?? AL_FILTER_NULL));
			Audio.CheckALError("Could not set AudioChannel.Effect/Filter");
		}
	}
}