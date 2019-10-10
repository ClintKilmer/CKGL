using System;
using static CKGL.WebGL.WebGLGraphics; // WebGL Context Methods

namespace CKGL.WebGL
{
	public class WebGLFramebuffer : Framebuffer
	{
		private Retyped.dom.WebGLFramebuffer id;

		internal static Framebuffer CreateDefault() => new WebGLFramebuffer();

		// Default WebGLFramebuffer
		private WebGLFramebuffer()
		{
			id = null;
			IsDefault = true;
		}

		public WebGLFramebuffer(int width, int height, int colourTextures, TextureFormat textureColourFormat, TextureFormat? textureDepthFormat = null)
		{
			if (colourTextures < 1)
				throw new CKGLException("WebGL 1.0 framebuffers must have 1 colour texture.");
			if (colourTextures > 1) // WebGL 1.0 only allows 1 colour attachment
				throw new CKGLException("WebGL 1.0 framebuffers must have 1 colour texture.");
			if (textureColourFormat.ToWebGLPixelFormat() == GL.DEPTH_COMPONENT || textureColourFormat.ToWebGLPixelFormat() == GL.DEPTH_STENCIL)
				throw new CKGLException("textureColourFormat cannot be a depth(stencil) texture.");
			if (textureDepthFormat.HasValue && !(textureDepthFormat.Value.ToWebGLPixelFormat() == GL.DEPTH_COMPONENT || textureDepthFormat.Value.ToWebGLPixelFormat() == GL.DEPTH_STENCIL))
				throw new CKGLException("textureDepthFormat is not a depth(stencil) texture.");

			this.width = width;
			this.height = height;

			camera2D.Width = width;
			camera2D.Height = height;

			id = GL.createFramebuffer();

			Bind();

			Textures = new Texture[1];
			Textures[0] = Texture.Create2D(Width, Height, textureColourFormat);
			GL.framebufferTexture2D(GL.FRAMEBUFFER, GL.COLOR_ATTACHMENT0, (Textures[0] as WebGLTexture).TextureTarget, (Textures[0] as WebGLTexture).ID, 0);
			Textures[0].Unbind();
			CheckStatus();

			if (textureDepthFormat.HasValue)
			{
				DepthStencilTexture = Texture.Create2D(Width, Height, textureDepthFormat.Value);
				GL.framebufferTexture2D(GL.FRAMEBUFFER, textureDepthFormat.Value.ToWebGLTextureAttachment(), (DepthStencilTexture as WebGLTexture).TextureTarget, (DepthStencilTexture as WebGLTexture).ID, 0);
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
			if (id != (Current as WebGLFramebuffer).id)
			{
				Graphics.State.OnStateChanging?.Invoke();
				UnbindTextures();
				GL.bindFramebuffer(GL.FRAMEBUFFER, id);
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

		// WebGL 1.0 does not support blitting framebuffers
		public void BlitTextureTo(Framebuffer target, TextureAttachment textureAttachment, BlitFilter filter) => BlitTextureTo(target, textureAttachment, filter, new RectangleI(Width, Height));
		public void BlitTextureTo(Framebuffer target, TextureAttachment textureAttachment, BlitFilter filter, int x, int y) => BlitTextureTo(target, textureAttachment, filter, new RectangleI(x, y, Width, Height));
		public void BlitTextureTo(Framebuffer target, TextureAttachment textureAttachment, BlitFilter filter, RectangleI rect)
		{
			throw new CKGLException($"WebGL 1.0 does not support blitting framebuffers.");
		}

		private void CheckStatus()
		{
			double status = GL.checkFramebufferStatus(GL.FRAMEBUFFER);
			if (status != GL.FRAMEBUFFER_COMPLETE)
				throw new CKGLException("Invalid Framebuffer: " + status);
		}

		#region Overrides
		public override string ToString()
		{
			return $"Framebuffer: [id: {id}]";
		}

		public override bool Equals(object obj)
		{
			return obj is WebGLFramebuffer && Equals((WebGLFramebuffer)obj);
		}
		public override bool Equals(Framebuffer framebuffer)
		{
			return this == (WebGLFramebuffer)framebuffer;
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
		public static bool operator ==(WebGLFramebuffer a, WebGLFramebuffer b)
		{
			return (a?.id ?? null) == (b?.id ?? null);
		}

		public static bool operator !=(WebGLFramebuffer a, WebGLFramebuffer b)
		{
			return (a?.id ?? null) != (b?.id ?? null);
		}
		#endregion
	}
}