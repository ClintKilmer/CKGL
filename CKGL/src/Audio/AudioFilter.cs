#nullable enable

using System.Collections.Generic;
using static OpenAL.Bindings;

namespace CKGL
{
	public abstract class AudioFilter
	{
		internal uint ID;

		internal readonly List<AudioSource> Sources = new List<AudioSource>();
		internal readonly List<AudioChannel> Channels = new List<AudioChannel>();

		protected AudioFilter()
		{
			ID = alGenFilter();
			Audio.CheckALError("Could not create Filter");

			Audio.Filters.Add(this);
		}

		public void Destroy()
		{
			for (int i = Sources.Count - 1; i >= 0; i--)
				Sources[i].Filter = null;

			for (int i = Channels.Count - 1; i >= 0; i--)
				Channels[i].Filter = null;

			alDeleteFilter(ID);
			Audio.CheckALError("Could not destroy Filter");
			ID = default;

			Audio.Filters.Remove(this);
		}

		protected void Apply()
		{
			foreach (AudioSource source in Sources)
				source.Filter = this;

			foreach (AudioChannel channel in Channels)
				channel.Filter = this;
		}
	}
}