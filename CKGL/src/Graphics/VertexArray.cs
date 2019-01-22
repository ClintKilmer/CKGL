using OpenGL;
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
			if (id != default)
			{
				GL.DeleteVertexArray(id);
				id = default;
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

		public void AddBuffer(VertexBuffer vertexBuffer, VertexAttributeLayout vertexDeclaration)
		{
			Bind();
			vertexBuffer.Bind();
			vertexDeclaration.SetVertexAttributes();
		}
	}
}