namespace CKGL
{
	public class Sprite
	{
		public SpriteSheet SpriteSheet { get; private set; }
		public int X { get; private set; }
		public int Y { get; private set; }
		public int Width { get; private set; }
		public int Height { get; private set; }
		public Vector2 UV_BL { get; private set; }
		public Vector2 UV_BR { get; private set; }
		public Vector2 UV_TL { get; private set; }
		public Vector2 UV_TR { get; private set; }

		public Vector2 Position { get { return new Vector2(X, Y); } }
		public Vector2 Size { get { return new Vector2(Width, Height); } }
		public RectangleI Bounds { get { return new RectangleI(X, Y, Width, Height); } }

		public Sprite(SpriteSheet spriteSheet, int x, int y, int width, int height)
		{
			SpriteSheet = spriteSheet;
			X = x;
			Y = y;
			Width = width;
			Height = height;
			UV_BL = new Vector2(X / (float)SpriteSheet.Width,
								Y / (float)SpriteSheet.Height);
			UV_BR = new Vector2((X + Width) / (float)SpriteSheet.Width,
								Y / (float)SpriteSheet.Height);
			UV_TL = new Vector2(X / (float)SpriteSheet.Width,
								(Y + Height) / (float)SpriteSheet.Height);
			UV_TR = new Vector2((X + Width) / (float)SpriteSheet.Width,
								(Y + Height) / (float)SpriteSheet.Height);
		}
	}
}