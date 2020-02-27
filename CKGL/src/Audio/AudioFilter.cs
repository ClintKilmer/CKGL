#nullable enable

using static OpenAL.Bindings;

namespace CKGL
{
	public abstract class AudioFilter
	{
		internal uint ID;

		protected AudioFilter()
		{
			ID = alGenFilter();
			Audio.CheckALError("Could not create Filter");

			Audio.Filters.Add(this);
		}

		public void Destroy()
		{
			alDeleteFilter(ID);
			Audio.CheckALError("Could not destroy Filter");

			Audio.Filters.Remove(this);
		}

		protected void Apply()
		{
			foreach (AudioSource source in Audio.Sources)
			{
				if (source.DirectFilter == this)
					source.DirectFilter = this;

				for (int i = 0; i < Audio.ChannelCount; i++)
				{
					if (source.Channels[i].Filter == this)
						source.Channels[i].Filter = this;
				}
			}
		}
	}
}