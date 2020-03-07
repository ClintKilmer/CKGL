using static OpenAL.Bindings;

namespace CKGL
{
	public class EAXReverbEffect : AudioEffect
	{
		public EAXReverbEffect() : base()
		{
			alEffecti(ID, AL_EFFECT_TYPE, AL_EFFECT_EAXREVERB);
			Audio.CheckALError("Could not set Effect Type");
			Apply();
		}

		public EAXReverbEffect(Preset preset) : this() => LoadPreset(preset);

		/// <summary>
		/// 1f (0f - 1f)
		/// </summary>
		public float Density
		{
			get
			{
				alGetEffectf(ID, AL_EAXREVERB_DENSITY, out float value);
				Audio.CheckALError("Could not read EAXReverbEffect.Density");
				return value;
			}
			set
			{
				Audio.CheckRange("EAXReverbEffect.Density", value, AL_EAXREVERB_MIN_DENSITY, AL_EAXREVERB_MAX_DENSITY);
				alEffectf(ID, AL_EAXREVERB_DENSITY, value);
				Audio.CheckALError("Could not update EAXReverbEffect.Density");
				Apply();
			}
		}

		/// <summary>
		/// 1f (0f - 1f)
		/// </summary>
		public float Diffusion
		{
			get
			{
				alGetEffectf(ID, AL_EAXREVERB_DIFFUSION, out float value);
				Audio.CheckALError("Could not read EAXReverbEffect.Diffusion");
				return value;
			}
			set
			{
				Audio.CheckRange("EAXReverbEffect.Diffusion", value, AL_EAXREVERB_MIN_DIFFUSION, AL_EAXREVERB_MAX_DIFFUSION);
				alEffectf(ID, AL_EAXREVERB_DIFFUSION, value);
				Audio.CheckALError("Could not update EAXReverbEffect.Diffusion");
				Apply();
			}
		}

		/// <summary>
		/// 0.32f (0f - 1f)
		/// </summary>
		public float Gain
		{
			get
			{
				alGetEffectf(ID, AL_EAXREVERB_GAIN, out float value);
				Audio.CheckALError("Could not read EAXReverbEffect.Gain");
				return value;
			}
			set
			{
				Audio.CheckRange("EAXReverbEffect.Gain", value, AL_EAXREVERB_MIN_GAIN, AL_EAXREVERB_MAX_GAIN);
				alEffectf(ID, AL_EAXREVERB_GAIN, value);
				Audio.CheckALError("Could not update EAXReverbEffect.Gain");
				Apply();
			}
		}

		/// <summary>
		/// 0.89f (0f - 1f)
		/// </summary>
		public float GainHF
		{
			get
			{
				alGetEffectf(ID, AL_EAXREVERB_GAINHF, out float value);
				Audio.CheckALError("Could not read EAXReverbEffect.GainHF");
				return value;
			}
			set
			{
				Audio.CheckRange("EAXReverbEffect.GainHF", value, AL_EAXREVERB_MIN_GAINHF, AL_EAXREVERB_MAX_GAINHF);
				alEffectf(ID, AL_EAXREVERB_GAINHF, value);
				Audio.CheckALError("Could not update EAXReverbEffect.GainHF");
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
				alGetEffectf(ID, AL_EAXREVERB_GAINLF, out float value);
				Audio.CheckALError("Could not read EAXReverbEffect.GainLF");
				return value;
			}
			set
			{
				Audio.CheckRange("EAXReverbEffect.GainLF", value, AL_EAXREVERB_MIN_GAINLF, AL_EAXREVERB_MAX_GAINLF);
				alEffectf(ID, AL_EAXREVERB_GAINLF, value);
				Audio.CheckALError("Could not update EAXReverbEffect.GainLF");
				Apply();
			}
		}

		/// <summary>
		/// 1.49f (0.1f - 20f) Seconds
		/// </summary>
		public float DecayTime
		{
			get
			{
				alGetEffectf(ID, AL_EAXREVERB_DECAY_TIME, out float value);
				Audio.CheckALError("Could not read EAXReverbEffect.DecayTime");
				return value;
			}
			set
			{
				Audio.CheckRange("EAXReverbEffect.DecayTime", value, AL_EAXREVERB_MIN_DECAY_TIME, AL_EAXREVERB_MAX_DECAY_TIME);
				alEffectf(ID, AL_EAXREVERB_DECAY_TIME, value);
				Audio.CheckALError("Could not update EAXReverbEffect.DecayTime");
				Apply();
			}
		}

		/// <summary>
		/// 0.83f (0.1f - 2f)
		/// </summary>
		public float DecayHFRatio
		{
			get
			{
				alGetEffectf(ID, AL_EAXREVERB_DECAY_HFRATIO, out float value);
				Audio.CheckALError("Could not read EAXReverbEffect.DecayHFRatio");
				return value;
			}
			set
			{
				Audio.CheckRange("EAXReverbEffect.DecayHFRatio", value, AL_EAXREVERB_MIN_DECAY_HFRATIO, AL_EAXREVERB_MAX_DECAY_HFRATIO);
				alEffectf(ID, AL_EAXREVERB_DECAY_HFRATIO, value);
				Audio.CheckALError("Could not update EAXReverbEffect.DecayHFRatio");
				Apply();
			}
		}

		/// <summary>
		/// 1f (0.1f - 2f)
		/// </summary>
		public float DecayLFRatio
		{
			get
			{
				alGetEffectf(ID, AL_EAXREVERB_DECAY_LFRATIO, out float value);
				Audio.CheckALError("Could not read EAXReverbEffect.DecayLFRatio");
				return value;
			}
			set
			{
				Audio.CheckRange("EAXReverbEffect.DecayLFRatio", value, AL_EAXREVERB_MIN_DECAY_LFRATIO, AL_EAXREVERB_MAX_DECAY_LFRATIO);
				alEffectf(ID, AL_EAXREVERB_DECAY_LFRATIO, value);
				Audio.CheckALError("Could not update EAXReverbEffect.DecayLFRatio");
				Apply();
			}
		}

		/// <summary>
		/// 0.05f (0f - 3.16f)
		/// </summary>
		public float ReflectionsGain
		{
			get
			{
				alGetEffectf(ID, AL_EAXREVERB_REFLECTIONS_GAIN, out float value);
				Audio.CheckALError("Could not read EAXReverbEffect.ReflectionsGain");
				return value;
			}
			set
			{
				Audio.CheckRange("EAXReverbEffect.ReflectionsGain", value, AL_EAXREVERB_MIN_REFLECTIONS_GAIN, AL_EAXREVERB_MAX_REFLECTIONS_GAIN);
				alEffectf(ID, AL_EAXREVERB_REFLECTIONS_GAIN, value);
				Audio.CheckALError("Could not update EAXReverbEffect.ReflectionsGain");
				Apply();
			}
		}

		/// <summary>
		/// 0.007f (0f - 0.3f) Seconds
		/// </summary>
		public float ReflectionsDelay
		{
			get
			{
				alGetEffectf(ID, AL_EAXREVERB_REFLECTIONS_DELAY, out float value);
				Audio.CheckALError("Could not read EAXReverbEffect.ReflectionsDelay");
				return value;
			}
			set
			{
				Audio.CheckRange("EAXReverbEffect.ReflectionsDelay", value, AL_EAXREVERB_MIN_REFLECTIONS_DELAY, AL_EAXREVERB_MAX_REFLECTIONS_DELAY);
				alEffectf(ID, AL_EAXREVERB_REFLECTIONS_DELAY, value);
				Audio.CheckALError("Could not update EAXReverbEffect.ReflectionsDelay");
				Apply();
			}
		}

		/// <summary>
		/// (0f, 0f, 0f) ( - ) (Magnitude 0f - 1f)
		/// </summary>
		public Vector3 ReflectionsPan
		{
			get
			{
				float[] values = new float[3];
				alGetEffectfv(ID, AL_EAXREVERB_REFLECTIONS_PAN, values);
				Audio.CheckALError("Could not read EAXReverbEffect.ReflectionsPan");
				return new Vector3(values[0], values[1], -values[2]);
			}
			set
			{
				Audio.CheckRange("EAXReverbEffect.ReflectionsPan", value, -1f, 1f);
				alEffectfv(ID, AL_EAXREVERB_REFLECTIONS_PAN, new float[] { value.X, value.Y, -value.Z });
				Audio.CheckALError("Could not update EAXReverbEffect.ReflectionsPan");
				Apply();
			}
		}

		/// <summary>
		/// 1.26f (0f - 10f)
		/// </summary>
		public float LateReverbGain
		{
			get
			{
				alGetEffectf(ID, AL_EAXREVERB_LATE_REVERB_GAIN, out float value);
				Audio.CheckALError("Could not read EAXReverbEffect.LateReverbGain");
				return value;
			}
			set
			{
				Audio.CheckRange("EAXReverbEffect.ReflectionsGain", value, AL_EAXREVERB_MIN_LATE_REVERB_GAIN, AL_EAXREVERB_MAX_LATE_REVERB_GAIN);
				alEffectf(ID, AL_EAXREVERB_LATE_REVERB_GAIN, value);
				Audio.CheckALError("Could not update EAXReverbEffect.LateReverbGain");
				Apply();
			}
		}

		/// <summary>
		/// 0.011f (0f - 0.1f) Seconds
		/// </summary>
		public float LateReverbDelay
		{
			get
			{
				alGetEffectf(ID, AL_EAXREVERB_LATE_REVERB_DELAY, out float value);
				Audio.CheckALError("Could not read EAXReverbEffect.LateReverbDelay");
				return value;
			}
			set
			{
				Audio.CheckRange("EAXReverbEffect.LateReverbDelay", value, AL_EAXREVERB_MIN_LATE_REVERB_DELAY, AL_EAXREVERB_MAX_LATE_REVERB_DELAY);
				alEffectf(ID, AL_EAXREVERB_LATE_REVERB_DELAY, value);
				Audio.CheckALError("Could not update EAXReverbEffect.LateReverbDelay");
				Apply();
			}
		}

		/// <summary>
		/// (0f, 0f, 0f) ( - ) (Magnitude 0f - 1f)
		/// </summary>
		public Vector3 LateReverbPan
		{
			get
			{
				float[] values = new float[3];
				alGetEffectfv(ID, AL_EAXREVERB_LATE_REVERB_PAN, values);
				Audio.CheckALError("Could not read EAXReverbEffect.LateReverbPan");
				return new Vector3(values[0], values[1], -values[2]);
			}
			set
			{
				Audio.CheckRange("EAXReverbEffect.LateReverbPan", value, -1f, 1f);
				alEffectfv(ID, AL_EAXREVERB_LATE_REVERB_PAN, new float[] { value.X, value.Y, -value.Z });
				Audio.CheckALError("Could not update EAXReverbEffect.LateReverbPan");
				Apply();
			}
		}

		/// <summary>
		/// 0.25f (0.075f - 0.25f)
		/// </summary>
		public float EchoTime
		{
			get
			{
				alGetEffectf(ID, AL_EAXREVERB_ECHO_TIME, out float value);
				Audio.CheckALError("Could not read EAXReverbEffect.EchoTime");
				return value;
			}
			set
			{
				Audio.CheckRange("EAXReverbEffect.EchoTime", value, AL_EAXREVERB_MIN_ECHO_TIME, AL_EAXREVERB_MAX_ECHO_TIME);
				alEffectf(ID, AL_EAXREVERB_ECHO_TIME, value);
				Audio.CheckALError("Could not update EAXReverbEffect.EchoTime");
				Apply();
			}
		}

		/// <summary>
		/// 0f (0f - 1f)
		/// </summary>
		public float EchoDepth
		{
			get
			{
				alGetEffectf(ID, AL_EAXREVERB_ECHO_DEPTH, out float value);
				Audio.CheckALError("Could not read EAXReverbEffect.EchoDepth");
				return value;
			}
			set
			{
				Audio.CheckRange("EAXReverbEffect.EchoDepth", value, AL_EAXREVERB_MIN_ECHO_DEPTH, AL_EAXREVERB_MAX_ECHO_DEPTH);
				alEffectf(ID, AL_EAXREVERB_ECHO_DEPTH, value);
				Audio.CheckALError("Could not update EAXReverbEffect.EchoDepth");
				Apply();
			}
		}

		/// <summary>
		/// 0.25f (0.04f - 4f)
		/// </summary>
		public float ModulationTime
		{
			get
			{
				alGetEffectf(ID, AL_EAXREVERB_MODULATION_TIME, out float value);
				Audio.CheckALError("Could not read EAXReverbEffect.ModulationTime");
				return value;
			}
			set
			{
				Audio.CheckRange("EAXReverbEffect.ModulationTime", value, AL_EAXREVERB_MIN_MODULATION_TIME, AL_EAXREVERB_MAX_MODULATION_TIME);
				alEffectf(ID, AL_EAXREVERB_MODULATION_TIME, value);
				Audio.CheckALError("Could not update EAXReverbEffect.ModulationTime");
				Apply();
			}
		}

		/// <summary>
		/// 0f (0f - 1f)
		/// </summary>
		public float ModulationDepth
		{
			get
			{
				alGetEffectf(ID, AL_EAXREVERB_MODULATION_DEPTH, out float value);
				Audio.CheckALError("Could not read EAXReverbEffect.ModulationDepth");
				return value;
			}
			set
			{
				Audio.CheckRange("EAXReverbEffect.ModulationDepth", value, AL_EAXREVERB_MIN_MODULATION_DEPTH, AL_EAXREVERB_MAX_MODULATION_DEPTH);
				alEffectf(ID, AL_EAXREVERB_MODULATION_DEPTH, value);
				Audio.CheckALError("Could not update EAXReverbEffect.ModulationDepth");
				Apply();
			}
		}

		/// <summary>
		/// 0.994f (0.892f - 1f)
		/// </summary>
		public float AirAbsorptionGainHF
		{
			get
			{
				alGetEffectf(ID, AL_EAXREVERB_AIR_ABSORPTION_GAINHF, out float value);
				Audio.CheckALError("Could not read EAXReverbEffect.AirAbsorptionGainHF");
				return value;
			}
			set
			{
				Audio.CheckRange("EAXReverbEffect.AirAbsorptionGainHF", value, AL_EAXREVERB_MIN_AIR_ABSORPTION_GAINHF, AL_EAXREVERB_MAX_AIR_ABSORPTION_GAINHF);
				alEffectf(ID, AL_EAXREVERB_AIR_ABSORPTION_GAINHF, value);
				Audio.CheckALError("Could not update EAXReverbEffect.AirAbsorptionGainHF");
				Apply();
			}
		}

		/// <summary>
		/// 5000f (1000f - 20000f) Hz
		/// </summary>
		public float HFReference
		{
			get
			{
				alGetEffectf(ID, AL_EAXREVERB_HFREFERENCE, out float value);
				Audio.CheckALError("Could not read EAXReverbEffect.HFReference");
				return value;
			}
			set
			{
				Audio.CheckRange("EAXReverbEffect.HFReference", value, AL_EAXREVERB_MIN_HFREFERENCE, AL_EAXREVERB_MAX_HFREFERENCE);
				alEffectf(ID, AL_EAXREVERB_HFREFERENCE, value);
				Audio.CheckALError("Could not update EAXReverbEffect.HFReference");
				Apply();
			}
		}

		/// <summary>
		/// 250f (20f - 1000f) Hz
		/// </summary>
		public float LFReference
		{
			get
			{
				alGetEffectf(ID, AL_EAXREVERB_LFREFERENCE, out float value);
				Audio.CheckALError("Could not read EAXReverbEffect.LFReference");
				return value;
			}
			set
			{
				Audio.CheckRange("EAXReverbEffect.LFReference", value, AL_EAXREVERB_MIN_LFREFERENCE, AL_EAXREVERB_MAX_LFREFERENCE);
				alEffectf(ID, AL_EAXREVERB_LFREFERENCE, value);
				Audio.CheckALError("Could not update EAXReverbEffect.LFReference");
				Apply();
			}
		}

		/// <summary>
		/// 0f (0f - 10f)
		/// </summary>
		public float RoomRolloffFactor
		{
			get
			{
				alGetEffectf(ID, AL_EAXREVERB_ROOM_ROLLOFF_FACTOR, out float value);
				Audio.CheckALError("Could not read EAXReverbEffect.RoomRolloffFactor");
				return value;
			}
			set
			{
				Audio.CheckRange("EAXReverbEffect.RoomRolloffFactor", value, AL_EAXREVERB_MIN_ROOM_ROLLOFF_FACTOR, AL_EAXREVERB_MAX_ROOM_ROLLOFF_FACTOR);
				alEffectf(ID, AL_EAXREVERB_ROOM_ROLLOFF_FACTOR, value);
				Audio.CheckALError("Could not update EAXReverbEffect.RoomRolloffFactor");
				Apply();
			}
		}

		/// <summary>
		/// true (false/true)
		/// </summary>
		public bool DecayHFLimit
		{
			get
			{
				alGetEffecti(ID, AL_EAXREVERB_DECAY_HFLIMIT, out int decayHFLimit);
				Audio.CheckALError("Could not read EAXReverbEffect.DecayHFLimit");
				return decayHFLimit == 1;
			}
			set
			{
				alEffecti(ID, AL_EAXREVERB_DECAY_HFLIMIT, value ? 1 : 0);
				Audio.CheckALError("Could not update EAXReverbEffect.DecayHFLimit");
				Apply();
			}
		}

		public void LoadPreset(Preset preset)
		{
			alEffectf(ID, AL_EAXREVERB_DENSITY, preset.Density);
			Audio.CheckALError("Could not update EAXReverbEffect.Density");

			alEffectf(ID, AL_EAXREVERB_DIFFUSION, preset.Diffusion);
			Audio.CheckALError("Could not update EAXReverbEffect.Diffusion");

			alEffectf(ID, AL_EAXREVERB_GAIN, preset.Gain);
			Audio.CheckALError("Could not update EAXReverbEffect.Gain");

			alEffectf(ID, AL_EAXREVERB_GAINHF, preset.GainHF);
			Audio.CheckALError("Could not update EAXReverbEffect.GainHF");

			alEffectf(ID, AL_EAXREVERB_GAINLF, preset.GainLF);
			Audio.CheckALError("Could not update EAXReverbEffect.GainLF");

			alEffectf(ID, AL_EAXREVERB_DECAY_TIME, preset.DecayTime);
			Audio.CheckALError("Could not update EAXReverbEffect.DecayTime");

			alEffectf(ID, AL_EAXREVERB_DECAY_HFRATIO, preset.DecayHFRatio);
			Audio.CheckALError("Could not update EAXReverbEffect.DecayHFRatio");

			alEffectf(ID, AL_EAXREVERB_DECAY_LFRATIO, preset.DecayLFRatio);
			Audio.CheckALError("Could not update EAXReverbEffect.DecayLFRatio");

			alEffectf(ID, AL_EAXREVERB_REFLECTIONS_GAIN, preset.ReflectionsGain);
			Audio.CheckALError("Could not update EAXReverbEffect.ReflectionsGain");

			alEffectf(ID, AL_EAXREVERB_REFLECTIONS_DELAY, preset.ReflectionsDelay);
			Audio.CheckALError("Could not update EAXReverbEffect.ReflectionsDelay");

			alEffectfv(ID, AL_EAXREVERB_REFLECTIONS_PAN, new float[] { preset.ReflectionsPan.X, preset.ReflectionsPan.Y, -preset.ReflectionsPan.Z });
			Audio.CheckALError("Could not update EAXReverbEffect.ReflectionsPan");

			alEffectf(ID, AL_EAXREVERB_LATE_REVERB_GAIN, preset.LateReverbGain);
			Audio.CheckALError("Could not update EAXReverbEffect.LateReverbGain");

			alEffectf(ID, AL_EAXREVERB_LATE_REVERB_DELAY, preset.LateReverbDelay);
			Audio.CheckALError("Could not update EAXReverbEffect.LateReverbDelay");

			alEffectfv(ID, AL_EAXREVERB_LATE_REVERB_PAN, new float[] { preset.LateReverbPan.X, preset.LateReverbPan.Y, -preset.LateReverbPan.Z });
			Audio.CheckALError("Could not update EAXReverbEffect.LateReverbPan");

			alEffectf(ID, AL_EAXREVERB_ECHO_TIME, preset.EchoTime);
			Audio.CheckALError("Could not update EAXReverbEffect.EchoTime");

			alEffectf(ID, AL_EAXREVERB_ECHO_DEPTH, preset.EchoDepth);
			Audio.CheckALError("Could not update EAXReverbEffect.EchoDepth");

			alEffectf(ID, AL_EAXREVERB_MODULATION_TIME, preset.ModulationTime);
			Audio.CheckALError("Could not update EAXReverbEffect.ModulationTime");

			alEffectf(ID, AL_EAXREVERB_MODULATION_DEPTH, preset.ModulationDepth);
			Audio.CheckALError("Could not update EAXReverbEffect.ModulationDepth");

			alEffectf(ID, AL_EAXREVERB_AIR_ABSORPTION_GAINHF, preset.AirAbsorptionGainHF);
			Audio.CheckALError("Could not update EAXReverbEffect.AirAbsorptionGainHF");

			alEffectf(ID, AL_EAXREVERB_HFREFERENCE, preset.HFReference);
			Audio.CheckALError("Could not update EAXReverbEffect.HFReference");

			alEffectf(ID, AL_EAXREVERB_LFREFERENCE, preset.LFReference);
			Audio.CheckALError("Could not update EAXReverbEffect.LFReference");

			alEffectf(ID, AL_EAXREVERB_ROOM_ROLLOFF_FACTOR, preset.RoomRolloffFactor);
			Audio.CheckALError("Could not update EAXReverbEffect.RoomRolloffFactor");

			alEffecti(ID, AL_EAXREVERB_DECAY_HFLIMIT, preset.DecayHFLimit ? 1 : 0);
			Audio.CheckALError("Could not update EAXReverbEffect.DecayHFLimit");

			Apply();
		}

		#region Presets
		public struct Preset
		{
			public float Density;
			public float Diffusion;
			public float Gain;
			public float GainHF;
			public float GainLF;
			public float DecayTime;
			public float DecayHFRatio;
			public float DecayLFRatio;
			public float ReflectionsGain;
			public float ReflectionsDelay;
			public Vector3 ReflectionsPan;
			public float LateReverbGain;
			public float LateReverbDelay;
			public Vector3 LateReverbPan;
			public float EchoTime;
			public float EchoDepth;
			public float ModulationTime;
			public float ModulationDepth;
			public float AirAbsorptionGainHF;
			public float HFReference;
			public float LFReference;
			public float RoomRolloffFactor;
			public bool DecayHFLimit;

			public Preset(float density, float diffusion, float gain, float gainHF, float gainLF, float decayTime, float decayHFRatio, float decayLFRatio, float reflectionsGain, float reflectionsDelay, Vector3 reflectionsPan, float lateReverbGain, float lateReverbDelay, Vector3 lateReverbPan, float echoTime, float echoDepth, float modulationTime, float modulationDepth, float airAbsorptionGainHF, float hfReference, float lfReference, float roomRolloffFactor, bool decayHFLimit)
			{
				Density = density;
				Diffusion = diffusion;
				Gain = gain;
				GainHF = gainHF;
				GainLF = gainLF;
				DecayTime = decayTime;
				DecayHFRatio = decayHFRatio;
				DecayLFRatio = decayLFRatio;
				ReflectionsGain = reflectionsGain;
				ReflectionsDelay = reflectionsDelay;
				ReflectionsPan = reflectionsPan;
				LateReverbGain = lateReverbGain;
				LateReverbDelay = lateReverbDelay;
				LateReverbPan = lateReverbPan;
				EchoTime = echoTime;
				EchoDepth = echoDepth;
				ModulationTime = modulationTime;
				ModulationDepth = modulationDepth;
				AirAbsorptionGainHF = airAbsorptionGainHF;
				HFReference = hfReference;
				LFReference = lfReference;
				RoomRolloffFactor = roomRolloffFactor;
				DecayHFLimit = decayHFLimit;
			}
		}

		// Default Presets
		public static readonly Preset Generic = new Preset(1.0000f, 1.0000f, 0.3162f, 0.8913f, 1.0000f, 1.4900f, 0.8300f, 1.0000f, 0.0500f, 0.0070f, new Vector3(0.0000f, 0.0000f, 0.0000f), 1.2589f, 0.0110f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, true);
		public static readonly Preset PaddedCell = new Preset(0.1715f, 1.0000f, 0.3162f, 0.0010f, 1.0000f, 0.1700f, 0.1000f, 1.0000f, 0.2500f, 0.0010f, new Vector3(0.0000f, 0.0000f, 0.0000f), 1.2691f, 0.0020f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, true);
		public static readonly Preset Room = new Preset(0.4287f, 1.0000f, 0.3162f, 0.5929f, 1.0000f, 0.4000f, 0.8300f, 1.0000f, 0.1503f, 0.0020f, new Vector3(0.0000f, 0.0000f, 0.0000f), 1.0629f, 0.0030f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, true);
		public static readonly Preset Bathroom = new Preset(0.1715f, 1.0000f, 0.3162f, 0.2512f, 1.0000f, 1.4900f, 0.5400f, 1.0000f, 0.6531f, 0.0070f, new Vector3(0.0000f, 0.0000f, 0.0000f), 3.2734f, 0.0110f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, true);
		public static readonly Preset LivingRoom = new Preset(0.9766f, 1.0000f, 0.3162f, 0.0010f, 1.0000f, 0.5000f, 0.1000f, 1.0000f, 0.2051f, 0.0030f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2805f, 0.0040f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, true);
		public static readonly Preset StoneRoom = new Preset(1.0000f, 1.0000f, 0.3162f, 0.7079f, 1.0000f, 2.3100f, 0.6400f, 1.0000f, 0.4411f, 0.0120f, new Vector3(0.0000f, 0.0000f, 0.0000f), 1.1003f, 0.0170f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, true);
		public static readonly Preset Auditorium = new Preset(1.0000f, 1.0000f, 0.3162f, 0.5781f, 1.0000f, 4.3200f, 0.5900f, 1.0000f, 0.4032f, 0.0200f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.7170f, 0.0300f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, true);
		public static readonly Preset ConcertHall = new Preset(1.0000f, 1.0000f, 0.3162f, 0.5623f, 1.0000f, 3.9200f, 0.7000f, 1.0000f, 0.2427f, 0.0200f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.9977f, 0.0290f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, true);
		public static readonly Preset Cave = new Preset(1.0000f, 1.0000f, 0.3162f, 1.0000f, 1.0000f, 2.9100f, 1.3000f, 1.0000f, 0.5000f, 0.0150f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.7063f, 0.0220f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, false);
		public static readonly Preset Arena = new Preset(1.0000f, 1.0000f, 0.3162f, 0.4477f, 1.0000f, 7.2400f, 0.3300f, 1.0000f, 0.2612f, 0.0200f, new Vector3(0.0000f, 0.0000f, 0.0000f), 1.0186f, 0.0300f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, true);
		public static readonly Preset Hangar = new Preset(1.0000f, 1.0000f, 0.3162f, 0.3162f, 1.0000f, 10.0500f, 0.2300f, 1.0000f, 0.5000f, 0.0200f, new Vector3(0.0000f, 0.0000f, 0.0000f), 1.2560f, 0.0300f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, true);
		public static readonly Preset CarpetedHallway = new Preset(0.4287f, 1.0000f, 0.3162f, 0.0100f, 1.0000f, 0.3000f, 0.1000f, 1.0000f, 0.1215f, 0.0020f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.1531f, 0.0300f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, true);
		public static readonly Preset Hallway = new Preset(0.3645f, 1.0000f, 0.3162f, 0.7079f, 1.0000f, 1.4900f, 0.5900f, 1.0000f, 0.2458f, 0.0070f, new Vector3(0.0000f, 0.0000f, 0.0000f), 1.6615f, 0.0110f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, true);
		public static readonly Preset StoneCorridor = new Preset(1.0000f, 1.0000f, 0.3162f, 0.7612f, 1.0000f, 2.7000f, 0.7900f, 1.0000f, 0.2472f, 0.0130f, new Vector3(0.0000f, 0.0000f, 0.0000f), 1.5758f, 0.0200f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, true);
		public static readonly Preset Alley = new Preset(1.0000f, 0.3000f, 0.3162f, 0.7328f, 1.0000f, 1.4900f, 0.8600f, 1.0000f, 0.2500f, 0.0070f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.9954f, 0.0110f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.1250f, 0.9500f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, true);
		public static readonly Preset Forest = new Preset(1.0000f, 0.3000f, 0.3162f, 0.0224f, 1.0000f, 1.4900f, 0.5400f, 1.0000f, 0.0525f, 0.1620f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.7682f, 0.0880f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.1250f, 1.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, true);
		public static readonly Preset City = new Preset(1.0000f, 0.5000f, 0.3162f, 0.3981f, 1.0000f, 1.4900f, 0.6700f, 1.0000f, 0.0730f, 0.0070f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.1427f, 0.0110f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, true);
		public static readonly Preset Mountains = new Preset(1.0000f, 0.2700f, 0.3162f, 0.0562f, 1.0000f, 1.4900f, 0.2100f, 1.0000f, 0.0407f, 0.3000f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.1919f, 0.1000f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 1.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, false);
		public static readonly Preset Quarry = new Preset(1.0000f, 1.0000f, 0.3162f, 0.3162f, 1.0000f, 1.4900f, 0.8300f, 1.0000f, 0.0000f, 0.0610f, new Vector3(0.0000f, 0.0000f, 0.0000f), 1.7783f, 0.0250f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.1250f, 0.7000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, true);
		public static readonly Preset Plain = new Preset(1.0000f, 0.2100f, 0.3162f, 0.1000f, 1.0000f, 1.4900f, 0.5000f, 1.0000f, 0.0585f, 0.1790f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.1089f, 0.1000f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 1.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, true);
		public static readonly Preset ParkingLot = new Preset(1.0000f, 1.0000f, 0.3162f, 1.0000f, 1.0000f, 1.6500f, 1.5000f, 1.0000f, 0.2082f, 0.0080f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2652f, 0.0120f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, false);
		public static readonly Preset SewerPipe = new Preset(0.3071f, 0.8000f, 0.3162f, 0.3162f, 1.0000f, 2.8100f, 0.1400f, 1.0000f, 1.6387f, 0.0140f, new Vector3(0.0000f, 0.0000f, 0.0000f), 3.2471f, 0.0210f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, true);
		public static readonly Preset Underwater = new Preset(0.3645f, 1.0000f, 0.3162f, 0.0100f, 1.0000f, 1.4900f, 0.1000f, 1.0000f, 0.5963f, 0.0070f, new Vector3(0.0000f, 0.0000f, 0.0000f), 7.0795f, 0.0110f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.0000f, 1.1800f, 0.3480f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, true);
		public static readonly Preset Drugged = new Preset(0.4287f, 0.5000f, 0.3162f, 1.0000f, 1.0000f, 8.3900f, 1.3900f, 1.0000f, 0.8760f, 0.0020f, new Vector3(0.0000f, 0.0000f, 0.0000f), 3.1081f, 0.0300f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.0000f, 0.2500f, 1.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, false);
		public static readonly Preset Dizzy = new Preset(0.3645f, 0.6000f, 0.3162f, 0.6310f, 1.0000f, 17.2300f, 0.5600f, 1.0000f, 0.1392f, 0.0200f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.4937f, 0.0300f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 1.0000f, 0.8100f, 0.3100f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, false);
		public static readonly Preset Psychotic = new Preset(0.0625f, 0.5000f, 0.3162f, 0.8404f, 1.0000f, 7.5600f, 0.9100f, 1.0000f, 0.4864f, 0.0200f, new Vector3(0.0000f, 0.0000f, 0.0000f), 2.4378f, 0.0300f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.0000f, 4.0000f, 1.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, false);

		// Castle Presets
		public static readonly Preset Castle_SmallRoom = new Preset(1.0000f, 0.8900f, 0.3162f, 0.3981f, 0.1000f, 1.2200f, 0.8300f, 0.3100f, 0.8913f, 0.0220f, new Vector3(0.0000f, 0.0000f, 0.0000f), 1.9953f, 0.0110f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.1380f, 0.0800f, 0.2500f, 0.0000f, 0.9943f, 5168.6001f, 139.5000f, 0.0000f, true);
		public static readonly Preset Castle_ShortPassage = new Preset(1.0000f, 0.8900f, 0.3162f, 0.3162f, 0.1000f, 2.3200f, 0.8300f, 0.3100f, 0.8913f, 0.0070f, new Vector3(0.0000f, 0.0000f, 0.0000f), 1.2589f, 0.0230f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.1380f, 0.0800f, 0.2500f, 0.0000f, 0.9943f, 5168.6001f, 139.5000f, 0.0000f, true);
		public static readonly Preset Castle_MediumRoom = new Preset(1.0000f, 0.9300f, 0.3162f, 0.2818f, 0.1000f, 2.0400f, 0.8300f, 0.4600f, 0.6310f, 0.0220f, new Vector3(0.0000f, 0.0000f, 0.0000f), 1.5849f, 0.0110f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.1550f, 0.0300f, 0.2500f, 0.0000f, 0.9943f, 5168.6001f, 139.5000f, 0.0000f, true);
		public static readonly Preset Castle_LargeRoom = new Preset(1.0000f, 0.8200f, 0.3162f, 0.2818f, 0.1259f, 2.5300f, 0.8300f, 0.5000f, 0.4467f, 0.0340f, new Vector3(0.0000f, 0.0000f, 0.0000f), 1.2589f, 0.0160f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.1850f, 0.0700f, 0.2500f, 0.0000f, 0.9943f, 5168.6001f, 139.5000f, 0.0000f, true);
		public static readonly Preset Castle_LongPassage = new Preset(1.0000f, 0.8900f, 0.3162f, 0.3981f, 0.1000f, 3.4200f, 0.8300f, 0.3100f, 0.8913f, 0.0070f, new Vector3(0.0000f, 0.0000f, 0.0000f), 1.4125f, 0.0230f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.1380f, 0.0800f, 0.2500f, 0.0000f, 0.9943f, 5168.6001f, 139.5000f, 0.0000f, true);
		public static readonly Preset Castle_Hall = new Preset(1.0000f, 0.8100f, 0.3162f, 0.2818f, 0.1778f, 3.1400f, 0.7900f, 0.6200f, 0.1778f, 0.0560f, new Vector3(0.0000f, 0.0000f, 0.0000f), 1.1220f, 0.0240f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5168.6001f, 139.5000f, 0.0000f, true);
		public static readonly Preset Castle_Cupboard = new Preset(1.0000f, 0.8900f, 0.3162f, 0.2818f, 0.1000f, 0.6700f, 0.8700f, 0.3100f, 1.4125f, 0.0100f, new Vector3(0.0000f, 0.0000f, 0.0000f), 3.5481f, 0.0070f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.1380f, 0.0800f, 0.2500f, 0.0000f, 0.9943f, 5168.6001f, 139.5000f, 0.0000f, true);
		public static readonly Preset Castle_Courtyard = new Preset(1.0000f, 0.4200f, 0.3162f, 0.4467f, 0.1995f, 2.1300f, 0.6100f, 0.2300f, 0.2239f, 0.1600f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.7079f, 0.0360f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.3700f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, false);
		public static readonly Preset Castle_Alcove = new Preset(1.0000f, 0.8900f, 0.3162f, 0.5012f, 0.1000f, 1.6400f, 0.8700f, 0.3100f, 1.0000f, 0.0070f, new Vector3(0.0000f, 0.0000f, 0.0000f), 1.4125f, 0.0340f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.1380f, 0.0800f, 0.2500f, 0.0000f, 0.9943f, 5168.6001f, 139.5000f, 0.0000f, true);

		// Factory Presets
		public static readonly Preset Factory_SmallRoom = new Preset(0.3645f, 0.8200f, 0.3162f, 0.7943f, 0.5012f, 1.7200f, 0.6500f, 1.3100f, 0.7079f, 0.0100f, new Vector3(0.0000f, 0.0000f, 0.0000f), 1.7783f, 0.0240f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.1190f, 0.0700f, 0.2500f, 0.0000f, 0.9943f, 3762.6001f, 362.5000f, 0.0000f, true);
		public static readonly Preset Factory_ShortPassage = new Preset(0.3645f, 0.6400f, 0.2512f, 0.7943f, 0.5012f, 2.5300f, 0.6500f, 1.3100f, 1.0000f, 0.0100f, new Vector3(0.0000f, 0.0000f, 0.0000f), 1.2589f, 0.0380f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.1350f, 0.2300f, 0.2500f, 0.0000f, 0.9943f, 3762.6001f, 362.5000f, 0.0000f, true);
		public static readonly Preset Factory_MediumRoom = new Preset(0.4287f, 0.8200f, 0.2512f, 0.7943f, 0.5012f, 2.7600f, 0.6500f, 1.3100f, 0.2818f, 0.0220f, new Vector3(0.0000f, 0.0000f, 0.0000f), 1.4125f, 0.0230f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.1740f, 0.0700f, 0.2500f, 0.0000f, 0.9943f, 3762.6001f, 362.5000f, 0.0000f, true);
		public static readonly Preset Factory_LargeRoom = new Preset(0.4287f, 0.7500f, 0.2512f, 0.7079f, 0.6310f, 4.2400f, 0.5100f, 1.3100f, 0.1778f, 0.0390f, new Vector3(0.0000f, 0.0000f, 0.0000f), 1.1220f, 0.0230f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2310f, 0.0700f, 0.2500f, 0.0000f, 0.9943f, 3762.6001f, 362.5000f, 0.0000f, true);
		public static readonly Preset Factory_LongPassage = new Preset(0.3645f, 0.6400f, 0.2512f, 0.7943f, 0.5012f, 4.0600f, 0.6500f, 1.3100f, 1.0000f, 0.0200f, new Vector3(0.0000f, 0.0000f, 0.0000f), 1.2589f, 0.0370f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.1350f, 0.2300f, 0.2500f, 0.0000f, 0.9943f, 3762.6001f, 362.5000f, 0.0000f, true);
		public static readonly Preset Factory_Hall = new Preset(0.4287f, 0.7500f, 0.3162f, 0.7079f, 0.6310f, 7.4300f, 0.5100f, 1.3100f, 0.0631f, 0.0730f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.8913f, 0.0270f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.0700f, 0.2500f, 0.0000f, 0.9943f, 3762.6001f, 362.5000f, 0.0000f, true);
		public static readonly Preset Factory_Cupboard = new Preset(0.3071f, 0.6300f, 0.2512f, 0.7943f, 0.5012f, 0.4900f, 0.6500f, 1.3100f, 1.2589f, 0.0100f, new Vector3(0.0000f, 0.0000f, 0.0000f), 1.9953f, 0.0320f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.1070f, 0.0700f, 0.2500f, 0.0000f, 0.9943f, 3762.6001f, 362.5000f, 0.0000f, true);
		public static readonly Preset Factory_Courtyard = new Preset(0.3071f, 0.5700f, 0.3162f, 0.3162f, 0.6310f, 2.3200f, 0.2900f, 0.5600f, 0.2239f, 0.1400f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.3981f, 0.0390f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.2900f, 0.2500f, 0.0000f, 0.9943f, 3762.6001f, 362.5000f, 0.0000f, true);
		public static readonly Preset Factory_Alcove = new Preset(0.3645f, 0.5900f, 0.2512f, 0.7943f, 0.5012f, 3.1400f, 0.6500f, 1.3100f, 1.4125f, 0.0100f, new Vector3(0.0000f, 0.0000f, 0.0000f), 1.0000f, 0.0380f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.1140f, 0.1000f, 0.2500f, 0.0000f, 0.9943f, 3762.6001f, 362.5000f, 0.0000f, true);

		// Ice Palace Presets
		public static readonly Preset IcePalace_SmallRoom = new Preset(1.0000f, 0.8400f, 0.3162f, 0.5623f, 0.2818f, 1.5100f, 1.5300f, 0.2700f, 0.8913f, 0.0100f, new Vector3(0.0000f, 0.0000f, 0.0000f), 1.4125f, 0.0110f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.1640f, 0.1400f, 0.2500f, 0.0000f, 0.9943f, 12428.5000f, 99.6000f, 0.0000f, true);
		public static readonly Preset IcePalace_ShortPassage = new Preset(1.0000f, 0.7500f, 0.3162f, 0.5623f, 0.2818f, 1.7900f, 1.4600f, 0.2800f, 0.5012f, 0.0100f, new Vector3(0.0000f, 0.0000f, 0.0000f), 1.1220f, 0.0190f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.1770f, 0.0900f, 0.2500f, 0.0000f, 0.9943f, 12428.5000f, 99.6000f, 0.0000f, true);
		public static readonly Preset IcePalace_MediumRoom = new Preset(1.0000f, 0.8700f, 0.3162f, 0.5623f, 0.4467f, 2.2200f, 1.5300f, 0.3200f, 0.3981f, 0.0390f, new Vector3(0.0000f, 0.0000f, 0.0000f), 1.1220f, 0.0270f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.1860f, 0.1200f, 0.2500f, 0.0000f, 0.9943f, 12428.5000f, 99.6000f, 0.0000f, true);
		public static readonly Preset IcePalace_LargeRoom = new Preset(1.0000f, 0.8100f, 0.3162f, 0.5623f, 0.4467f, 3.1400f, 1.5300f, 0.3200f, 0.2512f, 0.0390f, new Vector3(0.0000f, 0.0000f, 0.0000f), 1.0000f, 0.0270f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2140f, 0.1100f, 0.2500f, 0.0000f, 0.9943f, 12428.5000f, 99.6000f, 0.0000f, true);
		public static readonly Preset IcePalace_LongPassage = new Preset(1.0000f, 0.7700f, 0.3162f, 0.5623f, 0.3981f, 3.0100f, 1.4600f, 0.2800f, 0.7943f, 0.0120f, new Vector3(0.0000f, 0.0000f, 0.0000f), 1.2589f, 0.0250f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.1860f, 0.0400f, 0.2500f, 0.0000f, 0.9943f, 12428.5000f, 99.6000f, 0.0000f, true);
		public static readonly Preset IcePalace_Hall = new Preset(1.0000f, 0.7600f, 0.3162f, 0.4467f, 0.5623f, 5.4900f, 1.5300f, 0.3800f, 0.1122f, 0.0540f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.6310f, 0.0520f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2260f, 0.1100f, 0.2500f, 0.0000f, 0.9943f, 12428.5000f, 99.6000f, 0.0000f, true);
		public static readonly Preset IcePalace_Cupboard = new Preset(1.0000f, 0.8300f, 0.3162f, 0.5012f, 0.2239f, 0.7600f, 1.5300f, 0.2600f, 1.1220f, 0.0120f, new Vector3(0.0000f, 0.0000f, 0.0000f), 1.9953f, 0.0160f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.1430f, 0.0800f, 0.2500f, 0.0000f, 0.9943f, 12428.5000f, 99.6000f, 0.0000f, true);
		public static readonly Preset IcePalace_Courtyard = new Preset(1.0000f, 0.5900f, 0.3162f, 0.2818f, 0.3162f, 2.0400f, 1.2000f, 0.3800f, 0.3162f, 0.1730f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.3162f, 0.0430f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2350f, 0.4800f, 0.2500f, 0.0000f, 0.9943f, 12428.5000f, 99.6000f, 0.0000f, true);
		public static readonly Preset IcePalace_Alcove = new Preset(1.0000f, 0.8400f, 0.3162f, 0.5623f, 0.2818f, 2.7600f, 1.4600f, 0.2800f, 1.1220f, 0.0100f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.8913f, 0.0300f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.1610f, 0.0900f, 0.2500f, 0.0000f, 0.9943f, 12428.5000f, 99.6000f, 0.0000f, true);

		// Space Station Presets
		public static readonly Preset SpaceStation_SmallRoom = new Preset(0.2109f, 0.7000f, 0.3162f, 0.7079f, 0.8913f, 1.7200f, 0.8200f, 0.5500f, 0.7943f, 0.0070f, new Vector3(0.0000f, 0.0000f, 0.0000f), 1.4125f, 0.0130f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.1880f, 0.2600f, 0.2500f, 0.0000f, 0.9943f, 3316.1001f, 458.2000f, 0.0000f, true);
		public static readonly Preset SpaceStation_ShortPassage = new Preset(0.2109f, 0.8700f, 0.3162f, 0.6310f, 0.8913f, 3.5700f, 0.5000f, 0.5500f, 1.0000f, 0.0120f, new Vector3(0.0000f, 0.0000f, 0.0000f), 1.1220f, 0.0160f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.1720f, 0.2000f, 0.2500f, 0.0000f, 0.9943f, 3316.1001f, 458.2000f, 0.0000f, true);
		public static readonly Preset SpaceStation_MediumRoom = new Preset(0.2109f, 0.7500f, 0.3162f, 0.6310f, 0.8913f, 3.0100f, 0.5000f, 0.5500f, 0.3981f, 0.0340f, new Vector3(0.0000f, 0.0000f, 0.0000f), 1.1220f, 0.0350f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2090f, 0.3100f, 0.2500f, 0.0000f, 0.9943f, 3316.1001f, 458.2000f, 0.0000f, true);
		public static readonly Preset SpaceStation_LargeRoom = new Preset(0.3645f, 0.8100f, 0.3162f, 0.6310f, 0.8913f, 3.8900f, 0.3800f, 0.6100f, 0.3162f, 0.0560f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.8913f, 0.0350f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2330f, 0.2800f, 0.2500f, 0.0000f, 0.9943f, 3316.1001f, 458.2000f, 0.0000f, true);
		public static readonly Preset SpaceStation_LongPassage = new Preset(0.4287f, 0.8200f, 0.3162f, 0.6310f, 0.8913f, 4.6200f, 0.6200f, 0.5500f, 1.0000f, 0.0120f, new Vector3(0.0000f, 0.0000f, 0.0000f), 1.2589f, 0.0310f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.2300f, 0.2500f, 0.0000f, 0.9943f, 3316.1001f, 458.2000f, 0.0000f, true);
		public static readonly Preset SpaceStation_Hall = new Preset(0.4287f, 0.8700f, 0.3162f, 0.6310f, 0.8913f, 7.1100f, 0.3800f, 0.6100f, 0.1778f, 0.1000f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.6310f, 0.0470f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.2500f, 0.2500f, 0.0000f, 0.9943f, 3316.1001f, 458.2000f, 0.0000f, true);
		public static readonly Preset SpaceStation_Cupboard = new Preset(0.1715f, 0.5600f, 0.3162f, 0.7079f, 0.8913f, 0.7900f, 0.8100f, 0.5500f, 1.4125f, 0.0070f, new Vector3(0.0000f, 0.0000f, 0.0000f), 1.7783f, 0.0180f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.1810f, 0.3100f, 0.2500f, 0.0000f, 0.9943f, 3316.1001f, 458.2000f, 0.0000f, true);
		public static readonly Preset SpaceStation_Alcove = new Preset(0.2109f, 0.7800f, 0.3162f, 0.7079f, 0.8913f, 1.1600f, 0.8100f, 0.5500f, 1.4125f, 0.0070f, new Vector3(0.0000f, 0.0000f, 0.0000f), 1.0000f, 0.0180f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.1920f, 0.2100f, 0.2500f, 0.0000f, 0.9943f, 3316.1001f, 458.2000f, 0.0000f, true);

		// Wooden Galleon Presets
		public static readonly Preset Wooden_SmallRoom = new Preset(1.0000f, 1.0000f, 0.3162f, 0.1122f, 0.3162f, 0.7900f, 0.3200f, 0.8700f, 1.0000f, 0.0320f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.8913f, 0.0290f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 4705.0000f, 99.6000f, 0.0000f, true);
		public static readonly Preset Wooden_ShortPassage = new Preset(1.0000f, 1.0000f, 0.3162f, 0.1259f, 0.3162f, 1.7500f, 0.5000f, 0.8700f, 0.8913f, 0.0120f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.6310f, 0.0240f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 4705.0000f, 99.6000f, 0.0000f, true);
		public static readonly Preset Wooden_MediumRoom = new Preset(1.0000f, 1.0000f, 0.3162f, 0.1000f, 0.2818f, 1.4700f, 0.4200f, 0.8200f, 0.8913f, 0.0490f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.8913f, 0.0290f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 4705.0000f, 99.6000f, 0.0000f, true);
		public static readonly Preset Wooden_LargeRoom = new Preset(1.0000f, 1.0000f, 0.3162f, 0.0891f, 0.2818f, 2.6500f, 0.3300f, 0.8200f, 0.8913f, 0.0660f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.7943f, 0.0490f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 4705.0000f, 99.6000f, 0.0000f, true);
		public static readonly Preset Wooden_LongPassage = new Preset(1.0000f, 1.0000f, 0.3162f, 0.1000f, 0.3162f, 1.9900f, 0.4000f, 0.7900f, 1.0000f, 0.0200f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.4467f, 0.0360f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 4705.0000f, 99.6000f, 0.0000f, true);
		public static readonly Preset Wooden_Hall = new Preset(1.0000f, 1.0000f, 0.3162f, 0.0794f, 0.2818f, 3.4500f, 0.3000f, 0.8200f, 0.8913f, 0.0880f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.7943f, 0.0630f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 4705.0000f, 99.6000f, 0.0000f, true);
		public static readonly Preset Wooden_Cupboard = new Preset(1.0000f, 1.0000f, 0.3162f, 0.1413f, 0.3162f, 0.5600f, 0.4600f, 0.9100f, 1.1220f, 0.0120f, new Vector3(0.0000f, 0.0000f, 0.0000f), 1.1220f, 0.0280f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 4705.0000f, 99.6000f, 0.0000f, true);
		public static readonly Preset Wooden_Courtyard = new Preset(1.0000f, 0.6500f, 0.3162f, 0.0794f, 0.3162f, 1.7900f, 0.3500f, 0.7900f, 0.5623f, 0.1230f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.1000f, 0.0320f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 4705.0000f, 99.6000f, 0.0000f, true);
		public static readonly Preset Wooden_Alcove = new Preset(1.0000f, 1.0000f, 0.3162f, 0.1259f, 0.3162f, 1.2200f, 0.6200f, 0.9100f, 1.1220f, 0.0120f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.7079f, 0.0240f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 4705.0000f, 99.6000f, 0.0000f, true);

		// Sports Presets
		public static readonly Preset Sport_EmptyStadium = new Preset(1.0000f, 1.0000f, 0.3162f, 0.4467f, 0.7943f, 6.2600f, 0.5100f, 1.1000f, 0.0631f, 0.1830f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.3981f, 0.0380f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, true);
		public static readonly Preset Sport_SquashCourt = new Preset(1.0000f, 0.7500f, 0.3162f, 0.3162f, 0.7943f, 2.2200f, 0.9100f, 1.1600f, 0.4467f, 0.0070f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.7943f, 0.0110f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.1260f, 0.1900f, 0.2500f, 0.0000f, 0.9943f, 7176.8999f, 211.2000f, 0.0000f, true);
		public static readonly Preset Sport_SmallSwimmingPool = new Preset(1.0000f, 0.7000f, 0.3162f, 0.7943f, 0.8913f, 2.7600f, 1.2500f, 1.1400f, 0.6310f, 0.0200f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.7943f, 0.0300f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.1790f, 0.1500f, 0.8950f, 0.1900f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, false);
		public static readonly Preset Sport_LargeSwimmingPool = new Preset(1.0000f, 0.8200f, 0.3162f, 0.7943f, 1.0000f, 5.4900f, 1.3100f, 1.1400f, 0.4467f, 0.0390f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.5012f, 0.0490f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2220f, 0.5500f, 1.1590f, 0.2100f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, false);
		public static readonly Preset Sport_Gymnasium = new Preset(1.0000f, 0.8100f, 0.3162f, 0.4467f, 0.8913f, 3.1400f, 1.0600f, 1.3500f, 0.3981f, 0.0290f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.5623f, 0.0450f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.1460f, 0.1400f, 0.2500f, 0.0000f, 0.9943f, 7176.8999f, 211.2000f, 0.0000f, true);
		public static readonly Preset Sport_FullStadium = new Preset(1.0000f, 1.0000f, 0.3162f, 0.0708f, 0.7943f, 5.2500f, 0.1700f, 0.8000f, 0.1000f, 0.1880f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2818f, 0.0380f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, true);
		public static readonly Preset Sport_StadiumTannoy = new Preset(1.0000f, 0.7800f, 0.3162f, 0.5623f, 0.5012f, 2.5300f, 0.8800f, 0.6800f, 0.2818f, 0.2300f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.5012f, 0.0630f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.2000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, true);

		// Prefab Presets
		public static readonly Preset Prefab_Workshop = new Preset(0.4287f, 1.0000f, 0.3162f, 0.1413f, 0.3981f, 0.7600f, 1.0000f, 1.0000f, 1.0000f, 0.0120f, new Vector3(0.0000f, 0.0000f, 0.0000f), 1.1220f, 0.0120f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, false);
		public static readonly Preset Prefab_SchoolRoom = new Preset(0.4022f, 0.6900f, 0.3162f, 0.6310f, 0.5012f, 0.9800f, 0.4500f, 0.1800f, 1.4125f, 0.0170f, new Vector3(0.0000f, 0.0000f, 0.0000f), 1.4125f, 0.0150f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.0950f, 0.1400f, 0.2500f, 0.0000f, 0.9943f, 7176.8999f, 211.2000f, 0.0000f, true);
		public static readonly Preset Prefab_PractiseRoom = new Preset(0.4022f, 0.8700f, 0.3162f, 0.3981f, 0.5012f, 1.1200f, 0.5600f, 0.1800f, 1.2589f, 0.0100f, new Vector3(0.0000f, 0.0000f, 0.0000f), 1.4125f, 0.0110f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.0950f, 0.1400f, 0.2500f, 0.0000f, 0.9943f, 7176.8999f, 211.2000f, 0.0000f, true);
		public static readonly Preset Prefab_Outhouse = new Preset(1.0000f, 0.8200f, 0.3162f, 0.1122f, 0.1585f, 1.3800f, 0.3800f, 0.3500f, 0.8913f, 0.0240f, new Vector3(0.0000f, 0.0000f, -0.0000f), 0.6310f, 0.0440f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.1210f, 0.1700f, 0.2500f, 0.0000f, 0.9943f, 2854.3999f, 107.5000f, 0.0000f, false);
		public static readonly Preset Prefab_Caravan = new Preset(1.0000f, 1.0000f, 0.3162f, 0.0891f, 0.1259f, 0.4300f, 1.5000f, 1.0000f, 1.0000f, 0.0120f, new Vector3(0.0000f, 0.0000f, 0.0000f), 1.9953f, 0.0120f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, false);

		// Dome and Pipe Presets
		public static readonly Preset Dome_Tomb = new Preset(1.0000f, 0.7900f, 0.3162f, 0.3548f, 0.2239f, 4.1800f, 0.2100f, 0.1000f, 0.3868f, 0.0300f, new Vector3(0.0000f, 0.0000f, 0.0000f), 1.6788f, 0.0220f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.1770f, 0.1900f, 0.2500f, 0.0000f, 0.9943f, 2854.3999f, 20.0000f, 0.0000f, false);
		public static readonly Preset Pipe_Small = new Preset(1.0000f, 1.0000f, 0.3162f, 0.3548f, 0.2239f, 5.0400f, 0.1000f, 0.1000f, 0.5012f, 0.0320f, new Vector3(0.0000f, 0.0000f, 0.0000f), 2.5119f, 0.0150f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 2854.3999f, 20.0000f, 0.0000f, true);
		public static readonly Preset Dome_SaintPauls = new Preset(1.0000f, 0.8700f, 0.3162f, 0.3548f, 0.2239f, 10.4800f, 0.1900f, 0.1000f, 0.1778f, 0.0900f, new Vector3(0.0000f, 0.0000f, 0.0000f), 1.2589f, 0.0420f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.1200f, 0.2500f, 0.0000f, 0.9943f, 2854.3999f, 20.0000f, 0.0000f, true);
		public static readonly Preset Pipe_LongThin = new Preset(0.2560f, 0.9100f, 0.3162f, 0.4467f, 0.2818f, 9.2100f, 0.1800f, 0.1000f, 0.7079f, 0.0100f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.7079f, 0.0220f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 2854.3999f, 20.0000f, 0.0000f, false);
		public static readonly Preset Pipe_Large = new Preset(1.0000f, 1.0000f, 0.3162f, 0.3548f, 0.2239f, 8.4500f, 0.1000f, 0.1000f, 0.3981f, 0.0460f, new Vector3(0.0000f, 0.0000f, 0.0000f), 1.5849f, 0.0320f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 2854.3999f, 20.0000f, 0.0000f, true);
		public static readonly Preset Pipe_Resonant = new Preset(0.1373f, 0.9100f, 0.3162f, 0.4467f, 0.2818f, 6.8100f, 0.1800f, 0.1000f, 0.7079f, 0.0100f, new Vector3(0.0000f, 0.0000f, 0.0000f), 1.0000f, 0.0220f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 2854.3999f, 20.0000f, 0.0000f, false);

		// Outdoors Presets
		public static readonly Preset Outdoors_Backyard = new Preset(1.0000f, 0.4500f, 0.3162f, 0.2512f, 0.5012f, 1.1200f, 0.3400f, 0.4600f, 0.4467f, 0.0690f, new Vector3(0.0000f, 0.0000f, -0.0000f), 0.7079f, 0.0230f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2180f, 0.3400f, 0.2500f, 0.0000f, 0.9943f, 4399.1001f, 242.9000f, 0.0000f, false);
		public static readonly Preset Outdoors_RollingPlains = new Preset(1.0000f, 0.0000f, 0.3162f, 0.0112f, 0.6310f, 2.1300f, 0.2100f, 0.4600f, 0.1778f, 0.3000f, new Vector3(0.0000f, 0.0000f, -0.0000f), 0.4467f, 0.0190f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 1.0000f, 0.2500f, 0.0000f, 0.9943f, 4399.1001f, 242.9000f, 0.0000f, false);
		public static readonly Preset Outdoors_DeepCanyon = new Preset(1.0000f, 0.7400f, 0.3162f, 0.1778f, 0.6310f, 3.8900f, 0.2100f, 0.4600f, 0.3162f, 0.2230f, new Vector3(0.0000f, 0.0000f, -0.0000f), 0.3548f, 0.0190f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 1.0000f, 0.2500f, 0.0000f, 0.9943f, 4399.1001f, 242.9000f, 0.0000f, false);
		public static readonly Preset Outdoors_Creek = new Preset(1.0000f, 0.3500f, 0.3162f, 0.1778f, 0.5012f, 2.1300f, 0.2100f, 0.4600f, 0.3981f, 0.1150f, new Vector3(0.0000f, 0.0000f, -0.0000f), 0.1995f, 0.0310f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2180f, 0.3400f, 0.2500f, 0.0000f, 0.9943f, 4399.1001f, 242.9000f, 0.0000f, false);
		public static readonly Preset Outdoors_Valley = new Preset(1.0000f, 0.2800f, 0.3162f, 0.0282f, 0.1585f, 2.8800f, 0.2600f, 0.3500f, 0.1413f, 0.2630f, new Vector3(0.0000f, 0.0000f, -0.0000f), 0.3981f, 0.1000f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.3400f, 0.2500f, 0.0000f, 0.9943f, 2854.3999f, 107.5000f, 0.0000f, false);

		// Mood Presets
		public static readonly Preset Mood_Heaven = new Preset(1.0000f, 0.9400f, 0.3162f, 0.7943f, 0.4467f, 5.0400f, 1.1200f, 0.5600f, 0.2427f, 0.0200f, new Vector3(0.0000f, 0.0000f, 0.0000f), 1.2589f, 0.0290f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.0800f, 2.7420f, 0.0500f, 0.9977f, 5000.0000f, 250.0000f, 0.0000f, true);
		public static readonly Preset Mood_Hell = new Preset(1.0000f, 0.5700f, 0.3162f, 0.3548f, 0.4467f, 3.5700f, 0.4900f, 2.0000f, 0.0000f, 0.0200f, new Vector3(0.0000f, 0.0000f, 0.0000f), 1.4125f, 0.0300f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.1100f, 0.0400f, 2.1090f, 0.5200f, 0.9943f, 5000.0000f, 139.5000f, 0.0000f, false);
		public static readonly Preset Mood_Memory = new Preset(1.0000f, 0.8500f, 0.3162f, 0.6310f, 0.3548f, 4.0600f, 0.8200f, 0.5600f, 0.0398f, 0.0000f, new Vector3(0.0000f, 0.0000f, 0.0000f), 1.1220f, 0.0000f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.0000f, 0.4740f, 0.4500f, 0.9886f, 5000.0000f, 250.0000f, 0.0000f, false);

		// Driving Presets
		public static readonly Preset Driving_Commentator = new Preset(1.0000f, 0.0000f, 0.3162f, 0.5623f, 0.5012f, 2.4200f, 0.8800f, 0.6800f, 0.1995f, 0.0930f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2512f, 0.0170f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 1.0000f, 0.2500f, 0.0000f, 0.9886f, 5000.0000f, 250.0000f, 0.0000f, true);
		public static readonly Preset Driving_PitGarage = new Preset(0.4287f, 0.5900f, 0.3162f, 0.7079f, 0.5623f, 1.7200f, 0.9300f, 0.8700f, 0.5623f, 0.0000f, new Vector3(0.0000f, 0.0000f, 0.0000f), 1.2589f, 0.0160f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.1100f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, false);
		public static readonly Preset Driving_InCar_Racer = new Preset(0.0832f, 0.8000f, 0.3162f, 1.0000f, 0.7943f, 0.1700f, 2.0000f, 0.4100f, 1.7783f, 0.0070f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.7079f, 0.0150f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 10268.2002f, 251.0000f, 0.0000f, true);
		public static readonly Preset Driving_InCar_Sports = new Preset(0.0832f, 0.8000f, 0.3162f, 0.6310f, 1.0000f, 0.1700f, 0.7500f, 0.4100f, 1.0000f, 0.0100f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.5623f, 0.0000f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 10268.2002f, 251.0000f, 0.0000f, true);
		public static readonly Preset Driving_InCar_Luxury = new Preset(0.2560f, 1.0000f, 0.3162f, 0.1000f, 0.5012f, 0.1300f, 0.4100f, 0.4600f, 0.7943f, 0.0100f, new Vector3(0.0000f, 0.0000f, 0.0000f), 1.5849f, 0.0100f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 10268.2002f, 251.0000f, 0.0000f, true);
		public static readonly Preset Driving_FullGrandstand = new Preset(1.0000f, 1.0000f, 0.3162f, 0.2818f, 0.6310f, 3.0100f, 1.3700f, 1.2800f, 0.3548f, 0.0900f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.1778f, 0.0490f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 10420.2002f, 250.0000f, 0.0000f, false);
		public static readonly Preset Driving_EmptyGrandstand = new Preset(1.0000f, 1.0000f, 0.3162f, 1.0000f, 0.7943f, 4.6200f, 1.7500f, 1.4000f, 0.2082f, 0.0900f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2512f, 0.0490f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.0000f, 0.2500f, 0.0000f, 0.9943f, 10420.2002f, 250.0000f, 0.0000f, false);
		public static readonly Preset Driving_Tunnel = new Preset(1.0000f, 0.8100f, 0.3162f, 0.3981f, 0.8913f, 3.4200f, 0.9400f, 1.3100f, 0.7079f, 0.0510f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.7079f, 0.0470f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2140f, 0.0500f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 155.3000f, 0.0000f, true);

		// City Presets
		public static readonly Preset City_Streets = new Preset(1.0000f, 0.7800f, 0.3162f, 0.7079f, 0.8913f, 1.7900f, 1.1200f, 0.9100f, 0.2818f, 0.0460f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.1995f, 0.0280f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.2000f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, true);
		public static readonly Preset City_Subway = new Preset(1.0000f, 0.7400f, 0.3162f, 0.7079f, 0.8913f, 3.0100f, 1.2300f, 0.9100f, 0.7079f, 0.0460f, new Vector3(0.0000f, 0.0000f, 0.0000f), 1.2589f, 0.0280f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.1250f, 0.2100f, 0.2500f, 0.0000f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, true);
		public static readonly Preset City_Museum = new Preset(1.0000f, 0.8200f, 0.3162f, 0.1778f, 0.1778f, 3.2800f, 1.4000f, 0.5700f, 0.2512f, 0.0390f, new Vector3(0.0000f, 0.0000f, -0.0000f), 0.8913f, 0.0340f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.1300f, 0.1700f, 0.2500f, 0.0000f, 0.9943f, 2854.3999f, 107.5000f, 0.0000f, false);
		public static readonly Preset City_Library = new Preset(1.0000f, 0.8200f, 0.3162f, 0.2818f, 0.0891f, 2.7600f, 0.8900f, 0.4100f, 0.3548f, 0.0290f, new Vector3(0.0000f, 0.0000f, -0.0000f), 0.8913f, 0.0200f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.1300f, 0.1700f, 0.2500f, 0.0000f, 0.9943f, 2854.3999f, 107.5000f, 0.0000f, false);
		public static readonly Preset City_Underpass = new Preset(1.0000f, 0.8200f, 0.3162f, 0.4467f, 0.8913f, 3.5700f, 1.1200f, 0.9100f, 0.3981f, 0.0590f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.8913f, 0.0370f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.1400f, 0.2500f, 0.0000f, 0.9920f, 5000.0000f, 250.0000f, 0.0000f, true);
		public static readonly Preset City_Abandoned = new Preset(1.0000f, 0.6900f, 0.3162f, 0.7943f, 0.8913f, 3.2800f, 1.1700f, 0.9100f, 0.4467f, 0.0440f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2818f, 0.0240f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.2000f, 0.2500f, 0.0000f, 0.9966f, 5000.0000f, 250.0000f, 0.0000f, true);

		// Misc. Presets
		public static readonly Preset DustyRoom = new Preset(0.3645f, 0.5600f, 0.3162f, 0.7943f, 0.7079f, 1.7900f, 0.3800f, 0.2100f, 0.5012f, 0.0020f, new Vector3(0.0000f, 0.0000f, 0.0000f), 1.2589f, 0.0060f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2020f, 0.0500f, 0.2500f, 0.0000f, 0.9886f, 13046.0000f, 163.3000f, 0.0000f, true);
		public static readonly Preset Chapel = new Preset(1.0000f, 0.8400f, 0.3162f, 0.5623f, 1.0000f, 4.6200f, 0.6400f, 1.2300f, 0.4467f, 0.0320f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.7943f, 0.0490f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.2500f, 0.0000f, 0.2500f, 0.1100f, 0.9943f, 5000.0000f, 250.0000f, 0.0000f, true);
		public static readonly Preset SmallWaterRoom = new Preset(1.0000f, 0.7000f, 0.3162f, 0.4477f, 1.0000f, 1.5100f, 1.2500f, 1.1400f, 0.8913f, 0.0200f, new Vector3(0.0000f, 0.0000f, 0.0000f), 1.4125f, 0.0300f, new Vector3(0.0000f, 0.0000f, 0.0000f), 0.1790f, 0.1500f, 0.8950f, 0.1900f, 0.9920f, 5000.0000f, 250.0000f, 0.0000f, false);
		#endregion
	}
}