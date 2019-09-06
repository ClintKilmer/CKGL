using static CKGL.WebGL2.WebGL2Graphics; // WebGL Context Methods
using static Retyped.dom; // WebGL Types
using static Retyped.es5; // JS TypedArrays
using static Retyped.webgl2.WebGL2RenderingContext; // WebGL Enums

namespace CKGL.WebGL2
{
	internal class WebGL2IndexBuffer : IndexBuffer
	{
		private static WebGLBuffer currentlyBoundIndexBuffer;

		private WebGLBuffer buffer;

		internal WebGL2IndexBuffer(BufferUsage bufferUsage)
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
				GL.bindBuffer(ELEMENT_ARRAY_BUFFER, buffer);
				currentlyBoundIndexBuffer = buffer;
			}
		}

		public override void LoadData(ushort[] indices)
		{
			Bind();
			GL.bufferData(ELEMENT_ARRAY_BUFFER, new Uint16Array(indices), BufferUsage.ToWebGL2());
			IndexType = IndexType.UnsignedShort;
			Count = indices.Length;
		}

		public override void LoadData(uint[] indices)
		{
			Bind();
			GL.bufferData(ELEMENT_ARRAY_BUFFER, new Uint32Array(indices), BufferUsage.ToWebGL2());
			IndexType = IndexType.UnsignedInt;
			Count = indices.Length;
		}
	}
}