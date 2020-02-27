using static OpenAL.Bindings;

namespace CKGL
{
	public abstract class AudioEffect
	{
		internal uint ID;
		internal uint EffectSlotID;

		/// <summary>
		/// 1f (0f - 1f)
		/// </summary>
		public float MasterGain
		{
			get
			{
				alGetAuxiliaryEffectSlotf(ID, AL_EFFECTSLOT_GAIN, out float value);
				Audio.CheckALError("Could not read AudioEffect.MasterGain");
				return value;
			}
			set
			{
				Audio.CheckRange("AudioEffect.Gain", value, 0f, 1f);
				alAuxiliaryEffectSlotf(EffectSlotID, AL_EFFECTSLOT_GAIN, value);
				Audio.CheckALError("Could not update AudioEffect.MasterGain");
			}
		}

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

		protected void Apply()
		{
			alAuxiliaryEffectSloti(EffectSlotID, AL_EFFECTSLOT_EFFECT, (int)ID);
			Audio.CheckALError("Could not set Effect to Effect Slot");
		}
	}
}