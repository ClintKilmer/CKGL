using System.Runtime.InteropServices;

namespace CKGL
{
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct RectangleI
	{
		public int X;
		public int Y;
		public int W;
		public int H;

		#region Properties
		public int Left
		{
			get { return X; }
		}

		public int Right
		{
			get { return X + W; }
		}

		public int Top
		{
			get { return Y; }
		}

		public int Bottom
		{
			get { return Y + H; }
		}

		public int MinX
		{
			get { return Math.Min(X, X + W); }
		}

		public int MinY
		{
			get { return Math.Min(Y, Y + H); }
		}

		public int MaxX
		{
			get { return Math.Max(X, X + W); }
		}

		public int MaxY
		{
			get { return Math.Max(Y, Y + H); }
		}

		public int AbsW
		{
			get { return Math.Abs(W); }
		}

		public int AbsH
		{
			get { return Math.Abs(H); }
		}

		public int CenterX
		{
			get { return X + W / 2; }
		}

		public int CenterY
		{
			get { return Y + H / 2; }
		}

		public Point2 TopLeft
		{
			get { return new Point2(Left, Top); }
		}

		public Point2 BottomLeft
		{
			get { return new Point2(Left, Bottom); }
		}

		public Point2 BottomRight
		{
			get { return new Point2(Right, Bottom); }
		}

		public Point2 TopRight
		{
			get { return new Point2(Right, Top); }
		}

		public Point2 Center
		{
			get { return new Point2(CenterX, CenterY); }
		}

		public Point2 TopCenter
		{
			get { return new Point2(CenterX, Top); }
		}

		public Point2 BottomCenter
		{
			get { return new Point2(CenterX, Bottom); }
		}

		public Point2 LeftCenter
		{
			get { return new Point2(Left, CenterY); }
		}

		public Point2 RightCenter
		{
			get { return new Point2(Right, CenterY); }
		}

		public bool IsEmpty
		{
			get { return W == 0f && H == 0f; }
		}

		public bool IsRegular
		{
			get { return W >= 0f && H >= 0f; }
		}

		public RectangleI Regular
		{
			get
			{
				var copy = this;
				if (copy.W < 0)
				{
					copy.X += copy.W;
					copy.W = -copy.W;
				}
				if (copy.H < 0)
				{
					copy.Y += copy.H;
					copy.H = -copy.H;
				}
				return copy;
			}
		}

		public int Area
		{
			get { return Math.Abs(W * H); }
		}
		#endregion

		#region Constructors
		public RectangleI(int x, int y, int w, int h)
		{
			X = x;
			Y = y;
			W = w;
			H = h;
		}
		public RectangleI(int w, int h) : this(0, 0, w, h) { }
		#endregion

		#region Static Constructors
		public static readonly RectangleI Empty = new RectangleI();
		#endregion

		#region Overrides
		public override string ToString()
		{
			return $"{X}, {Y}, {W}, {H}";
		}

		public override bool Equals(object obj)
		{
			return obj is RectangleI && Equals((RectangleI)obj);
		}
		public bool Equals(RectangleI rectangleI)
		{
			return this == rectangleI;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hash = 17;
				hash = hash * 23 + X.GetHashCode();
				hash = hash * 23 + Y.GetHashCode();
				hash = hash * 23 + W.GetHashCode();
				hash = hash * 23 + H.GetHashCode();
				return hash;
			}
		}
		#endregion

		#region Operators
		public static bool operator ==(RectangleI a, RectangleI b)
		{
			return a.X == b.X && a.Y == b.Y && a.W == b.W && a.H == b.H;
		}

		public static bool operator !=(RectangleI a, RectangleI b)
		{
			return a.X != b.X || a.Y != b.Y || a.W != b.W || a.H != b.H;
		}

		public static RectangleI operator *(RectangleI r, int n)
		{
			return new RectangleI(r.X * n, r.Y * n, r.W * n, r.H * n);
		}

		public static RectangleI operator /(RectangleI r, int n)
		{
			return new RectangleI(r.X / n, r.Y / n, r.W / n, r.H / n);
		}
		#endregion

		// TODO - RectangleI -> Rectangle
		#region Implicit Convertion Operators
		//public static implicit operator Rectangle(RectangleI r)
		//{
		//	return new Rectangle(r.X, r.Y, r.W, r.H);
		//} 
		#endregion
	}
}