using System;
using static CKGL.WebGL.WebGLGraphics; // WebGL Context Methods
using static Retyped.dom; // DOM / WebGL Types
using static Retyped.es5; // JS TypedArrays

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

		internal WebGLTexture(string file, TextureType type,
							  TextureFormat format,
							  TextureFilter minFilter, TextureFilter magFilter,
							  TextureWrap wrapX, TextureWrap wrapY)
		{
			Type = type;
			Width = 1;
			Height = 1;
			Depth = 1;
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

			//SetData(new byte[] { 255, 20, 147, 255 }); // Debug Pink
			SetData(new byte[] { 255, 255, 255, 255 });

			// Load image async
			HTMLImageElement image = new HTMLImageElement();
			image.crossOrigin = "";
			image.onload = (ev) => { HandleLoadedTexture(image); };
			image.src = file;
		}
		private void HandleLoadedTexture(HTMLImageElement image)
		{
			SetData(image);
		}

		private void SetData(byte[] data)
		{
			switch (Type)
			{
				case TextureType.Texture2D:
					if (data != null && data.Length < Width * Height * Format.Components())
						throw new CKGLException("Data array is not large enough to fill texture.");
					Bind();
					GL.texImage2D(TextureTarget, 0, Format.ToWebGL(), Width, Height, 0, Format.ToWebGLPixelFormat(), Format.ToWebGLPixelType(), data != null ? new Uint8Array(data).As<ArrayBufferView>() : null);
					break;
				//case TextureType.Texture2DArray: // Not available in WebGL 1.0
				//	break;
				//case TextureType.Texture3D: // Not available in WebGL 1.0
				//	break;
				default:
					throw new IllegalValueException(typeof(TextureType), Type);
			}
		}

		private void SetData(HTMLImageElement image)
		{
			switch (Type)
			{
				case TextureType.Texture2D:
					Width = (int)image.width;
					Height = (int)image.height;
					Bind();
					GL.pixelStorei(GL.UNPACK_FLIP_Y_WEBGL, true);
					GL.texImage2D(TextureTarget, 0, Format.ToWebGL(), Format.ToWebGLPixelFormat(), Format.ToWebGLPixelType(), image);
					break;
				//case TextureType.Texture2DArray: // Not available in WebGL 1.0
				//	break;
				//case TextureType.Texture3D: // Not available in WebGL 1.0
				//	break;
				default:
					throw new IllegalValueException(typeof(TextureType), Type);
			}
		}

		internal void UpdateData(HTMLImageElement image, int xOffset, int yOffset)
		{
			switch (Type)
			{
				case TextureType.Texture2D:
					Bind();
					GL.pixelStorei(GL.UNPACK_FLIP_Y_WEBGL, true);
					GL.texSubImage2D(TextureTarget, 0, xOffset, yOffset, Format.ToWebGLPixelFormat(), Format.ToWebGLPixelType(), image);
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

		public override Bitmap Bitmap(RectangleI rectangle) => throw new NotImplementedException();

		#region Parameters
		public override TextureWrap Wrap
		{
			set
			{
				WrapX = value;
				WrapY = value;
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
					GL.texParameteri(TextureTarget, GL.TEXTURE_WRAP_S, value.ToWebGL());
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
				if (wrapY != value)
				{
					Bind();
					Graphics.State.OnStateChanging?.Invoke();
					GL.texParameteri(TextureTarget, GL.TEXTURE_WRAP_T, value.ToWebGL());
					Graphics.State.OnStateChanged?.Invoke();
					wrapY = value;
				}
			}
		}

		public override TextureWrap WrapZ
		{
			get => throw new CKGLException("WebGL 1.0 doesn't support 3d textures.");
			set => throw new CKGLException("WebGL 1.0 doesn't support 3d textures.");
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
					GL.texParameteri(TextureTarget, GL.TEXTURE_MIN_FILTER, value.ToWebGL());
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
					GL.texParameteri(TextureTarget, GL.TEXTURE_MAG_FILTER, value.ToWebGL());
					Graphics.State.OnStateChanged?.Invoke();
					magFilter = value;
				}
			}
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