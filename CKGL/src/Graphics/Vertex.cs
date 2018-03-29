using OpenGL;

using System.Runtime.InteropServices;

namespace CKGL
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct Vertex
	{
		public Vector3 Position;
		public Colour Colour;
		public Vector2 UV;
		public float Textured;

		public static int FloatStride = 10;

		public static void SetAttributes()
		{
			GL.EnableVertexAttribArray(0);
			GL.EnableVertexAttribArray(1);
			GL.EnableVertexAttribArray(2);
			GL.EnableVertexAttribArray(3);
			GL.VertexAttribPointer(0, 3, VertexType.Float, false, FloatStride * sizeof(float), 0);
			GL.VertexAttribPointer(1, 4, VertexType.Float, false, FloatStride * sizeof(float), 3 * sizeof(float));
			GL.VertexAttribPointer(2, 2, VertexType.Float, false, FloatStride * sizeof(float), 4 * sizeof(float));
			GL.VertexAttribPointer(3, 1, VertexType.Float, false, FloatStride * sizeof(float), 2 * sizeof(float));
		}

		public static float[] GetVBO(Vertex[] vertices)
		{
			float[] vbo = new float[FloatStride * vertices.Length];

			for (int i = 0; i < vertices.Length; i++)
			{
				vbo[i * FloatStride + 0] = vertices[i].Position.X;
				vbo[i * FloatStride + 1] = vertices[i].Position.Y;
				vbo[i * FloatStride + 2] = vertices[i].Position.Z;
				vbo[i * FloatStride + 3] = vertices[i].Colour.R;
				vbo[i * FloatStride + 4] = vertices[i].Colour.G;
				vbo[i * FloatStride + 5] = vertices[i].Colour.B;
				vbo[i * FloatStride + 6] = vertices[i].Colour.A;
			}

			return vbo;
		}
	}
}