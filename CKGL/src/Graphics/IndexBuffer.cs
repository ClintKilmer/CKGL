using OpenGL;

using GLint = System.Int32;
using GLuint = System.UInt32;

namespace CKGL
{
	public class IndexBuffer
	{
		private static GLuint currentlyBoundIndexBuffer;

		private GLuint id;

		public int Count { get; private set; }
		public IndexType IndexType { get; } = IndexType.UnsignedInt;

		public IndexBuffer()
		{
			id = GL.GenBuffer();
		}

		public void Destroy()
		{
			if (id != default)
			{
				GL.DeleteBuffer(id);
				id = default;
			}
		}

		public void Bind()
		{
			if (id != currentlyBoundIndexBuffer)
			{
				GL.BindBuffer(BufferTarget.ElementArray, id);
				currentlyBoundIndexBuffer = id;
			}
		}

		public void LoadData(GLuint[] indices, BufferUsage bufferUsage)
		{
			Bind();
			GL.BufferData(BufferTarget.ElementArray, sizeof(GLuint) * indices.Length, indices, bufferUsage);
			Count = indices.Length;
		}
	}
}