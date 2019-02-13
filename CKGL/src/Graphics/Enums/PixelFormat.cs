namespace CKGL
{
	public enum PixelFormat : byte
	{
		Depth,
		DepthStencil,
		R,
		RG,
		RGB,
		RGBA
	}

	internal static class PixelFormatExt
	{
		internal static OpenGLBindings.PixelFormat ToOpenGL(this PixelFormat pixelFormat)
		{
			switch (pixelFormat)
			{
				case PixelFormat.Depth:
					return OpenGLBindings.PixelFormat.Depth;
				case PixelFormat.DepthStencil:
					return OpenGLBindings.PixelFormat.DepthStencil;
				case PixelFormat.R:
					return OpenGLBindings.PixelFormat.R;
				case PixelFormat.RG:
					return OpenGLBindings.PixelFormat.RG;
				case PixelFormat.RGB:
					return OpenGLBindings.PixelFormat.RGB;
				case PixelFormat.RGBA:
					return OpenGLBindings.PixelFormat.RGBA;
				default:
					throw new IllegalValueException(typeof(PixelFormat), pixelFormat);
			}
		}

		public static int Components(this PixelFormat pixelFormat)
		{
			switch (pixelFormat)
			{
				case PixelFormat.Depth:
					return 1;
				case PixelFormat.DepthStencil:
					return 2;
				case PixelFormat.R:
					return 1;
				case PixelFormat.RG:
					return 2;
				case PixelFormat.RGB:
					return 3;
				case PixelFormat.RGBA:
					return 4;
				default:
					throw new IllegalValueException(typeof(PixelFormat), pixelFormat);
			}
		}
	}
}