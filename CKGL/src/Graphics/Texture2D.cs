using System.IO;

using OpenGL;

using GLint = System.Int32;
using GLuint = System.UInt32;

namespace CKGL
{
	public class Texture2D : Texture
	{
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
		// TODO - comp size lookup
		public Texture2D(int width, int height, TextureFormat format) : this(format)
		{
			Width = width;
			Height = height;
			SetPixels(null as byte[], 1, format.PixelFormat());
		}
		public Texture2D(TextureFormat format)
			: base(format, TextureTarget.Texture2D, TextureTarget.Texture2D) { }
		public Texture2D(TextureFormat format, TextureFilter filter, TextureWrap wrap)
			: base(format, TextureTarget.Texture2D, TextureTarget.Texture2D, filter, wrap) { }
		public Texture2D(TextureFormat format, TextureFilter minFilter, TextureFilter magFilter, TextureWrap wrapX, TextureWrap wrapY)
			: base(format, TextureTarget.Texture2D, TextureTarget.Texture2D, minFilter, magFilter, wrapX, wrapY) { }

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
		public static Texture2D LoadTexture2DFromStream(string file, TextureFilter textureFilter)
		{
			Texture2D texture = LoadTexture2DFromStream(file);
			texture.SetFilter(textureFilter);
			return texture;
		}
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

		// TODO - custom TextureFormat?
		#region Public Static Texture2D Load Methods
		public static Texture2D FromStream(Stream stream)
		{
			// Read the image data from the stream
			TextureDataFromStream(stream, out int width, out int height, out byte[] pixels);

			// Create the Texture2D from the raw pixel data
			Texture2D result = new Texture2D(width, height, TextureFormat.RGBA8);
			result.SetPixelsRGBA(pixels);
			return result;
		}

		public static Texture2D FromStream(Stream stream, int width, int height, bool zoom)
		{
			// Read the image data from the stream
			TextureDataFromStream(stream, out int realWidth, out int realHeight, out byte[] pixels, width, height, zoom);

			// Create the Texture2D from the raw pixel data
			Texture2D result = new Texture2D(realWidth, realHeight, TextureFormat.RGBA8);
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