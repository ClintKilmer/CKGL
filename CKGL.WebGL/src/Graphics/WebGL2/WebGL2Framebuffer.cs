using System;
using static CKGL.WebGL2.WebGL2Graphics; // WebGL 2.0 Context Methods
using static Retyped.dom; // DOM / WebGL Types
using static Retyped.webgl2.WebGL2RenderingContext; // WebGL 2.0 Enums

namespace CKGL.WebGL2
{
	public class WebGL2Framebuffer : Framebuffer
	{
		private WebGLFramebuffer id;

		// Default WebGL2Framebuffer
		internal WebGL2Framebuffer()
		{
			id = null;
			IsDefault = true;
		}

		public WebGL2Framebuffer(int width, int height, int colourTextures, TextureFormat textureColourFormat, TextureFormat? textureDepthFormat = null)
		{
			if (colourTextures < 1)
				throw new CKGLException("Must have at least 1 colour texture.");
			//if (colourTextures > GL.MaxColourAttachments || colourTextures > GL.MaxDrawBuffers)
			if (colourTextures > 32 || colourTextures > 32)
				throw new CKGLException("Too many colour textures.");
			if (textureColourFormat.ToWebGL2PixelFormat() == DEPTH_COMPONENT || textureColourFormat.ToWebGL2PixelFormat() == DEPTH_STENCIL)
				throw new CKGLException("textureColourFormat cannot be a depth(stencil) texture.");
			if (textureDepthFormat.HasValue && !(textureDepthFormat.Value.ToWebGL2PixelFormat() == DEPTH_COMPONENT || textureDepthFormat.Value.ToWebGL2PixelFormat() == DEPTH_STENCIL))
				throw new CKGLException("textureDepthFormat is not a depth(stencil) texture.");

			Width = width;
			Height = height;

			camera2D.Width = width;
			camera2D.Height = height;

			id = GL.createFramebuffer();

			Bind();

			Textures = new Texture[colourTextures];
			double[] drawBuffers = new double[colourTextures];
			for (int i = 0; i < colourTextures; i++)
			{
				Textures[i] = Texture.Create2D(Width, Height, textureColourFormat);
				drawBuffers[i] = COLOR_ATTACHMENT0 + i;
				GL.framebufferTexture2D(FRAMEBUFFER, COLOR_ATTACHMENT0 + i, (Textures[i] as WebGL2Texture).TextureTarget, (Textures[i] as WebGL2Texture).ID, 0);
				Textures[i].Unbind();
				CheckStatus();
			}
			GL.drawBuffers(drawBuffers);
			CheckStatus();

			if (textureDepthFormat.HasValue)
			{
				DepthStencilTexture = Texture.Create2D(Width, Height, textureDepthFormat.Value);
				GL.framebufferTexture2D(FRAMEBUFFER, textureDepthFormat.Value.ToWebGL2TextureAttachment(), (DepthStencilTexture as WebGL2Texture).TextureTarget, (DepthStencilTexture as WebGL2Texture).ID, 0);
				DepthStencilTexture.Unbind();
				CheckStatus();
			}
		}

		public override void Destroy()
		{
			if (!IsDefault)
			{
				for (int i = Textures.Length; i == 0; i--)
				{
					Textures[i]?.Destroy();
					Textures[i] = null;
				}

				DepthStencilTexture?.Destroy();
				DepthStencilTexture = null;

				if (id != null)
				{
					GL.deleteFramebuffer(id);
					id = null;
				}
			}
		}

		public override void Bind()
		{
			if (id != (Current as WebGL2Framebuffer).id)
			{
				Graphics.State.OnStateChanging?.Invoke();
				UnbindTextures();
				GL.bindFramebuffer(FRAMEBUFFER, id);
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

		// Blitting is currently WebGL2 only - cast to WebGL2Framebuffer to use BlitTextureTo
		// TODO - Figure out Depth Texture Blitting
		public void BlitTextureTo(Framebuffer target, TextureAttachment textureAttachment, BlitFilter filter) => BlitTextureTo(target, textureAttachment, filter, new RectangleI(Width, Height));
		public void BlitTextureTo(Framebuffer target, TextureAttachment textureAttachment, BlitFilter filter, int x, int y) => BlitTextureTo(target, textureAttachment, filter, new RectangleI(x, y, Width, Height));
		public void BlitTextureTo(Framebuffer target, TextureAttachment textureAttachment, BlitFilter filter, RectangleI rect)
		{
			if (textureAttachment < 0)
				throw new ArgumentOutOfRangeException($"Can't blit a depth texture.");

			Graphics.State.OnStateChanging.Invoke();

			WebGL2Framebuffer originalFramebuffer = Current as WebGL2Framebuffer;

			GL.bindFramebuffer(READ_FRAMEBUFFER_Static, id);
			Swaps++;
			GL.bindFramebuffer(DRAW_FRAMEBUFFER_Static, ((WebGL2Framebuffer)target ?? (WebGL2Framebuffer)Default).id);
			Swaps++;

			Graphics.SetViewport(target);
			Graphics.SetScissorTest(target);
			GL.readBuffer(textureAttachment.ToWebGL2());
			GL.blitFramebuffer(0, 0, Width, Height, rect.Left, rect.Bottom, rect.Right, rect.Top, COLOR_BUFFER_BIT, filter.ToWebGL2());
			//GL.blitFramebuffer(new RectangleI(Width, Height), rect, textureAttachment >= 0 ? BufferBit.Colour : DEPTH_BUFFER_BIT, filter.ToWebGL2());

			// Reset Framebuffer
			GL.bindFramebuffer(FRAMEBUFFER, (originalFramebuffer ?? (WebGL2Framebuffer)Default).id);

			Blits++;

			Graphics.State.OnStateChanged.Invoke();
		}

		private void CheckStatus()
		{
			double status = GL.checkFramebufferStatus(FRAMEBUFFER);
			if (status != FRAMEBUFFER_COMPLETE)
				throw new CKGLException("Invalid Framebuffer: " + status);
		}

		#region Overrides
		public override string ToString()
		{
			return $"Framebuffer: [id: {id}]";
		}

		public override bool Equals(object obj)
		{
			return obj is WebGL2Framebuffer && Equals((WebGL2Framebuffer)obj);
		}
		public override bool Equals(Framebuffer framebuffer)
		{
			return this == (WebGL2Framebuffer)framebuffer;
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
		public static bool operator ==(WebGL2Framebuffer a, WebGL2Framebuffer b)
		{
			return (a?.id ?? null) == (b?.id ?? null);
		}

		public static bool operator !=(WebGL2Framebuffer a, WebGL2Framebuffer b)
		{
			return (a?.id ?? null) != (b?.id ?? null);
		}
		#endregion
	}
}