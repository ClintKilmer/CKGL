using System;
using System.Collections.Generic;

namespace CKGL
{
	public class SpriteSheet
	{
		public Texture2D Texture { get; private set; }
		public List<Sprite> Sprites { get; private set; } = new List<Sprite>();
		private List<AABBi> aabbs = new List<AABBi>();

		public int Width { get { return Texture.Width; } }
		public int Height { get { return Texture.Height; } }

		private readonly int padding;
		private readonly Colour[,] data;

		public SpriteSheet(int size, int padding) : this(size, size, padding) { }
		public SpriteSheet(int width, int height, int padding)
		{
			if (padding < 0)
				throw new Exception("Padding must be greater or equal to 0");
			else if (padding >= Math.Min(width, height))
				throw new Exception("Padding cannot be larger than the width or height of SpriteSheet.Texture");

			Texture = new Texture2D(width, height, TextureFormat.RGBA8, TextureFilter.Nearest, TextureWrap.Clamp);
			data = new Colour[Texture.Width, Texture.Height];

			// Clear Texture
			for (int y = 0; y < Texture.Height; y++)
				for (int x = 0; x < Texture.Width; x++)
					data[x, y] = Colour.Transparent;

			this.padding = padding;
		}

		public Sprite AddSprite(Texture2D texture)
		{
			return AddSprite(texture, new RectangleI(0, 0, texture.Width, texture.Height));
		}
		public Sprite AddSprite(Texture2D texture, RectangleI source)
		{
			Colour[,] spriteData = texture.GetData2D();
			texture.Destroy();

#if DEBUG
			Point2 offset = GetValidPlacementNaive(source.W, source.H);
#else
			Point2 offset = GetValidPlacement(source.W, source.H);
#endif

			for (int y = 0; y < source.H; y++)
				for (int x = 0; x < source.W; x++)
					data[offset.X + x, offset.Y + y] = spriteData[source.X + x, source.Y + y];

			Texture.SetData2D(data);

			Sprite sprite = new Sprite(this, offset.X, offset.Y, source.W, source.H);
			Sprites.Add(sprite);
			aabbs.Add(new AABBi(sprite.X, sprite.Y, sprite.Width + padding, sprite.Height + padding));
			return sprite;
		}

		public Sprite AddSpriteFontGlyph(Texture2D texture, bool xtrim = false)
		{
			return AddSpriteFontGlyph(texture, new RectangleI(0, 0, texture.Width, texture.Height), xtrim);
		}
		public Sprite AddSpriteFontGlyph(Texture2D texture, RectangleI source, bool xtrim = false)
		{
			Colour[,] spriteData = texture.GetData2D();

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
			Point2 offset = GetValidPlacement(spriteWidth, texture.Height);
#endif

			for (int y = 0; y < source.H; y++)
				for (int x = spriteOffsetX; x < spriteOffsetX + spriteWidth; x++)
					data[offset.X + x - spriteOffsetX, offset.Y + y] = spriteData[source.X + x, source.Y + y];

			Texture.SetData2D(data);

			Sprite sprite = new Sprite(this, offset.X, offset.Y, spriteWidth, source.H);
			Sprites.Add(sprite);
			aabbs.Add(new AABBi(sprite.X, sprite.Y, sprite.Width + padding, sprite.Height + padding));
			return sprite;
		}

		// Slow, pixel-sweep version
		private Point2 GetValidPlacement(int width, int height)
		{
			width += padding;
			height += padding;

			AABBi aabb = new AABBi(0, 0, width, height);
			for (int y = padding; y <= Texture.Height - height; y++)
			{
				for (int x = padding; x <= Texture.Width - width; x++)
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

			throw new Exception("SpriteSheet.Texture is not large enough for submitted sprites.");
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

			if (_offsetX + width > Texture.Width)
			{
				_offsetY += _offsetYRowMax;
				_offsetX = padding;
				_offsetYRowMax = 0;
			}

			if (_offsetY + height > Texture.Height)
			{
				throw new Exception("SpriteSheet.Texture is not large enough for submitted sprites.");
			}

			Point2 Point2 = new Point2(_offsetX, _offsetY);

			_offsetX += width;
			_offsetYRowMax = Math.Max(_offsetYRowMax, height);

			return Point2;
		}
	}
}