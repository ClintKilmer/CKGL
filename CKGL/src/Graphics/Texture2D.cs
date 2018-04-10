using System;
using System.IO;

using OpenGL;

using GLint = System.Int32;
using GLuint = System.UInt32;

namespace CKGL
{
	public class Texture2D : Texture
	{
		public Texture2D(int width, int height, TextureFormat textureFormat)
			: this(width, height, textureFormat, DefaultMinFilter, DefaultMagFilter, DefaultWrapX, DefaultWrapY) { }
		public Texture2D(int width, int height, TextureFormat textureFormat, TextureFilter filter, TextureWrap wrap)
			: this(width, height, textureFormat, filter, filter, wrap, wrap) { }
		public Texture2D(int width, int height, TextureFormat textureFormat, TextureFilter minFilter, TextureFilter magFilter, TextureWrap wrapX, TextureWrap wrapY)
			: base(textureFormat, TextureTarget.Texture2D, TextureTarget.Texture2D, minFilter, magFilter, wrapX, wrapY)
		{
			Width = width;
			Height = height;
			SetData(null as byte[], textureFormat.PixelFormat());
		}

		#region SetData
		public void SetData2D(Colour[,] data)
		{
			Colour[] _data = new Colour[Width * Height];

			for (int y = 0; y < Height; y++)
			{
				for (int x = 0; x < Width; x++)
				{
					_data[Width * y + x] = data[x, y];
				}
			}

			SetData(_data);
		}
		public unsafe void SetData(Colour[] data)
		{
			if (data != null && data.Length < Width * Height)
				throw new Exception("Data array is not large enough.");
			Bind();
			fixed (Colour* ptr = data)
				GL.TexImage2D(DataTarget, 0, Format, Width, Height, 0, PixelFormat.RGBA, PixelType.UnsignedByte, new IntPtr(ptr));
		}

		public unsafe void SetData(byte[] data, PixelFormat pixelFormat)
		{
			if (data != null && data.Length < Width * Height * pixelFormat.Components())
				throw new Exception("Data array is not large enough.");
			Bind();
			fixed (byte* ptr = data)
				GL.TexImage2D(DataTarget, 0, Format, Width, Height, 0, pixelFormat, PixelType.UnsignedByte, new IntPtr(ptr));
		}
		#endregion

		#region GetData
		public Colour[,] GetData2D()
		{
			Colour[] _data = GetData();
			Colour[,] data = new Colour[Width, Height];

			for (int y = 0; y < Height; y++)
			{
				for (int x = 0; x < Width; x++)
				{
					data[x, y] = _data[Width * y + x];
				}
			}

			return data;
		}
		public unsafe Colour[] GetData()
		{
			Colour[] data = new Colour[Width * Height];
			Bind();
			fixed (Colour* ptr = data)
				GL.GetTexImage(TextureTarget.Texture2D, 0, PixelFormat.RGBA, PixelType.UnsignedByte, new IntPtr(ptr));
			return data;
		}

		public unsafe byte[] GetData(PixelFormat pixelFormat)
		{
			byte[] data = new byte[Width * Height * pixelFormat.Components()];
			Bind();
			fixed (byte* ptr = data)
				GL.GetTexImage(DataTarget, 0, pixelFormat, PixelType.UnsignedByte, new IntPtr(ptr));
			return data;
		}
		#endregion

		#region Public Texture2D Save Methods
		public void SaveJPG(Stream stream) => SaveJPG(stream, Width, Height);
		public void SaveJPG(Stream stream, int width, int height)
		{
			Platform.SaveJPG(stream, width, height, Width, Height, GetData(PixelFormat.RGBA));
		}

		public void SavePNG(Stream stream) => SavePNG(stream, Width, Height);
		public void SavePNG(Stream stream, int width, int height)
		{
			Platform.SavePNG(stream, width, height, Width, Height, GetData(PixelFormat.RGBA));
		}
		#endregion

		#region Public Static Texture2D Load Methods
		public static Texture2D LoadTexture2DFromStream(string file, TextureFilter textureFilter, TextureWrap textureWrap)
		{
			Texture2D texture = LoadTexture2DFromStream(file);
			texture.SetFilter(textureFilter);
			texture.SetWrap(textureWrap);
			return texture;
		}
		public static Texture2D LoadTexture2DFromStream(string file)
		{
			Texture2D texture;
			using (var fileStream = new System.IO.FileStream(file, System.IO.FileMode.Open))
				texture = FromStream(fileStream);
			return texture;
		}
		public static Texture2D FromStream(Stream stream)
		{
			// Read the image data from the stream
			Platform.TextureDataFromStream(stream, out int width, out int height, out byte[] data);

			// Create the Texture2D from the raw pixel data
			Texture2D result = new Texture2D(width, height, TextureFormat.RGBA8);
			result.SetData(data, PixelFormat.RGBA);
			return result;
		}
		#endregion
	}
}