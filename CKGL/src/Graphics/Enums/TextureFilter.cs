namespace CKGL
{
	public enum TextureFilter : byte
	{
		Nearest, // Min/Max
		Linear, // Min/Max
		NearestMipmapNearest, // Min
		LinearMipmapNearest, // Min
		NearestMipmapLinear, // Min
		LinearMipmapLinear // Min
	}

	internal static class TextureFilterExt
	{
		internal static OpenGLBindings.TextureFilter ToOpenGL(this TextureFilter textureFilter)
		{
			switch (textureFilter)
			{
				case TextureFilter.Nearest:
					return OpenGLBindings.TextureFilter.Nearest;
				case TextureFilter.Linear:
					return OpenGLBindings.TextureFilter.Linear;
				case TextureFilter.NearestMipmapNearest:
					return OpenGLBindings.TextureFilter.NearestMipmapNearest;
				case TextureFilter.LinearMipmapNearest:
					return OpenGLBindings.TextureFilter.LinearMipmapNearest;
				case TextureFilter.NearestMipmapLinear:
					return OpenGLBindings.TextureFilter.NearestMipmapLinear;
				case TextureFilter.LinearMipmapLinear:
					return OpenGLBindings.TextureFilter.LinearMipmapLinear;
				default:
					throw new IllegalValueException(typeof(TextureFilter), textureFilter);
			}
		}
	}
}