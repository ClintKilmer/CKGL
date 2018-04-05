namespace CKGL
{
	public class AABBi
	{
		public int X { get; set; }
		public int Y { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }

		public int Top { get { return Y; } }
		public int Bottom { get { return Y + Height; } }
		public int Left { get { return X; } }
		public int Right { get { return X + Width; } }

		public AABBi(int x, int y, int width, int height)
		{
			X = x;
			Y = y;
			Width = width;
			Height = height;
		}

		public bool Intersects(AABBi aabbi)
		{
			return Right > aabbi.Left &&
				   Left < aabbi.Right &&
				   Bottom > aabbi.Top &&
				   Top < aabbi.Bottom;
		}
	}
}