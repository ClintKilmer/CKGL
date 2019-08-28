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

	public static class PixelFormatExt
	{
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