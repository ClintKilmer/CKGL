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
							   TextureWrap wrapX, TextureWrap? wrapY, TextureWrap? wrapZ)
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
					WrapY = wrapY.Value;
					break;
				case TextureType.Texture2DArray:
					TextureTarget = TextureTarget.Texture2DArray;
					WrapY = wrapY.Value;
					break;
				case TextureType.Texture2DMultisample:
					TextureTarget = TextureTarget.Texture2DMultisample;
					WrapY = wrapY.Value;
					break;
				case TextureType.Texture3D:
					TextureTarget = TextureTarget.Texture3D;
					WrapY = wrapY.Value;
					WrapZ = wrapZ.Value;
					break;
				default:
					throw new IllegalValueException(typeof(TextureType), Type);
			}
			MinFilter = minFilter;
			MagFilter = magFilter;
			WrapX = wrapX;

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

					GLuint tempFramebuffer = GL.GenFramebuffer();
					GL.BindFramebuffer(FramebufferTarget.Framebuffer, tempFramebuffer);
					//Framebuffer.Swaps++; // TODO

					GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, TextureAttachment.Colour0.ToOpenGL(), TextureTarget, ID, 0);
					FramebufferStatus status = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
					if (status != FramebufferStatus.Complete)
						throw new CKGLException("Invalid Framebuffer: " + status);

					GL.ReadBuffer(ReadBuffer.Colour0);
					Bitmap bitmap = new Bitmap(GL.ReadPixelsAsColourArray(rectangle, PixelFormat.RGBA), rectangle.W, rectangle.H);

					GL.DeleteFramebuffer(tempFramebuffer);

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
				if (Type != TextureType.Texture1D && Type != TextureType.Texture1DArray)
					WrapY = value;
				if (Type == TextureType.Texture3D)
					WrapZ = value;
			}
		}

		private TextureWrap? wrapX;
		public override TextureWrap WrapX
		{
			get { return wrapX.Value; }
			set
			{
				if (wrapX != value)
				{
					Bind();
					Graphics.State.OnStateChanging?.Invoke();
					GL.TexParameterI(TextureTarget, TextureParam.WrapS, (GLint)value.ToOpenGL());
					Graphics.State.OnStateChanged?.Invoke();
					wrapX = value;
				}
			}
		}

		private TextureWrap? wrapY;
		public override TextureWrap WrapY
		{
			get { return wrapY.Value; }
			set
			{
				if (Type != TextureType.Texture1D && Type != TextureType.Texture1DArray)
				{
					if (wrapY != value)
					{
						Bind();
						Graphics.State.OnStateChanging?.Invoke();
						GL.TexParameterI(TextureTarget, TextureParam.WrapT, (GLint)value.ToOpenGL());
						Graphics.State.OnStateChanged?.Invoke();
						wrapY = value;
					}
				}
			}
		}

		private TextureWrap? wrapZ;
		public override TextureWrap WrapZ
		{
			get { return wrapZ.Value; }
			set
			{
				if (Type == TextureType.Texture3D)
				{
					if (wrapZ != value)
					{
						Bind();
						Graphics.State.OnStateChanging?.Invoke();
						GL.TexParameterI(TextureTarget, TextureParam.WrapR, (GLint)value.ToOpenGL());
						Graphics.State.OnStateChanged?.Invoke();
						wrapZ = value;
					}
				}
			}
		}

		public override TextureFilter Filter
		{
			set
			{
				MinFilter = value;
				MagFilter = value;
			}
		}

		private TextureFilter? minFilter;
		public override TextureFilter MinFilter
		{
			get { return minFilter.Value; }
			set
			{
				if (minFilter != value)
				{
					Bind();
					Graphics.State.OnStateChanging?.Invoke();
					GL.TexParameterI(TextureTarget, TextureParam.MinFilter, (GLint)value.ToOpenGL());
					Graphics.State.OnStateChanged?.Invoke();
					minFilter = value;
				}
			}
		}

		private TextureFilter? magFilter;
		public override TextureFilter MagFilter
		{
			get { return magFilter.Value; }
			set
			{
				if (value != TextureFilter.Linear && value != TextureFilter.Nearest)
					throw new IllegalValueException(typeof(TextureFilter), value);

				if (magFilter != value)
				{
					Bind();
					Graphics.State.OnStateChanging?.Invoke();
					GL.TexParameterI(TextureTarget, TextureParam.MagFilter, (GLint)value.ToOpenGL());
					Graphics.State.OnStateChanged?.Invoke();
					magFilter = value;
				}
			}
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