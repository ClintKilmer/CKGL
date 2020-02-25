using static OpenAL.Bindings;

namespace CKGL
{
	public class DistortionEffect : Effect
	{
		public DistortionEffect() : base()
		{
			alEffecti(ID, AL_EFFECT_TYPE, AL_EFFECT_DISTORTION);
			if (Audio.CheckALError())
				Output.WriteLine("OpenAL Error: Could not create DistortionEffect");
		}

		public float Edge
		{
			get
			{
				alGetEffectf(ID, AL_DISTORTION_EDGE, out float value);
				if (Audio.CheckALError())
					Output.WriteLine("OpenAL Error: Could not read DistortionEffect Edge");
				return value;
			}
			set
			{
				if (value < AL_DISTORTION_MIN_EDGE || value > AL_DISTORTION_MAX_EDGE)
					throw new CKGLException($"DistortionEffect.Edge must be between {AL_DISTORTION_MIN_EDGE} and {AL_DISTORTION_MAX_EDGE}. Attempted value = {value}");
				alEffectf(ID, AL_DISTORTION_EDGE, value);
				if (Audio.CheckALError())
					Output.WriteLine("OpenAL Error: Could not update DistortionEffect Edge");
			}
		}

		public float Gain
		{
			get
			{
				alGetEffectf(ID, AL_DISTORTION_GAIN, out float value);
				if (Audio.CheckALError())
					Output.WriteLine("OpenAL Error: Could not read DistortionEffect Gain");
				return value;
			}
			set
			{
				if (value < AL_DISTORTION_MIN_GAIN || value > AL_DISTORTION_MAX_GAIN)
					throw new CKGLException($"DistortionEffect.Gain must be between {AL_DISTORTION_MIN_GAIN} and {AL_DISTORTION_MAX_GAIN}. Attempted value = {value}");
				alEffectf(ID, AL_DISTORTION_GAIN, value);
				if (Audio.CheckALError())
					Output.WriteLine("OpenAL Error: Could not update DistortionEffect Gain");
			}
		}
	}
}