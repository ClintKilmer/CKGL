namespace CKGL
{
	public enum TexImageTarget : byte
	{
		Texture2D,
		TextureCubeMapPosX,
		TextureCubeMapNegX,
		TextureCubeMapPosY,
		TextureCubeMapNegY,
		TextureCubeMapPosZ,
		TextureCubeMapNegZ
	}

	internal static class TexImageTargetExt
	{
		internal static OpenGLBindings.TexImageTarget ToOpenGL(this TexImageTarget texImageTarget)
		{
			switch (texImageTarget)
			{
				case TexImageTarget.Texture2D:
					return OpenGLBindings.TexImageTarget.Texture2D;
				case TexImageTarget.TextureCubeMapPosX:
					return OpenGLBindings.TexImageTarget.TextureCubeMapPosX;
				case TexImageTarget.TextureCubeMapNegX:
					return OpenGLBindings.TexImageTarget.TextureCubeMapNegX;
				case TexImageTarget.TextureCubeMapPosY:
					return OpenGLBindings.TexImageTarget.TextureCubeMapPosY;
				case TexImageTarget.TextureCubeMapNegY:
					return OpenGLBindings.TexImageTarget.TextureCubeMapNegY;
				case TexImageTarget.TextureCubeMapPosZ:
					return OpenGLBindings.TexImageTarget.TextureCubeMapPosZ;
				case TexImageTarget.TextureCubeMapNegZ:
					return OpenGLBindings.TexImageTarget.TextureCubeMapNegZ;
				default:
					throw new IllegalValueException(typeof(TexImageTarget), texImageTarget);
			}
		}
	}
}