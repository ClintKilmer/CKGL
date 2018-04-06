using System;
using System.Collections.Generic;

using OpenGL;

using GLint = System.Int32;
using GLuint = System.UInt32;

namespace CKGL
{
	// TODO - Remove internals?
	public abstract class Texture
	{
		public static GLuint currentlyBoundTexture { get; private set; }

		private struct Binding
		{
			public GLuint ID;
			public TextureTarget Target;
		}

		private static Binding[] bindings = new Binding[GL.MaxTextureUnits];
		private static HashSet<GLuint> unbind = new HashSet<GLuint>();

		public static TextureFilter DefaultMinFilter = TextureFilter.Linear;
		public static TextureFilter DefaultMagFilter = TextureFilter.Linear;
		public static TextureWrap DefaultWrapX = TextureWrap.ClampToEdge;
		public static TextureWrap DefaultWrapY = TextureWrap.ClampToEdge;

		internal GLuint ID { get; private set; }
		public TextureFormat Format { get; private set; }
		internal TextureTarget BindTarget { get; private set; }
		internal TextureTarget DataTarget { get; private set; }
		public int Width { get; internal set; }
		public int Height { get; internal set; }

		internal Texture(GLuint id, TextureFormat format, TextureTarget bindTarget, TextureTarget dataTarget)
		{
			ID = id;
			Format = format;
			BindTarget = bindTarget;
			DataTarget = dataTarget;
		}

		public void Destroy()
		{
			GL.DeleteTexture(ID);
		}

		public void Bind()
		{
			MakeCurrent();
			currentlyBoundTexture = ID;
		}

		#region SetPixels
		internal unsafe void SetPixels(byte[] pixels, int comp, PixelFormat format)
		{
			if (pixels != null && pixels.Length < Width * Height * comp)
				throw new Exception("Pixels array is not large enough.");
			MakeCurrent();
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

		#region GetPixels
		internal unsafe void GetPixels(ref byte[] pixels, int comp, PixelFormat format)
		{
			if (pixels == null)
				pixels = new byte[Width * Height * comp];
			else if (pixels.Length < Width * Height * comp)
				throw new Exception("Pixels array is not large enough.");
			MakeCurrent();
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

		#region MakeCurrent
		internal void MakeCurrent()
		{
			MakeCurrent(ID, BindTarget);
		}
		internal static void MakeCurrent(GLuint id, TextureTarget target)
		{
			if (bindings[0].ID != id)
			{
				GL.ActiveTexture(0);

				//If a texture is already binded to slot 0, unbind it
				if (bindings[0].ID != 0 && bindings[0].Target != target)
					GL.BindTexture(bindings[0].Target, 0);

				bindings[0].ID = id;
				bindings[0].Target = target;
				GL.BindTexture(target, id);
			}
		}
		#endregion

		#region Binding
		internal static GLuint Bind(GLuint id, TextureTarget target)
		{
			//If we're marked for unbinding, unmark us
			unbind.Remove(id);

			//If we're already binded, return our slot
			for (GLuint i = 0; i < bindings.Length; ++i)
				if (bindings[i].ID == id)
					return i;

			//If we're not already binded, bind us and return the slot
			for (GLuint i = 0; i < bindings.Length; ++i)
			{
				if (bindings[i].ID == 0)
				{
					bindings[i].ID = id;
					bindings[i].Target = target;
					GL.ActiveTexture(i);
					GL.BindTexture(target, id);
					return i;
				}
			}

			throw new Exception("You have exceeded the maximum amount of texture bindings: " + GL.MaxTextureUnits);
		}

		internal static void MarkAllForUnbinding()
		{
			for (GLuint i = 0; i < bindings.Length; ++i)
				if (bindings[i].ID != 0)
					unbind.Add(bindings[i].ID);
		}

		internal static void UnbindMarked()
		{
			for (GLuint i = 0; i < bindings.Length; ++i)
			{
				if (bindings[i].ID != 0 && unbind.Contains(bindings[i].ID))
				{
					GL.ActiveTexture(i);
					GL.BindTexture(bindings[i].Target, 0);
					bindings[i].ID = 0;
				}
			}
			unbind.Clear();
		}

		internal static void UnbindAll()
		{
			unbind.Clear();
			for (GLuint i = 0; i < bindings.Length; ++i)
			{
				if (bindings[i].ID != 0)
				{
					GL.ActiveTexture(i);
					GL.BindTexture(bindings[i].Target, 0);
					bindings[i].ID = 0;
				}
			}
		}
		#endregion

		#region Operators
		public static bool operator ==(Texture a, Texture b)
		{
			return a.ID == b.ID;
		}
		public static bool operator !=(Texture a, Texture b)
		{
			return a.ID != b.ID;
		}
		#endregion

		// TODO - Get rid of this original code
		// CKGL Original

		//private GLuint ID;
		//private GLuint localBuffer = 0;

		//public OpenGLTexture()
		//{
		//	Bind();
		//}

		//private void Generate()
		//{
		//	if (ID == default(GLuint))
		//		ID = GL.GenTexture();
		//}

		//public void Destroy()
		//{
		//	if (ID != default(GLuint))
		//	{
		//		GL.DeleteTexture(ID);
		//		ID = default(GLuint);
		//	}
		//}

		//public void Bind()
		//{
		//	Generate();

		//	if (ID != currentlyBoundTexture)
		//	{
		//		GL.BindTexture(TextureTarget.Texture2D, ID);
		//		currentlyBoundTexture = ID;
		//	}
		//}

		//public void UnBind()
		//{
		//	GL.BindTexture(TextureTarget.Texture2D, 0);
		//	currentlyBoundTexture = 0;
		//}
	}
}