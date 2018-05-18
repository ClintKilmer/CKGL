using System;
using OpenGL;
using GLint = System.Int32;

namespace CKGL
{
	public static class Graphics
	{
		public static int DrawCalls { get; private set; }

		public static void Init()
		{
			GL.Init();
			State.Init();

			Platform.Events.OnWinResized += () =>
			{
				SetViewport();
				SetScissorTest();
			};

			// Debug
			Output.WriteLine($"Graphics Initialized");
		}

		public static void PreDraw()
		{
			DrawCalls = 0;
			State.PreDraw();
		}

		#region Viewport
		public static void SetViewport() => SetViewport(RenderTarget.Current);
		public static void SetViewport(RenderTarget renderTarget)
		{
			SetViewport(0, 0, (renderTarget ?? RenderTarget.Default).Width, (renderTarget ?? RenderTarget.Default).Height);
		}

		public static void SetViewport(GLint x, GLint y, GLint width, GLint height)
		{
			GL.Viewport(x, y, width, height);
		}
		#endregion

		#region ScissorTest
		public static void SetScissorTest() => SetScissorTest(RenderTarget.Current);
		public static void SetScissorTest(RenderTarget renderTarget)
		{
			SetScissorTest(0, 0, (renderTarget ?? RenderTarget.Default).Width, (renderTarget ?? RenderTarget.Default).Height);
		}

		public static void SetScissorTest(GLint x, GLint y, GLint width, GLint height)
		{
			GL.Enable(EnableCap.ScissorTest);
			GL.Scissor(x, y, width, height);
		}

		public static void SetScissorTest(bool enabled)
		{
			if (enabled)
				GL.Enable(EnableCap.ScissorTest);
			else
				GL.Disable(EnableCap.ScissorTest);
		}
		#endregion

		#region Clear
		public static void SetClearColour(Colour colour)
		{
			GL.ClearColour(colour);
		}
		public static void SetClearDepth(float depth)
		{
			GL.ClearDepth(depth);
		}

		public static void ClearColour()
		{
			GL.Clear(BufferBit.Colour);
		}
		public static void ClearColour(Colour colour)
		{
			GL.ClearColour(colour);
			GL.Clear(BufferBit.Colour);
		}

		public static void ClearDepth()
		{
			GL.Clear(BufferBit.Depth);
		}
		public static void ClearDepth(float depth)
		{
			GL.ClearDepth(depth);
			GL.Clear(BufferBit.Depth);
		}

		public static void Clear()
		{
			GL.Clear(BufferBit.Colour | BufferBit.Depth);
		}

		public static void Clear(Colour colour)
		{
			GL.ClearColour(colour);
			GL.Clear(BufferBit.Colour | BufferBit.Depth);
		}
		public static void Clear(float depth)
		{
			GL.ClearDepth(depth);
			GL.Clear(BufferBit.Colour | BufferBit.Depth);
		}
		public static void Clear(Colour colour, float depth)
		{
			GL.ClearColour(colour);
			GL.ClearDepth(depth);
			GL.Clear(BufferBit.Colour | BufferBit.Depth);
		}
		#endregion

		#region State
		public static class State
		{
			public static Action OnStateChanging;
			public static Action OnStateChanged;

			public static int Changes { get; private set; }

			public static void Init()
			{
				FrontFaceState.OnStateChanging += () => { OnStateChanging?.Invoke(); };
				FrontFaceState.OnStateChanged += () => { OnStateChanged?.Invoke(); };
				CullState.OnStateChanging += () => { OnStateChanging?.Invoke(); };
				CullState.OnStateChanged += () => { OnStateChanged?.Invoke(); };
				PolygonModeState.OnStateChanging += () => { OnStateChanging?.Invoke(); };
				PolygonModeState.OnStateChanged += () => { OnStateChanged?.Invoke(); };
				BlendState.OnStateChanging += () => { OnStateChanging?.Invoke(); };
				BlendState.OnStateChanged += () => { OnStateChanged?.Invoke(); };
				DepthState.OnStateChanging += () => { OnStateChanging?.Invoke(); };
				DepthState.OnStateChanged += () => { OnStateChanged?.Invoke(); };

				Shader.OnBinding += () => { OnStateChanging?.Invoke(); };
				Shader.OnBound += () => { OnStateChanged?.Invoke(); };
				Shader.OnUniformChanging += () => { OnStateChanging?.Invoke(); };
				Shader.OnUniformChanged += () => { OnStateChanged?.Invoke(); };
				RenderTarget.OnBinding += () => { OnStateChanging?.Invoke(); };
				RenderTarget.OnBound += () => { OnStateChanged?.Invoke(); };
				Texture.OnBinding += () => { OnStateChanging?.Invoke(); };
				Texture.OnBound += () => { OnStateChanged?.Invoke(); };

				OnStateChanged += () => { Changes++; };
			}

			public static void Reset()
			{
				FrontFaceState.Reset();
				CullState.Reset();
				PolygonModeState.Reset();
				BlendState.Reset();
				DepthState.Reset();
			}

			public static void PreDraw()
			{
				Changes = 0;
			}
		}
		#endregion

		#region Draw
		public static void DrawVertexArrays(DrawMode drawMode, GLint offset, GLint count)
		{
			GL.DrawArrays(drawMode, offset, count);
			DrawCalls++;
		}

		public static void DrawIndexedVertexArrays(DrawMode drawMode, GLint offset, GLint count)
		{
			GL.DrawElements(drawMode, count, IndexType.UnsignedInt, offset);
			DrawCalls++;
		}
		#endregion
	}
}