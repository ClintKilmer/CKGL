using static CKGL.WebGL.WebGLGraphics; // WebGL Context Methods
using static Retyped.dom; // DOM / WebGL Types
using WebGL_EXT = Retyped.dom.Literals; // WebGL Extensions

namespace CKGL.WebGL
{
	internal class VertexArray
	{
		private static WebGLVertexArrayObjectOES currentlyBoundVertexArray;

		private static OES_vertex_array_object OES_vertex_array_object;
		private WebGLVertexArrayObjectOES vao;

		internal VertexArray()
		{
			if (OES_vertex_array_object == null)
			{
				OES_vertex_array_object = GL.getExtension(WebGL_EXT.OES_vertex_array_object);

				if (OES_vertex_array_object == null)
					throw new CKGLException("WebGL 1.0 does not support Vertex Array Objects.");
			}

			vao = OES_vertex_array_object.createVertexArrayOES();
		}

		internal void Destroy()
		{
			if (vao != null)
			{
				OES_vertex_array_object.deleteVertexArrayOES(vao);
				vao = null;
			}
		}

		internal void Bind()
		{
			if (vao != currentlyBoundVertexArray)
			{
				OES_vertex_array_object.bindVertexArrayOES(vao);
				currentlyBoundVertexArray = vao;
			}
		}
	}
}