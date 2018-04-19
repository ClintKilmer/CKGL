using System;

using OpenGL;

using GLint = System.Int32;
using GLuint = System.UInt32;

namespace CKGL
{
	public class Texture2D : Texture
	{
		#region Constructors
		public Texture2D(int width, int height, TextureFormat textureFormat) : this(width, height, textureFormat, TextureFilter.Nearest, TextureWrap.Clamp) { }
		public Texture2D(int width, int height, TextureFormat textureFormat, TextureFilter filter, TextureWrap wrap)
			: base(textureFormat, TextureTarget.Texture2D, TextureTarget.Texture2D, filter, filter, wrap, wrap)
		{
			Width = width;
			Height = height;
			SetData(null as byte[], textureFormat.PixelFormat());
		}

		public Texture2D(byte[] data, int width, int height, TextureFormat textureFormat) : this(data, width, height, textureFormat, TextureFilter.Nearest, TextureWrap.Clamp) { }
		public Texture2D(byte[] data, int width, int height, TextureFormat textureFormat, TextureFilter filter, TextureWrap wrap)
			: base(textureFormat, TextureTarget.Texture2D, TextureTarget.Texture2D, filter, filter, wrap, wrap)
		{
			Width = width;
			Height = height;
			SetData(data, textureFormat.PixelFormat());
		}

		public Texture2D(Colour[] data, int width, int height, TextureFormat textureFormat) : this(data, width, height, textureFormat, TextureFilter.Nearest, TextureWrap.Clamp) { }
		public Texture2D(Colour[] data, int width, int height, TextureFormat textureFormat, TextureFilter filter, TextureWrap wrap)
			: base(textureFormat, TextureTarget.Texture2D, TextureTarget.Texture2D, filter, filter, wrap, wrap)
		{
			Width = width;
			Height = height;
			SetData(data);
		}

		public Texture2D(Colour[,] data, int width, int height, TextureFormat textureFormat) : this(data, width, height, textureFormat, TextureFilter.Nearest, TextureWrap.Clamp) { }
		public Texture2D(Colour[,] data, int width, int height, TextureFormat textureFormat, TextureFilter filter, TextureWrap wrap)
			: base(textureFormat, TextureTarget.Texture2D, TextureTarget.Texture2D, filter, filter, wrap, wrap)
		{
			Width = width;
			Height = height;
			SetData2D(data);
		}
		#endregion

		#region Static Constructors
		public static Texture2D CreateFromFile(string file) => CreateFromFile(file, TextureFilter.Nearest, TextureWrap.Clamp);
		public static Texture2D CreateFromFile(string file, TextureFilter textureFilter, TextureWrap textureWrap)
		{
			Platform.GetImageData(file, out int width, out int height, out byte[] data);
			Texture2D result = new Texture2D(data, width, height, TextureFormat.RGBA8, textureFilter, textureWrap);
			return result;
		}
		#endregion

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
		public void SavePNG(string file) => SavePNG(file, Width, Height);
		public void SavePNG(string file, int width, int height)
		{
			Platform.SavePNG(file, width, height, Width, Height, GetData(PixelFormat.RGBA));
		}

		public void SaveJPG(string file) => SaveJPG(file, Width, Height);
		public void SaveJPG(string file, int width, int height)
		{
			Platform.SaveJPG(file, width, height, Width, Height, GetData(PixelFormat.RGBA));
		}
		#endregion
	}
}