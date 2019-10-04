namespace CKGL
{
	public abstract class Framebuffer
	{
		// Dummy class for Graphics

		public static readonly Framebuffer Default;
		public static Framebuffer Current { get; private set; } = Default;

		public int Width = 0;
		public int Height = 0;
	}
}