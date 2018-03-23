namespace CKGL
{
	public static class Math
	{
		public const float Epsilon = float.Epsilon;
		public const float PI = (float)System.Math.PI;
		public const float HalfPI = (float)(System.Math.PI * 0.5);
		public const float Tau = (float)(System.Math.PI * 2.0);
		public const float Deg = (float)(180.0 / System.Math.PI);
		public const float Rad = (float)(System.Math.PI / 180.0);
		public const float ZeroTolerance = 1e-6f;

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

		public static float Clamp(float f, int min, int max)
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

		public static float Sign(float x)
		{
			return x > 0f ? 1f : (x < 0f ? -1f : 0f);
		}

		public static int SignInt(float x)
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
	}
}