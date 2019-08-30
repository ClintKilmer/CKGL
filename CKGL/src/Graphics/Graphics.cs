using System;

namespace CKGL
{
	public static class Graphics
	{
		private static GraphicsBase graphics;

		internal static void Init()
		{
			switch (Platform.GraphicsBackend)
			{
#if VULKAN
				case GraphicsBackend.Vulkan:
					throw new NotImplementedException("Vulkan Graphics Backend not implemented yet.");
#endif
#if OPENGL
				case GraphicsBackend.OpenGL:
#endif
#if OPENGLES
				case GraphicsBackend.OpenGLES:
#endif
#if OPENGL || OPENGLES
					graphics = new OpenGL.OpenGLGraphics();
					break;
#endif
#if WEBGL
				case GraphicsBackend.WebGL:
					graphics = new WebGL.WebGLGraphics();
					break;
#endif
				default:
					throw new NotSupportedException($"GraphicsBackend {Platform.GraphicsBackend} not supported.");
			}

			graphics.Init();

#if !WEBGL // Temporary
			State.Init();

			Platform.Events.OnWinResized += () =>
			{
				SetViewport();
				SetScissorTest();
			};
#endif // Temporary

			// Debug
			Output.WriteLine($"Graphics Initialized");
		}

		public static int DrawCalls { get; private set; }

		public static void PreDraw()
		{
			DrawCalls = 0;
#if !WEBGL // Temporary
			State.PreDraw();
#endif // Temporary
		}

		#region Resources
		internal static VertexBuffer CreateVertexBuffer(BufferUsage bufferUsage) => graphics.CreateVertexBuffer(bufferUsage);
		internal static IndexBuffer CreateIndexBuffer(BufferUsage bufferUsage) => graphics.CreateIndexBuffer(bufferUsage);
		internal static GeometryInput CreateGeometryInput(IndexBuffer indexBuffer, VertexStream[] vertexStreams) => graphics.CreateGeometryInput(indexBuffer, vertexStreams);
#if !WEBGL
		internal static Shader CreateShader(string source) => graphics.CreateShader(source);
		internal static Shader CreateShaderFromFile(string file) => graphics.CreateShaderFromFile(file); 
#endif
		#endregion

#if !WEBGL // Temporary
		// TODO - Move SetViewport() to own state
		#region Viewport
		public static void SetViewport() => graphics.SetViewport();
		public static void SetViewport(RenderTarget renderTarget) => graphics.SetViewport(renderTarget);
		public static void SetViewport(int x, int y, int width, int height) => graphics.SetViewport(x, y, width, height);
		#endregion

		// TODO - Move SetScissorTest() to own state
		#region ScissorTest
		public static void SetScissorTest() => graphics.SetScissorTest();
		public static void SetScissorTest(RenderTarget renderTarget) => graphics.SetScissorTest(renderTarget);
		public static void SetScissorTest(int x, int y, int width, int height) => graphics.SetScissorTest(x, y, width, height);
		public static void SetScissorTest(bool enabled) => graphics.SetScissorTest(enabled);
		#endregion

		// TODO - Move SetDepthRange() to own state
		#region DepthRange
		public static void SetDepthRange(double near, double far) => graphics.SetDepthRange(near, far);
		#endregion
#endif // Temporary

		#region Clear
		public static void Clear(Colour colour) => graphics.Clear(colour);
		public static void Clear(double depth) => graphics.Clear(depth);
		public static void Clear(Colour colour, double depth) => graphics.Clear(colour, depth);
		#endregion

#if !WEBGL // Temporary
		#region State Setters
		internal static void SetFrontFace(FrontFaceState frontFaceState) => graphics.SetFrontFace(frontFaceState);
		internal static void SetCullMode(CullModeState cullModeState) => graphics.SetCullMode(cullModeState);
		internal static void SetPolygonMode(PolygonModeState polygonModeState) => graphics.SetPolygonMode(polygonModeState);
		internal static void SetColourMask(ColourMaskState colourMaskState) => graphics.SetColourMask(colourMaskState);
		internal static void SetDepthMask(DepthMaskState depthMaskState) => graphics.SetDepthMask(depthMaskState);
		internal static void SetDepth(DepthState depthState) => graphics.SetDepth(depthState);
		internal static void SetBlend(BlendState blendState) => graphics.SetBlend(blendState);
		#endregion

		#region State
		public static class State
		{
			internal static Action OnStateChanging;
			internal static Action OnStateChanged;

			public static int Changes { get; private set; }

			internal static void Init()
			{
				Reset();

				OnStateChanged += () => { Changes++; };
			}

			public static void Reset()
			{
				FrontFaceState.Reset();
				CullModeState.Reset();
				PolygonModeState.Reset();
				BlendState.Reset();
				DepthState.Reset();
				ColourMaskState.Reset();
				DepthMaskState.Reset();
			}

			internal static void PreDraw()
			{
				Changes = 0;
			}
		}
		#endregion
#endif

		#region Draw
		public static void DrawVertexArrays(PrimitiveTopology primitiveTopology, int offset, int count)
		{
			graphics.DrawVertexArrays(primitiveTopology, offset, count);
			DrawCalls++;
		}

		public static void DrawIndexedVertexArrays(PrimitiveTopology primitiveTopology, int offset, int count, IndexType indexType)
		{
			graphics.DrawIndexedVertexArrays(primitiveTopology, offset, count, indexType);
			DrawCalls++;
		}

		public static void DrawVertexArraysInstanced(PrimitiveTopology primitiveTopology, int offset, int count, int primitiveCount)
		{
			graphics.DrawVertexArraysInstanced(primitiveTopology, offset, count, primitiveCount);
			DrawCalls++;
		}

		public static void DrawIndexedVertexArraysInstanced(PrimitiveTopology primitiveTopology, int offset, int count, int primitiveCount, IndexType indexType)
		{
			graphics.DrawIndexedVertexArraysInstanced(primitiveTopology, offset, count, primitiveCount, indexType);
			DrawCalls++;
		}
		#endregion
	}
}