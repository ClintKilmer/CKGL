using System.Runtime.InteropServices;

namespace CKGL
{
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct Point2
	{
		public static readonly Point2 Zero = new Point2(0, 0);
		public static readonly Point2 One = new Point2(1, 1);

		public int X;
		public int Y;

		public Point2(int x, int y)
		{
			X = x;
			Y = y;
		}

		#region Overrides
		public override string ToString()
		{
			return string.Format("{0}, {1}", X, Y);
		}
		#endregion

		#region Operators
		public static bool operator ==(Point2 a, Point2 b)
		{
			return a.X == b.X && a.Y == b.Y;
		}
		public static bool operator !=(Point2 a, Point2 b)
		{
			return a.X != b.X || a.Y != b.Y;
		}

		public static Point2 operator +(Point2 a, Point2 b)
		{
			a.X += b.X;
			a.Y += b.Y;
			return a;
		}

		public static Point2 operator -(Point2 a, Point2 b)
		{
			a.X -= b.X;
			a.Y -= b.Y;
			return a;
		}

		public static Point2 operator *(Point2 a, Point2 b)
		{
			a.X *= b.X;
			a.Y *= b.Y;
			return a;
		}
		public static Point2 operator *(Point2 p, int n)
		{
			p.X *= n;
			p.Y *= n;
			return p;
		}
		public static Point2 operator *(int n, Point2 p)
		{
			p.X *= n;
			p.Y *= n;
			return p;
		}

		public static Point2 operator /(Point2 a, Point2 b)
		{
			a.X /= b.X;
			a.Y /= b.Y;
			return a;
		}
		public static Point2 operator /(Point2 p, int n)
		{
			p.X /= n;
			p.Y /= n;
			return p;
		}
		public static Point2 operator /(int n, Point2 p)
		{
			p.X /= n;
			p.Y /= n;
			return p;
		}
		#endregion

		#region Implicit Convertion Operators
		public static implicit operator Point2(Vector2 p)
		{
			return new Vector2(p.X, p.Y);
		}
		#endregion
	}
}