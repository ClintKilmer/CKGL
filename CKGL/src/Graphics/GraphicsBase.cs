namespace CKGL
{
	internal abstract class GraphicsBase
	{
		internal abstract void Init();

		#region Viewport
		public abstract void SetViewport();
		public abstract void SetViewport(RenderTarget renderTarget);
		public abstract void SetViewport(int x, int y, int width, int height);
		#endregion

		#region ScissorTest
		public abstract void SetScissorTest();
		public abstract void SetScissorTest(RenderTarget renderTarget);
		public abstract void SetScissorTest(int x, int y, int width, int height);
		public abstract void SetScissorTest(bool enabled);
		#endregion

		#region DepthRange
		public abstract void SetDepthRange(float near, float far);
		#endregion

		#region State Setters
		internal abstract void SetFrontFace(FrontFace frontFace);
		internal abstract void SetCullMode(bool enabled, Face face);
		internal abstract void SetPolygonMode(PolygonMode polygonMode);
		#endregion
	}
}