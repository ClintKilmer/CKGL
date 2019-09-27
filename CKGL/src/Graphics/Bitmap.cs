using System;

namespace CKGL
{
	public class Bitmap
	{
		private Colour[] pixels;
		public Colour[] Pixels => pixels;
		public int Width { get; private set; }
		public int Height { get; private set; }
		public int PixelCount { get; private set; }

		public Bitmap(int width, int height)
		{
			Width = width;
			Height = height;
			PixelCount = width * height;
			pixels = new Colour[PixelCount];
		}
		public Bitmap(int width, int height, Colour colour) : this(width, height)
		{
			Clear(colour);
		}
		public Bitmap(byte[] data, int width, int height, int components = 4) : this(width, height)
		{
			for (int i = 0; i < width * height * components; i += components)
			{
				int j = i / components;
				int y = j / width;
				int x = j - y * width;
				switch (components)
				{
					case 1:
						this[x, y] = new Colour(data[i], 0, 0, 255);
						break;
					case 2:
						this[x, y] = new Colour(data[i], data[i + 1], 0, 255);
						break;
					case 3:
						this[x, y] = new Colour(data[i], data[i + 1], data[i + 2], 255);
						break;
					case 4:
						this[x, y] = new Colour(data[i], data[i + 1], data[i + 2], data[i + 3]);
						break;
					default:
						throw new IllegalValueException(typeof(int), components);
				}
			}
		}
		public Bitmap(Colour[] pixels, int width, int height) : this(width, height)
		{
			SetPixels(pixels, width, height);
		}
		public Bitmap(Colour[,] pixels) : this(pixels.GetLength(0), pixels.GetLength(1))
		{
			SetPixels(pixels);
		}

		public byte[] Bytes(int components = 4)
		{
			byte[] bytes = new byte[PixelCount * components];

			for (int i = 0; i < PixelCount; i++)
			{
				bytes[i * components] = pixels[i].r;
				if (components > 1)
					bytes[i * components + 1] = pixels[i].g;
				if (components > 2)
					bytes[i * components + 2] = pixels[i].b;
				if (components > 3)
					bytes[i * components + 3] = pixels[i].a;
			}

			return bytes;
		}

		public Colour this[int index]
		{
			get
			{
				return pixels[index];
			}
			set
			{
				pixels[index] = value;
			}
		}

		public Colour this[int x, int y]
		{
			get
			{
				return pixels[Width * y + x];
			}
			set
			{
				pixels[Width * y + x] = value;
			}
		}

		public void Clear(Colour colour)
		{
			for (int i = 0; i < PixelCount; ++i)
				pixels[i] = colour;
		}
		public void Clear()
		{
			Clear(Colour.Transparent);
		}

		public void SetPixels(Colour[] pixels, int width, int height)
		{
			if (width * height != pixels.Length)
				throw new Exception("Pixel array length must match bitmap pixel array length.");

			if (width != Width || height != Height)
				throw new Exception("Pixel array dimensions must match bitmap dimensions.");

			this.pixels = pixels;
		}

		public void SetPixels(Colour[,] pixels)
		{
			if (pixels.Length != PixelCount)
				throw new Exception("Pixel array length must match bitmap pixel array length.");

			if (pixels.GetLength(0) != Width || pixels.GetLength(1) != Height)
				throw new Exception("Pixel array dimensions must match bitmap dimensions.");

			Colour[] _pixels = new Colour[Width * Height];
			for (int y = 0; y < Height; y++)
			{
				for (int x = 0; x < Width; x++)
				{
					_pixels[Width * y + x] = pixels[x, y];
				}
			}
			this.pixels = _pixels;
		}

		public void Resize(int width, int height)
		{
			if (width != Width || height != Height)
			{
				Width = width;
				Height = height;
				if (PixelCount != width * height)
				{
					PixelCount = width * height;
					if (pixels.Length < PixelCount)
						Array.Resize(ref pixels, PixelCount);
				}
			}
		}

		#region Public Texture2D Save Methods
		public void SavePNG(string file) => SavePNG(file, Width, Height);
		public void SavePNG(string file, int width, int height)
		{
			// Temporary
			//Platform.SavePNG(file, width, height, Width, Height, GetData(PixelFormat.RGBA));
			throw new CKGLException("Temporary disable");
		}

		public void SaveJPG(string file) => SaveJPG(file, Width, Height);
		public void SaveJPG(string file, int width, int height)
		{
			// Temporary
			//Platform.SaveJPG(file, width, height, Width, Height, GetData(PixelFormat.RGBA));
			throw new CKGLException("Temporary disable");
		}
		#endregion

		#region Overrides
		public override string ToString()
		{
			return $"Bitmap [Width: {Width}, Height: {Height}, PixelCount: {PixelCount}]";
		}

		public override bool Equals(object obj)
		{
			return obj is Bitmap && Equals((Bitmap)obj);
		}
		public bool Equals(Bitmap bitmap)
		{
			return this == bitmap;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hash = 17;
				for (int i = 0; i < PixelCount; ++i)
					hash = hash * 23 + pixels[i].GetHashCode();
				return hash;
			}
		}
		#endregion

		#region Operators
		public static bool operator ==(Bitmap a, Bitmap b)
		{
			if (a.Width != b.Width || a.Height != b.Height || a.PixelCount != b.PixelCount)
				return false;

			for (int i = 0; i < a.PixelCount; ++i)
				if (a[i] != b[i])
					return false;

			return true;
		}

		public static bool operator !=(Bitmap a, Bitmap b)
		{
			return !(a == b);
		}
		#endregion

		#region Implicit Conversion Operators
		public static implicit operator Colour[](Bitmap val)
		{
			return val.pixels;
		}
		#endregion
	}
}