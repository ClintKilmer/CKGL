using OpenGL;

using GLint = System.Int32;
using GLuint = System.UInt32;
//using GLfloat = float;

namespace CKGL
{
	public static class Graphics
	{
		#region State
		private class State
		{
			public static FrontFace FrontFaceState = FrontFace.CounterClockwise; // OpenGL Default
			public static CullState CullState = CullState.Off; // OpenGL Default
			public static BlendState BlendState = BlendState.None; // OpenGL Default
		}
		#endregion

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

		#region State
		public static FrontFace FrontFaceState
		{
			get
			{
				return State.FrontFaceState;
			}
			set
			{
				if (value != State.FrontFaceState)
				{
					GL.FrontFace(value);
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
					GL.BlendFunc(value.Src, value.Dst);
					GL.BlendEquation(value.Eq);
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