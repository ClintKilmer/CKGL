namespace CKGL
{
	public enum TextureParam : byte
	{
		BaseLevel,
		CompareFunc,
		CompareMode,
		MinFilter,
		MagFilter,
		MinLOD,
		MaxLOD,
		MaxLevel,
		SwizzleR,
		SwizzleG,
		SwizzleB,
		SwizzleA,
		WrapS,
		WrapT,
		WrapR
		//DepthTextureMode // Unavailable in OpenGL 3.x, Available in ES 3.0
	}

	internal static class TextureParamExt
	{
		internal static OpenGLBindings.TextureParam ToOpenGL(this TextureParam textureParam)
		{
			switch (textureParam)
			{
				case TextureParam.BaseLevel:
					return OpenGLBindings.TextureParam.BaseLevel;
				case TextureParam.CompareFunc:
					return OpenGLBindings.TextureParam.CompareFunc;
				case TextureParam.CompareMode:
					return OpenGLBindings.TextureParam.CompareMode;
				case TextureParam.MinFilter:
					return OpenGLBindings.TextureParam.MinFilter;
				case TextureParam.MagFilter:
					return OpenGLBindings.TextureParam.MagFilter;
				case TextureParam.MinLOD:
					return OpenGLBindings.TextureParam.MinLOD;
				case TextureParam.MaxLOD:
					return OpenGLBindings.TextureParam.MaxLOD;
				case TextureParam.MaxLevel:
					return OpenGLBindings.TextureParam.MaxLevel;
				case TextureParam.SwizzleR:
					return OpenGLBindings.TextureParam.SwizzleR;
				case TextureParam.SwizzleG:
					return OpenGLBindings.TextureParam.SwizzleG;
				case TextureParam.SwizzleB:
					return OpenGLBindings.TextureParam.SwizzleB;
				case TextureParam.SwizzleA:
					return OpenGLBindings.TextureParam.SwizzleA;
				case TextureParam.WrapS:
					return OpenGLBindings.TextureParam.WrapS;
				case TextureParam.WrapT:
					return OpenGLBindings.TextureParam.WrapT;
				case TextureParam.WrapR:
					return OpenGLBindings.TextureParam.WrapR;
				//case TextureParam.DepthTextureMode:
				//	return OpenGLBindings.TextureParam.DepthTextureMode;
				default:
					throw new IllegalValueException(typeof(TextureParam), textureParam);
			}
		}
	}
}