using static OpenAL.Bindings;

namespace CKGL
{
	public class HighpassFilter : AudioFilter
	{
		public HighpassFilter() : base()
		{
			alFilteri(ID, AL_FILTER_TYPE, AL_FILTER_HIGHPASS);
			Audio.CheckALError("Could not set Filter Type");
		}

		/// <summary>
		/// 1f (0f - 1f)
		/// </summary>
		public float Gain
		{
			get
			{
				alGetFilterf(ID, AL_HIGHPASS_GAIN, out float value);
				Audio.CheckALError("Could not read HighpassFilter.Gain");
				return value;
			}
			set
			{
				Audio.CheckRange("HighpassFilter.Gain", value, AL_HIGHPASS_MIN_GAIN, AL_HIGHPASS_MAX_GAIN);
				alFilterf(ID, AL_HIGHPASS_GAIN, value);
				Audio.CheckALError("Could not update HighpassFilter.Gain");
			}
		}

		/// <summary>
		/// 1f (0f - 1f)
		/// </summary>
		public float GainLF
		{
			get
			{
				alGetFilterf(ID, AL_HIGHPASS_GAINLF, out float value);
				Audio.CheckALError("Could not read HighpassFilter.GainLF");
				return value;
			}
			set
			{
				Audio.CheckRange("HighpassFilter.GainLF", value, AL_HIGHPASS_MIN_GAINLF, AL_HIGHPASS_MAX_GAINLF);
				alFilterf(ID, AL_HIGHPASS_GAINLF, value);
				Audio.CheckALError("Could not update HighpassFilter.GainLF");
			}
		}
	}
}