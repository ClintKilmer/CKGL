using System.Runtime.InteropServices;
using static CKGL.WebGL.WebGLGraphics; // WebGL Context Methods
using static Retyped.dom; // WebGL Types
using static Retyped.webgl2; // WebGL Context Types
using static Retyped.webgl2.WebGL2RenderingContext; // WebGL Enums

namespace CKGL.WebGL
{
	internal class WebGLVertexBuffer : VertexBuffer
	{
		private static WebGLBuffer currentlyBoundVertexBuffer;

		private WebGLBuffer buffer;

		internal WebGLVertexBuffer(BufferUsage bufferUsage)
		{
			buffer = GL.createBuffer();
			BufferUsage = bufferUsage;
		}

		public override void Destroy()
		{
			if (buffer != null)
			{
				GL.deleteBuffer(buffer);
				buffer = null;
			}
		}

		internal override void Bind()
		{
			if (buffer != currentlyBoundVertexBuffer)
			{
				GL.bindBuffer(ARRAY_BUFFER, buffer);
				currentlyBoundVertexBuffer = buffer;
			}
		}

		public override void LoadData(byte[] data)
		{
			//Bind();
			//GL.bufferData(ARRAY_BUFFER, sizeof(byte) * data.Length, data, BufferUsage.ToWebGL());
		}

		public override void LoadData<T>(T[] data)// where T : struct // TODO - add this back in .NET Core 3.0
		{
			//Bind();
			//GL.bufferData(ARRAY_BUFFER, Marshal.SizeOf(typeof(T)) * data.Length, data, BufferUsage.ToWebGL());
		}
	}
}