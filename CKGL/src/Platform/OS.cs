namespace CKGL
{
	public enum OS : byte
	{
		Windows,
		WinRT,
		Linux,
		Mac,
		iOS,
		tvOS,
		Android,
		Emscripten,
#if WEBGL
		HTML5
#endif
	}
}