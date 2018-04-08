using System;
using System.Collections.Generic;

using OpenGL;

using GLint = System.Int32;
using GLuint = System.UInt32;

namespace CKGL
{
	public abstract class Texture
	{
		private struct Binding
		{
			public GLuint ID;
			public TextureTarget Target;
		}
		private static Binding[] bindings = new Binding[GL.MaxTextureUnits];

		public static TextureFilter DefaultMinFilter = TextureFilter.Linear;
		public static TextureFilter DefaultMagFilter = TextureFilter.Linear;
		public static TextureWrap DefaultWrapX = TextureWrap.ClampToEdge;
		public static TextureWrap DefaultWrapY = TextureWrap.ClampToEdge;

		private GLuint id;

		public int Width { get; set; }
		public int Height { get; set; }
		public TextureFormat Format { get; private set; }
		public TextureTarget BindTarget { get; private set; }
		public TextureTarget DataTarget { get; private set; }

		protected Texture(TextureFormat format, TextureTarget bindTarget, TextureTarget dataTarget) : this(format, bindTarget, dataTarget, DefaultMinFilter, DefaultMagFilter, DefaultWrapX, DefaultWrapY) { }
		protected Texture(TextureFormat format, TextureTarget bindTarget, TextureTarget dataTarget, TextureFilter filter, TextureWrap wrap) : this(format, bindTarget, dataTarget, filter, filter, wrap, wrap) { }
		protected Texture(TextureFormat format, TextureTarget bindTarget, TextureTarget dataTarget, TextureFilter minFilter, TextureFilter magFilter, TextureWrap wrapX, TextureWrap wrapY)
		{
			id = GL.GenTexture();
			Format = format;
			BindTarget = bindTarget;
			DataTarget = dataTarget;
			MinFilter = minFilter;
			MagFilter = magFilter;
			WrapX = wrapX;
			WrapY = wrapY;
		}

		public void Destroy()
		{
			if (id != default(GLuint))
			{
				GL.DeleteTexture(id);
				id = default(GLuint);
			}
		}

		#region Parameters
		public TextureWrap WrapX
		{
			get { return (TextureWrap)GetParam(TextureParam.WrapS); }
			set { SetParam(TextureParam.WrapS, (GLint)value); }
		}

		public TextureWrap WrapY
		{
			get { return (TextureWrap)GetParam(TextureParam.WrapT); }
			set { SetParam(TextureParam.WrapT, (GLint)value); }
		}

		public void SetWrap(TextureWrap wrap)
		{
			WrapX = wrap;
			WrapY = wrap;
		}

		public TextureFilter MinFilter
		{
			get { return (TextureFilter)GetParam(TextureParam.MinFilter); }
			set { SetParam(TextureParam.MinFilter, (GLint)value); }
		}

		public TextureFilter MagFilter
		{
			get { return (TextureFilter)GetParam(TextureParam.MagFilter); }
			set { SetParam(TextureParam.MagFilter, (GLint)value); }
		}

		public void SetFilter(TextureFilter filter)
		{
			MinFilter = filter;
			MagFilter = filter;
		}

		private int GetParam(TextureParam p)
		{
			Bind();
			GL.GetTexParameterI(BindTarget, p, out GLint val);
			return val;
		}

		private void SetParam(TextureParam p, GLint val)
		{
			Bind();
			GL.TexParameterI(BindTarget, p, val);
		}
		#endregion

		#region Bind
		public bool IsBound() => IsBound(0);
		public bool IsBound(GLuint textureSlot) => IsBound(textureSlot, BindTarget);
		private bool IsBound(GLuint textureSlot, TextureTarget target)
		{
			return bindings[textureSlot].ID != id && bindings[textureSlot].Target != target;
		}

		public void Bind() => Bind(0, BindTarget);
		public void Bind(GLuint textureSlot) => Bind(textureSlot, BindTarget);
		private void Bind(GLuint textureSlot, TextureTarget target)
		{
			if (!IsBound(textureSlot, target))
			{
				GL.ActiveTexture(textureSlot);
				GL.BindTexture(target, id);

				bindings[textureSlot].ID = id;
				bindings[textureSlot].Target = target;
			}
		}
		#endregion

		// TODO - refactor comp into lookup method
		#region SetPixels
		internal unsafe void SetPixels(byte[] pixels, int comp, PixelFormat format)
		{
			if (pixels != null && pixels.Length < Width * Height * comp)
				throw new Exception("Pixels array is not large enough.");
			Bind();
			fixed (byte* ptr = pixels)
				GL.TexImage2D(DataTarget, 0, Format, Width, Height, 0, format, PixelType.UnsignedByte, new IntPtr(ptr));
		}
		public void SetPixelsRGBA(byte[] pixels)
		{
			SetPixels(pixels, 4, PixelFormat.RGBA);
		}
		public void SetPixelsRGB(byte[] pixels)
		{
			SetPixels(pixels, 3, PixelFormat.RGB);
		}
		public void SetPixelsRG(byte[] pixels)
		{
			SetPixels(pixels, 2, PixelFormat.RG);
		}
		public void SetPixelsR(byte[] pixels)
		{
			SetPixels(pixels, 1, PixelFormat.R);
		}
		#endregion

		// TODO - refactor comp into lookup method
		#region GetPixels
		internal unsafe void GetPixels(ref byte[] pixels, int comp, PixelFormat format)
		{
			if (pixels == null)
				pixels = new byte[Width * Height * comp];
			else if (pixels.Length < Width * Height * comp)
				throw new Exception("Pixels array is not large enough.");
			Bind();
			fixed (byte* ptr = pixels)
				GL.GetTexImage(DataTarget, 0, format, PixelType.UnsignedByte, new IntPtr(ptr));
		}
		public void GetPixelsRGBA(ref byte[] pixels)
		{
			GetPixels(ref pixels, 4, PixelFormat.RGBA);
		}
		public void GetPixelsRGB(ref byte[] pixels)
		{
			GetPixels(ref pixels, 3, PixelFormat.RGB);
		}
		public void GetPixelsRG(ref byte[] pixels)
		{
			GetPixels(ref pixels, 2, PixelFormat.RG);
		}
		public void GetPixelsR(ref byte[] pixels)
		{
			GetPixels(ref pixels, 1, PixelFormat.R);
		}
		#endregion

		#region Operators
		public static bool operator ==(Texture a, Texture b)
		{
			return a.id == b.id;
		}
		public static bool operator !=(Texture a, Texture b)
		{
			return a.id != b.id;
		}
		#endregion

		#region Static TextureFormat Size Method

		public static int GetTextureFormatSize(TextureFormat textureFormat)
		{
			// TODO?
			//TextureFormatExt.PixelFormat()

			switch (textureFormat)
			{
				case TextureFormat.RGBA8:
					return 8;
				default:
					throw new NotImplementedException("Unexpected value from TextureFormat");
			}
		}

		#endregion
	}
}