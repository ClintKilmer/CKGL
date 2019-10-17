using System;
using static CKGL.WebGL2.WebGL2Graphics; // WebGL 2.0 Context Methods
using static Retyped.dom; // DOM / WebGL Types
using static Retyped.es5; // JS TypedArrays
using static Retyped.webgl2.WebGL2RenderingContext; // WebGL 2.0 Enums

namespace CKGL.WebGL2
{
	public class WebGL2Texture : Texture
	{
		internal WebGLTexture ID => id;
		internal double TextureTarget;

		private WebGLTexture id;

		private struct Binding
		{
			public WebGLTexture ID;
			public double Target;
		}
		private static readonly Binding[] bindings = new Binding[(int)GL.getParameter(MAX_TEXTURE_IMAGE_UNITS)];

		internal WebGL2Texture(byte[] data, TextureType type,
							   int width, int height, int depth,
							   TextureFormat format,
							   TextureFilter minFilter, TextureFilter magFilter,
							   TextureWrap wrapX, TextureWrap wrapY, TextureWrap? wrapZ)
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
					TextureTarget = TEXTURE_2D;
					break;
				case TextureType.Texture2DArray:
					TextureTarget = TEXTURE_2D_ARRAY_Static;
					break;
				case TextureType.Texture3D:
					TextureTarget = TEXTURE_3D_Static;
					WrapZ = wrapZ.Value;
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

		internal WebGL2Texture(string file, TextureType type,
							   TextureFormat format,
							   TextureFilter minFilter, TextureFilter magFilter,
							   TextureWrap wrapX, TextureWrap wrapY, TextureWrap? wrapZ)
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
					TextureTarget = TEXTURE_2D;
					break;
				case TextureType.Texture2DArray:
					TextureTarget = TEXTURE_2D_ARRAY_Static;
					break;
				case TextureType.Texture3D:
					TextureTarget = TEXTURE_3D_Static;
					WrapZ = wrapZ.Value;
					break;
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
			var textureImageElement = new HTMLImageElement();
			textureImageElement.onload = (ev) => { HandleLoadedTexture(textureImageElement); };
			textureImageElement.src = file;
		}
		public void HandleLoadedTexture(HTMLImageElement image)
		{
			GL.pixelStorei(UNPACK_FLIP_Y_WEBGL, true);
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
					GL.texImage2D(TextureTarget, 0, Format.ToWebGL2(), Width, Height, 0, Format.ToWebGL2PixelFormat(), Format.ToWebGL2PixelType(), data != null ? new Uint8Array(data).As<ArrayBufferView>() : null);
					break;
				//case TextureType.Texture2DArray:
				//	break;
				//case TextureType.Texture3D:
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
					GL.texImage2D(TextureTarget, 0, Format.ToWebGL2(), Width, Height, 0, Format.ToWebGL2PixelFormat(), Format.ToWebGL2PixelType(), image);
					break;
				//case TextureType.Texture2DArray:
				//	break;
				//case TextureType.Texture3D:
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
					GL.texParameteri(TextureTarget, TEXTURE_WRAP_S, value.ToWebGL2());
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
					GL.texParameteri(TextureTarget, TEXTURE_WRAP_T, value.ToWebGL2());
					Graphics.State.OnStateChanged?.Invoke();
					wrapY = value;
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
						GL.texParameteri(TextureTarget, TEXTURE_WRAP_R_Static, value.ToWebGL2());
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
					GL.texParameteri(TextureTarget, TEXTURE_MIN_FILTER, value.ToWebGL2());
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
					GL.texParameteri(TextureTarget, TEXTURE_MAG_FILTER, value.ToWebGL2());
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
				GL.activeTexture(TEXTURE0 + textureSlot);
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
					GL.activeTexture(TEXTURE0 + i);
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
			return obj is WebGL2Texture && Equals((WebGL2Texture)obj);
		}
		public override bool Equals(Texture texture)
		{
			return this == (WebGL2Texture)texture;
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
		public static bool operator ==(WebGL2Texture a, WebGL2Texture b)
		{
			return (a?.id ?? null) == (b?.id ?? null);
		}

		public static bool operator !=(WebGL2Texture a, WebGL2Texture b)
		{
			return (a?.id ?? null) != (b?.id ?? null);
		}
		#endregion
	}
}