using OpenGL;

using GLint = System.Int32;
using GLuint = System.UInt32;

namespace CKGL
{
	public class VertexArray
	{
		private static GLuint currentlyBoundVertexArray;

		private GLuint ID;

		public VertexArray()
		{
			Bind();
		}

		private void Generate()
		{
			if (ID == default(GLuint))
				ID = GL.GenVertexArray();
		}

		public void Destroy()
		{
			if (ID != 0)
			{
				GL.DeleteVertexArray(ID);
				ID = default(GLuint);
			}
		}

		public void Bind()
		{
			Generate();

			if (ID != currentlyBoundVertexArray)
			{
				GL.BindVertexArray(ID);
				currentlyBoundVertexArray = ID;
			}
		}

		//public void UnBind()
		//{
		//	GL.BindVertexArray(0);
		//	currentlyBoundVertexArray = 0;
		//}

		public void AddBuffer(VertexBuffer vertexBuffer, VertexBufferLayout vertexBufferLayout)
		{
			Bind();
			vertexBufferLayout.SetLayout(vertexBuffer);
		}
	}
}