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
}