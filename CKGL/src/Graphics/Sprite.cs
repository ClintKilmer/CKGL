namespace CKGL
{
	public class Sprite
	{
		public SpriteSheet SpriteSheet { get; private set; }
		public int X { get; private set; }
		public int Y { get; private set; }
		public int Width { get; private set; }
		public int Height { get; private set; }
		public Vector2[] UVs { get; private set; }

		public Vector2 Position { get { return new Vector2(X, Y); } }
		public Vector2 Size { get { return new Vector2(Width, Height); } }
		public RectangleI Bounds { get { return new RectangleI(X, Y, Width, Height); } }

		// TODO - remove uv offset epsilon if unneeded - not on laptop, check on desktop
		//private float offset = Math.Epsilon;
		private float offset = 0;

		public Sprite(SpriteSheet spriteSheet, int x, int y, int width, int height)
		{
			SpriteSheet = spriteSheet;
			X = x;
			Y = y;
			Width = width;
			Height = height;
			UVs = new Vector2[4] {
				new Vector2(X / (float)SpriteSheet.Width + offset, Y / (float)SpriteSheet.Height + offset),
				new Vector2((X + Width) / (float)SpriteSheet.Width, Y / (float)SpriteSheet.Height + offset),
				new Vector2(X / (float)SpriteSheet.Width + offset, (Y + Height) / (float)SpriteSheet.Height),
				new Vector2((X + Width) / (float)SpriteSheet.Width, (Y + Height) / (float)SpriteSheet.Height)
			};
		}
	}
}