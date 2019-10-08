using static CKGL.WebGL.WebGLGraphics; // WebGL Context Methods

namespace CKGL.WebGL
{
	public class WebGLTexture : Texture
	{
		internal Retyped.dom.WebGLTexture ID => id;
		internal uint TextureTarget;

		private Retyped.dom.WebGLTexture id;

		private struct Binding
		{
			public Retyped.dom.WebGLTexture ID;
			public uint Target;
		}

		private static readonly Binding[] bindings = new Binding[(int)GL.getParameter(GL.MAX_TEXTURE_IMAGE_UNITS)];

		internal WebGLTexture(byte[] data, TextureType type,
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
			id = GL.createTexture();
			switch (Type)
			{
				case TextureType.Texture2D:
					TextureTarget = GL.TEXTURE_2D;
					break;
				//case TextureType.Texture2DArray: // Not available in WebGL 1.0
				//	TextureTarget = GL.TEXTURE_2D_ARRAY;
				//	break;
				//case TextureType.Texture3D: // Not available in WebGL 1.0
				//	TextureTarget = GL.TEXTURE_3D;
				//	break;
				default:
					throw new IllegalValueException(typeof(TextureType), Type);
			}
			MinFilter = minFilter;
			MagFilter = magFilter;
			WrapX = wrapX;
			WrapY = wrapY;

			SetData(data);
		}

		private void SetData(byte[] data)
		{
			switch (Type)
			{
				case TextureType.Texture2D:
					if (data != null && data.Length < Width * Height * Format.Components())
						throw new CKGLException("Data array is not large enough to fill texture.");
					Bind();
					//fixed (byte* ptr = data)
					//	GL.TexImage2D(TextureTarget, 0, Format.ToWebGL(), Width, Height, 0, Format.ToWebGL().PixelFormat(), Format.ToWebGL().PixelType(), data != null ? ptr : null);
					GL.texImage2D(TextureTarget, 0, Format.ToWebGL(), Width, Height, 0, Format.ToWebGLPixelFormat(), Format.ToWebGLPixelType(), null);
					break;
				//case TextureType.Texture2DArray: // Not available in WebGL 1.0
				//	break;
				//case TextureType.Texture3D: // Not available in WebGL 1.0
				//	break;
				default:
					throw new IllegalValueException(typeof(TextureType), Type);
			}
		}

		public override void Destroy()
		{
			if (id != null)
			{
				GL.deleteTexture(id);
				id = null;
			}
		}

		public override byte[] GetData()
		{
			switch (Type)
			{
				case TextureType.Texture2D:
					byte[] data = new byte[Width * Height * Format.Components()];
					Bind();
					//fixed (byte* ptr = data)
					//	GL.GetTexImage(TextureTarget, 0, Format.ToWebGLPixelFormat(), Format.ToWebGLPixelType(), ptr);
					return data;
				//case TextureType.Texture2DArray: // Not available in WebGL 1.0
				//	break;
				//case TextureType.Texture3D: // Not available in WebGL 1.0
				//	break;
				default:
					throw new IllegalValueException(typeof(TextureType), Type);
			}
		}

		public override Bitmap GetBitmap()
		{
			switch (Type)
			{
				case TextureType.Texture2D:
					Colour[] data = new Colour[Width * Height];
					Bind();
					//fixed (Colour* ptr = data)
					//	GL.GetTexImage(TextureTarget, 0, Format.ToWebGLPixelFormat(), Format.ToWebGLPixelType(), ptr);
					return new Bitmap(data, Width, Height);
				//case TextureType.Texture2DArray: // Not available in WebGL 1.0
				//	break;
				//case TextureType.Texture3D: // Not available in WebGL 1.0
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
			get { return (TextureWrap)GetParam(GL.TEXTURE_WRAP_S); }
			set { SetParam(GL.TEXTURE_WRAP_S, value.ToWebGL()); }
		}

		public override TextureWrap WrapY
		{
			get { return (TextureWrap)GetParam(GL.TEXTURE_WRAP_T); }
			set { SetParam(GL.TEXTURE_WRAP_T, value.ToWebGL()); }
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
			get { return (TextureFilter)GetParam(GL.TEXTURE_MIN_FILTER); }
			set { SetParam(GL.TEXTURE_MIN_FILTER, value.ToWebGL()); }
		}

		public override TextureFilter MagFilter
		{
			get { return (TextureFilter)GetParam(GL.TEXTURE_MAG_FILTER); }
			set
			{
				switch (value)
				{
					case TextureFilter.Linear:
					case TextureFilter.LinearMipmapLinear:
					case TextureFilter.LinearMipmapNearest:
						SetParam(GL.TEXTURE_MAG_FILTER, TextureFilter.Linear.ToWebGL());
						break;
					case TextureFilter.Nearest:
					case TextureFilter.NearestMipmapLinear:
					case TextureFilter.NearestMipmapNearest:
						SetParam(GL.TEXTURE_MAG_FILTER, TextureFilter.Nearest.ToWebGL());
						break;
				}
			}
		}

		private uint GetParam(uint param)
		{
			Bind();
			return (uint)GL.getTexParameter(TextureTarget, param);
		}

		private void SetParam(uint param, uint val)
		{
			Bind();
			GL.texParameteri(TextureTarget, param, val);
		}
		#endregion

		#region Bind
		public override void Bind() => Bind(0);
		public override void Bind(uint textureSlot)
		{
			if (id != bindings[textureSlot].ID || TextureTarget != bindings[textureSlot].Target)
			{
				Graphics.State.OnStateChanging?.Invoke();
				GL.activeTexture(GL.TEXTURE0 + textureSlot);
				GL.bindTexture(TextureTarget, id);
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
					GL.activeTexture(GL.TEXTURE0 + i);
					GL.bindTexture(TextureTarget, null);
					Swaps++;

					bindings[i].ID = null;
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
			return obj is WebGLTexture && Equals((WebGLTexture)obj);
		}
		public override bool Equals(Texture texture)
		{
			return this == (WebGLTexture)texture;
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
		public static bool operator ==(WebGLTexture a, WebGLTexture b)
		{
			return (a?.id ?? null) == (b?.id ?? null);
		}

		public static bool operator !=(WebGLTexture a, WebGLTexture b)
		{
			return (a?.id ?? null) != (b?.id ?? null);
		}
		#endregion
	}
}