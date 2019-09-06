using static CKGL.WebGL2.WebGL2Graphics; // WebGL Context Methods
using static Retyped.webgl2; // WebGL Types - WebGL2RenderingContext, WebGLVertexArrayObject

namespace CKGL.WebGL2
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