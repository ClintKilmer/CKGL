using System;
using System.Runtime.InteropServices;

namespace CKGL.OpenGL
{
	internal class OpenGLRenderer : RendererBase
	{
		[StructLayout(LayoutKind.Sequential, Pack = 4)]
		internal struct Vertex : IVertex
		{
			internal Vector3 Position;
			internal Colour Colour;
			internal UV UV;
			internal byte Textured;

			internal Vertex(Vector3 position, Colour colour, UV uv, bool textured)
			{
				Position = position;
				Colour = colour;
				UV = uv;
				Textured = (byte)(textured ? 255 : 0);
			}

			internal readonly static VertexAttributeLayout AttributeLayout = new VertexAttributeLayout
			(
				Marshal.SizeOf(typeof(Vertex)),                         // Dynamic Stride - For larger StructLayout Pack sizes
				new VertexAttribute(VertexType.Float, 3, false),        // Position
				new VertexAttribute(VertexType.UnsignedByte, 4, true),  // Colour
				new VertexAttribute(VertexType.UnsignedShort, 2, true), // UV
				new VertexAttribute(VertexType.UnsignedByte, 1, true)   // Textured
			);

			VertexAttributeLayout IVertex.AttributeLayout
			{
				get { return AttributeLayout; }
			}
		}

		private VertexArray vao;
		private VertexBuffer vbo;
		private DrawMode currentDrawMode = DrawMode.TriangleList;
		// Adaptive vertex buffer size
		//private int bufferSize = 16384; // Performant on Laptop
		private int bufferSize = 1024;
		private readonly int maxBufferSize = OpenGLBindings.GL.MaxElementVertices; // TODO - Move into Graphics.Init
		//private const int bufferSize = 1998; // Divisible by 3 and 2 for no vertex wrapping per batch
		private Vertex[] vertices;
		private int vertexCount = 0;

		internal override void Init()
		{
			vao = new VertexArray();
			vbo = new VertexBuffer();
			vao.AddBuffer(vbo, Vertex.AttributeLayout);

			vertices = new Vertex[bufferSize];

			Graphics.State.OnStateChanging += () => { Flush(); };

			// Debug
			Output.WriteLine($"Renderer Initialized");
		}

		internal override void Destroy()
		{
			vao.Destroy();
			vbo.Destroy();

			vao = null;
			vbo = null;
		}

		public override void Flush()
		{
			if (
				(currentDrawMode == DrawMode.TriangleList && vertexCount >= 3) ||
				(currentDrawMode == DrawMode.TriangleStrip && vertexCount >= 3) ||
				(currentDrawMode == DrawMode.TriangleFan && vertexCount >= 3) ||
				(currentDrawMode == DrawMode.LineList && vertexCount >= 2) ||
				(currentDrawMode == DrawMode.LineStrip && vertexCount >= 2) ||
				(currentDrawMode == DrawMode.LineLoop && vertexCount >= 2) ||
				(currentDrawMode == DrawMode.PointList && vertexCount >= 1)
			)
			{
				vao.Bind();
				vbo.LoadData(Vertex.AttributeLayout, ref vertices, vertexCount, BufferUsage.DynamicDraw);
				Graphics.DrawVertexArrays(currentDrawMode, 0, vertexCount);
			}

			// Reset vertexCount so we don't lose any vertex data
			int remainder = 0;
			switch (currentDrawMode)
			{
				case (DrawMode.TriangleList):
					remainder = vertexCount % 3;
					break;
				case (DrawMode.LineList):
					remainder = vertexCount % 2;
					break;
				default:
					remainder = 0;
					break;
			}
			for (int i = 0; i < remainder; i++)
			{
				vertices[i] = vertices[vertexCount - remainder + i];
			}
			vertexCount = remainder;
		}

		internal override void AddVertex(DrawMode type, Vector3 position, Colour? colour, UV? uv)
		{
			if (currentDrawMode != type)
			{
				Flush();
				currentDrawMode = type;
				// We can lose vertices here, but it's ok as we're switching primitive types anyways
				vertexCount = 0;
			}

			if (vertexCount >= bufferSize)
			{
				Flush();

				// Adaptive vertex buffer size
				if (bufferSize < maxBufferSize)
				{
					bufferSize *= 2;
					if (bufferSize > maxBufferSize)
						bufferSize = maxBufferSize;

					Array.Resize(ref vertices, bufferSize);

					Output.WriteLine($"Renderer - VertexBuffer size: {bufferSize:n0}");
				}
			}

			vertices[vertexCount] = new Vertex(position, colour ?? Colour.White, uv ?? UV.Zero, uv != null);
			vertexCount++;
		}
	}
}