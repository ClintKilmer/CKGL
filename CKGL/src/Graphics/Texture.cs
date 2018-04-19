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

		public static TextureFilter DefaultMinFilter = TextureFilter.Nearest;
		public static TextureFilter DefaultMagFilter = TextureFilter.Nearest;
		public static TextureWrap DefaultWrapX = TextureWrap.Clamp;
		public static TextureWrap DefaultWrapY = TextureWrap.Clamp;

		public GLuint ID { get; private set; }

		public int Width { get; set; }
		public int Height { get; set; }
		public TextureFormat Format { get; private set; }
		public TextureTarget BindTarget { get; private set; }
		public TextureTarget DataTarget { get; private set; }

		protected Texture(TextureFormat format,
						  TextureTarget bindTarget, TextureTarget dataTarget,
						  TextureFilter minFilter, TextureFilter magFilter,
						  TextureWrap wrapX, TextureWrap wrapY)
		{
			ID = GL.GenTexture();
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
			if (ID != default(GLuint))
			{
				GL.DeleteTexture(ID);
				ID = default(GLuint);
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
		public static GLuint BoundAt(GLuint textureSlot)
		{
			return bindings[textureSlot].ID;
		}

		public bool IsBound() => IsBound(0);
		public bool IsBound(GLuint textureSlot) => IsBound(textureSlot, BindTarget);
		private bool IsBound(GLuint textureSlot, TextureTarget target)
		{
			return bindings[textureSlot].ID == ID && bindings[textureSlot].Target == target;
		}

		public void Bind() => Bind(0, BindTarget);
		public void Bind(GLuint textureSlot) => Bind(textureSlot, BindTarget);
		private void Bind(GLuint textureSlot, TextureTarget target)
		{
			if (!IsBound(textureSlot, target))
			{
				GL.ActiveTexture(textureSlot);
				GL.BindTexture(target, ID);

				bindings[textureSlot].ID = ID;
				bindings[textureSlot].Target = target;
			}
		}
		#endregion

		#region Overrides
		public override string ToString()
		{
			return $"{ID}";
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
	}
}