using OpenGL;

using GLint = System.Int32;
using GLuint = System.UInt32;

namespace CKGL
{
	public class VertexBuffer
	{
		private static GLuint currentlyBoundVertexBuffer;

		private GLuint ID;

		public VertexBuffer()
		{
			Bind();
		}

		private void Generate()
		{
			if (ID == 0)
				ID = GL.GenBuffer();
		}

		public void Destroy()
		{
			if (ID != 0)
			{
				GL.DeleteBuffer(ID);
				ID = 0;
			}
		}

		public void Bind()
		{
			Generate();

			if (ID != currentlyBoundVertexBuffer)
			{
				GL.BindBuffer(BufferTarget.Array, ID);
				currentlyBoundVertexBuffer = ID;
			}
		}

		//public void UnBind()
		//{
		//	GL.BindBuffer(BufferTarget.Array, 0);
		//	currentlyBoundVertexBuffer = 0;
		//}

		public void LoadData<T>(GLint sizeInBytes, T[] vertices, BufferUsage bufferUsage) where T : struct
		{
			Bind();
			GL.BufferData(BufferTarget.Array, sizeInBytes, vertices, bufferUsage);
		}

		public void LoadData(float[] vertices, BufferUsage bufferUsage)
		{
			Bind();
			GL.BufferData(BufferTarget.Array, sizeof(float) * vertices.Length, vertices, bufferUsage);
		}
	}
}