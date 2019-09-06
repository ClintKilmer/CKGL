using static CKGL.WebGL.WebGLGraphics; // WebGL Context Methods
using static Retyped.dom; // DOM / WebGL Types
using static Retyped.es5; // JS TypedArrays

namespace CKGL.WebGL
{
	internal class WebGLIndexBuffer : IndexBuffer
	{
		private static WebGLBuffer currentlyBoundIndexBuffer;

		private WebGLBuffer buffer;

		internal WebGLIndexBuffer(BufferUsage bufferUsage)
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
			if (buffer != currentlyBoundIndexBuffer)
			{
				GL.bindBuffer(GL.ELEMENT_ARRAY_BUFFER, buffer);
				currentlyBoundIndexBuffer = buffer;
			}
		}

		public override void LoadData(ushort[] indices)
		{
			Bind();
			GL.bufferData(GL.ELEMENT_ARRAY_BUFFER, new Uint16Array(indices), BufferUsage.ToWebGL());
			IndexType = IndexType.UnsignedShort;
			Count = indices.Length;
		}

		public override void LoadData(uint[] indices)
		{
			Bind();
			GL.bufferData(GL.ELEMENT_ARRAY_BUFFER, new Uint32Array(indices), BufferUsage.ToWebGL());
			IndexType = IndexType.UnsignedInt;
			Count = indices.Length;
		}
	}
}