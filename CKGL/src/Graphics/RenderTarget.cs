using System;
using CKGL.OpenGLBindings;
using GLint = System.Int32;
using GLuint = System.UInt32;

namespace CKGL
{
	#region Enums
	public enum TextureSlot : int
	{
		Depth = -1,
		Colour0 = 0,
		Colour1 = 1,
		Colour2 = 2,
		Colour3 = 3,
		Colour4 = 4,
		Colour5 = 5,
		Colour6 = 6,
		Colour7 = 7,
		Colour8 = 8,
		Colour9 = 9,
		Colour10 = 10,
		Colour11 = 11,
		Colour12 = 12,
		Colour13 = 13,
		Colour14 = 14,
		Colour15 = 15
	}
	#endregion

	public class RenderTarget
	{
		public static readonly RenderTarget Default = new RenderTarget();
		public static RenderTarget Current { get; private set; } = Default;

		public static int Swaps { get; private set; }
		public static int Blits { get; private set; }

		private Camera2D camera2D = new Camera2D();
		public Matrix Matrix
		{
			get
			{
				if (id == 0)
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
				if (id == 0)
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
				if (id == 0)
				{
					camera2D.Width = Width;
					camera2D.Height = Height;
				}
				return camera2D.ProjectionMatrix;
			}
		}

		private readonly int width = 0;
		private readonly int height = 0;
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
		public float AspectRatio
		{
			get { return Width / (float)Height; }
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

			Bind();

			depthTexture = new Texture2D(Width, Height, textureDepthFormat);
			GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, textureDepthFormat.ToOpenGL().TextureAttachment(), depthTexture.DataTarget.ToOpenGL(), depthTexture.ID, 0);
			CheckStatus();
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

			camera2D.Width = width;
			camera2D.Height = height;

			id = GL.GenFramebuffer();

			Bind();

			textures = new Texture2D[colourTextures];
			DrawBuffer[] drawBuffers = new DrawBuffer[colourTextures];
			for (int i = 0; i < colourTextures; i++)
			{
				textures[i] = new Texture2D(Width, Height, textureColourFormat);
				drawBuffers[i] = (DrawBuffer)((GLuint)DrawBuffer.Colour0 + i);
				GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, ((TextureAttachment)((uint)TextureAttachment.Colour0 + i)).ToOpenGL(), textures[i].DataTarget.ToOpenGL(), textures[i].ID, 0);
				CheckStatus();
			}
			GL.DrawBuffers(colourTextures, drawBuffers);
			CheckStatus();
		}

		public static void PreDraw()
		{
			Swaps = 0;
			Blits = 0;
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

				if (id != default)
				{
					GL.DeleteFramebuffer(id);
					id = default;
				}
			}
		}

		#region Bind
		public void Bind()
		{
			if (id != Current.id)
			{
				Graphics.State.OnStateChanging?.Invoke();
				GL.BindFramebuffer(FramebufferTarget.Framebuffer, id);
				Swaps++;
				Current = this;
				Graphics.SetViewport();
				Graphics.SetScissorTest();
				Graphics.State.OnStateChanged?.Invoke();
			}
		}
		#endregion

		public Texture GetTexture(TextureSlot textureSlot)
		{
			Texture result = null;
			if (textureSlot == TextureSlot.Depth)
				result = depthTexture;
			else
				result = textures[(int)textureSlot];

			if (result == null)
				throw new ArgumentOutOfRangeException($"No suitable texture found in RenderTarget texture slot {textureSlot}.");

			return result;
		}

		// TODO - Figure out Depth Texture Blitting
		public void BlitTextureTo(RenderTarget target, TextureSlot textureSlot, BlitFilter filter) => BlitTextureTo(target, textureSlot, filter, new RectangleI(Width, Height));
		public void BlitTextureTo(RenderTarget target, TextureSlot textureSlot, BlitFilter filter, int x, int y) => BlitTextureTo(target, textureSlot, filter, new RectangleI(x, y, Width, Height));
		public void BlitTextureTo(RenderTarget target, TextureSlot textureSlot, BlitFilter filter, RectangleI rect)
		{
			if (textureSlot < 0)
				throw new ArgumentOutOfRangeException($"Can't blit a depth texture.");

			Graphics.State.OnStateChanging.Invoke();

			RenderTarget originalRenderTarget = Current;

			GL.BindFramebuffer(FramebufferTarget.Read, id);
			Swaps++;
			GL.BindFramebuffer(FramebufferTarget.Draw, (target ?? Default).id);
			Swaps++;

			Graphics.SetViewport(target);
			Graphics.SetScissorTest(target);
			GL.ReadBuffer((ReadBuffer)((uint)ReadBuffer.Colour0 + (uint)textureSlot));
			GL.BlitFramebuffer(new RectangleI(Width, Height), rect, BufferBit.Colour, filter.ToOpenGL());
			//GL.BlitFramebuffer(new RectangleI(Width, Height), rect, textureSlot >= 0 ? BufferBit.Colour : BufferBit.Depth, filter.ToOpenGL());

			// Reset Framebuffer
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, (originalRenderTarget ?? Default).id);

			Blits++;

			Graphics.State.OnStateChanged.Invoke();
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