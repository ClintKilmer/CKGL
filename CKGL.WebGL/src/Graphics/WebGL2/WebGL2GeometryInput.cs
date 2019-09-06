using static CKGL.WebGL2.WebGL2Graphics; // WebGL Context Methods

namespace CKGL.WebGL2
{
	public class WebGL2GeometryInput : GeometryInput
	{
		private readonly VertexArray vao;

		internal WebGL2GeometryInput(IndexBuffer indexBuffer, VertexStream[] vertexStreams)
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
					GL.vertexAttribPointer(i, vertexStream.VertexFormat.Attributes[(int)i].Count, vertexStream.VertexFormat.Attributes[(int)i].Type.ToWebGL2(), vertexStream.VertexFormat.Attributes[(int)i].Normalized, vertexStream.VertexFormat.Stride, vertexStream.VertexFormat.Attributes[(int)i].Offset);
					if (vertexStream.VertexFormat.Attributes[(int)i].Divisor > 0)
						GL.vertexAttribDivisor(i, vertexStream.VertexFormat.Attributes[(int)i].Divisor);
					//Output.WriteLine($"id: {i}, Count: {vertexStream.VertexFormat.Attributes[(int)i].Count}, Type: {vertexStream.VertexFormat.Attributes[(int)i].Type}, Normalized: {vertexStream.VertexFormat.Attributes[(int)i].Normalized}, Size/Stride: {vertexStream.VertexFormat.Attributes[(int)i].Size}/{vertexStream.VertexFormat.Stride}, offset: {vertexStream.VertexFormat.Attributes[(int)i].Offset}, divisor: {vertexStream.VertexFormat.Attributes[(int)i].Divisor}"); // Debug
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