#nullable enable

using static OpenAL.Bindings;

namespace CKGL
{
	public class AudioChannel
	{
		private readonly AudioSource source;

		public readonly int Number;

		private AudioFilter? filter;
		private AudioEffect? effect;

		public AudioFilter? Filter
		{
			get
			{
				return filter;
			}
			set
			{
				filter = value;
				Apply();
			}
		}

		public AudioEffect? Effect
		{
			get
			{
				return effect;
			}
			set
			{
				effect = value;
				Apply();
			}
		}

		public (AudioFilter? Filter, AudioEffect? Effect) FilterEffect
		{
			set
			{
				filter = value.Filter;
				effect = value.Effect;
				Apply();
			}
		}

		internal AudioChannel(AudioSource source, int number)
		{
			this.source = source;
			Number = number;
		}

		private void Apply()
		{
			alSource3i(source.ID, AL_AUXILIARY_SEND_FILTER, (int)(effect?.ID ?? AL_EFFECTSLOT_NULL), 0, (int)(filter?.ID ?? AL_FILTER_NULL));
			Audio.CheckALError("Could not set AudioChannel.Effect/Filter");
		}
	}
}