using System;
using System.Runtime.InteropServices;

namespace CKGL.OpenGL
{
	internal class OpenGLRenderer : RendererBase
	{
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		internal struct Vertex
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

			internal Vertex(Vector3 position, Colour? colour, UV? uv)
				: this(position, colour ?? Colour.White, uv ?? UV.Zero, uv != null) { }
		}

		private VertexBuffer vertexBuffer;
		private VertexFormat vertexFormat;
		private GeometryInput geometryInput;
		private PrimitiveTopology currentPrimitiveTopology = PrimitiveTopology.TriangleList;
		// Adaptive vertex buffer size
		//private int bufferSize = 16384; // Performant on Laptop
		private int bufferSize = 1024;
		private readonly int maxBufferSize = OpenGLBindings.GL.MaxElementVertices; // TODO - Move into Graphics.Init
																				   //private const int bufferSize = 1998; // Divisible by 3 and 2 for no vertex wrapping per batch
		private Vertex[] vertices;
		private int vertexCount = 0;

		internal override void Init()
		{
			vertexBuffer = VertexBuffer.Create(BufferUsage.Dynamic);
			vertexFormat = new VertexFormat(
				Marshal.SizeOf(typeof(Vertex)),                       // Dynamic Stride - For larger StructLayout Pack sizes
				new VertexAttribute(DataType.Float, 3, false),        // Position
				new VertexAttribute(DataType.UnsignedByte, 4, true),  // Colour
				new VertexAttribute(DataType.UnsignedShort, 2, true), // UV
				new VertexAttribute(DataType.UnsignedByte, 1, true)   // Textured
			);
			geometryInput = GeometryInput.Create(new VertexStream(vertexBuffer, vertexFormat));

			vertices = new Vertex[bufferSize];

			Graphics.State.OnStateChanging += () => { Flush(); };

			// Debug
			Output.WriteLine($"Renderer Initialized");
		}

		internal override void Destroy()
		{
			geometryInput.Destroy();
			vertexBuffer.Destroy();

			geometryInput = null;
			vertexBuffer = null;
		}

		internal override void Flush()
		{
			if (
				(currentPrimitiveTopology == PrimitiveTopology.TriangleList && vertexCount >= 3) ||
				(currentPrimitiveTopology == PrimitiveTopology.TriangleStrip && vertexCount >= 3) ||
				(currentPrimitiveTopology == PrimitiveTopology.TriangleFan && vertexCount >= 3) ||
				(currentPrimitiveTopology == PrimitiveTopology.LineList && vertexCount >= 2) ||
				(currentPrimitiveTopology == PrimitiveTopology.LineStrip && vertexCount >= 2) ||
				(currentPrimitiveTopology == PrimitiveTopology.LineLoop && vertexCount >= 2) ||
				(currentPrimitiveTopology == PrimitiveTopology.PointList && vertexCount >= 1)
			)
			{
				geometryInput.Bind();
				vertexBuffer.LoadData(ref vertices);
				Graphics.DrawVertexArrays(currentPrimitiveTopology, 0, vertexCount);
			}

			// Reset vertexCount so we don't lose any vertex data
			int remainder = 0;
			switch (currentPrimitiveTopology)
			{
				case (PrimitiveTopology.TriangleList):
					remainder = vertexCount % 3;
					break;
				case (PrimitiveTopology.LineList):
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

		internal override void AddVertex(PrimitiveTopology type, Vector3 position, Colour? colour, UV? uv)
		{
			if (currentPrimitiveTopology != type)
			{
				Flush();
				currentPrimitiveTopology = type;
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

			vertices[vertexCount] = new Vertex(position, colour, uv);
			vertexCount++;
		}
	}
}