using static OpenAL.Bindings;

namespace CKGL
{
	public class EchoEffect : AudioEffect
	{
		public EchoEffect() : base()
		{
			alEffecti(ID, AL_EFFECT_TYPE, AL_EFFECT_ECHO);
			Audio.CheckALError("Could not set Effect Type");
			Apply();
		}

		/// <summary>
		/// 0.1f (0f - 0.207f)
		/// </summary>
		public float Delay
		{
			get
			{
				alGetEffectf(ID, AL_ECHO_DELAY, out float value);
				Audio.CheckALError("Could not read EchoEffect.Delay");
				return value;
			}
			set
			{
				Audio.CheckRange("EchoEffect.Delay", value, AL_ECHO_MIN_DELAY, AL_ECHO_MAX_DELAY);
				alEffectf(ID, AL_ECHO_DELAY, value);
				Audio.CheckALError("Could not update EchoEffect.Delay");
				Apply();
			}
		}

		/// <summary>
		/// 0.1f (0f - 0.404f)
		/// </summary>
		public float LRDelay
		{
			get
			{
				alGetEffectf(ID, AL_ECHO_LRDELAY, out float value);
				Audio.CheckALError("Could not read EchoEffect.LRDelay");
				return value;
			}
			set
			{
				Audio.CheckRange("EchoEffect.LRDelay", value, AL_ECHO_MIN_LRDELAY, AL_ECHO_MAX_LRDELAY);
				alEffectf(ID, AL_ECHO_LRDELAY, value);
				Audio.CheckALError("Could not update EchoEffect.LRDelay");
				Apply();
			}
		}

		/// <summary>
		/// 0.5f (0f - 0.99f)
		/// </summary>
		public float Damping
		{
			get
			{
				alGetEffectf(ID, AL_ECHO_DAMPING, out float value);
				Audio.CheckALError("Could not read EchoEffect.Damping");
				return value;
			}
			set
			{
				Audio.CheckRange("EchoEffect.Damping", value, AL_ECHO_MIN_DAMPING, AL_ECHO_MAX_DAMPING);
				alEffectf(ID, AL_ECHO_DAMPING, value);
				Audio.CheckALError("Could not update EchoEffect.Damping");
				Apply();
			}
		}

		/// <summary>
		/// 0.5f (0f - 1f)
		/// </summary>
		public float Feedback
		{
			get
			{
				alGetEffectf(ID, AL_ECHO_FEEDBACK, out float value);
				Audio.CheckALError("Could not read EchoEffect.Feedback");
				return value;
			}
			set
			{
				Audio.CheckRange("EchoEffect.Feedback", value, AL_ECHO_MIN_FEEDBACK, AL_ECHO_MAX_FEEDBACK);
				alEffectf(ID, AL_ECHO_FEEDBACK, value);
				Audio.CheckALError("Could not update EchoEffect.Feedback");
				Apply();
			}
		}

		/// <summary>
		/// -1f (-1f - 1f)
		/// </summary>
		public float Spread
		{
			get
			{
				alGetEffectf(ID, AL_ECHO_SPREAD, out float value);
				Audio.CheckALError("Could not read EchoEffect.Spread");
				return value;
			}
			set
			{
				Audio.CheckRange("EchoEffect.Spread", value, AL_ECHO_MIN_SPREAD, AL_ECHO_MAX_SPREAD);
				alEffectf(ID, AL_ECHO_SPREAD, value);
				Audio.CheckALError("Could not update EchoEffect.Spread");
				Apply();
			}
		}
	}
}