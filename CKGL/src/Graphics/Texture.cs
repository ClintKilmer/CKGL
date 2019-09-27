namespace CKGL
{
	public abstract class Texture
	{
		public TextureType Type { get; protected set; }
		public int Width { get; protected set; }
		public int Height { get; protected set; }
		public int Depth { get; protected set; }
		public TextureFormat Format { get; protected set; }

		public static int Swaps { get; protected set; }

		private const TextureFormat DefaultFormat = TextureFormat.RGBA8;
		private const TextureFilter DefaultMinFilter = TextureFilter.Nearest;
		private const TextureFilter DefaultMagFilter = TextureFilter.Nearest;
		private const TextureWrap DefaultWrapX = TextureWrap.Clamp;
		private const TextureWrap DefaultWrapY = TextureWrap.Clamp;

		public static Texture Create2D(int width, int height,
									   TextureFormat textureFormat,
									   TextureFilter filter,
									   TextureWrap wrap) => Create2D(width, height, textureFormat, filter, filter, wrap, wrap);
		public static Texture Create2D(int width, int height,
									   TextureFormat format = DefaultFormat,
									   TextureFilter minFilter = DefaultMinFilter, TextureFilter magFilter = DefaultMagFilter,
									   TextureWrap wrapX = DefaultWrapX, TextureWrap wrapY = DefaultWrapY) => Create2D(null, width, height, format, minFilter, magFilter, wrapX, wrapY);

		public static Texture Create2D(byte[] data,
									   int width, int height,
									   TextureFormat textureFormat,
									   TextureFilter filter,
									   TextureWrap wrap) => Create2D(data, width, height, textureFormat, filter, filter, wrap, wrap);
		public static Texture Create2D(byte[] data,
									   int width, int height,
									   TextureFormat format = DefaultFormat,
									   TextureFilter minFilter = DefaultMinFilter, TextureFilter magFilter = DefaultMagFilter,
									   TextureWrap wrapX = DefaultWrapX, TextureWrap wrapY = DefaultWrapY)
		{
			return Graphics.CreateTexture2D(data,
											width, height,
											format,
											minFilter, magFilter,
											wrapX, wrapY);
		}

		public static Texture Create2D(Bitmap bitmap,
									   TextureFormat textureFormat,
									   TextureFilter filter,
									   TextureWrap wrap) => Create2D(bitmap, textureFormat, filter, filter, wrap, wrap);
		public static Texture Create2D(Bitmap bitmap,
									   TextureFormat format = DefaultFormat,
									   TextureFilter minFilter = DefaultMinFilter, TextureFilter magFilter = DefaultMagFilter,
									   TextureWrap wrapX = DefaultWrapX, TextureWrap wrapY = DefaultWrapY) => Create2D(bitmap.Bytes(), bitmap.Width, bitmap.Height, format, minFilter, magFilter, wrapX, wrapY);

		public static Texture Create2DFromFile(string file,
											   TextureFormat textureFormat,
											   TextureFilter filter,
											   TextureWrap wrap) => Create2DFromFile(file, textureFormat, filter, filter, wrap, wrap);
		public static Texture Create2DFromFile(string file,
											   TextureFormat format = DefaultFormat,
											   TextureFilter minFilter = DefaultMinFilter, TextureFilter magFilter = DefaultMagFilter,
											   TextureWrap wrapX = DefaultWrapX, TextureWrap wrapY = DefaultWrapY)
		{
			Platform.LoadImage(file, out int width, out int height, out byte[] data);
			return Graphics.CreateTexture2D(data,
											width, height,
											format,
											minFilter, magFilter,
											wrapX, wrapY);
		}

		public static void PreDraw()
		{
			Swaps = 0;
		}

		public abstract void Destroy();

		public abstract byte[] GetData();
		public abstract Bitmap GetBitmap();

		#region Public Texture2D Save Methods
		public void SavePNG(string file) => SavePNG(file, Width, Height);
		public void SavePNG(string file, int width, int height)
		{
			GetBitmap().SavePNG(file, width, height);
		}

		public void SaveJPG(string file) => SaveJPG(file, Width, Height);
		public void SaveJPG(string file, int width, int height)
		{
			GetBitmap().SaveJPG(file, width, height);
		}
		#endregion

		#region Parameters
		protected abstract TextureWrap Wrap { set; }
		protected abstract TextureWrap WrapX { get; set; }
		protected abstract TextureWrap WrapY { get; set; }
		protected abstract TextureFilter Filter { set; }
		protected abstract TextureFilter MinFilter { get; set; }
		protected abstract TextureFilter MagFilter { get; set; }
		#endregion

		#region Bind
		public abstract void Bind();
		public abstract void Bind(uint textureSlot);
		#endregion

		#region Overrides
		public abstract override string ToString();

		public abstract override bool Equals(object obj);
		public abstract bool Equals(Texture texture);

		public abstract override int GetHashCode();
		#endregion
	}
}