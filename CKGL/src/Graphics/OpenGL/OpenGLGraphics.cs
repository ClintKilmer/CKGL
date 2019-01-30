using CKGL.OpenGLBindings;

namespace CKGL.OpenGL
{
	internal class OpenGLGraphics : GraphicsBase
	{
		internal override void Init()
		{
			GL.Init();
		}

		#region Viewport
		public override void SetViewport() => SetViewport(RenderTarget.Current);
		public override void SetViewport(RenderTarget renderTarget)
			=> SetViewport(0, 0, (renderTarget ?? RenderTarget.Default).Width, (renderTarget ?? RenderTarget.Default).Height);
		public override void SetViewport(int x, int y, int width, int height)
			=> GL.Viewport(x, y, width, height);
		#endregion

		#region ScissorTest
		public override void SetScissorTest() => SetScissorTest(RenderTarget.Current);
		public override void SetScissorTest(RenderTarget renderTarget)
			=> SetScissorTest(0, 0, (renderTarget ?? RenderTarget.Default).Width, (renderTarget ?? RenderTarget.Default).Height);
		public override void SetScissorTest(int x, int y, int width, int height)
		{
			GL.Enable(EnableCap.ScissorTest);
			GL.Scissor(x, y, width, height);
		}

		public override void SetScissorTest(bool enabled)
		{
			if (enabled)
				GL.Enable(EnableCap.ScissorTest);
			else
				GL.Disable(EnableCap.ScissorTest);
		}
		#endregion

		#region DepthRange
		public override void SetDepthRange(float near, float far)
		{
			GL.DepthRange(near.Clamp(0f, 1f), far.Clamp(0f, 1f));
		}
		#endregion

		#region State Setters
		internal override void SetFrontFace(FrontFace frontFace)
		{
			GL.FrontFace(frontFace.ToOpenGL());
		}

		internal override void SetCullMode(bool enabled, Face face)
		{
			if (enabled)
				GL.Enable(EnableCap.CullFace);
			else
				GL.Disable(EnableCap.CullFace);
			GL.CullFace(face.ToOpenGL());
		}

		internal override void SetPolygonMode(PolygonMode polygonMode)
		{
			GL.PolygonMode(polygonMode.ToOpenGL());
		}
		#endregion
	}
}