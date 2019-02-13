namespace CKGL
{
	public static partial class Extensions
	{
		public static float DegreesToRadians(this float degrees)
		{
			return degrees * Math.DegreesToRadiansCoefficient;
		}

		public static float RadiansToDegrees(this float radians)
		{
			return radians * Math.RadiansToDegreesCoefficient;
		}

		public static float DegreesToRotations(this float degrees)
		{
			return degrees * Math.DegreesToRotationsCoefficient;
		}

		public static float RotationsToDegrees(this float rotations)
		{
			return rotations * Math.RotationsToDegreesCoefficient;
		}

		public static float RotationsToRadians(this float rotations)
		{
			return rotations * Math.RotationsToRadiansCoefficient;
		}

		public static float RadiansToRotations(this float radians)
		{
			return radians / Math.RotationsToRadiansCoefficient;
		}

		internal static int Min(this int a, int b)
		{
			return (a < b ? a : b);
		}

		public static float Min(this float a, float b)
		{
			return (a < b ? a : b);
		}

		public static int Max(this int a, int b)
		{
			return (a > b ? a : b);
		}

		public static float Max(this float a, float b)
		{
			return (a > b ? a : b);
		}

		public static int Clamp(this int i, int min, int max)
		{
			return (i > max ? max : (i < min ? min : i));

			//if (i > max)
			//	return max;
			//if (i < min)
			//	return min;
			//return i;
		}

		public static float Clamp(this float f, float min, float max)
		{
			return (f > max ? max : (f < min ? min : f));

			//if (f > max)
			//	return max;
			//if (f < min)
			//	return min;
			//return f;
		}

		public static double Clamp(this double d, double min, double max)
		{
			return (d > max ? max : (d < min ? min : d));

			//if (d > max)
			//	return max;
			//if (d < min)
			//	return min;
			//return d;
		}

		public static float Lerp(this float a, float b, float t)
		{
			//return a + t * (b - a);// Imprecise method, which does not guarantee a = b when t = 1.
			return (1f - t) * a + t * b; // Precise method, which guarantees a = b when t = 1.
		}

		public static float Eerp(this float a, float b, float t)
		{
			return a * Pow(b / a, t);
		}

		public static float Round(this float x)
		{
			return (int)(x + 0.5f);
		}

		public static int RoundToInt(this float x)
		{
			return (int)(x + 0.5f);
		}

		public static float Ceil(this float x)
		{
			return (int)(x + 1f);
		}

		public static int CeilToInt(this float x)
		{
			return (int)(x + 1f);
		}

		public static float Floor(this float x)
		{
			return (int)x;
		}

		public static int FloorToInt(this float x)
		{
			return (int)x;
		}

		public static int Sign(this int x)
		{
			return x > 0 ? 1 : (x < 0 ? -1 : 0);
		}

		public static int Sign(this float x)
		{
			return x > 0f ? 1 : (x < 0f ? -1 : 0);
		}

		public static bool SameSign(this int x, int y)
		{
			return x > 0 ? y > 0 : (x < 0 ? y < 0 : y == 0);
		}

		public static bool SameSign(this float x, float y)
		{
			return x > 0f ? y > 0f : (x < 0f ? y < 0f : y == 0f);
		}

		public static bool InRange(this int x, int min, int max)
		{
			return (min < max ? (x >= min && x <= max) : (x >= max && x <= min));
		}

		public static bool InRange(this float x, float min, float max)
		{
			return (min < max ? (x >= min && x <= max) : (x >= max && x <= min));
		}

		public static float Approach(this float a, float b, float amount)
		{
			if (a < b)
			{
				a += amount;
				return a > b ? b : a;
			}
			if (a > b)
			{
				a -= amount;
				return a < b ? b : a;
			}
			return b;
		}

		public static int Abs(this int x)
		{
			return x >= 0 ? x : -x;
		}

		public static float Abs(this float x)
		{
			return x >= 0f ? x : -x;
		}

		public static float Pow(this float x, float power)
		{
			return (float)System.Math.Pow(x, power);
		}

		public static float Sqrt(this float x)
		{
			return (float)System.Math.Sqrt(x);
		}

		public static float Sin(this float x)
		{
			return (float)System.Math.Sin(x);
		}

		public static float SinNormalized(this float x)
		{
			return (float)((System.Math.Sin(x) + 1.0) * 0.5);
		}

		public static float Asin(this float x)
		{
			return (float)System.Math.Asin(x);
		}

		public static float Cos(this float x)
		{
			return (float)System.Math.Cos(x);
		}

		public static float CosNormalized(this float x)
		{
			return (float)((System.Math.Cos(x) + 1.0) * 0.5);
		}

		public static float Acos(this float x)
		{
			return (float)System.Math.Acos(x);
		}

		public static float Tan(this float x)
		{
			return (float)System.Math.Tan(x);
		}

		public static float Atan(this float x)
		{
			return (float)System.Math.Atan(x);
		}

		public static float Atan2(this float y, float x)
		{
			return (float)System.Math.Atan2(y, x);
		}
	}
}