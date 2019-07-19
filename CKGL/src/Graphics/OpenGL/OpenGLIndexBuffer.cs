using CKGL.OpenGLBindings;
using GLubyte = System.Byte;
using GLushort = System.UInt16;
using GLuint = System.UInt32;

namespace CKGL.OpenGL
{
	internal class OpenGLIndexBuffer : IndexBuffer
	{
		private static GLuint currentlyBoundIndexBuffer;

		private GLuint id;

		internal OpenGLIndexBuffer(BufferUsage bufferUsage)
		{
			id = GL.GenBuffer();
			BufferUsage = bufferUsage;
		}

		public override void Destroy()
		{
			if (id != default)
			{
				GL.DeleteBuffer(id);
				id = default;
			}
		}

		internal override void Bind()
		{
			if (id != currentlyBoundIndexBuffer)
			{
				GL.BindBuffer(BufferTarget.ElementArray, id);
				currentlyBoundIndexBuffer = id;
			}
		}

		public override void LoadData(in GLushort[] indices)
		{
			Bind();
			GL.BufferData(BufferTarget.ElementArray, sizeof(GLushort) * indices.Length, indices, BufferUsage.ToOpenGL());
			IndexType = IndexType.UnsignedShort;
			Count = indices.Length;
		}

		public override void LoadData(in GLuint[] indices)
		{
			Bind();
			GL.BufferData(BufferTarget.ElementArray, sizeof(GLuint) * indices.Length, indices, BufferUsage.ToOpenGL());
			IndexType = IndexType.UnsignedInt;
			Count = indices.Length;
		}
	}
}