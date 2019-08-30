namespace CKGL
{
	internal abstract class GraphicsBase
	{
		internal abstract void Init();

		#region Resources
		internal abstract VertexBuffer CreateVertexBuffer(BufferUsage bufferUsage);
		internal abstract IndexBuffer CreateIndexBuffer(BufferUsage bufferUsage);
		internal abstract GeometryInput CreateGeometryInput(IndexBuffer indexBuffer, VertexStream[] vertexStreams);
		#endregion

#if !WEBGL // Temporary
		#region Viewport
		internal abstract void SetViewport();
		internal abstract void SetViewport(RenderTarget renderTarget);
		internal abstract void SetViewport(int x, int y, int width, int height);
		#endregion

		#region ScissorTest
		internal abstract void SetScissorTest();
		internal abstract void SetScissorTest(RenderTarget renderTarget);
		internal abstract void SetScissorTest(int x, int y, int width, int height);
		internal abstract void SetScissorTest(bool enabled);
		#endregion

		#region DepthRange
		internal abstract void SetDepthRange(double near, double far);
		#endregion
#endif // Temporary

		#region Clear
		internal abstract void Clear(Colour colour);
		internal abstract void Clear(double depth);
		internal abstract void Clear(Colour colour, double depth);
		#endregion

#if !WEBGL // Temporary
		#region State Setters
		internal abstract void SetFrontFace(FrontFaceState frontFaceState);
		internal abstract void SetCullMode(CullModeState cullModeState);
		internal abstract void SetPolygonMode(PolygonModeState polygonModeState);
		internal abstract void SetColourMask(ColourMaskState colourMaskState);
		internal abstract void SetDepthMask(DepthMaskState depthMaskState);
		internal abstract void SetDepth(DepthState depthState);
		internal abstract void SetBlend(BlendState blendState);
		#endregion

		#region Draw
		internal abstract void DrawVertexArrays(PrimitiveTopology primitiveTopology, int offset, int count);
		internal abstract void DrawIndexedVertexArrays(PrimitiveTopology primitiveTopology, int offset, int count, IndexType indexType);
		internal abstract void DrawVertexArraysInstanced(PrimitiveTopology primitiveTopology, int offset, int count, int primitiveCount);
		internal abstract void DrawIndexedVertexArraysInstanced(PrimitiveTopology primitiveTopology, int offset, int count, int primitiveCount, IndexType indexType);
		#endregion
#endif // Temporary
	}
}