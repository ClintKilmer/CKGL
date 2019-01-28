using CKGL.OpenGLBindings;

using GLint = System.Int32;
using GLuint = System.UInt32;

namespace CKGL.OpenGL
{
	internal class IndexBuffer
	{
		private static GLuint currentlyBoundIndexBuffer;

		private GLuint id;

		internal int Count { get; private set; }
		internal IndexType IndexType { get; } = IndexType.UnsignedInt;

		internal IndexBuffer()
		{
			id = GL.GenBuffer();
		}

		internal void Destroy()
		{
			if (id != default)
			{
				GL.DeleteBuffer(id);
				id = default;
			}
		}

		internal void Bind()
		{
			if (id != currentlyBoundIndexBuffer)
			{
				GL.BindBuffer(BufferTarget.ElementArray, id);
				currentlyBoundIndexBuffer = id;
			}
		}

		internal void LoadData(GLuint[] indices, BufferUsage bufferUsage)
		{
			Bind();
			GL.BufferData(BufferTarget.ElementArray, sizeof(GLuint) * indices.Length, indices, bufferUsage);
			Count = indices.Length;
		}
	}
}