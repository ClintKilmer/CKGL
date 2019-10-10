namespace CKGL
{
	public abstract class Framebuffer
	{
		public static Framebuffer Default { get; internal set; } = Graphics.CreateDefaultFramebuffer();
		public static Framebuffer Current { get; internal set; } = Default;

		public static int Swaps { get; protected set; }
		public static int Blits { get; protected set; }

		public Texture[] Textures = new Texture[0];
		public Texture DepthStencilTexture;

		protected bool IsDefault = false;

		protected Camera2D camera2D = new Camera2D();
		public Matrix Matrix
		{
			get
			{
				if (IsDefault)
				{
					camera2D.Width = Width;
					camera2D.Height = Height;
				}
				return camera2D.Matrix;
			}
		}
		public Matrix ViewMatrix
		{
			get
			{
				if (IsDefault)
				{
					camera2D.Width = Width;
					camera2D.Height = Height;
				}
				return camera2D.ViewMatrix;
			}
		}
		public Matrix ProjectionMatrix
		{
			get
			{
				if (IsDefault)
				{
					camera2D.Width = Width;
					camera2D.Height = Height;
				}
				return camera2D.ProjectionMatrix;
			}
		}

		private int width = 0;
		public int Width
		{
			get
			{
				if (IsDefault)
					return Window.Width;

				return width;
			}

			protected set
			{
				if (IsDefault)
					throw new CKGLException("Modifying the Width of the Default Framebuffer is not allowed.");

				width = value;
			}
		}

		private int height = 0;
		public int Height
		{
			get
			{
				if (IsDefault)
					return Window.Height;

				return height;
			}

			protected set
			{
				if (IsDefault)
					throw new CKGLException("Modifying the Height of the Default Framebuffer is not allowed.");

				height = value;
			}
		}

		public float AspectRatio
		{
			get { return Width / (float)Height; }
		}

		public static Framebuffer Create(int width, int height, int colourTextures, TextureFormat textureColourFormat, TextureFormat? textureDepthFormat = null)
		{
			return Graphics.CreateFramebuffer(width, height, colourTextures, textureColourFormat, textureDepthFormat);
		}

		public static void PreDraw()
		{
			Swaps = 0;
			Blits = 0;
		}

		public abstract void Destroy();

		public abstract void Bind();

		public abstract Texture GetTexture(TextureAttachment textureAttachment);

		public void UnbindTextures()
		{
			if (!IsDefault)
			{
				foreach (Texture texture in Textures ?? null)
					texture?.Unbind();
				DepthStencilTexture?.Unbind();
			}
		}

		#region Overrides
		public abstract override string ToString();

		public abstract override bool Equals(object obj);
		public abstract bool Equals(Framebuffer framebuffer);

		public abstract override int GetHashCode();
		#endregion
	}
}