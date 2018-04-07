using OpenGL;

using GLint = System.Int32;
using GLuint = System.UInt32;

namespace CKGL
{
	public class VertexArray
	{
		private static GLuint currentlyBoundVertexArray;

		private GLuint id;

		public VertexArray()
		{
			id = GL.GenVertexArray();
		}

		public void Destroy()
		{
			if (id != default(GLuint))
			{
				GL.DeleteVertexArray(id);
				id = default(GLuint);
			}
		}

		public void Bind()
		{
			if (id != currentlyBoundVertexArray)
			{
				GL.BindVertexArray(id);
				currentlyBoundVertexArray = id;
			}
		}

		public void AddBuffer(VertexBuffer vertexBuffer, VertexBufferLayout vertexBufferLayout)
		{
			Bind();
			vertexBufferLayout.SetLayout(vertexBuffer);
		}
	}
}