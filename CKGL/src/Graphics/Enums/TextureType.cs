namespace CKGL
{
	public enum TextureType : byte
	{
		Texture1D, // Not available in OpenGL ES / WebGL
		Texture2D,
		Texture2DMultisample,
		Texture3D,
		Texture1DArray, // Not available in OpenGL ES / WebGL
		Texture2DArray,
		TextureCubeMap
	}
}