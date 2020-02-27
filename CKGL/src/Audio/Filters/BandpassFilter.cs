using static OpenAL.Bindings;

namespace CKGL
{
	public class BandpassFilter : AudioFilter
	{
		public BandpassFilter() : base()
		{
			alFilteri(ID, AL_FILTER_TYPE, AL_FILTER_BANDPASS);
			Audio.CheckALError("Could not set Filter Type");
			Apply();
		}

		/// <summary>
		/// 1f (0f - 1f)
		/// </summary>
		public float Gain
		{
			get
			{
				alGetFilterf(ID, AL_BANDPASS_GAIN, out float value);
				Audio.CheckALError("Could not read BandpassFilter.Gain");
				return value;
			}
			set
			{
				Audio.CheckRange("BandpassFilter.Gain", value, AL_BANDPASS_MIN_GAIN, AL_BANDPASS_MAX_GAIN);
				alFilterf(ID, AL_BANDPASS_GAIN, value);
				Audio.CheckALError("Could not update BandpassFilter.Gain");
				Apply();
			}
		}

		/// <summary>
		/// 1f (0f - 1f)
		/// </summary>
		public float GainLF
		{
			get
			{
				alGetFilterf(ID, AL_BANDPASS_GAINLF, out float value);
				Audio.CheckALError("Could not read BandpassFilter.GainLF");
				return value;
			}
			set
			{
				Audio.CheckRange("BandpassFilter.GainLF", value, AL_BANDPASS_MIN_GAINLF, AL_BANDPASS_MAX_GAINLF);
				alFilterf(ID, AL_BANDPASS_GAINLF, value);
				Audio.CheckALError("Could not update BandpassFilter.GainLF");
				Apply();
			}
		}

		/// <summary>
		/// 1f (0f - 1f)
		/// </summary>
		public float GainHF
		{
			get
			{
				alGetFilterf(ID, AL_BANDPASS_GAINHF, out float value);
				Audio.CheckALError("Could not read BandpassFilter.GainHF");
				return value;
			}
			set
			{
				Audio.CheckRange("BandpassFilter.GainHF", value, AL_BANDPASS_MIN_GAINHF, AL_BANDPASS_MAX_GAINHF);
				alFilterf(ID, AL_BANDPASS_GAINHF, value);
				Audio.CheckALError("Could not update BandpassFilter.GainHF");
				Apply();
			}
		}
	}
}