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
}