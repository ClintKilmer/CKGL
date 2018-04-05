using System.IO;

using OpenGL;

using GLint = System.Int32;
using GLuint = System.UInt32;

namespace CKGL
{
	public class Texture2D : Texture
	{
		public static GLuint currentlyBoundTexture2D { get; private set; }

		Texture2D(TextureFormat format) : base(GL.GenTexture(), format, TextureTarget.Texture2D, TextureTarget.Texture2D)
		{
			WrapX = DefaultWrapX;
			WrapY = DefaultWrapY;
			MinFilter = DefaultMinFilter;
			MagFilter = DefaultMagFilter;
		}
		//public Texture2D(Bitmap bitmap) : this(TextureFormat.RGBA)
		//{
		//	Width = bitmap.Width;
		//	Height = bitmap.Height;
		//	SetPixels(bitmap);
		//}
		//public Texture2D(string file, bool premultiply) : this(TextureFormat.RGBA)
		//{
		//	var bitmap = new Bitmap(file);
		//	if (premultiply)
		//		bitmap.Premultiply();

		//	Width = bitmap.Width;
		//	Height = bitmap.Height;
		//	SetPixels(bitmap);
		//}
		public Texture2D(int width, int height, TextureFormat format) : this(format)
		{
			Width = width;
			Height = height;
			SetPixels(null as byte[], 1, format.PixelFormat());
		}

		public TextureWrap WrapX
		{
			get { return (TextureWrap)GetParam(TextureParam.WrapS); }
			set { SetParam(TextureParam.WrapS, (int)value); }
		}

		public TextureWrap WrapY
		{
			get { return (TextureWrap)GetParam(TextureParam.WrapT); }
			set { SetParam(TextureParam.WrapT, (int)value); }
		}

		public void SetWrap(TextureWrap wrap)
		{
			WrapX = wrap;
			WrapY = wrap;
		}

		public TextureFilter MinFilter
		{
			get { return (TextureFilter)GetParam(TextureParam.MinFilter); }
			set { SetParam(TextureParam.MinFilter, (int)value); }
		}

		public TextureFilter MagFilter
		{
			get { return (TextureFilter)GetParam(TextureParam.MagFilter); }
			set { SetParam(TextureParam.MagFilter, (int)value); }
		}

		public void SetFilter(TextureFilter filter)
		{
			MinFilter = filter;
			MagFilter = filter;
		}

		int GetParam(TextureParam p)
		{
			MakeCurrent();
			GL.GetTexParameterI(BindTarget, p, out int val);
			return val;
		}

		void SetParam(TextureParam p, int val)
		{
			MakeCurrent();
			GL.TexParameterI(BindTarget, p, val);
		}

		public void Bind()
		{
			MakeCurrent();
			currentlyBoundTexture2D = ID;
		}

		#region Public Texture2D Save Methods
		public void SaveAsJpeg(Stream stream, int width, int height)
		{
			// Get the Texture2D pixels
			byte[] data = new byte[Width * Height * 4]; // TextureFormat.RGBA = 4
			GetPixels(ref data, 4, PixelFormat.RGBA);
			Platform.SaveJPG(stream, width, height, Width, Height, data);
		}

		public void SaveAsPng(Stream stream, int width, int height)
		{
			// Get the Texture2D pixels
			byte[] data = new byte[Width * Height * 4]; // TextureFormat.RGBA = 4
			GetPixels(ref data, 4, PixelFormat.RGBA);
			Platform.SavePNG(stream, width, height, Width, Height, data);
		}
		#endregion

		#region LoadTexture2DFromStream
		public static Texture2D LoadTexture2DFromStream(string file)
		{
			Texture2D texture;
			using (var fileStream = new System.IO.FileStream(file, System.IO.FileMode.Open))
			{
				texture = FromStream(fileStream);
			}
			return texture;
		}
		#endregion

		#region Public Static Texture2D Load Methods
		public static Texture2D FromStream(Stream stream)
		{
			// Read the image data from the stream
			TextureDataFromStream(stream, out int width, out int height, out byte[] pixels);

			// Create the Texture2D from the raw pixel data
			Texture2D result = new Texture2D(width, height, TextureFormat.RGBA);
			result.SetPixelsRGBA(pixels);
			return result;
		}

		public static Texture2D FromStream(Stream stream, int width, int height, bool zoom)
		{
			// Read the image data from the stream
			TextureDataFromStream(stream, out int realWidth, out int realHeight, out byte[] pixels, width, height, zoom);

			// Create the Texture2D from the raw pixel data
			Texture2D result = new Texture2D(realWidth, realHeight, TextureFormat.RGBA);
			result.SetPixelsRGBA(pixels);
			return result;
		}
		#endregion

		#region Public Static Texture2D Extensions
		public static void TextureDataFromStream(Stream stream, out int width, out int height, out byte[] pixels, int requestedWidth = -1, int requestedHeight = -1, bool zoom = false)
		{
			Platform.TextureDataFromStream(stream, out width, out height, out pixels, requestedWidth, requestedHeight, zoom);
		}
		#endregion
	}
}