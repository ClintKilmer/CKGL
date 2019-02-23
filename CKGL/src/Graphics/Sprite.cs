namespace CKGL
{
	public class Sprite
	{
		public SpriteSheet SpriteSheet { get; private set; }
		public int SpriteSheetX { get; private set; }
		public int SpriteSheetY { get; private set; }

		public int Width { get; private set; }
		public int Height { get; private set; }
		public Vector2 Size { get; private set; }
		public int MaxLength { get; private set; }

		public Vector2 Position_BL { get; private set; }
		public Vector2 Position_BR { get; private set; }
		public Vector2 Position_TL { get; private set; }
		public Vector2 Position_TR { get; private set; }

		public UV UV_BL { get; private set; }
		public UV UV_BR { get; private set; }
		public UV UV_TL { get; private set; }
		public UV UV_TR { get; private set; }

		public Sprite(SpriteSheet spriteSheet, int x, int y, int width, int height)
		{
			SpriteSheet = spriteSheet;
			SpriteSheetX = x;
			SpriteSheetY = y;

			Width = width;
			Height = height;
			Size = new Vector2(Width, Height);
			MaxLength = Math.Max(Width, Height);

			Position_BL = new Vector2(0f, 0f);
			Position_BR = new Vector2(Size.X, 0f);
			Position_TL = new Vector2(0f, Size.Y);
			Position_TR = new Vector2(Size.X, Size.Y);

			UV_BL = new UV(SpriteSheetX / (float)SpriteSheet.Width,
						   SpriteSheetY / (float)SpriteSheet.Height);
			UV_BR = new UV((SpriteSheetX + Width) / (float)SpriteSheet.Width,
						   SpriteSheetY / (float)SpriteSheet.Height);
			UV_TL = new UV(SpriteSheetX / (float)SpriteSheet.Width,
						  (SpriteSheetY + Height) / (float)SpriteSheet.Height);
			UV_TR = new UV((SpriteSheetX + Width) / (float)SpriteSheet.Width,
						  (SpriteSheetY + Height) / (float)SpriteSheet.Height);
		}
	}
}