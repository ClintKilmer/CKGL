namespace CKGL
{
	public abstract class RenderTarget
	{
		// Dummy class for Graphics

		public static readonly RenderTarget Default;
		public static RenderTarget Current { get; private set; } = Default;

		public int Width = 0;
		public int Height = 0;
	}
}