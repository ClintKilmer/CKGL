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
				if (Audio.CheckALError())
					Output.WriteLine("OpenAL Error: Could not read AudioListener Position");
				return new Vector3(x, y, -z);
			}
			set
			{
				alListener3f(alListener3fParameter.Position, value.X, value.Y, -value.Z);
				if (Audio.CheckALError())
					Output.WriteLine("OpenAL Error: Could not update AudioListener Position");
			}
		}

		public static Vector3 Velocity
		{
			get
			{
				alGetListener3f(alListener3fParameter.Velocity, out float x, out float y, out float z);
				if (Audio.CheckALError())
					Output.WriteLine("OpenAL Error: Could not read AudioListener Velocity");
				return new Vector3(x, y, -z);
			}
			set
			{
				alListener3f(alListener3fParameter.Velocity, value.X, value.Y, -value.Z);
				if (Audio.CheckALError())
					Output.WriteLine("OpenAL Error: Could not update AudioListener Velocity");
			}
		}

		public static (Vector3 Forward, Vector3 Up) Orientation
		{
			get
			{
				float[] orientation = new float[6];
				alGetListenerfv(alListenerfvParameter.Orientation, orientation);
				if (Audio.CheckALError())
					Output.WriteLine("OpenAL Error: Could not read AudioListener Orientation");
				return (new Vector3(orientation[0], orientation[1], -orientation[2]), new Vector3(orientation[3], orientation[4], -orientation[5]));
			}
			set
			{
				alListenerfv(alListenerfvParameter.Orientation, new float[] { value.Forward.X, value.Forward.Y, -value.Forward.Z, value.Up.X, value.Up.Y, -value.Up.Z });
				if (Audio.CheckALError())
					Output.WriteLine("OpenAL Error: Could not update AudioListener Orientation");
			}
		}

		public static float Gain
		{
			get
			{
				alGetListenerf(alListenerfParameter.Gain, out float gain);
				if (Audio.CheckALError())
					Output.WriteLine("OpenAL Error: Could not read AudioListener Gain");
				return gain;
			}
			set
			{
				alListenerf(alListenerfParameter.Gain, value);
				if (Audio.CheckALError())
					Output.WriteLine("OpenAL Error: Could not update AudioListener Gain");
			}
		}

		public static float MetersPerUnit
		{
			get
			{
				alGetListenerf(AL_METERS_PER_UNIT, out float value);
				if (Audio.CheckALError())
					Output.WriteLine("OpenAL Error: Could not read AudioListener MetersPerUnit");
				return value;
			}
			set
			{
				if (value < AL_MIN_METERS_PER_UNIT || value > AL_MAX_METERS_PER_UNIT)
					throw new CKGLException($"Listener.MetersPerUnit must be between {AL_MIN_METERS_PER_UNIT} and {AL_MAX_METERS_PER_UNIT}. Attempted value = {value}");
				alListenerf(AL_METERS_PER_UNIT, value);
				if (Audio.CheckALError())
					Output.WriteLine("OpenAL Error: Could not update AudioListener MetersPerUnit");
			}
		}

		public static void Reset()
		{
			Position = Vector3.Zero;
			Velocity = Vector3.Zero;
			Orientation = (Vector3.Forward, Vector3.Up);
			Gain = 1f;
			MetersPerUnit = AL_DEFAULT_METERS_PER_UNIT;
		}
	}
}