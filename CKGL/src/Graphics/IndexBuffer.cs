using OpenGL;

using GLint = System.Int32;
using GLuint = System.UInt32;

namespace CKGL
{
	public class IndexBuffer
	{
		private static GLuint currentlyBoundIndexBuffer;

		private GLuint ID;

		public int Count { get; private set; }
		public IndexType IndexType { get; } = IndexType.UnsignedInt;

		public IndexBuffer()
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

			if (ID != currentlyBoundIndexBuffer)
			{
				GL.BindBuffer(BufferTarget.ElementArray, ID);
				currentlyBoundIndexBuffer = ID;
			}
		}

		//public void UnBind()
		//{
		//	GL.BindBuffer(BufferTarget.ArElementArrayray, 0);
		//	currentlyBoundIndexBuffer = 0;
		//}

		public void LoadData(GLint[] indices, BufferUsage bufferUsage)
		{
			Bind();
			GL.BufferData(BufferTarget.ElementArray, sizeof(GLint) * indices.Length, indices, bufferUsage);
			Count = indices.Length;
		}
	}
}