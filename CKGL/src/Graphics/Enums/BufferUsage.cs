namespace CKGL
{
	public enum BufferUsage : byte
	{
		StreamDraw,
		StreamRead,
		StreamCopy,
		StaticDraw,
		StaticRead,
		StaticCopy,
		DynamicDraw,
		DynamicRead,
		DynamicCopy
	}

	internal static class BufferUsageExt
	{
		internal static OpenGLBindings.BufferUsage ToOpenGL(this BufferUsage bufferUsage)
		{
			switch (bufferUsage)
			{
				case BufferUsage.StreamDraw:
					return OpenGLBindings.BufferUsage.StreamDraw;
				case BufferUsage.StreamRead:
					return OpenGLBindings.BufferUsage.StreamRead;
				case BufferUsage.StreamCopy:
					return OpenGLBindings.BufferUsage.StreamCopy;
				case BufferUsage.StaticDraw:
					return OpenGLBindings.BufferUsage.StaticDraw;
				case BufferUsage.StaticRead:
					return OpenGLBindings.BufferUsage.StaticRead;
				case BufferUsage.StaticCopy:
					return OpenGLBindings.BufferUsage.StaticCopy;
				case BufferUsage.DynamicDraw:
					return OpenGLBindings.BufferUsage.DynamicDraw;
				case BufferUsage.DynamicRead:
					return OpenGLBindings.BufferUsage.DynamicRead;
				case BufferUsage.DynamicCopy:
					return OpenGLBindings.BufferUsage.DynamicCopy;
				default:
					throw new IllegalValueException(typeof(BufferUsage), bufferUsage);
			}
		}
	}
}