using CKGL.OpenGLBindings;
using GLint = System.Int32;
using GLuint = System.UInt32;

namespace CKGL.OpenGL
{
	public class OpenGLTexture : Texture
	{
		internal GLuint ID => id;
		internal TextureTarget TextureTarget;

		private GLuint id;

		private struct Binding
		{
			public GLuint ID;
			public TextureTarget Target;
		}
		private static readonly Binding[] bindings = new Binding[GL.MaxTextureImageUnits];

		internal OpenGLTexture(byte[] data, TextureType type,
							   int width, int height, int depth,
							   TextureFormat format,
							   TextureFilter minFilter, TextureFilter magFilter,
							   TextureWrap wrapX, TextureWrap wrapY)
		{
			Type = type;
			Width = width;
			Height = height;
			Depth = depth;
			Format = format;
			id = GL.GenTexture();
			switch (Type)
			{
				case TextureType.Texture1D when Platform.GraphicsBackend == GraphicsBackend.OpenGL: // Not available in OpenGL ES
					TextureTarget = TextureTarget.Texture1D;
					break;
				case TextureType.Texture1DArray when Platform.GraphicsBackend == GraphicsBackend.OpenGL: // Not available in OpenGL ES
					TextureTarget = TextureTarget.Texture1DArray;
					break;
				case TextureType.Texture2D:
					TextureTarget = TextureTarget.Texture2D;
					break;
				case TextureType.Texture2DArray:
					TextureTarget = TextureTarget.Texture2DArray;
					break;
				case TextureType.Texture2DMultisample:
					TextureTarget = TextureTarget.Texture2DMultisample;
					break;
				case TextureType.Texture3D:
					TextureTarget = TextureTarget.Texture3D;
					break;
				default:
					throw new IllegalValueException(typeof(TextureType), Type);
			}
			MinFilter = minFilter;
			MagFilter = magFilter;
			WrapX = wrapX;
			WrapY = wrapY;

			SetData(data);
		}

		private unsafe void SetData(byte[] data)
		{
			switch (Type)
			{
				//case TextureType.Texture1D when Platform.GraphicsBackend == GraphicsBackend.OpenGL: // Not available in OpenGL ES
				//	break;
				//case TextureType.Texture1DArray when Platform.GraphicsBackend == GraphicsBackend.OpenGL: // Not available in OpenGL ES
				//	break;
				case TextureType.Texture2D:
					if (data != null && data.Length < Width * Height * Format.ToOpenGL().PixelFormat().Components())
						throw new CKGLException("Data array is not large enough to fill texture.");
					Bind();
					fixed (byte* ptr = data)
						GL.TexImage2D(TextureTarget, 0, Format.ToOpenGL(), Width, Height, 0, Format.ToOpenGL().PixelFormat(), Format.ToOpenGL().PixelType(), data != null ? ptr : null);
					break;
				//case TextureType.Texture2DArray:
				//	break;
				//case TextureType.Texture2DMultisample:
				//	break;
				//case TextureType.Texture3D:
				//	break;
				default:
					throw new IllegalValueException(typeof(TextureType), Type);
			}
		}

		public override void Destroy()
		{
			if (id != default)
			{
				GL.DeleteTexture(id);
				id = default;
			}
		}

		public override unsafe Bitmap Bitmap(RectangleI rectangle)
		{
			switch (Type)
			{
				//case TextureType.Texture1D when Platform.GraphicsBackend == GraphicsBackend.OpenGL: // Not available in OpenGL ES
				//	break;
				//case TextureType.Texture1DArray when Platform.GraphicsBackend == GraphicsBackend.OpenGL: // Not available in OpenGL ES
				//	break;
				case TextureType.Texture2D:
					#region Old OpenGL-only implementation with glTexImage
					//Colour[] data = new Colour[Width * Height];
					//Bind();
					//fixed (Colour* ptr = data)
					//	GL.GetTexImage(TextureTarget, 0, Format.ToOpenGL().PixelFormat(), Format.ToOpenGL().PixelType(), ptr);
					//return new Bitmap(data, Width, Height); 
					#endregion

					// OpenGL ES friendly implementation with crop functionality
					Framebuffer originalFramebuffer = Framebuffer.Current;

					GLuint id = GL.GenFramebuffer();
					GL.BindFramebuffer(FramebufferTarget.Framebuffer, id);
					//Framebuffer.Swaps++; // TODO
					GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, TextureAttachment.Colour0.ToOpenGL(), TextureTarget, ID, 0);
					FramebufferStatus status = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
					if (status != FramebufferStatus.Complete)
						throw new CKGLException("Invalid Framebuffer: " + status);

					GL.ReadBuffer(ReadBuffer.Colour0);
					Bitmap bitmap = new Bitmap(GL.ReadPixelsAsColourArray(rectangle, PixelFormat.RGBA), rectangle.W, rectangle.H);

					originalFramebuffer.Bind();

					return bitmap;
				//case TextureType.Texture2DArray:
				//	break;
				//case TextureType.Texture2DMultisample:
				//	break;
				//case TextureType.Texture3D:
				//	break;
				default:
					throw new IllegalValueException(typeof(TextureType), Type);
			}
		}

		#region Parameters
		public override TextureWrap Wrap
		{
			set
			{
				WrapX = value;
				WrapY = value;
			}
		}

		public override TextureWrap WrapX
		{
			get { return (TextureWrap)GetParam(TextureParam.WrapS); }
			set { SetParam(TextureParam.WrapS, (GLint)value.ToOpenGL()); }
		}

		public override TextureWrap WrapY
		{
			get { return (TextureWrap)GetParam(TextureParam.WrapT); }
			set { SetParam(TextureParam.WrapT, (GLint)value.ToOpenGL()); }
		}

		public override TextureFilter Filter
		{
			set
			{
				MinFilter = value;
				MagFilter = value;
			}
		}

		public override TextureFilter MinFilter
		{
			get { return (TextureFilter)GetParam(TextureParam.MinFilter); }
			set { SetParam(TextureParam.MinFilter, (GLint)value.ToOpenGL()); }
		}

		public override TextureFilter MagFilter
		{
			get { return (TextureFilter)GetParam(TextureParam.MagFilter); }
			set
			{
				switch (value)
				{
					case TextureFilter.Linear:
					case TextureFilter.LinearMipmapLinear:
					case TextureFilter.LinearMipmapNearest:
						SetParam(TextureParam.MagFilter, (GLint)TextureFilter.Linear.ToOpenGL());
						break;
					case TextureFilter.Nearest:
					case TextureFilter.NearestMipmapLinear:
					case TextureFilter.NearestMipmapNearest:
						SetParam(TextureParam.MagFilter, (GLint)TextureFilter.Nearest.ToOpenGL());
						break;
				}
			}
		}

		private int GetParam(TextureParam param)
		{
			Bind();
			GL.GetTexParameterI(TextureTarget, param, out GLint val);
			return val;
		}

		private void SetParam(TextureParam param, GLint val)
		{
			Bind();
			GL.TexParameterI(TextureTarget, param, val);
		}
		#endregion

		#region Bind
		public override void Bind() => Bind(0);
		public override void Bind(GLuint textureSlot)
		{
			if (id != bindings[textureSlot].ID || TextureTarget != bindings[textureSlot].Target)
			{
				Graphics.State.OnStateChanging?.Invoke();
				GL.ActiveTexture(textureSlot);
				GL.BindTexture(TextureTarget, id);
				Swaps++;

				bindings[textureSlot].ID = id;
				bindings[textureSlot].Target = TextureTarget;
				Graphics.State.OnStateChanged?.Invoke();
			}
		}

		public override void Unbind()
		{
			for (int i = 0; i < bindings.Length; i++)
			{
				if (id == bindings[i].ID)
				{
					Graphics.State.OnStateChanging?.Invoke();
					GL.ActiveTexture((GLuint)i);
					GL.BindTexture(TextureTarget, 0);
					Swaps++;

					bindings[i].ID = 0;
					bindings[i].Target = TextureTarget;
					Graphics.State.OnStateChanged?.Invoke();
				}
			}
		}
		#endregion

		#region Overrides
		public override string ToString()
		{
			return $"Texture: [id: {id}]";
		}

		public override bool Equals(object obj)
		{
			return obj is OpenGLTexture && Equals((OpenGLTexture)obj);
		}
		public override bool Equals(Texture texture)
		{
			return this == (OpenGLTexture)texture;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hash = 17;
				hash = hash * 23 + id.GetHashCode();
				return hash;
			}
		}
		#endregion

		#region Operators
		public static bool operator ==(OpenGLTexture a, OpenGLTexture b)
		{
			return (a?.id ?? null) == (b?.id ?? null);
		}

		public static bool operator !=(OpenGLTexture a, OpenGLTexture b)
		{
			return (a?.id ?? null) != (b?.id ?? null);
		}
		#endregion
	}
}