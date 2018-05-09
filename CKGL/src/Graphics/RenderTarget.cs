using System;

using OpenGL;

using GLint = System.Int32;
using GLuint = System.UInt32;

namespace CKGL
{
	public class RenderTarget
	{
		public static Action OnBinding;
		public static Action OnBound;

		public static readonly RenderTarget Default = new RenderTarget();
		public static RenderTarget Current { get; private set; } = Default;

		public static GLuint Swaps { get; private set; }

		private int width = 0;
		private int height = 0;
		public int Width
		{
			get
			{
				if (id == 0)
					return Window.Width;

				return width;
			}
		}
		public int Height
		{
			get
			{
				if (id == 0)
					return Window.Height;

				return height;
			}
		}
		public Texture2D[] textures;
		public Texture2D depthTexture;

		private GLuint id;

		// Default Framebuffer
		private RenderTarget()
		{
			id = 0;
		}

		public RenderTarget(int width, int height, GLint colourTextures, TextureFormat textureColourFormat, TextureFormat textureDepthFormat) : this(width, height, colourTextures, textureColourFormat)
		{
			if (!(textureDepthFormat.PixelFormat() == PixelFormat.Depth || textureDepthFormat.PixelFormat() == PixelFormat.DepthStencil))
				throw new Exception("textureDepthFormat is not a depth(stencil) texture.");

			RenderTarget originalRenderTarget = Current;
			Bind();

			depthTexture = new Texture2D(Width, Height, textureDepthFormat);
			GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, textureDepthFormat.TextureAttachment(), depthTexture.BindTarget, depthTexture.ID, 0);
			CheckStatus();

			originalRenderTarget.Bind();
		}
		public RenderTarget(int width, int height, GLint colourTextures, TextureFormat textureColourFormat)
		{
			if (colourTextures < 1)
				throw new Exception("Must have at least 1 colour texture.");
			if (colourTextures > GL.MaxColourAttachments || colourTextures > GL.MaxDrawBuffers)
				throw new Exception("Too many colour textures.");
			if (textureColourFormat.PixelFormat() == PixelFormat.Depth || textureColourFormat.PixelFormat() == PixelFormat.DepthStencil)
				throw new Exception("textureColourFormat cannot be a depth(stencil) texture.");

			this.width = width;
			this.height = height;

			id = GL.GenFramebuffer();

			RenderTarget originalRenderTarget = Current;
			Bind();

			textures = new Texture2D[colourTextures];
			DrawBuffer[] drawBuffers = new DrawBuffer[colourTextures];
			for (int i = 0; i < colourTextures; i++)
			{
				textures[i] = new Texture2D(Width, Height, textureColourFormat);
				drawBuffers[i] = (DrawBuffer)((GLuint)DrawBuffer.Colour0 + i);
				GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, (TextureAttachment)((uint)TextureAttachment.Colour0 + i), textures[i].DataTarget, textures[i].ID, 0);
				CheckStatus();
			}
			GL.DrawBuffers(colourTextures, drawBuffers);
			CheckStatus();

			originalRenderTarget.Bind();
		}

		public static void PreDraw()
		{
			Swaps = 0;
		}

		public void Destroy()
		{
			if (id != 0)
			{
				for (int i = textures.Length; i == 0; i--)
				{
					textures[i]?.Destroy();
					textures[i] = null;
				}

				depthTexture?.Destroy();
				depthTexture = null;

				if (id != default(GLuint))
				{
					GL.DeleteFramebuffer(id);
					id = default(GLuint);
				}
			}
		}

		#region Bind
		public void Bind()
		{
			if (id != Current.id)
			{
				OnBinding?.Invoke();
				GL.BindFramebuffer(FramebufferTarget.Framebuffer, id);
				Current = this;
				Swaps++;
				Graphics.SetViewport();
				Graphics.SetScissorTest();
				OnBound?.Invoke();
			}
		}
		#endregion

		public void BlitTextureTo(RenderTarget target, int textureNum, BlitFilter filter) => BlitTextureTo(target, textureNum, filter, new RectangleI(Width, Height));
		public void BlitTextureTo(RenderTarget target, int textureNum, BlitFilter filter, int x, int y) => BlitTextureTo(target, textureNum, filter, new RectangleI(x, y, Width, Height));
		public void BlitTextureTo(RenderTarget target, int textureNum, BlitFilter filter, RectangleI rect)
		{
			if (textures[textureNum].ID == 0)
				throw new Exception("RenderTarget does not have a texture in slot: " + textureNum);

			GL.BindFramebuffer(FramebufferTarget.Read, id);
			GL.BindFramebuffer(FramebufferTarget.Draw, (target ?? Default).id);
			Graphics.SetViewport(target);
			Graphics.SetScissorTest(target);
			GL.ReadBuffer((ReadBuffer)((uint)ReadBuffer.Colour0 + textureNum));
			GL.BlitFramebuffer(new RectangleI(Width, Height), rect, BufferBit.Colour, filter);
		}

		private void CheckStatus()
		{
			FramebufferStatus status = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
			if (status != FramebufferStatus.Complete)
				throw new Exception("Invalid RenderTarget: " + status);
		}

		#region Overrides
		public override string ToString()
		{
			return $"{id}";
		}
		#endregion
	}
}