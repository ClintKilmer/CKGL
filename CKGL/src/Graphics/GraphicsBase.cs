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
		public abstract void SetDepthRange(double near, double far);
		#endregion

		#region Clear
		internal abstract void Clear(Colour colour);
		internal abstract void Clear(double depth);
		internal abstract void Clear(Colour colour, double depth);
		#endregion

		#region State Setters
		internal abstract void SetFrontFace(FrontFaceState frontFaceState);
		internal abstract void SetCullMode(CullModeState cullModeState);
		internal abstract void SetPolygonMode(PolygonModeState polygonModeState);
		internal abstract void SetColourMask(ColourMaskState colourMaskState);
		internal abstract void SetDepthMask(DepthMaskState depthMaskState);
		internal abstract void SetDepth(DepthState depthState);
		internal abstract void SetBlend(BlendState blendState);
		#endregion
	}
}