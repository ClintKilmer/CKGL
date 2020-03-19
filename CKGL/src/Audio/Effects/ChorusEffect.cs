using static OpenAL.Bindings;

namespace CKGL
{
	public class ChorusEffect : AudioEffect
	{
		public ChorusEffect() : base()
		{
			alEffecti(ID, AL_EFFECT_TYPE, AL_EFFECT_CHORUS);
			Audio.CheckALError("Could not set Effect Type");
			Apply();
		}

		public enum Waveforms : int
		{
			Sin = AL_CHORUS_WAVEFORM_SINUSOID,
			Triangle = AL_CHORUS_WAVEFORM_TRIANGLE // Default
		}

		/// <summary>
		/// Triangle (Triangle, Sin)
		/// </summary>
		public Waveforms Waveform
		{
			get
			{
				alGetEffecti(ID, AL_CHORUS_WAVEFORM, out int value);
				Audio.CheckALError("Could not read ChorusEffect.Waveform");
				return (Waveforms)value;
			}
			set
			{
				alEffecti(ID, AL_CHORUS_WAVEFORM, (int)value);
				Audio.CheckALError("Could not update ChorusEffect.Waveform");
				Apply();
			}
		}

		/// <summary>
		/// 90 (-180 - 180) Degrees
		/// </summary>
		public int Phase
		{
			get
			{
				alGetEffecti(ID, AL_CHORUS_PHASE, out int value);
				Audio.CheckALError("Could not read ChorusEffect.Phase");
				return value;
			}
			set
			{
				Audio.CheckRange("ChorusEffect.Phase", value, AL_CHORUS_MIN_PHASE, AL_CHORUS_MAX_PHASE);
				alEffecti(ID, AL_CHORUS_PHASE, value);
				Audio.CheckALError("Could not update ChorusEffect.Phase");
				Apply();
			}
		}

		/// <summary>
		/// 1.1f (0f - 10f) Hz
		/// </summary>
		public float Rate
		{
			get
			{
				alGetEffectf(ID, AL_CHORUS_RATE, out float value);
				Audio.CheckALError("Could not read ChorusEffect.Rate");
				return value;
			}
			set
			{
				Audio.CheckRange("ChorusEffect.Rate", value, AL_CHORUS_MIN_RATE, AL_CHORUS_MAX_RATE);
				alEffectf(ID, AL_CHORUS_RATE, value);
				Audio.CheckALError("Could not update ChorusEffect.Rate");
				Apply();
			}
		}

		/// <summary>
		/// 0.1f (0f - 1f)
		/// </summary>
		public float Depth
		{
			get
			{
				alGetEffectf(ID, AL_CHORUS_DEPTH, out float value);
				Audio.CheckALError("Could not read ChorusEffect.Depth");
				return value;
			}
			set
			{
				Audio.CheckRange("ChorusEffect.Depth", value, AL_CHORUS_MIN_DEPTH, AL_CHORUS_MAX_DEPTH);
				alEffectf(ID, AL_CHORUS_DEPTH, value);
				Audio.CheckALError("Could not update ChorusEffect.Depth");
				Apply();
			}
		}

		/// <summary>
		/// 0.25f (-1f - 1f)
		/// </summary>
		public float Feedback
		{
			get
			{
				alGetEffectf(ID, AL_CHORUS_FEEDBACK, out float value);
				Audio.CheckALError("Could not read ChorusEffect.Feedback");
				return value;
			}
			set
			{
				Audio.CheckRange("ChorusEffect.Feedback", value, AL_CHORUS_MIN_FEEDBACK, AL_CHORUS_MAX_FEEDBACK);
				alEffectf(ID, AL_CHORUS_FEEDBACK, value);
				Audio.CheckALError("Could not update ChorusEffect.Feedback");
				Apply();
			}
		}

		/// <summary>
		/// 0.016f (0f - 0.016f) Seconds
		/// </summary>
		public float Delay
		{
			get
			{
				alGetEffectf(ID, AL_CHORUS_DELAY, out float value);
				Audio.CheckALError("Could not read ChorusEffect.Delay");
				return value;
			}
			set
			{
				Audio.CheckRange("ChorusEffect.Delay", value, AL_CHORUS_MIN_DELAY, AL_CHORUS_MAX_DELAY);
				alEffectf(ID, AL_CHORUS_DELAY, value);
				Audio.CheckALError("Could not update ChorusEffect.Delay");
				Apply();
			}
		}
	}
}