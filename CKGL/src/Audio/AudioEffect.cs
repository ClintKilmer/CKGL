#nullable enable

using System.Collections.Generic;
using static OpenAL.Bindings;

namespace CKGL
{
	public abstract class AudioEffect
	{
		internal uint ID;
		internal uint EffectSlotID;

		internal readonly List<AudioChannel> Channels = new List<AudioChannel>();

		private readonly List<AudioEffect> targetOf = new List<AudioEffect>();

		private AudioEffect? target;
		public AudioEffect? Target
		{
			get => target;
			set
			{
				if (value == this)
					throw new CKGLException("OpenAL Error: Setting AudioEffect.Target as itself is not allowed");

				if (value != null)
				{
					AudioEffect? targetIterator = value;
					while (targetIterator?.target != null)
					{
						if (targetIterator.target == this)
							throw new CKGLException("OpenAL Error: Could not set AudioEffect.Target as this would create a circular chain");

						targetIterator = targetIterator.target;
					}
				}

				alAuxiliaryEffectSloti(EffectSlotID, AL_EFFECTSLOT_TARGET_SOFT, (int)(value?.EffectSlotID ?? AL_EFFECTSLOT_NULL));
				Audio.CheckALError("Could not set AudioEffect.Target");

				target?.targetOf.Remove(this); // Remove this from old target.targetOf
				value?.targetOf.Add(this); // Add this to new target.targetOf

				target = value;
			}
		}

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
			Target = null;
			for (int i = targetOf.Count - 1; i >= 0; i--)
				targetOf[i].Target = null;

			for (int i = Channels.Count - 1; i >= 0; i--)
				Channels[i].Effect = null;

			alDeleteAuxiliaryEffectSlot(EffectSlotID);
			Audio.CheckALError("Could not destroy Effect Slot");
			EffectSlotID = default;

			alDeleteEffect(ID);
			Audio.CheckALError("Could not destroy Effect");
			ID = default;

			Audio.Effects.Remove(this);
		}

		protected void Apply()
		{
			alAuxiliaryEffectSloti(EffectSlotID, AL_EFFECTSLOT_EFFECT, (int)ID);
			Audio.CheckALError("Could not set Effect to Effect Slot");
		}
	}
}