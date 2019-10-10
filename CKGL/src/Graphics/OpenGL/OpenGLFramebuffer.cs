using System;
using CKGL.OpenGLBindings;
using GLint = System.Int32;
using GLuint = System.UInt32;

namespace CKGL.OpenGL
{
	public class OpenGLFramebuffer : Framebuffer
	{
		private GLuint id;

		internal static Framebuffer CreateDefault() => new OpenGLFramebuffer();

		// Default OpenGLFramebuffer
		private OpenGLFramebuffer()
		{
			id = 0;
			IsDefault = true;
		}

		public OpenGLFramebuffer(int width, int height, GLint colourTextures, TextureFormat textureColourFormat, TextureFormat? textureDepthFormat = null)
		{
			if (colourTextures < 1)
				throw new CKGLException("Must have at least 1 colour texture.");
			if (colourTextures > GL.MaxColourAttachments || colourTextures > GL.MaxDrawBuffers)
				throw new CKGLException("Too many colour textures.");
			if (textureColourFormat.ToOpenGL().PixelFormat() == PixelFormat.Depth || textureColourFormat.ToOpenGL().PixelFormat() == PixelFormat.DepthStencil)
				throw new CKGLException("textureColourFormat cannot be a depth(stencil) texture.");
			if (textureDepthFormat.HasValue && !(textureDepthFormat.Value.ToOpenGL().PixelFormat() == PixelFormat.Depth || textureDepthFormat.Value.ToOpenGL().PixelFormat() == PixelFormat.DepthStencil))
				throw new CKGLException("textureDepthFormat is not a depth(stencil) texture.");

			Width = width;
			Height = height;

			camera2D.Width = width;
			camera2D.Height = height;

			id = GL.GenFramebuffer();

			Bind();

			Textures = new Texture[colourTextures];
			DrawBuffer[] drawBuffers = new DrawBuffer[colourTextures];
			for (int i = 0; i < colourTextures; i++)
			{
				Textures[i] = Texture.Create2D(Width, Height, textureColourFormat);
				drawBuffers[i] = (DrawBuffer)((GLuint)DrawBuffer.Colour0 + i);
				GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, ((TextureAttachment)((uint)TextureAttachment.Colour0 + i)).ToOpenGL(), (Textures[i] as OpenGLTexture).TextureTarget, (Textures[i] as OpenGLTexture).ID, 0);
				Textures[i].Unbind();
				CheckStatus();
			}
			GL.DrawBuffers(colourTextures, drawBuffers);
			CheckStatus();

			if (textureDepthFormat.HasValue)
			{
				DepthStencilTexture = Texture.Create2D(Width, Height, textureDepthFormat.Value);
				GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, textureDepthFormat.Value.ToOpenGL().TextureAttachment(), (DepthStencilTexture as OpenGLTexture).TextureTarget, (DepthStencilTexture as OpenGLTexture).ID, 0);
				DepthStencilTexture.Unbind();
				CheckStatus();
			}
		}

		public override void Destroy()
		{
			if (id != 0)
			{
				for (int i = Textures.Length; i == 0; i--)
				{
					Textures[i]?.Destroy();
					Textures[i] = null;
				}

				DepthStencilTexture?.Destroy();
				DepthStencilTexture = null;

				if (id != default)
				{
					GL.DeleteFramebuffer(id);
					id = default;
				}
			}
		}

		public override void Bind()
		{
			if (id != (Current as OpenGLFramebuffer).id)
			{
				Graphics.State.OnStateChanging?.Invoke();
				UnbindTextures();
				GL.BindFramebuffer(FramebufferTarget.Framebuffer, id);
				Swaps++;
				Current = this;
				Graphics.SetViewport();
				Graphics.SetScissorTest();
				Graphics.State.OnStateChanged?.Invoke();
			}
		}

		public override Texture GetTexture(TextureAttachment textureAttachment)
		{
			Texture result;
			if (textureAttachment == TextureAttachment.Depth || textureAttachment == TextureAttachment.DepthStencil)
				result = DepthStencilTexture;
			else
				result = Textures[(int)textureAttachment];

			if (result == null)
				throw new ArgumentOutOfRangeException($"No suitable texture found in Framebuffer texture attachment {textureAttachment}.");

			return result;
		}

		// Blitting is currently OpenGL only - cast to OpenGLFramebuffer to use BlitTextureTo
		// TODO - Figure out Depth Texture Blitting
		public void BlitTextureTo(Framebuffer target, TextureAttachment textureAttachment, BlitFilter filter) => BlitTextureTo(target, textureAttachment, filter, new RectangleI(Width, Height));
		public void BlitTextureTo(Framebuffer target, TextureAttachment textureAttachment, BlitFilter filter, int x, int y) => BlitTextureTo(target, textureAttachment, filter, new RectangleI(x, y, Width, Height));
		public void BlitTextureTo(Framebuffer target, TextureAttachment textureAttachment, BlitFilter filter, RectangleI rect)
		{
			if (textureAttachment < 0)
				throw new ArgumentOutOfRangeException($"Can't blit a depth texture.");

			Graphics.State.OnStateChanging.Invoke();

			OpenGLFramebuffer originalFramebuffer = Current as OpenGLFramebuffer;

			GL.BindFramebuffer(FramebufferTarget.Read, id);
			Swaps++;
			GL.BindFramebuffer(FramebufferTarget.Draw, ((OpenGLFramebuffer)target ?? (OpenGLFramebuffer)Default).id);
			Swaps++;

			Graphics.SetViewport(target);
			Graphics.SetScissorTest(target);
			GL.ReadBuffer((ReadBuffer)((uint)ReadBuffer.Colour0 + (uint)textureAttachment));
			GL.BlitFramebuffer(new RectangleI(Width, Height), rect, BufferBit.Colour, filter.ToOpenGL());
			//GL.BlitFramebuffer(new RectangleI(Width, Height), rect, textureAttachment >= 0 ? BufferBit.Colour : BufferBit.Depth, filter.ToOpenGL());

			// Reset Framebuffer
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, (originalFramebuffer ?? (OpenGLFramebuffer)Default).id);

			Blits++;

			Graphics.State.OnStateChanged.Invoke();
		}

		private void CheckStatus()
		{
			FramebufferStatus status = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
			if (status != FramebufferStatus.Complete)
				throw new CKGLException("Invalid Framebuffer: " + status);
		}

		#region Overrides
		public override string ToString()
		{
			return $"Framebuffer: [id: {id}]";
		}

		public override bool Equals(object obj)
		{
			return obj is OpenGLFramebuffer && Equals((OpenGLFramebuffer)obj);
		}
		public override bool Equals(Framebuffer framebuffer)
		{
			return this == (OpenGLFramebuffer)framebuffer;
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
		public static bool operator ==(OpenGLFramebuffer a, OpenGLFramebuffer b)
		{
			return (a?.id ?? null) == (b?.id ?? null);
		}

		public static bool operator !=(OpenGLFramebuffer a, OpenGLFramebuffer b)
		{
			return (a?.id ?? null) != (b?.id ?? null);
		}
		#endregion
	}
}