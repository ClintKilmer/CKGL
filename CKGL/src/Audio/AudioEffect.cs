using static OpenAL.Bindings;

namespace CKGL
{
	public abstract class AudioEffect
	{
		public uint ID;
		public uint EffectSlotID;

		protected AudioEffect()
		{
			ID = alGenEffect();
			Audio.CheckALError("Could not create Effect");

			EffectSlotID = alGenAuxiliaryEffectSlot();
			Audio.CheckALError("Could not create Effect Slot");
		}

		public void Destroy()
		{
			alDeleteEffect(ID);
			Audio.CheckALError("Could not destroy Effect");

			alDeleteAuxiliaryEffectSlot(EffectSlotID);
			Audio.CheckALError("Could not destroy Effect Slot");
		}

		public void Apply()
		{
			alAuxiliaryEffectSloti(EffectSlotID, AL_EFFECTSLOT_EFFECT, (int)ID);
			Audio.CheckALError("Could not set Effect to Effect Slot");
		}

		protected void CheckRange(string name, float value, float min, float max)
		{
			if (value < min || value > max)
				throw new CKGLException($"Illegal Value for \"{name}\" = {value} | Range: ({min} - {max})");
		}

		protected void CheckRange(string name, int value, int min, int max)
		{
			if (value < min || value > max)
				throw new CKGLException($"Illegal Value for \"{name}\" = {value} | Range: ({min} - {max})");
		}
	}
}