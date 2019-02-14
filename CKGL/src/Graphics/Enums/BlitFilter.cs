namespace CKGL
{
	public enum BlitFilter : byte
	{
		Nearest,
		Linear
	}

	internal static class BlitFilterExt
	{
		internal static OpenGLBindings.BlitFilter ToOpenGL(this BlitFilter blitFilter)
		{
			switch (blitFilter)
			{
				case BlitFilter.Nearest:
					return OpenGLBindings.BlitFilter.Nearest;
				case BlitFilter.Linear:
					return OpenGLBindings.BlitFilter.Linear;
				default:
					throw new IllegalValueException(typeof(BlitFilter), blitFilter);
			}
		}
	}
}