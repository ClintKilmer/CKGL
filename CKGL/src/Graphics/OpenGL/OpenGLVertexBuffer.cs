using System.Runtime.InteropServices;
using CKGL.OpenGLBindings;
using GLuint = System.UInt32;

namespace CKGL.OpenGL
{
	internal class OpenGLVertexBuffer : VertexBuffer
	{
		private static GLuint currentlyBoundVertexBuffer;

		private GLuint id;

		internal OpenGLVertexBuffer(BufferUsage bufferUsage)
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
			if (id != currentlyBoundVertexBuffer)
			{
				GL.BindBuffer(BufferTarget.Array, id);
				currentlyBoundVertexBuffer = id;
			}
		}

		public override void LoadData(in byte[] data)
		{
			Bind();
			GL.BufferData(BufferTarget.Array, sizeof(byte) * data.Length, data, BufferUsage.ToOpenGL());
		}

		public override void LoadData<T>(in T[] data)// where T : struct // TODO - add this back in .NET Core 3.0
		{
			Bind();
			GL.BufferData(BufferTarget.Array, Marshal.SizeOf(typeof(T)) * data.Length, data, BufferUsage.ToOpenGL());
		}
	}
}