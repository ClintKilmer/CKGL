using System;
using System.Runtime.InteropServices;

namespace CKGL
{
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct Vector4
	{
		public float X;
		public float Y;
		public float Z;
		public float W;

		#region Constructors
		public Vector4(float x, float y, float z, float w)
		{
			X = x;
			Y = y;
			Z = z;
			W = w;
		}
		public Vector4(float value)
		{
			X = value;
			Y = value;
			Z = value;
			W = value;
		}
		#endregion

		#region Static Constructors
		public static readonly Vector4 Zero = new Vector4(0f);
		public static readonly Vector4 One = new Vector4(1f);
		#endregion

		#region Properties
		public byte[] FloatByteArray
		{
			get
			{
				byte[] buffer = new byte[sizeof(float) * 4];
				Buffer.BlockCopy(BitConverter.GetBytes(X), 0, buffer, 0 * sizeof(float), sizeof(float));
				Buffer.BlockCopy(BitConverter.GetBytes(Y), 0, buffer, 1 * sizeof(float), sizeof(float));
				Buffer.BlockCopy(BitConverter.GetBytes(Z), 0, buffer, 2 * sizeof(float), sizeof(float));
				Buffer.BlockCopy(BitConverter.GetBytes(W), 0, buffer, 3 * sizeof(float), sizeof(float));
				return buffer;
			}
		}

		public Vector4 Normalized
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
		public Vector4 Round()
		{
			return new Vector4(X.Round(), Y.Round(), Z.Round(), W.Round());
		}

		public Vector4 Floor()
		{
			return new Vector4(X.Floor(), Y.Floor(), Z.Floor(), W.Floor());
		}

		public Vector4 Ceil()
		{
			return new Vector4(X.Ceil(), Y.Ceil(), Z.Ceil(), W.Ceil());
		}

		public Vector4 Lerp(Vector4 b, float t)
		{
			return new Vector4(X.Lerp(b.X, t), Y.Lerp(b.Y, t), Z.Lerp(b.Z, t), W.Lerp(b.W, t));
		}

		public float Magnitude()
		{
			return Math.Sqrt(X * X + Y * Y + Z * Z + W * W);
		}

		public float SqrMagnitude()
		{
			return X * X + Y * Y + Z * Z + W * W;
		}

		public Vector4 Normalize()
		{
			float num = Magnitude(this);
			if (num > 1E-05f)
				this /= num;
			else
				this = Zero;

			return this;
		}

		public float Dot(Vector4 vector)
		{
			return X * vector.X + Y * vector.Y + Z * vector.Z + W * vector.W;
		}
		#endregion

		#region Static Methods
		public static Vector4 Round(Vector4 v)
		{
			return new Vector4(v.X.Round(), v.Y.Round(), v.Z.Round(), v.W.Round());
		}

		public static Vector4 Floor(Vector4 v)
		{
			return new Vector4(v.X.Floor(), v.Y.Floor(), v.Z.Floor(), v.W.Floor());
		}

		public static Vector4 Ceil(Vector4 v)
		{
			return new Vector4(v.X.Ceil(), v.Y.Ceil(), v.Z.Ceil(), v.W.Ceil());
		}

		public static Vector4 Lerp(Vector4 a, Vector4 b, float t)
		{
			return new Vector4(a.X.Lerp(b.X, t), a.Y.Lerp(b.Y, t), a.Z.Lerp(b.Z, t), a.W.Lerp(b.W, t));
		}

		public static float Magnitude(Vector4 v)
		{
			return Math.Sqrt(v.X * v.X + v.Y * v.Y + v.Z * v.Z + v.W * v.W);
		}

		public static float MagnitudeSquared(Vector4 v)
		{
			return v.X * v.X + v.Y * v.Y + v.Z * v.Z + v.W * v.W;
		}

		public static Vector4 Normalize(Vector4 v)
		{
			float num = Magnitude(v);
			if (num > 1E-05f)
				v /= num;
			else
				v = Zero;

			return v;
		}

		public static float Dot(Vector4 vector1, Vector4 vector2)
		{
			return vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z + vector1.W * vector2.W;
		}
		#endregion

		#region Overrides
		public override string ToString()
		{
			return $"{X}, {Y}, {Z}, {W}";
		}
		#endregion

		#region Operators
		public static bool operator ==(Vector4 a, Vector4 b)
		{
			return a.X == b.X && a.Y == b.Y && a.Z == b.Z && a.W == b.W;
		}
		public static bool operator !=(Vector4 a, Vector4 b)
		{
			return a.X != b.X || a.Y != b.Y || a.Z != b.Z || a.W != b.W;
		}

		public static Vector4 operator +(Vector4 a, Vector4 b)
		{
			a.X += b.X;
			a.Y += b.Y;
			a.Z += b.Z;
			a.W += b.W;
			return a;
		}

		public static Vector4 operator -(Vector4 a, Vector4 b)
		{
			a.X -= b.X;
			a.Y -= b.Y;
			a.Z -= b.Z;
			a.W -= b.W;
			return a;
		}

		public static Vector4 operator *(Vector4 a, Vector4 b)
		{
			a.X *= b.X;
			a.Y *= b.Y;
			a.Z *= b.Z;
			a.W *= b.W;
			return a;
		}
		public static Vector4 operator *(Vector4 v, float n)
		{
			v.X *= n;
			v.Y *= n;
			v.Z *= n;
			v.W *= n;
			return v;
		}
		public static Vector4 operator *(float n, Vector4 v)
		{
			v.X *= n;
			v.Y *= n;
			v.Z *= n;
			v.W *= n;
			return v;
		}

		public static Vector4 operator /(Vector4 a, Vector4 b)
		{
			a.X /= b.X;
			a.Y /= b.Y;
			a.Z /= b.Z;
			a.W /= b.W;
			return a;
		}
		public static Vector4 operator /(Vector4 v, float n)
		{
			v.X /= n;
			v.Y /= n;
			v.Z /= n;
			v.W /= n;
			return v;
		}

		public static Vector4 operator -(Vector4 v)
		{
			v.X = -v.X;
			v.Y = -v.Y;
			v.Z = -v.Z;
			v.W = -v.W;
			return v;
		}
		#endregion

		#region Implicit Convertion Operators
		public static implicit operator Vector4(Vector2 v)
		{
			return new Vector4(v.X, v.Y, 0f, 0f);
		}

		public static implicit operator Vector4(Vector3 v)
		{
			return new Vector4(v.X, v.Y, v.Z, 0f);
		}

		public static implicit operator Vector4(Colour c)
		{
			return new Vector4(c.R, c.G, c.B, c.A);
		}
		#endregion
	}
}