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
	}
}