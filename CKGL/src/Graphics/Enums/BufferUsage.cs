namespace CKGL
{
	public enum BufferUsage : byte
	{
		Static,     // GPU read, CPU write only once			- OpenGL: GL_STATIC_DRAW,  D3D11: D3D11_USAGE_IMMUTABLE
		Default,    // GPU read/write, CPU write occationally	- OpenGL: GL_DYNAMIC_DRAW, D3D11: D3D11_USAGE_DEFAULT
		Dynamic,    // GPU read, CPU write once per frame		- OpenGL: GL_STREAM_DRAW,  D3D11: D3D11_USAGE_DYNAMIC
		Copy        // GPU write, CPU read						- OpenGL: GL_DYNAMIC_READ, D3D11: D3D11_USAGE_STAGING
	}

	internal static class BufferUsageExt
	{
		internal static OpenGLBindings.BufferUsage ToOpenGL(this BufferUsage bufferUsage)
		{
			switch (bufferUsage)
			{
				case BufferUsage.Static:
					return OpenGLBindings.BufferUsage.StaticDraw;
				case BufferUsage.Default:
					return OpenGLBindings.BufferUsage.DynamicDraw;
				case BufferUsage.Dynamic:
					return OpenGLBindings.BufferUsage.StreamDraw;
				case BufferUsage.Copy:
					return OpenGLBindings.BufferUsage.DynamicRead;
				default:
					throw new IllegalValueException(typeof(BufferUsage), bufferUsage);
			}
		}
	}
}