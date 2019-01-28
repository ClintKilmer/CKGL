using CKGL.OpenGLBindings;
using GLuint = System.UInt32;

namespace CKGL.OpenGL
{
	internal class VertexArray
	{
		private static GLuint currentlyBoundVertexArray;

		private GLuint id;

		internal VertexArray()
		{
			id = GL.GenVertexArray();
		}

		internal void Destroy()
		{
			if (id != default)
			{
				GL.DeleteVertexArray(id);
				id = default;
			}
		}

		internal void Bind()
		{
			if (id != currentlyBoundVertexArray)
			{
				GL.BindVertexArray(id);
				currentlyBoundVertexArray = id;
			}
		}

		internal void AddBuffer(VertexBuffer vertexBuffer, VertexAttributeLayout vertexDeclaration)
		{
			Bind();
			vertexBuffer.Bind();
			vertexDeclaration.SetVertexAttributes();
		}
	}
}