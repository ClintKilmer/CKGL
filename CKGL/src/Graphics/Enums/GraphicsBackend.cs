namespace CKGL
{
	public enum GraphicsBackend : byte
	{
#if VULKAN
		Vulkan, 
#endif
#if OPENGL
		OpenGL, 
#endif
#if OPENGLES
		OpenGLES, 
#endif
#if WEBGL
		WebGL 
#endif
	}
}