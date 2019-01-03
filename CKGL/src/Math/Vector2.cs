using System.Runtime.InteropServices;

namespace CKGL
{
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct Vector2
	{
		public float X;
		public float Y;

		#region Constructors
		public Vector2(float x, float y)
		{
			X = x;
			Y = y;
		}
		public Vector2(float value)
		{
			X = value;
			Y = value;
		}
		#endregion

		#region Static Constructors
		public static readonly Vector2 Zero = new Vector2(0f);
		public static readonly Vector2 One = new Vector2(1f);
		public static readonly Vector2 Right = new Vector2(1f, 0f);
		public static readonly Vector2 Left = new Vector2(-1f, 0f);
		public static readonly Vector2 Up = new Vector2(0f, 1f);
		public static readonly Vector2 Down = new Vector2(0f, -1f);
		#endregion

		#region Properties
		public Vector2 Normalized
		{
			get
			{
				float num = Magnitude(this);
				if (num > 1E-05f)
					return this / num;
				else
					return Zero;
			}
		}
		#endregion

		#region Methods
		public Vector2 Round()
		{
			return new Vector2(X.Round(), Y.Round());
		}

		public Vector2 Floor()
		{
			return new Vector2(X.Floor(), Y.Floor());
		}

		public Vector2 Ceil()
		{
			return new Vector2(X.Ceil(), Y.Ceil());
		}

		public Vector2 Lerp(Vector2 b, float t)
		{
			return new Vector2(X.Lerp(b.X, t), Y.Lerp(b.Y, t));
		}

		public Vector2 Eerp(Vector2 b, float t)
		{
			return new Vector2(X.Eerp(b.X, t), Y.Eerp(b.Y, t));
		}

		public float Magnitude()
		{
			return Math.Sqrt(X * X + Y * Y);
		}

		public float SqrMagnitude()
		{
			return X * X + Y * Y;
		}

		public Vector2 Normalize()
		{
			float num = Magnitude(this);
			if (num > 1E-05f)
				this /= num;
			else
				this = Zero;

			return this;
		}
		#endregion

		#region Static Methods
		public static Vector2 Round(Vector2 v)
		{
			return new Vector2(v.X.Round(), v.Y.Round());
		}

		public static Vector2 Floor(Vector2 v)
		{
			return new Vector2(v.X.Floor(), v.Y.Floor());
		}

		public static Vector2 Ceil(Vector2 v)
		{
			return new Vector2(v.X.Ceil(), v.Y.Ceil());
		}

		public static Vector2 Lerp(Vector2 a, Vector2 b, float t)
		{
			return new Vector2(a.X.Lerp(b.X, t), a.Y.Lerp(b.Y, t));
		}

		public static Vector2 Eerp(Vector2 a, Vector2 b, float t)
		{
			return new Vector2(a.X.Eerp(b.X, t), a.Y.Eerp(b.Y, t));
		}

		public static float Magnitude(Vector2 v)
		{
			return Math.Sqrt(v.X * v.X + v.Y * v.Y);
		}

		public static float MagnitudeSquared(Vector2 v)
		{
			return v.X * v.X + v.Y * v.Y;
		}

		public static Vector2 Normalize(Vector2 v)
		{
			float num = Magnitude(v);
			if (num > 1E-05f)
				v /= num;
			else
				v = Zero;

			return v;
		}
		#endregion

		#region Overrides
		public override string ToString()
		{
			return $"{X}, {Y}";
		}
		#endregion

		#region Operators
		public static bool operator ==(Vector2 a, Vector2 b)
		{
			return a.X == b.X && a.Y == b.Y;
		}
		public static bool operator !=(Vector2 a, Vector2 b)
		{
			return a.X != b.X || a.Y != b.Y;
		}

		public static Vector2 operator +(Vector2 a, Vector2 b)
		{
			a.X += b.X;
			a.Y += b.Y;
			return a;
		}

		public static Vector2 operator -(Vector2 a, Vector2 b)
		{
			a.X -= b.X;
			a.Y -= b.Y;
			return a;
		}

		public static Vector2 operator *(Vector2 a, Vector2 b)
		{
			a.X *= b.X;
			a.Y *= b.Y;
			return a;
		}
		public static Vector2 operator *(Vector2 v, float n)
		{
			v.X *= n;
			v.Y *= n;
			return v;
		}
		public static Vector2 operator *(float n, Vector2 v)
		{
			v.X *= n;
			v.Y *= n;
			return v;
		}
		// Pre-multiplication only
		public static Vector2 operator *(Vector2 v, Matrix2D m)
		{
			return new Vector2(
				v.X * m.M11 + v.Y * m.M21 + m.M31,
				v.X * m.M12 + v.Y * m.M22 + m.M32
			);
		}
		// Pre-multiplication conversion from Post-multiplication order
		public static Vector2 operator *(Matrix2D m, Vector2 v)
		{
			return new Vector2(
				v.X * m.M11 + v.Y * m.M21 + m.M31,
				v.X * m.M12 + v.Y * m.M22 + m.M32
			);
		}
		// Pre-multiplication only
		public static Vector3 operator *(Vector2 v, Quaternion q)
		{
			return v * q.Matrix2D;
		}
		// Pre-multiplication conversion from Post-multiplication order
		public static Vector3 operator *(Quaternion q, Vector2 v)
		{
			return v * q.Matrix2D;
		}

		public static Vector2 operator /(Vector2 a, Vector2 b)
		{
			a.X /= b.X;
			a.Y /= b.Y;
			return a;
		}
		public static Vector2 operator /(Vector2 v, float n)
		{
			v.X /= n;
			v.Y /= n;
			return v;
		}

		public static Vector2 operator -(Vector2 v)
		{
			v.X = -v.X;
			v.Y = -v.Y;
			return v;
		}
		#endregion

		#region Implicit Convertion Operators
		public static implicit operator Vector2(Point2 p)
		{
			return new Vector2(p.X, p.Y);
		}

		public static implicit operator Vector2(Vector3 v)
		{
			return new Vector2(v.X, v.Y);
		}

		public static implicit operator Vector2(Vector4 v)
		{
			return new Vector2(v.X, v.Y);
		}

		public static implicit operator Vector2(UV uv)
		{
			return new Vector2(uv.U, uv.V);
		}
		#endregion
	}
}