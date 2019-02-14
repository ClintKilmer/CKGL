namespace CKGL
{
	public class Sprite
	{
		public SpriteSheet SpriteSheet { get; private set; }
		public int X { get; private set; }
		public int Y { get; private set; }
		public int Width { get; private set; }
		public int Height { get; private set; }
		public UV UV_BL { get; private set; }
		public UV UV_BR { get; private set; }
		public UV UV_TL { get; private set; }
		public UV UV_TR { get; private set; }

		public Vector2 Position => new Vector2(X, Y);
		public Vector2 Size => new Vector2(Width, Height);
		public RectangleI Bounds => new RectangleI(X, Y, Width, Height);
		public int MaxLength => Math.Max(Width, Height);

		public Sprite(SpriteSheet spriteSheet, int x, int y, int width, int height)
		{
			SpriteSheet = spriteSheet;
			X = x;
			Y = y;
			Width = width;
			Height = height;
			UV_BL = new UV(X / (float)SpriteSheet.Width,
						   Y / (float)SpriteSheet.Height);
			UV_BR = new UV((X + Width) / (float)SpriteSheet.Width,
						   Y / (float)SpriteSheet.Height);
			UV_TL = new UV(X / (float)SpriteSheet.Width,
						  (Y + Height) / (float)SpriteSheet.Height);
			UV_TR = new UV((X + Width) / (float)SpriteSheet.Width,
						  (Y + Height) / (float)SpriteSheet.Height);
		}
	}
}