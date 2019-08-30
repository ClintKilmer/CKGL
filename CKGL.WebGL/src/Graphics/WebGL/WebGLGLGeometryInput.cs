using static CKGL.WebGL.WebGLGraphics; // WebGL Context Methods
using static Retyped.dom; // WebGL Types
using static Retyped.es5; // JS TypedArrays
using static Retyped.webgl2.WebGL2RenderingContext; // WebGL Enums

namespace CKGL.WebGL
{
	public class WebGLGeometryInput : GeometryInput
	{
		private readonly VertexArray vao;

		internal WebGLGeometryInput(IndexBuffer indexBuffer, VertexStream[] vertexStreams)
		{
			if (vertexStreams.Length < 1)
				throw new CKGLException("GeometryInput constructor requires at least 1 VertexStream");

			VertexStreams = vertexStreams;
			IndexBuffer = indexBuffer;

			vao = new VertexArray();

			vao.Bind();

			foreach (VertexStream vertexStream in vertexStreams)
			{
				vertexStream.VertexBuffer.Bind();

				for (uint i = 0; i < vertexStream.VertexFormat.Attributes.Length; i++)
				{
					GL.enableVertexAttribArray(i);
					GL.vertexAttribPointer(i, vertexStream.VertexFormat.Attributes[(int)i].Count, vertexStream.VertexFormat.Attributes[(int)i].Type.ToWebGL(), vertexStream.VertexFormat.Attributes[(int)i].Normalized, vertexStream.VertexFormat.Stride, vertexStream.VertexFormat.Attributes[(int)i].Offset);
					if (vertexStream.VertexFormat.Attributes[(int)i].Divisor > 0)
						GL.vertexAttribDivisor(i, vertexStream.VertexFormat.Attributes[(int)i].Divisor);
					//Output.WriteLine($"id: {i}, Count: {vertexStream.VertexFormat.Attributes[(int)i].Count}, Type: {vertexStream.VertexFormat.Attributes[(int)i].Type}, Normalized: {vertexStream.VertexFormat.Attributes[(int)i].Normalized}, Size/Stride: {vertexStream.VertexFormat.Attributes[(int)i].Size}/{vertexStream.VertexFormat.Stride}, offset: {vertexStream.VertexFormat.Attributes[(int)i].Offset}, divisor: {divisor}"); // Debug
				}
			}

			if (indexBuffer != null)
			{
				indexBuffer.Bind();
			}
		}

		public override void Destroy()
		{
			vao.Destroy();
		}

		public override void Bind()
		{
			vao.Bind();
		}
	}
}