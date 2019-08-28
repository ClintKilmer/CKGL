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
}