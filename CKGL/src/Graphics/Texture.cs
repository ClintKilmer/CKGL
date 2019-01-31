using CKGL.OpenGLBindings;
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
		private static readonly Binding[] bindings = new Binding[GL.MaxTextureImageUnits];

		public static int Swaps { get; private set; }

		public static TextureFilter DefaultMinFilter = TextureFilter.Nearest;
		public static TextureFilter DefaultMagFilter = TextureFilter.Nearest;
		public static TextureWrap DefaultWrapX = TextureWrap.Clamp;
		public static TextureWrap DefaultWrapY = TextureWrap.Clamp;

		private GLuint id;

		public GLuint ID { get => id; }
		public int Width { get; protected set; }
		public int Height { get; protected set; }
		public TextureFormat Format { get; private set; }
		public TextureTarget BindTarget { get; private set; }
		public TextureTarget DataTarget { get; private set; }

		protected Texture(TextureFormat format,
						  TextureTarget bindTarget, TextureTarget dataTarget,
						  TextureFilter minFilter, TextureFilter magFilter,
						  TextureWrap wrapX, TextureWrap wrapY)
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

		public static void PreDraw()
		{
			Swaps = 0;
		}

		public void Destroy()
		{
			if (id != default)
			{
				GL.DeleteTexture(id);
				id = default;
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
			set
			{
				switch (value)
				{
					case TextureFilter.Linear:
					case TextureFilter.LinearMipmapLinear:
					case TextureFilter.LinearMipmapNearest:
						SetParam(TextureParam.MagFilter, (GLint)TextureFilter.Linear);
						break;
					case TextureFilter.Nearest:
					case TextureFilter.NearestMipmapLinear:
					case TextureFilter.NearestMipmapNearest:
						SetParam(TextureParam.MagFilter, (GLint)TextureFilter.Nearest);
						break;
				}
			}
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
		public void Bind() => Bind(0);
		public void Bind(GLuint textureSlot)
		{
			if (id != bindings[textureSlot].ID || BindTarget != bindings[textureSlot].Target)
			{
				Graphics.State.OnStateChanging?.Invoke();
				GL.ActiveTexture(textureSlot);
				GL.BindTexture(BindTarget, id);
				Swaps++;

				bindings[textureSlot].ID = id;
				bindings[textureSlot].Target = BindTarget;
				Graphics.State.OnStateChanged?.Invoke();
			}
		}
		#endregion

		#region Overrides
		public override string ToString()
		{
			return $"{id}";
		}
		#endregion

		#region Operators
		public static bool operator ==(Texture a, Texture b)
		{
			return (a?.id ?? null) == (b?.id ?? null);
		}
		public static bool operator !=(Texture a, Texture b)
		{
			return (a?.id ?? null) != (b?.id ?? null);
		}
		#endregion
	}
}