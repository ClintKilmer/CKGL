using static OpenAL.Bindings;

namespace CKGL
{
	public class DistortionEffect : AudioEffect
	{
		public DistortionEffect() : base()
		{
			alEffecti(ID, AL_EFFECT_TYPE, AL_EFFECT_DISTORTION);
			Audio.CheckALError("Could not set Effect Type");
			Apply();
		}

		/// <summary>
		/// 0.2f (0f - 1f)
		/// </summary>
		public float Edge
		{
			get
			{
				alGetEffectf(ID, AL_DISTORTION_EDGE, out float value);
				Audio.CheckALError("Could not read DistortionEffect.Edge");
				return value;
			}
			set
			{
				Audio.CheckRange("DistortionEffect.Edge", value, AL_DISTORTION_MIN_EDGE, AL_DISTORTION_MAX_EDGE);
				alEffectf(ID, AL_DISTORTION_EDGE, value);
				Audio.CheckALError("Could not update DistortionEffect.Edge");
				Apply();
			}
		}

		/// <summary>
		/// 0.05f (0.01f - 1f)
		/// </summary>
		public float Gain
		{
			get
			{
				alGetEffectf(ID, AL_DISTORTION_GAIN, out float value);
				Audio.CheckALError("Could not read DistortionEffect.Gain");
				return value;
			}
			set
			{
				Audio.CheckRange("DistortionEffect.Gain", value, AL_DISTORTION_MIN_GAIN, AL_DISTORTION_MAX_GAIN);
				alEffectf(ID, AL_DISTORTION_GAIN, value);
				Audio.CheckALError("Could not update DistortionEffect.Gain");
				Apply();
			}
		}

		/// <summary>
		/// 8000f (80f - 24000f) Hz
		/// </summary>
		public float LowpassCutoff
		{
			get
			{
				alGetEffectf(ID, AL_DISTORTION_LOWPASS_CUTOFF, out float value);
				Audio.CheckALError("Could not read DistortionEffect.LowpassCutoff");
				return value;
			}
			set
			{
				Audio.CheckRange("DistortionEffect.LowpassCutoff", value, AL_DISTORTION_MIN_LOWPASS_CUTOFF, AL_DISTORTION_MAX_LOWPASS_CUTOFF);
				alEffectf(ID, AL_DISTORTION_LOWPASS_CUTOFF, value);
				Audio.CheckALError("Could not update DistortionEffect.LowpassCutoff");
				Apply();
			}
		}

		/// <summary>
		/// 3600f (80f - 24000f) Hz
		/// </summary>
		public float EQCenter
		{
			get
			{
				alGetEffectf(ID, AL_DISTORTION_EQCENTER, out float value);
				Audio.CheckALError("Could not read DistortionEffect.EQCenter");
				return value;
			}
			set
			{
				Audio.CheckRange("DistortionEffect.EQCenter", value, AL_DISTORTION_MIN_EQCENTER, AL_DISTORTION_MAX_EQCENTER);
				alEffectf(ID, AL_DISTORTION_EQCENTER, value);
				Audio.CheckALError("Could not update DistortionEffect.EQCenter");
				Apply();
			}
		}

		/// <summary>
		/// 3600f (80f - 24000f) Hz
		/// </summary>
		public float EQBandwidth
		{
			get
			{
				alGetEffectf(ID, AL_DISTORTION_EQBANDWIDTH, out float value);
				Audio.CheckALError("Could not read DistortionEffect.EQBandwidth");
				return value;
			}
			set
			{
				Audio.CheckRange("DistortionEffect.EQBandwidth", value, AL_DISTORTION_MIN_EQBANDWIDTH, AL_DISTORTION_MAX_EQBANDWIDTH);
				alEffectf(ID, AL_DISTORTION_EQBANDWIDTH, value);
				Audio.CheckALError("Could not update DistortionEffect.EQBandwidth");
				Apply();
			}
		}
	}
}