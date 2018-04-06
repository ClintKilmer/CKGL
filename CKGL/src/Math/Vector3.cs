﻿using System.Runtime.InteropServices;

namespace CKGL
{
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct Vector3
	{
		public float X;
		public float Y;
		public float Z;

		#region Constructors
		public Vector3(float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
		}
		public Vector3(float value)
		{
			X = value;
			Y = value;
			Z = value;
		}
		#endregion

		#region Static Constructors
		public static readonly Vector3 Zero = new Vector3(0f);
		public static readonly Vector3 One = new Vector3(1f);
		public static readonly Vector3 Right = new Vector3(1f, 0f, 0f);
		public static readonly Vector3 Left = new Vector3(-1f, 0f, 0f);
		public static readonly Vector3 Up = new Vector3(0f, 1f, 0f);
		public static readonly Vector3 Down = new Vector3(0f, -1f, 0f);
		public static readonly Vector3 Forward = new Vector3(0f, 0f, 1f);
		public static readonly Vector3 Backward = new Vector3(0f, 0f, -1f);
		#endregion

		#region Properties
		public Vector3 Normalized
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
		public Vector3 Round()
		{
			return new Vector3(X.Round(), Y.Round(), Z.Round());
		}

		public Vector3 Floor()
		{
			return new Vector3(X.Floor(), Y.Floor(), Z.Floor());
		}

		public Vector3 Ceil()
		{
			return new Vector3(X.Ceil(), Y.Ceil(), Z.Ceil());
		}

		public Vector3 Lerp(Vector3 b, float t)
		{
			return new Vector3(X.Lerp(b.X, t), Y.Lerp(b.Y, t), Z.Lerp(b.Z, t));
		}

		public float Magnitude()
		{
			return Math.Sqrt(X * X + Y * Y + Z * Z);
		}

		public float SqrMagnitude()
		{
			return X * X + Y * Y + Z * Z;
		}

		public Vector3 Normalize()
		{
			float num = Magnitude(this);
			if (num > 1E-05f)
				this /= num;
			else
				this = Zero;

			return this;
		}

		public float Dot(Vector3 vector)
		{
			return X * vector.X + Y * vector.Y + Z * vector.Z;
		}

		public Vector3 Cross(Vector3 vector)
		{
			return new Vector3(
				Y * vector.Z - vector.Y * Z,
				-(X * vector.Z - vector.X * Z),
				X * vector.Y - vector.X * Y
			);
		}
		#endregion

		#region Static Methods
		public static Vector3 Round(Vector3 v)
		{
			return new Vector3(v.X.Round(), v.Y.Round(), v.Z.Round());
		}

		public static Vector3 Floor(Vector3 v)
		{
			return new Vector3(v.X.Floor(), v.Y.Floor(), v.Z.Floor());
		}

		public static Vector3 Ceil(Vector3 v)
		{
			return new Vector3(v.X.Ceil(), v.Y.Ceil(), v.Z.Ceil());
		}

		public static Vector3 Lerp(Vector3 a, Vector3 b, float t)
		{
			return new Vector3(a.X.Lerp(b.X, t), a.Y.Lerp(b.Y, t), a.Z.Lerp(b.Z, t));
		}

		public static float Magnitude(Vector3 v)
		{
			return Math.Sqrt(v.X * v.X + v.Y * v.Y + v.Z * v.Z);
		}

		public static float MagnitudeSquared(Vector3 v)
		{
			return v.X * v.X + v.Y * v.Y + v.Z * v.Z;
		}

		public static Vector3 Normalize(Vector3 v)
		{
			float num = Magnitude(v);
			if (num > 1E-05f)
				v /= num;
			else
				v = Zero;

			return v;
		}

		public static float Dot(Vector3 vector1, Vector3 vector2)
		{
			return vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z;
		}

		public static Vector3 Cross(Vector3 vector1, Vector3 vector2)
		{
			return new Vector3(
				vector1.Y * vector2.Z - vector2.Y * vector1.Z,
				-(vector1.X * vector2.Z - vector2.X * vector1.Z),
				vector1.X * vector2.Y - vector2.X * vector1.Y
			);
		}
		#endregion

		#region Overrides
		public override string ToString()
		{
			return $"{X}, {Y}, {Z}";
		}
		#endregion

		#region Operators
		public static bool operator ==(Vector3 a, Vector3 b)
		{
			return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
		}
		public static bool operator !=(Vector3 a, Vector3 b)
		{
			return a.X != b.X || a.Y != b.Y || a.Z != b.Z;
		}

		public static Vector3 operator +(Vector3 a, Vector3 b)
		{
			a.X += b.X;
			a.Y += b.Y;
			a.Z += b.Z;
			return a;
		}

		public static Vector3 operator -(Vector3 a, Vector3 b)
		{
			a.X -= b.X;
			a.Y -= b.Y;
			a.Z -= b.Z;
			return a;
		}

		public static Vector3 operator *(Vector3 a, Vector3 b)
		{
			a.X *= b.X;
			a.Y *= b.Y;
			a.Z *= b.Z;
			return a;
		}
		public static Vector3 operator *(Vector3 v, float n)
		{
			v.X *= n;
			v.Y *= n;
			v.Z *= n;
			return v;
		}
		public static Vector3 operator *(float n, Vector3 v)
		{
			v.X *= n;
			v.Y *= n;
			v.Z *= n;
			return v;
		}
		// Pre-multiplication only
		public static Vector3 operator *(Vector3 v, Matrix m)
		{
			var x = (v.X * m.M11) + (v.Y * m.M21) + (v.Z * m.M31) + m.M41;
			var y = (v.X * m.M12) + (v.Y * m.M22) + (v.Z * m.M32) + m.M42;
			var z = (v.X * m.M13) + (v.Y * m.M23) + (v.Z * m.M33) + m.M43;
			return new Vector3(x, y, z);
		}
		// Pre-multiplication conversion from Post-multiplication order
		public static Vector3 operator *(Matrix m, Vector3 v)
		{
			var x = (v.X * m.M11) + (v.Y * m.M21) + (v.Z * m.M31) + m.M41;
			var y = (v.X * m.M12) + (v.Y * m.M22) + (v.Z * m.M32) + m.M42;
			var z = (v.X * m.M13) + (v.Y * m.M23) + (v.Z * m.M33) + m.M43;
			return new Vector3(x, y, z);
		}

		public static Vector3 operator /(Vector3 a, Vector3 b)
		{
			a.X /= b.X;
			a.Y /= b.Y;
			a.Z /= b.Z;
			return a;
		}
		public static Vector3 operator /(Vector3 v, float n)
		{
			v.X /= n;
			v.Y /= n;
			v.Z /= n;
			return v;
		}

		public static Vector3 operator -(Vector3 v)
		{
			v.X = -v.X;
			v.Y = -v.Y;
			v.Z = -v.Z;
			return v;
		}
		#endregion

		#region Implicit Convertion Operators
		public static implicit operator Vector3(Vector2 v)
		{
			return new Vector3(v.X, v.Y, 0f);
		}

		public static implicit operator Vector3(Vector4 v)
		{
			return new Vector3(v.X, v.Y, v.Z);
		}
		#endregion
	}
}