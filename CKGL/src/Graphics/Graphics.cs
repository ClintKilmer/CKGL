﻿using OpenGL;

using GLint = System.Int32;
using GLuint = System.UInt32;

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

		// TODO - ScissorState
		#region State
		private class State
		{
			public static FrontFaceState FrontFaceState = FrontFaceState.Default;
			public static CullState CullState = CullState.Default;
			public static BlendState BlendState = BlendState.Default;
			public static DepthState DepthState = DepthState.Default;
		}

		public static FrontFaceState FrontFaceState
		{
			get
			{
				return State.FrontFaceState;
			}
			set
			{
				if (value != State.FrontFaceState)
				{
					GL.FrontFace(value.FrontFace);
				}
			}
		}

		public static CullState CullState
		{
			get
			{
				return State.CullState;
			}
			set
			{
				if (value != State.CullState)
				{
					if (value.On)
						GL.Enable(EnableCap.CullFace);
					else
						GL.Disable(EnableCap.CullFace);
					GL.CullFace(value.CullFace);
				}
			}
		}

		public static BlendState BlendState
		{
			get
			{
				return State.BlendState;
			}
			set
			{
				if (value != State.BlendState)
				{
					if (value.On)
						GL.Enable(EnableCap.Blend);
					else
						GL.Disable(EnableCap.Blend);
					GL.BlendFuncSeparate(value.ColourSource, value.ColourDestination, value.AlphaSource, value.AlphaDestination);
					GL.BlendEquationSeparate(value.ColourEquation, value.AlphaEquation);
				}
			}
		}

		public static DepthState DepthState
		{
			get
			{
				return State.DepthState;
			}
			set
			{
				if (value != State.DepthState)
				{
					if (value.On)
						GL.Enable(EnableCap.DepthTest);
					else
						GL.Disable(EnableCap.DepthTest);
					GL.DepthFunc(value.DepthFunc);
				}
			}
		}
		#endregion

		#region Draw
		public static void DrawVertexArrays(DrawMode drawMode, GLint offset, GLint count)
		{
			GL.DrawArrays(drawMode, offset, count);
		}

		public static void DrawIndexedVertexArrays(DrawMode drawMode, GLint offset, GLint count)
		{
			GL.DrawElements(drawMode, count, IndexType.UnsignedInt, offset);
		}
		#endregion
	}
}