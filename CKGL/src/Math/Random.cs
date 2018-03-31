﻿namespace CKGL
{
	public static class Random
	{
		private static System.Random random = new System.Random();

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

		public static float Chance(float percentile)
		{
			return (float)random.NextDouble() * percentile;
		}
	}
}