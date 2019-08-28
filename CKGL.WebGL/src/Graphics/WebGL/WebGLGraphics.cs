using System;
using Bridge.Html5;
using Bridge.WebGL;

namespace CKGL.WebGL
{
	internal class WebGLGraphics : GraphicsBase
	{
		internal static WebGLRenderingContext GL = null;

		internal enum WEBGL_debug_renderer_info
		{
			UNMASKED_VENDOR_WEBGL = 0x9245,
			UNMASKED_RENDERER_WEBGL = 0x9246
		}

		internal override void Init()
		{// Create 3D Context
			string[] names = new string[] {
				"webgl2",
				"experimental-webgl2",
				"webgl",
				"experimental-webgl",
				//"webkit-3d",
				//"moz-webgl"
			};

			foreach (string name in names)
			{
				try
				{
					GL = Platform.Canvas.GetContext(name).As<WebGLRenderingContext>();
					//gl = (WebGLRenderingContext)Platform.Canvas.GetContext(name);
				}
				catch { }

				if (GL != null)
					break;
			}

			if (GL == null)
				Platform.Canvas.ParentElement.ReplaceChild(new HTMLParagraphElement { InnerHTML = "<b>Either the browser doesn't support WebGL or it is disabled.<br>Please follow <a href=\"http://get.webgl.com\">Get WebGL</a>.</b>" }, Platform.Canvas);

			// Debug
			Output.WriteLine($"Platform - HTML5 Initialized");
			Output.WriteLine($"Window.Navigator - Platform: {Window.Navigator.Platform}");
			Output.WriteLine($"Window.Navigator - UserAgent: {Window.Navigator.UserAgent}");
			Output.WriteLine($"WebGL Context - GLSL Version: {GL.GetParameter(GL.SHADING_LANGUAGE_VERSION)}");
			Output.WriteLine($"WebGL Context - VERSION: {GL.GetParameter(GL.VERSION)}");
			Output.WriteLine($"WebGL Context - VENDOR: {GL.GetParameter(GL.VENDOR)}");
			Output.WriteLine($"WebGL Context - RENDERER: {GL.GetParameter(GL.RENDERER)}");
			var dbgRenderInfo = GL.GetExtension("WEBGL_debug_renderer_info");
			if (dbgRenderInfo != null)
			{
				Output.WriteLine($"WebGL Context - WEBGL_debug_renderer_info.UNMASKED_VENDOR_WEBGL: {GL.GetParameter(WEBGL_debug_renderer_info.UNMASKED_VENDOR_WEBGL)}");
				Output.WriteLine($"WebGL Context - WEBGL_debug_renderer_info.UNMASKED_RENDERER_WEBGL: {GL.GetParameter(WEBGL_debug_renderer_info.UNMASKED_RENDERER_WEBGL)}");
			}
			//Output.WriteLine($"WebGL - Extensions: \n{string.Join("\n", gl.GetSupportedExtensions())}");
		}

#if false
		#region Resources
		internal override VertexBuffer CreateVertexBuffer(BufferUsage bufferUsage)
		{
			return new OpenGLVertexBuffer(bufferUsage);
		}

		internal override IndexBuffer CreateIndexBuffer(BufferUsage bufferUsage)
		{
			return new OpenGLIndexBuffer(bufferUsage);
		}

		internal override GeometryInput CreateGeometryInput(IndexBuffer indexBuffer, VertexStream[] vertexStreams)
		{
			return new OpenGLGeometryInput(indexBuffer, vertexStreams);
		}
		#endregion

		#region Viewport
		internal override void SetViewport() => SetViewport(RenderTarget.Current);
		internal override void SetViewport(RenderTarget renderTarget)
			=> SetViewport(0, 0, (renderTarget ?? RenderTarget.Default).Width, (renderTarget ?? RenderTarget.Default).Height);
		internal override void SetViewport(int x, int y, int width, int height)
			=> GL.Viewport(x, y, width, height);
		#endregion

		#region ScissorTest
		internal override void SetScissorTest() => SetScissorTest(RenderTarget.Current);
		internal override void SetScissorTest(RenderTarget renderTarget)
			=> SetScissorTest(0, 0, (renderTarget ?? RenderTarget.Default).Width, (renderTarget ?? RenderTarget.Default).Height);
		internal override void SetScissorTest(int x, int y, int width, int height)
		{
			GL.Enable(EnableCap.ScissorTest);
			GL.Scissor(x, y, width, height);
		}

		internal override void SetScissorTest(bool enabled)
		{
			if (enabled)
				GL.Enable(EnableCap.ScissorTest);
			else
				GL.Disable(EnableCap.ScissorTest);
		}
		#endregion

		#region DepthRange
		internal override void SetDepthRange(double near, double far)
		{
			GL.DepthRange(near.Clamp(0d, 1d), far.Clamp(0d, 1d));
		}
		#endregion
#endif

		#region Clear
		internal override void Clear(Colour colour)
		{
			SetClearColour(colour);
			GL.Clear(GL.COLOR_BUFFER_BIT | GL.DEPTH_BUFFER_BIT);
		}

		internal override void Clear(double depth)
		{
			SetClearDepth(depth);
			GL.Clear(GL.DEPTH_BUFFER_BIT);
		}

		internal override void Clear(Colour colour, double depth)
		{
			SetClearColour(colour);
			SetClearDepth(depth);
			GL.Clear(GL.COLOR_BUFFER_BIT | GL.DEPTH_BUFFER_BIT);
		}

		private Colour clearColour = new Colour(0, 0, 0, 0);
		private void SetClearColour(Colour colour)
		{
			if (clearColour != colour)
			{
				GL.ClearColor(colour.R, colour.G, colour.B, colour.A);
				clearColour = colour;
			}
		}

		private double clearDepth = 1d;
		private void SetClearDepth(double depth)
		{
			if (clearDepth != depth)
			{
				GL.ClearDepth(depth);
				clearDepth = depth;
			}
		}
		#endregion

#if false
		#region State Setters
		internal override void SetFrontFace(FrontFaceState frontFaceState)
		{
			GL.FrontFace(frontFaceState.FrontFace.ToOpenGL());
		}

		internal override void SetCullMode(CullModeState cullModeState)
		{
			if (cullModeState.Enabled)
				GL.Enable(EnableCap.CullFace);
			else
				GL.Disable(EnableCap.CullFace);

			GL.CullFace(cullModeState.Face.ToOpenGL());
		}

		internal override void SetPolygonMode(PolygonModeState polygonModeState)
		{
			GL.PolygonMode(polygonModeState.PolygonMode.ToOpenGL());
		}

		internal override void SetColourMask(ColourMaskState colourMaskState)
		{
			GL.ColourMask(colourMaskState.R, colourMaskState.G, colourMaskState.B, colourMaskState.A);
		}

		internal override void SetDepthMask(DepthMaskState depthMaskState)
		{
			GL.DepthMask(depthMaskState.Depth);
		}

		internal override void SetDepth(DepthState depthState)
		{
			if (depthState.Enabled)
				GL.Enable(EnableCap.DepthTest);
			else
				GL.Disable(EnableCap.DepthTest);

			GL.DepthFunc(depthState.DepthFunction.ToOpenGL());
		}

		internal override void SetBlend(BlendState blendState)
		{
			if (blendState.Enabled)
				GL.Enable(EnableCap.Blend);
			else
				GL.Disable(EnableCap.Blend);

			if (blendState.ColourSource == blendState.AlphaSource && blendState.ColourDestination == blendState.AlphaDestination)
				GL.BlendFunc(blendState.ColourSource.ToOpenGL(), blendState.ColourDestination.ToOpenGL());
			else
				GL.BlendFuncSeparate(blendState.ColourSource.ToOpenGL(), blendState.ColourDestination.ToOpenGL(), blendState.AlphaSource.ToOpenGL(), blendState.AlphaDestination.ToOpenGL());

			if (blendState.ColourEquation == blendState.AlphaEquation)
				GL.BlendEquation(blendState.ColourEquation.ToOpenGL());
			else
				GL.BlendEquationSeparate(blendState.ColourEquation.ToOpenGL(), blendState.AlphaEquation.ToOpenGL());
		}
		#endregion

		#region Draw
		internal override void DrawVertexArrays(PrimitiveTopology primitiveTopology, int offset, int count)
		{
			GL.DrawArrays(primitiveTopology.ToOpenGL(), offset, count);
		}

		internal override void DrawIndexedVertexArrays(PrimitiveTopology primitiveTopology, int offset, int count, IndexType indexType)
		{
			GL.DrawElements(primitiveTopology.ToOpenGL(), count, indexType.ToOpenGL(), offset);
		}

		internal override void DrawVertexArraysInstanced(PrimitiveTopology primitiveTopology, int offset, int count, int primitiveCount)
		{
			GL.DrawArraysInstanced(primitiveTopology.ToOpenGL(), offset, count, primitiveCount);
		}

		internal override void DrawIndexedVertexArraysInstanced(PrimitiveTopology primitiveTopology, int offset, int count, int primitiveCount, IndexType indexType)
		{
			GL.DrawElementsInstanced(primitiveTopology.ToOpenGL(), count, indexType.ToOpenGL(), offset, primitiveCount);
		}
		#endregion
#endif
	}
}