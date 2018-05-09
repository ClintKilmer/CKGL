using System;

using OpenGL;

using GLint = System.Int32;
using GLuint = System.UInt32;

namespace CKGL
{
	public static class Graphics
	{
		public static GLuint DrawCalls { get; private set; }

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

			public static GLuint Changes { get; private set; }

			public static FrontFaceState FrontFaceState { get; private set; }
			public static CullState CullState { get; private set; }
			public static PolygonModeState PolygonModeState { get; private set; }
			public static BlendState BlendState { get; private set; }
			public static DepthState DepthState { get; private set; }

			public static void Init()
			{
				OnStateChanged += () => { Changes++; };

				ResetFrontFaceState();
				ResetCullState();
				ResetPolygonModeState();
				ResetBlendState();
				ResetDepthState();

				Shader.OnBinding += () => { OnStateChanging?.Invoke(); };
				RenderTarget.OnBinding += () => { OnStateChanging?.Invoke(); };
				Texture.OnBinding += () => { OnStateChanging?.Invoke(); };

				Shader.OnBound += () => { OnStateChanged?.Invoke(); };
				RenderTarget.OnBound += () => { OnStateChanged?.Invoke(); };
				Texture.OnBound += () => { OnStateChanged?.Invoke(); };
			}

			public static void PreDraw()
			{
				Changes = 0;
			}

			#region FrontFaceState
			public static void SetFrontFaceState(FrontFaceState frontFaceState)
			{
				if (FrontFaceState != frontFaceState)
				{
					OnStateChanging?.Invoke();
					GL.FrontFace(frontFaceState.FrontFace);
					FrontFaceState = frontFaceState;
					OnStateChanged?.Invoke();
				}
			}
			public static void ResetFrontFaceState()
			{
				SetFrontFaceState(FrontFaceState.Default);
			}
			#endregion

			#region CullState
			public static void SetCullState(CullState cullState)
			{
				if (CullState != cullState)
				{
					OnStateChanging?.Invoke();
					if (cullState.On)
						GL.Enable(EnableCap.CullFace);
					else
						GL.Disable(EnableCap.CullFace);
					GL.CullFace(cullState.Face);
					CullState = cullState;
					OnStateChanged?.Invoke();
				}
			}
			public static void ResetCullState()
			{
				SetCullState(CullState.Default);
			}
			#endregion

			#region PolygonModeState
			public static void SetPolygonModeState(PolygonModeState polygonModeState)
			{
				if (PolygonModeState != polygonModeState)
				{
					OnStateChanging?.Invoke();
					if (polygonModeState.FrontAndBack)
					{
						GL.PolygonMode(Face.FrontAndBack, polygonModeState.BackPolygonMode);
					}
					else
					{
						GL.PolygonMode(Face.Front, polygonModeState.FrontPolygonMode);
						GL.PolygonMode(Face.Back, polygonModeState.BackPolygonMode);
					}
					PolygonModeState = polygonModeState;
					OnStateChanged?.Invoke();
				}
			}
			public static void ResetPolygonModeState()
			{
				SetPolygonModeState(PolygonModeState.Default);
			}
			#endregion

			#region BlendState
			public static void SetBlendState(BlendState blendState)
			{
				if (BlendState != blendState)
				{
					OnStateChanging?.Invoke();
					if (blendState.On)
						GL.Enable(EnableCap.Blend);
					else
						GL.Disable(EnableCap.Blend);
					GL.BlendFuncSeparate(blendState.ColourSource, blendState.ColourDestination, blendState.AlphaSource, blendState.AlphaDestination);
					GL.BlendEquationSeparate(blendState.ColourEquation, blendState.AlphaEquation);
					BlendState = blendState;
					OnStateChanged?.Invoke();
				}
			}
			public static void ResetBlendState()
			{
				SetBlendState(BlendState.Off);
			}
			#endregion

			#region DepthState
			public static void SetDepthState(DepthState depthState)
			{
				if (DepthState != depthState)
				{
					OnStateChanging?.Invoke();
					if (depthState.On)
						GL.Enable(EnableCap.DepthTest);
					else
						GL.Disable(EnableCap.DepthTest);
					GL.DepthFunc(depthState.DepthFunc);
					DepthState = depthState;
					OnStateChanged?.Invoke();
				}
			}
			public static void ResetDepthState()
			{
				SetDepthState(DepthState.Default);
			}
			#endregion
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