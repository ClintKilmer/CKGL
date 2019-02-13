namespace CKGL
{
	public enum TextureTarget : byte
	{
		//Texture1D, // Not available in ES
		Texture2D,
		Texture2DMultisample,
		Texture3D,
		//Texture1DArray, // Not available in ES
		Texture2DArray,
		TextureCubeMap
	}

	internal static class TextureTargetExt
	{
		internal static OpenGLBindings.TextureTarget ToOpenGL(this TextureTarget textureTarget)
		{
			switch (textureTarget)
			{
				//case TextureTarget.Texture1D:
				//	return OpenGLBindings.TextureTarget.Texture1D;
				case TextureTarget.Texture2D:
					return OpenGLBindings.TextureTarget.Texture2D;
				case TextureTarget.Texture2DMultisample:
					return OpenGLBindings.TextureTarget.Texture2DMultisample;
				case TextureTarget.Texture3D:
					return OpenGLBindings.TextureTarget.Texture3D;
				//case TextureTarget.Texture1DArray:
				//	return OpenGLBindings.TextureTarget.Texture1DArray;
				case TextureTarget.Texture2DArray:
					return OpenGLBindings.TextureTarget.Texture2DArray;
				case TextureTarget.TextureCubeMap:
					return OpenGLBindings.TextureTarget.TextureCubeMap;
				default:
					throw new IllegalValueException(typeof(TextureTarget), textureTarget);
			}
		}
	}
}