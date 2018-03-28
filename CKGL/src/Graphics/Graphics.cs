using OpenGL;

using GLint = System.Int32;
using GLuint = System.UInt32;
//using GLfloat = float;

namespace CKGL
{
	public static class Graphics
	{
		public static void Init()
		{
			GL.Init();

			Platform.OnWinResized += () =>
			{
				GL.Viewport(0, 0, Window.Width, Window.Height);
			};
		}

		#region Clear
		public static void Clear(Colour colour, float depth)
		{
			GL.ClearColour(colour);
			GL.ClearDepth(depth);
			GL.Clear(BufferBit.Colour | BufferBit.Depth);
		}

		public static void Clear(Colour colour)
		{
			GL.ClearColour(colour);
			GL.Clear(BufferBit.Colour);
		}

		public static void Clear(float depth)
		{
			GL.ClearDepth(depth);
			GL.Clear(BufferBit.Depth);
		}
		#endregion

		#region Draw
		//public void DrawIndexedPrimitives(
		//	DrawMode drawMode,
		//	int baseVertex,
		//	int minVertexIndex,
		//	int numVertices,
		//	int startIndex,
		//	int primitiveCount
		//)
		//{
		//	ApplyState();

		//	// Set up the vertex buffers
		//	GLDevice.ApplyVertexAttributes(
		//		vertexBufferBindings,
		//		vertexBufferCount,
		//		vertexBuffersUpdated,
		//		baseVertex
		//	);
		//	vertexBuffersUpdated = false;

		//	GLDevice.DrawIndexedPrimitives(
		//		drawMode,
		//		baseVertex,
		//		minVertexIndex,
		//		numVertices,
		//		startIndex,
		//		primitiveCount,
		//		Indices
		//	);
		//}

		//public void DrawPrimitives(
		//	DrawMode drawMode,
		//	int vertexStart,
		//	int primitiveCount
		//)
		//{
		//	ApplyState();

		//	// Set up the vertex buffers
		//	GLDevice.ApplyVertexAttributes(
		//		vertexBufferBindings,
		//		vertexBufferCount,
		//		vertexBuffersUpdated,
		//		0
		//	);
		//	vertexBuffersUpdated = false;

		//	GLDevice.DrawPrimitives(
		//		drawMode,
		//		vertexStart,
		//		primitiveCount
		//	);
		//} 
		#endregion
	}
}