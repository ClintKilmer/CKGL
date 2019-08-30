using static CKGL.WebGL.WebGLGraphics; // WebGL Context Methods
using static Retyped.webgl2; // WebGL Types - WebGL2RenderingContext, WebGLVertexArrayObject

namespace CKGL.WebGL
{
	internal class VertexArray
	{
		private static WebGLVertexArrayObject currentlyBoundVertexArray;

		private WebGLVertexArrayObject vao;

		internal VertexArray()
		{
			vao = GL.createVertexArray();
		}

		internal void Destroy()
		{
			if (vao != null)
			{
				GL.deleteVertexArray(vao);
				vao = null;
			}
		}

		internal void Bind()
		{
			if (vao != currentlyBoundVertexArray)
			{
				GL.bindVertexArray(vao);
				currentlyBoundVertexArray = vao;
			}
		}
	}
}