using static OpenAL.Bindings;

namespace CKGL
{
	public static class AudioListener
	{
		/// <summary>
		/// (0f, 0f, 0f) ( - )
		/// </summary>
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

		/// <summary>
		/// (0f, 0f, 0f) ( - )
		/// </summary>
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

		/// <summary>
		/// {(0f, 0f, 1f), (0f, 1f, 0f)} ( - )
		/// </summary>
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

		/// <summary>
		/// 1f (0f - )
		/// </summary>
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
				Audio.CheckRange("AudioListener.Gain", value, 0f, float.MaxValue);
				alListenerf(alListenerfParameter.Gain, value);
				Audio.CheckALError("Could not update AudioListener.Gain");
			}
		}

		/// <summary>
		/// 1f (float.Epsilon - )
		/// </summary>
		public static float MetersPerUnit
		{
			get
			{
				alGetListenerf(AL_METERS_PER_UNIT, out float metersPerUnit);
				Audio.CheckALError("Could not read AudioListener.MetersPerUnit");
				return metersPerUnit;
			}
			set
			{
				Audio.CheckRange("AudioListener.MetersPerUnit", value, AL_MIN_METERS_PER_UNIT, AL_MAX_METERS_PER_UNIT);
				alListenerf(AL_METERS_PER_UNIT, value);
				Audio.CheckALError("Could not update AudioListener.MetersPerUnit");
			}
		}

		public static void Init()
		{
			// Set default as we are converting the right-handedness
			// of OpenAL Soft to the left-handedness of CKGL
			AudioListener.Orientation = (Vector3.Forward, Vector3.Up);
		}
	}
}