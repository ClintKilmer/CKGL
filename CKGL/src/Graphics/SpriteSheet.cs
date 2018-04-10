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

		private Colour[,] data;

		public SpriteSheet(int size) : this(size, size) { }
		public SpriteSheet(int width, int height)
		{
			Texture = new Texture2D(width, height, TextureFormat.RGBA);
			data = new Colour[Texture.Width, Texture.Height];

			// Clear Texture
			for (int y = 0; y < Texture.Height - height; y++)
				for (int x = 0; x < Texture.Width - width; x++)
					data[x, y] = Colour.Transparent;
		}

		public Sprite AddSprite(Texture2D texture)
		{
			return AddSprite(texture, new RectangleI(0, 0, texture.Width, texture.Height));
		}
		public Sprite AddSprite(Texture2D texture, RectangleI source)
		{
			Colour[,] spriteData = new Colour[texture.Width, texture.Height];
			texture.GetData2D(spriteData);

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
			aabbs.Add(new AABBi(sprite.X, sprite.Y, sprite.Width, sprite.Height));
			return sprite;
		}

		public Sprite AddSpriteFontGlyph(Texture2D texture, bool xtrim = false)
		{
			return AddSpriteFontGlyph(texture, new RectangleI(0, 0, texture.Width, texture.Height), xtrim);
		}
		public Sprite AddSpriteFontGlyph(Texture2D texture, RectangleI source, bool xtrim = false)
		{
			Colour[,] spriteData = new Colour[texture.Width, texture.Height];
			texture.GetData2D(spriteData);

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
			aabbs.Add(new AABBi(sprite.X, sprite.Y, sprite.Width, sprite.Height));
			return sprite;
		}

		//Slow, pixel-sweep version
		private Point2 GetValidPlacement(int width, int height)
		{
			AABBi aabb = new AABBi(0, 0, width, height);
			for (int y = 0; y < Texture.Height - height; y++)
			{
				for (int x = 0; x < Texture.Width - width; x++)
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
		private int _offsetYMax = 0;
		private Point2 GetValidPlacementNaive(int width, int height)
		{
			if (_offsetX + width > Texture.Width)
			{
				_offsetY += _offsetYMax;
				_offsetX = 0;
				_offsetYMax = 0;
			}

			if (_offsetY + height > Texture.Height)
			{
				throw new Exception("SpriteSheet.Texture is not large enough for submitted sprites.");
			}

			Point2 Point2 = new Point2(_offsetX, _offsetY);

			_offsetX += width;
			_offsetYMax = Math.Max(_offsetYMax, height);

			return Point2;
		}
	}
}