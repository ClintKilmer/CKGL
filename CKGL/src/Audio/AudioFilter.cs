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
				if (source.Send1?.Filter == this)
					source.Send1 = (source.Send1?.Effect, this);
				if (source.Send2?.Filter == this)
					source.Send2 = (source.Send2?.Effect, this);
				if (source.Send3?.Filter == this)
					source.Send3 = (source.Send3?.Effect, this);
				if (source.Send4?.Filter == this)
					source.Send4 = (source.Send4?.Effect, this);
			}
		}
	}
}