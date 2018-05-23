using OpenGL;

using GLint = System.Int32;
using GLuint = System.UInt32;

namespace CKGL
{
	public class VertexBuffer
	{
		private static GLuint currentlyBoundVertexBuffer;

		private GLuint id;

		public VertexBuffer()
		{
			id = GL.GenBuffer();
		}

		public void Destroy()
		{
			if (id != default(GLuint))
			{
				GL.DeleteBuffer(id);
				id = default(GLuint);
			}
		}

		public void Bind()
		{
			if (id != currentlyBoundVertexBuffer)
			{
				GL.BindBuffer(BufferTarget.Array, id);
				currentlyBoundVertexBuffer = id;
			}
		}

		public void LoadData<T>(GLint sizeInBytes, T[] vertices, BufferUsage bufferUsage) where T : struct
		{
			Bind();
			GL.BufferData(BufferTarget.Array, sizeInBytes, vertices, bufferUsage);
		}

		public void LoadData(byte[] vertices, BufferUsage bufferUsage)
		{
			Bind();
			GL.BufferData(BufferTarget.Array, sizeof(byte) * vertices.Length, vertices, bufferUsage);
		}

		public void LoadData(float[] vertices, BufferUsage bufferUsage)
		{
			Bind();
			GL.BufferData(BufferTarget.Array, sizeof(float) * vertices.Length, vertices, bufferUsage);
		}
	}
}