namespace CKGL
{
	public enum TextureFormat : byte
	{
		// Depth textures
		Depth,
		Depth16,
		Depth24,
		Depth32,
		Depth32F,

		// Depth/Stencil textures
		DepthStencil,
		Depth24Stencil8,

		// Red textures
		R,
		R8,

		// RG textures
		RG,
		RG8,

		// RGB textures
		RGB,
		RGB8,

		// RGBA textures
		RGBA,
		RGBA8
	}

	internal static class TextureFormatExt
	{
		public static int Size(this TextureFormat textureFormat)
		{
			switch (textureFormat)
			{
				case TextureFormat.RGBA8:
					return 8;
				default:
					throw new IllegalValueException(typeof(TextureFormat), textureFormat);
			}
		}

		public static PixelFormat PixelFormat(this TextureFormat textureFormat)
		{
			switch (textureFormat)
			{
				case TextureFormat.Depth:
				case TextureFormat.Depth16:
				case TextureFormat.Depth24:
				case TextureFormat.Depth32:
				case TextureFormat.Depth32F:
					return CKGL.PixelFormat.Depth;
				case TextureFormat.DepthStencil:
				case TextureFormat.Depth24Stencil8:
					return CKGL.PixelFormat.DepthStencil;
				case TextureFormat.R:
				case TextureFormat.R8:
					//case TextureFormat.R8SNorm:
					//case TextureFormat.R8I:
					//case TextureFormat.R8UI:
					//case TextureFormat.R16I:
					//case TextureFormat.R16UI:
					//case TextureFormat.R16F:
					//case TextureFormat.R32I:
					//case TextureFormat.R32UI:
					//case TextureFormat.R32F:
					return CKGL.PixelFormat.R;
				case TextureFormat.RG:
				case TextureFormat.RG8:
					//case TextureFormat.RG8SNorm:
					//case TextureFormat.RG8I:
					//case TextureFormat.RG8UI:
					//case TextureFormat.RG16I:
					//case TextureFormat.RG16UI:
					//case TextureFormat.RG16F:
					//case TextureFormat.RG32I:
					//case TextureFormat.RG32UI:
					//case TextureFormat.RG32F:
					return CKGL.PixelFormat.RG;
				case TextureFormat.RGB:
				case TextureFormat.RGB8:
					//case TextureFormat.RGB8SNorm:
					//case TextureFormat.RGB8I:
					//case TextureFormat.RGB8UI:
					//case TextureFormat.RGB16I:
					//case TextureFormat.RGB16UI:
					//case TextureFormat.RGB16F:
					//case TextureFormat.RGB32I:
					//case TextureFormat.RGB32UI:
					//case TextureFormat.RGB32F:
					//case TextureFormat.R3G3B2:
					//case TextureFormat.R5G6B5:
					//case TextureFormat.R11G11B10F:
					return CKGL.PixelFormat.RGB;
				case TextureFormat.RGBA:
				case TextureFormat.RGBA8:
					//case TextureFormat.RGBA8SNorm:
					//case TextureFormat.RGBA16F:
					//case TextureFormat.RGBA32F:
					//case TextureFormat.RGBA8I:
					//case TextureFormat.RGBA8UI:
					//case TextureFormat.RGBA16I:
					//case TextureFormat.RGBA16UI:
					//case TextureFormat.RGBA32I:
					//case TextureFormat.RGBA32UI:
					//case TextureFormat.RGBA2:
					//case TextureFormat.RGBA4:
					//case TextureFormat.RGB5A1:
					//case TextureFormat.RGB10A2:
					//case TextureFormat.RGB10A2UI:
					return CKGL.PixelFormat.RGBA;
				default:
					throw new IllegalValueException(typeof(TextureFormat), textureFormat);
			}
		}

		public static TextureAttachment TextureAttachment(this TextureFormat textureFormat)
		{
			switch (textureFormat)
			{
				case TextureFormat.Depth:
				case TextureFormat.Depth16:
				case TextureFormat.Depth24:
				case TextureFormat.Depth32:
				case TextureFormat.Depth32F:
					return CKGL.TextureAttachment.Depth;
				case TextureFormat.DepthStencil:
				case TextureFormat.Depth24Stencil8:
					return CKGL.TextureAttachment.DepthStencil;
				default:
					throw new IllegalValueException(typeof(TextureFormat), textureFormat);
			}
		}
	}
}