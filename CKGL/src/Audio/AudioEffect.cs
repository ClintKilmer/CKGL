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

			Audio.Effects.Add(this);
		}

		public void Destroy()
		{
			alDeleteEffect(ID);
			Audio.CheckALError("Could not destroy Effect");

			alDeleteAuxiliaryEffectSlot(EffectSlotID);
			Audio.CheckALError("Could not destroy Effect Slot");

			Audio.Effects.Remove(this);
		}

		public void Apply()
		{
			alAuxiliaryEffectSloti(EffectSlotID, AL_EFFECTSLOT_EFFECT, (int)ID);
			Audio.CheckALError("Could not set Effect to Effect Slot");
		}
	}
}