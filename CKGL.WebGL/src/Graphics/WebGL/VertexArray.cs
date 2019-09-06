using static CKGL.WebGL.WebGLGraphics; // WebGL Context Methods
using static Retyped.dom; // DOM / WebGL Types

namespace CKGL.WebGL
{
	internal class VertexArray
	{
		private static WebGLVertexArrayObjectOES currentlyBoundVertexArray;

		private WebGLVertexArrayObjectOES vao;

		internal VertexArray()
		{
			vao = Extensions.OES_vertex_array_object.createVertexArrayOES();
		}

		internal void Destroy()
		{
			if (vao != null)
			{
				Extensions.OES_vertex_array_object.deleteVertexArrayOES(vao);
				vao = null;
			}
		}

		internal void Bind()
		{
			if (vao != currentlyBoundVertexArray)
			{
				Extensions.OES_vertex_array_object.bindVertexArrayOES(vao);
				currentlyBoundVertexArray = vao;
			}
		}
	}
}