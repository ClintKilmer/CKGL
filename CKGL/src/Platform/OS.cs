namespace CKGL
{
	public enum OS : byte
	{
		Windows,
		WinRT,
		Linux,
		macOS,
		iOS,
		tvOS,
		Android,
		Emscripten,
#if WEBGL
		HTML5
#endif
	}
}