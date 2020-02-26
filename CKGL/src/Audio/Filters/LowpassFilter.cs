using static OpenAL.Bindings;

namespace CKGL
{
	public class LowpassFilter : AudioFilter
	{
		public LowpassFilter() : base()
		{
			alFilteri(ID, AL_FILTER_TYPE, AL_FILTER_LOWPASS);
			Audio.CheckALError("Could not set Filter Type");
		}

		/// <summary>
		/// 1f (0f - 1f)
		/// </summary>
		public float Gain
		{
			get
			{
				alGetFilterf(ID, AL_LOWPASS_GAIN, out float value);
				Audio.CheckALError("Could not read LowpassFilter.Gain");
				return value;
			}
			set
			{
				Audio.CheckRange("LowpassFilter.Gain", value, AL_LOWPASS_MIN_GAIN, AL_LOWPASS_MAX_GAIN);
				alFilterf(ID, AL_LOWPASS_GAIN, value);
				Audio.CheckALError("Could not update LowpassFilter.Gain");
			}
		}

		/// <summary>
		/// 1f (0f - 1f)
		/// </summary>
		public float GainHF
		{
			get
			{
				alGetFilterf(ID, AL_LOWPASS_GAINHF, out float value);
				Audio.CheckALError("Could not read LowpassFilter.GainHF");
				return value;
			}
			set
			{
				Audio.CheckRange("LowpassFilter.GainHF", value, AL_LOWPASS_MIN_GAINHF, AL_LOWPASS_MAX_GAINHF);
				alFilterf(ID, AL_LOWPASS_GAINHF, value);
				Audio.CheckALError("Could not update LowpassFilter.GainHF");
			}
		}
	}
}