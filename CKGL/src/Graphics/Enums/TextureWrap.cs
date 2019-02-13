namespace CKGL
{
	public enum TextureWrap : byte
	{
		Clamp,
		Repeat,
		MirroredRepeat
	}

	internal static class TextureWrapExt
	{
		internal static OpenGLBindings.TextureWrap ToOpenGL(this TextureWrap textureWrap)
		{
			switch (textureWrap)
			{
				case TextureWrap.Clamp:
					return OpenGLBindings.TextureWrap.Clamp;
				case TextureWrap.Repeat:
					return OpenGLBindings.TextureWrap.Repeat;
				case TextureWrap.MirroredRepeat:
					return OpenGLBindings.TextureWrap.MirroredRepeat;
				default:
					throw new IllegalValueException(typeof(TextureWrap), textureWrap);
			}
		}
	}
}