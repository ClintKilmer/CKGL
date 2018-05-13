namespace CKGL
{
	public static class Math
	{
		public const float Epsilon = float.Epsilon;
		public const float PI = (float)System.Math.PI;
		public const float HalfPI = (float)(System.Math.PI * 0.5);
		public const float Tau = (float)(System.Math.PI * 2.0);
		public const float DegreesToRadiansCoefficient = (float)(System.Math.PI / 180.0);
		public const float RadiansToDegreesCoefficient = (float)(180.0 / System.Math.PI);
		public const float DegreesToRotationsCoefficient = (float)(1.0 / 360.0);
		public const float RotationsToDegreesCoefficient = (float)(360.0);
		public const float RotationsToRadiansCoefficient = (float)(360.0 * DegreesToRadiansCoefficient);
		public const float RadiansToRotationsCoefficient = (float)(RadiansToDegreesCoefficient / 360.0);
		public const float ZeroTolerance = 1e-6f;

		public static float DegreesToRadians(float degrees)
		{
			return degrees * DegreesToRadiansCoefficient;
		}

		public static float RadiansToDegrees(float radians)
		{
			return radians * RadiansToDegreesCoefficient;
		}

		public static float DegreesToRotations(float degrees)
		{
			return degrees * DegreesToRotationsCoefficient;
		}

		public static float RotationsToDegrees(float rotations)
		{
			return rotations * RotationsToDegreesCoefficient;
		}

		public static float RotationsToRadians(float rotations)
		{
			return rotations * RotationsToRadiansCoefficient;
		}

		public static float RadiansToRotations(float radians)
		{
			return radians / RotationsToRadiansCoefficient;
		}

		public static int Min(int a, int b)
		{
			return (a < b ? a : b);
		}

		public static float Min(float a, float b)
		{
			return (a < b ? a : b);
		}

		public static int Max(int a, int b)
		{
			return (a > b ? a : b);
		}

		public static float Max(float a, float b)
		{
			return (a > b ? a : b);
		}

		public static int Clamp(int i, int min, int max)
		{
			return (i > max ? max : (i < min ? min : i));

			//if (i > max)
			//	return max;
			//if (i < min)
			//	return min;
			//return i;
		}

		public static float Clamp(float f, float min, float max)
		{
			return (f > max ? max : (f < min ? min : f));

			//if (f > max)
			//	return max;
			//if (f < min)
			//	return min;
			//return f;
		}

		public static float Lerp(float a, float b, float t)
		{
			//return a + t * (b - a);// Imprecise method, which does not guarantee a = b when t = 1.
			return (1f - t) * a + t * b; // Precise method, which guarantees a = b when t = 1.
		}

		public static float Round(float x)
		{
			return (int)(x + 0.5f);
		}

		public static int RoundToInt(float x)
		{
			return (int)(x + 0.5f);
		}

		public static float Ceil(float x)
		{
			return (int)(x + 1f);
		}

		public static int CeilToInt(float x)
		{
			return (int)(x + 1f);
		}

		public static float Floor(float x)
		{
			return (int)x;
		}

		public static int FloorToInt(float x)
		{
			return (int)x;
		}

		public static int Sign(int x)
		{
			return x > 0 ? 1 : (x < 0 ? -1 : 0);
		}

		public static int Sign(float x)
		{
			return x > 0f ? 1 : (x < 0f ? -1 : 0);
		}

		public static bool SameSign(int x, int y)
		{
			return x > 0 ? y > 0 : (x < 0 ? y < 0 : y == 0);
		}

		public static bool SameSign(float x, float y)
		{
			return x > 0f ? y > 0f : (x < 0f ? y < 0f : y == 0f);
		}

		public static bool InRange(int x, int min, int max)
		{
			return (min < max ? (x >= min && x <= max) : (x >= max && x <= min));
		}

		public static bool InRange(float x, float min, float max)
		{
			return (min < max ? (x >= min && x <= max) : (x >= max && x <= min));
		}

		public static float Approach(float a, float b, float amount)
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

		public static int Abs(int x)
		{
			return x >= 0 ? x : -x;
		}

		public static float Abs(float x)
		{
			return x >= 0f ? x : -x;
		}

		public static float Pow(float x, float pow)
		{
			return (float)System.Math.Pow(x, pow);
		}

		public static float Sqrt(float x)
		{
			return (float)System.Math.Sqrt(x);
		}

		public static float Sin(float x)
		{
			return (float)System.Math.Sin(x);
		}

		public static float SinNormalized(float x)
		{
			return (float)((System.Math.Sin(x) + 1.0) * 0.5);
		}

		public static float Asin(float x)
		{
			return (float)System.Math.Asin(x);
		}

		public static float Cos(float x)
		{
			return (float)System.Math.Cos(x);
		}

		public static float CosNormalized(float x)
		{
			return (float)((System.Math.Cos(x) + 1.0) * 0.5);
		}

		public static float Acos(float x)
		{
			return (float)System.Math.Acos(x);
		}

		public static float Tan(float x)
		{
			return (float)System.Math.Tan(x);
		}

		public static float Atan(float x)
		{
			return (float)System.Math.Atan(x);
		}

		public static float Atan2(float y, float x)
		{
			return (float)System.Math.Atan2(y, x);
		}
	}
}