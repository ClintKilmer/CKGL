//namespace CKGL
//{
//	public class Sprite
//	{
//		public SpriteSheet SpriteSheet { get; private set; }
//		public int X { get; private set; }
//		public int Y { get; private set; }
//		public int Width { get; private set; }
//		public int Height { get; private set; }
//		public Vector2[] UVs { get; private set; }

//		public Vector2 Position { get { return new Vector2(X, Y); } }
//		public Vector2 Size { get { return new Vector2(Width, Height); } }
//		public RectangleI Bounds { get { return new RectangleI(X, Y, Width, Height); } }

//#if FNA || WINDOWS
//		private float offset = Mathf.epsilon;
//#else
//		private float offset = 0;
//#endif

//		public Sprite(SpriteSheet spriteSheet, int x, int y, int width, int height)
//		{
//			SpriteSheet = spriteSheet;
//			X = x;
//			Y = y;
//			Width = width;
//			Height = height;
//			UVs = new Vector2[4] {
//				new Vector2((float)X / (float)SpriteSheet.Width + offset, (float)Y / (float)SpriteSheet.Height + offset),
//				new Vector2((float)(X + Width) / (float)SpriteSheet.Width, (float)Y / (float)SpriteSheet.Height + offset),
//				new Vector2((float)X / SpriteSheet.Width + offset, (float)(Y + Height) / (float)SpriteSheet.Height),
//				new Vector2((float)(X + Width) / (float)SpriteSheet.Width, (float)(Y + Height) / (float)SpriteSheet.Height)
//			};
//		}

//		public float U(float u)
//		{
//			return UVs[0].X + UVs[3].X * u.Clamp(0f, 1f);
//		}
//		public float V(float v)
//		{
//			return UVs[0].Y + UVs[3].Y * v.Clamp(0f, 1f);
//		}
//		public Vector2 UV(Vector2 uvs)
//		{
//			return new Vector2(U(uvs.X), V(uvs.Y));
//		}
//	}
//}