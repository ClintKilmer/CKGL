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
			public static bool BlendState = false;
			public static BlendMode BlendMode = default(BlendMode);
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
		public static void FrontFace(FrontFace frontFace)
		{
			GL.FrontFace(frontFace);
		}

		public static void CullMode(CullFace cullMode)
		{
			GL.CullFace(cullMode);
		}

		public static void BlendState(bool enabled)
		{
			if (enabled != State.BlendState)
			{
				if (enabled)
					GL.Enable(EnableCap.Blend);
				else
					GL.Disable(EnableCap.Blend);
			}
		}

		public static void BlendMode(BlendMode blendMode)
		{
			if (blendMode != State.BlendMode)
			{
				GL.BlendFunc(blendMode.Src, blendMode.Dst);
				GL.BlendEquation(blendMode.Eq);
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