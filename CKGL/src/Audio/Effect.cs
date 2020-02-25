using static OpenAL.Bindings;

namespace CKGL
{
	public class Effect
	{
		public uint ID;

		protected Effect()
		{
			ID = alGenEffect();
			if (Audio.CheckALError())
				throw new CKGLException("Could not make OpenAL Effect");
		}

		public void Destroy()
		{
			alDeleteEffect(ID);
			if (Audio.CheckALError())
				throw new CKGLException("Could not destroy OpenAL Effect");
		}
	}
}