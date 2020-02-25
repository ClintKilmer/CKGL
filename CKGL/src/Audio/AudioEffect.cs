using static OpenAL.Bindings;

namespace CKGL
{
	public class AudioEffect
	{
		public uint ID;

		protected AudioEffect()
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