namespace CKGL
{
	public static class Random
	{
		public static int Seed { get; private set; } = System.DateTime.Now.Millisecond;

		private static System.Random random = new System.Random(Seed);

		public static void SetSeed(int seed)
		{
			Seed = seed;
			random = new System.Random(seed);
		}

		public static int Range(int max)
		{
			return random.Next(max + 1);
		}

		public static int Range(int min, int max)
		{
			return random.Next(min, max + 1);
		}

		public static float Range(float max)
		{
			return Math.Lerp(0f, max, (float)random.NextDouble());
		}

		public static float Range(float min, float max)
		{
			return Math.Lerp(min, max, (float)random.NextDouble());
		}

		public static bool Chance(float percentile)
		{
			return (float)random.NextDouble() <= percentile;
		}

		public static T Choose<T>(params T[] options)
		{
			return options[random.Next(options.Length)];
		}
	}
}