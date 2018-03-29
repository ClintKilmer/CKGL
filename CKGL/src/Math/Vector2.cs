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
		public static Vector2 operator /(float n, Vector2 v)
		{
			v.X /= n;
			v.Y /= n;
			return v;
		}
		#endregion

		// TODO
		#region Implicit Convertion Operators
		public static implicit operator Vector2(Point2 p)
		{
			return new Vector2(p.X, p.Y);
		}

		public static implicit operator Vector2(Vector3 v)
		{
			return new Vector2(v.X, v.Y);
		}

		//public static implicit operator Vector2(Vector4 v)
		//{
		//	return new Vector2(v.X, v.Y);
		//}
		#endregion
	}
}