using System.Collections.Generic;
using static Retyped.dom; // DOM / WebGL Types

namespace CKGL
{
	public class SpriteSheet
	{
		public Bitmap Bitmap;
		public List<Sprite> Sprites { get; private set; } = new List<Sprite>();
		private List<AABBi> aabbs = new List<AABBi>();

		public Texture Texture;

		public int Width { get { return Bitmap.Width; } }
		public int Height { get { return Bitmap.Height; } }

		private readonly int padding;

		public SpriteSheet(int size, int padding) : this(size, size, padding) { }
		public SpriteSheet(int width, int height, int padding)
		{
			if (padding < 0)
				throw new CKGLException("Padding must be greater or equal to 0.");
			else if (padding >= Math.Min(width, height))
				throw new CKGLException("Padding cannot be larger than the width or height of the SpriteSheet.");

			Bitmap = new Bitmap(width, height, Colour.Transparent);

			Texture = Texture.Create2D(Width, Height);

			this.padding = padding;
		}

		public Sprite AddSprite(string file, RectangleI? source = null)
		{
			Sprite sprite = new Sprite(this, 0, 0, 1, 1);

			// Load image async
			HTMLImageElement image = new HTMLImageElement();
			image.crossOrigin = "";
			image.onload = (ev) => { _AddSprite(image, sprite, source); };
			image.src = file;

			return sprite;
		}
		private void _AddSprite(HTMLImageElement image, Sprite sprite, RectangleI? source = null)
		{
			RectangleI s = source ?? new RectangleI((int)image.width, (int)image.height);

#if DEBUG
			Point2 offset = GetValidPlacementNaive(s.W, s.H);
#else
			Point2 offset = GetValidPlacement(source.W, source.H);
#endif

			//for (int y = 0; y < s.H; y++)
			//	for (int x = 0; x < s.W; x++)
			//		Bitmap[offset.X + x, offset.Y + y] = spriteData[s.X + x, s.Y + y];

			if (Platform.GraphicsBackend == GraphicsBackend.WebGL2)
				(Texture as WebGL2.WebGL2Texture).UpdateData(image, offset.X, offset.Y);
			else if (Platform.GraphicsBackend == GraphicsBackend.WebGL)
				(Texture as WebGL.WebGLTexture).UpdateData(image, offset.X, offset.Y);

			sprite.Update(this, offset.X, offset.Y, s.W, s.H);
			Sprites.Add(sprite);
			aabbs.Add(new AABBi(sprite.SpriteSheetX, sprite.SpriteSheetY, sprite.Width + padding, sprite.Height + padding));
		}

		public Sprite AddSpriteFontGlyph(Bitmap spriteData, bool xtrim = false) => AddSpriteFontGlyph(spriteData, new RectangleI(0, 0, spriteData.Width, spriteData.Height), xtrim);
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

			//for (int y = 0; y < source.H; y++)
			//	for (int x = spriteOffsetX; x < spriteOffsetX + spriteWidth; x++)
			//		Bitmap[offset.X + x - spriteOffsetX, offset.Y + y] = spriteData[source.X + x, source.Y + y];

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
			for (int y = padding; y <= Bitmap.Height - height; y++)
			{
				for (int x = padding; x <= Bitmap.Width - width; x++)
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

			if (_offsetX + width > Bitmap.Width)
			{
				_offsetY += _offsetYRowMax;
				_offsetX = padding;
				_offsetYRowMax = 0;
			}

			if (_offsetY + height > Bitmap.Height)
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