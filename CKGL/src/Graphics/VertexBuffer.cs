using System;
using System.Runtime.InteropServices;
using OpenGL;
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

		// TODO - Do we need this: VertexBuffer.LoadData(byte[] buffer, BufferUsage bufferUsage)
		public void LoadData(byte[] buffer, BufferUsage bufferUsage)
		{
			Bind();
			GL.BufferData(BufferTarget.Array, sizeof(byte) * buffer.Length, buffer, bufferUsage);
		}

		public void LoadData<T>(VertexAttributeLayout vertexAttributeLayout, ref T[] buffer, int vertexCount, BufferUsage bufferUsage) where T : struct
		{
			if (buffer == null)
				throw new ArgumentNullException("data");

			if (buffer.Length < vertexCount)
				throw new ArgumentOutOfRangeException("vertexCount", "This parameter must be a valid index within the array.");

			int bufferSize = Marshal.SizeOf(typeof(T));
			if (vertexAttributeLayout.Stride != bufferSize)
				throw new ArgumentOutOfRangeException("The VertexAttributeLayout.Stride does not match the Marshalled size of a buffer element.");

			Bind();
			GL.BufferData(BufferTarget.Array, bufferSize * vertexCount, buffer, bufferUsage);
		}
	}
}