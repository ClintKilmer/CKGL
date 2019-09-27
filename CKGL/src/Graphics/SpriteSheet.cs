using System.Collections.Generic;

namespace CKGL
{
	public class SpriteSheet
	{
		private Bitmap bitmap;
		public List<Sprite> Sprites { get; private set; } = new List<Sprite>();
		private List<AABBi> aabbs = new List<AABBi>();

		public Texture Texture
		{
			get
			{
				if (dirty || texture == null)
				{
					if (texture != null)
						texture.Destroy();

					texture = Texture.Create2D(bitmap);

					dirty = false;
				}

				return texture;
			}
		}

		private Texture texture;
		private bool dirty = true;

		public int Width { get { return bitmap.Width; } }
		public int Height { get { return bitmap.Height; } }

		private readonly int padding;

		public SpriteSheet(int size, int padding) : this(size, size, padding) { }
		public SpriteSheet(int width, int height, int padding)
		{
			if (padding < 0)
				throw new CKGLException("Padding must be greater or equal to 0.");
			else if (padding >= Math.Min(width, height))
				throw new CKGLException("Padding cannot be larger than the width or height of the SpriteSheet.");

			bitmap = new Bitmap(width, height, Colour.Transparent);

			this.padding = padding;
		}

		public Sprite AddSprite(string file)
		{
			Platform.LoadImage(file, out int width, out int height, out byte[] imageData);
			return AddSprite(file, new RectangleI(width, height));
		}
		public Sprite AddSprite(string file, RectangleI source)
		{
			Platform.LoadImage(file, out int width, out int height, out byte[] imageData);
			Bitmap spriteData = new Bitmap(imageData, width, height);

#if DEBUG
			Point2 offset = GetValidPlacementNaive(source.W, source.H);
#else
			Point2 offset = GetValidPlacement(source.W, source.H);
#endif

			for (int y = 0; y < source.H; y++)
				for (int x = 0; x < source.W; x++)
					bitmap[offset.X + x, offset.Y + y] = spriteData[source.X + x, source.Y + y];

			dirty = true;

			Sprite sprite = new Sprite(this, offset.X, offset.Y, source.W, source.H);
			Sprites.Add(sprite);
			aabbs.Add(new AABBi(sprite.SpriteSheetX, sprite.SpriteSheetY, sprite.Width + padding, sprite.Height + padding));
			return sprite;
		}

		public Sprite AddSpriteFontGlyph(Bitmap spriteData, bool xtrim = false)
		{
			return AddSpriteFontGlyph(spriteData, new RectangleI(0, 0, spriteData.Width, spriteData.Height), xtrim);
		}
		public Sprite AddSpriteFontGlyph(Bitmap spriteData, RectangleI source, bool xtrim = false)
		{
			int spriteOffsetX = 0;
			int spriteWidth = source.W;

			if (xtrim)
			{
				spriteOffsetX = source.W;
				spriteWidth = 0;

				for (int y = source.Top; y < source.Bottom; y++)
				{
					for (int x = source.Left; x < source.Right; x++)
					{
						if (spriteData[x, y] != Colour.Transparent)
						{
							spriteWidth = Math.Max(x - source.Left, spriteWidth);
							spriteOffsetX = Math.Min(x - source.Left, spriteOffsetX);
						}
					}
				}

				spriteWidth++;
			}

#if DEBUG
			Point2 offset = GetValidPlacementNaive(spriteWidth, source.H);
#else
			Point2 offset = GetValidPlacement(spriteWidth, source.H);
#endif

			for (int y = 0; y < source.H; y++)
				for (int x = spriteOffsetX; x < spriteOffsetX + spriteWidth; x++)
					bitmap[offset.X + x - spriteOffsetX, offset.Y + y] = spriteData[source.X + x, source.Y + y];

			dirty = true;

			Sprite sprite = new Sprite(this, offset.X, offset.Y, spriteWidth, source.H);
			Sprites.Add(sprite);
			aabbs.Add(new AABBi(sprite.SpriteSheetX, sprite.SpriteSheetY, sprite.Width + padding, sprite.Height + padding));
			return sprite;
		}

		// Slow, pixel-sweep version
		private Point2 GetValidPlacement(int width, int height)
		{
			width += padding;
			height += padding;

			AABBi aabb = new AABBi(0, 0, width, height);
			for (int y = padding; y <= bitmap.Height - height; y++)
			{
				for (int x = padding; x <= bitmap.Width - width; x++)
				{
					aabb.X = x;
					aabb.Y = y;

					bool valid = true;

					foreach (AABBi other in aabbs)
					{
						if (aabb.Intersects(other))
						{
							x = other.Right - 1;
							valid = false;
							break;
						}
					}

					if (valid)
						return new Point2(x, y);
				}
			}

			throw new CKGLException("SpriteSheet is not large enough for submitted sprites.");
		}

		// Fast, naive version
		private int _offsetX = 0;
		private int _offsetY = 0;
		private int _offsetYRowMax = 0;
		private Point2 GetValidPlacementNaive(int width, int height)
		{
			width += padding;
			height += padding;
			_offsetX = _offsetX.Max(padding);
			_offsetY = _offsetY.Max(padding);

			if (_offsetX + width > bitmap.Width)
			{
				_offsetY += _offsetYRowMax;
				_offsetX = padding;
				_offsetYRowMax = 0;
			}

			if (_offsetY + height > bitmap.Height)
			{
				throw new CKGLException("SpriteSheet is not large enough for submitted sprites.");
			}

			Point2 Point2 = new Point2(_offsetX, _offsetY);

			_offsetX += width;
			_offsetYRowMax = Math.Max(_offsetYRowMax, height);

			return Point2;
		}
	}
}