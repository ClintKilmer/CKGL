using static OpenAL.Bindings;

namespace CKGL
{
	public static class AudioListener
	{
		public static Vector3 Position
		{
			get
			{
				alGetListener3f(alListener3fParameter.Position, out float x, out float y, out float z);
				Audio.CheckALError("Could not read AudioListener.Position");
				return new Vector3(x, y, -z);
			}
			set
			{
				alListener3f(alListener3fParameter.Position, value.X, value.Y, -value.Z);
				Audio.CheckALError("Could not update AudioListener.Position");
			}
		}

		public static Vector3 Velocity
		{
			get
			{
				alGetListener3f(alListener3fParameter.Velocity, out float x, out float y, out float z);
				Audio.CheckALError("Could not read AudioListener.Velocity");
				return new Vector3(x, y, -z);
			}
			set
			{
				alListener3f(alListener3fParameter.Velocity, value.X, value.Y, -value.Z);
				Audio.CheckALError("Could not update AudioListener.Velocity");
			}
		}

		public static (Vector3 Forward, Vector3 Up) Orientation
		{
			get
			{
				float[] orientation = new float[6];
				alGetListenerfv(alListenerfvParameter.Orientation, orientation);
				Audio.CheckALError("Could not read AudioListener.Orientation");
				return (new Vector3(orientation[0], orientation[1], -orientation[2]), new Vector3(orientation[3], orientation[4], -orientation[5]));
			}
			set
			{
				alListenerfv(alListenerfvParameter.Orientation, new float[] { value.Forward.X, value.Forward.Y, -value.Forward.Z, value.Up.X, value.Up.Y, -value.Up.Z });
				Audio.CheckALError("Could not update AudioListener.Orientation");
			}
		}

		public static float Gain
		{
			get
			{
				alGetListenerf(alListenerfParameter.Gain, out float gain);
				Audio.CheckALError("Could not read AudioListener.Gain");
				return gain;
			}
			set
			{
				alListenerf(alListenerfParameter.Gain, value);
				Audio.CheckALError("Could not update AudioListener.Gain");
			}
		}

		public static float MetersPerUnit
		{
			get
			{
				alGetListenerf(AL_METERS_PER_UNIT, out float value);
				Audio.CheckALError("Could not read AudioListener.MetersPerUnit");
				return value;
			}
			set
			{
				Audio.CheckRange("AudioListener.MetersPerUnit", value, AL_MIN_METERS_PER_UNIT, AL_MAX_METERS_PER_UNIT);
				alListenerf(AL_METERS_PER_UNIT, value);
				Audio.CheckALError("Could not update AudioListener.MetersPerUnit");
			}
		}
		public static readonly float MetersPerUnitDefault = AL_DEFAULT_METERS_PER_UNIT;
	}
}