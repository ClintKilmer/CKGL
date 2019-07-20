using CKGL.OpenGLBindings;
using GLint = System.Int32;
using GLuint = System.UInt32;

namespace CKGL.OpenGL
{
	public class OpenGLGeometryInput : GeometryInput
	{
		private readonly VertexArray vao;

		internal OpenGLGeometryInput(IndexBuffer indexBuffer, VertexStream[] vertexStreams)
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

				GLint offset = 0;
				for (GLuint i = 0; i < vertexStream.VertexFormat.Attributes.Length; i++)
				{
					GL.EnableVertexAttribArray(i);
					GL.VertexAttribPointer(i, vertexStream.VertexFormat.Attributes[(int)i].Count, vertexStream.VertexFormat.Attributes[(int)i].Type.ToOpenGL(), vertexStream.VertexFormat.Attributes[(int)i].Normalized, vertexStream.VertexFormat.Stride, offset);
					//Output.WriteLine($"id: {i}, Count: {vertexStream.VertexFormat.Attributes[(int)i].Count}, Type: {vertexStream.VertexFormat.Attributes[(int)i].Type}, Normalized: {vertexStream.VertexFormat.Attributes[(int)i].Normalized}, Size/Stride: {vertexStream.VertexFormat.Attributes[(int)i].Size}/{vertexStream.VertexFormat.Stride}, offset: {offset}"); // Debug
					offset += vertexStream.VertexFormat.Attributes[(int)i].Size;
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