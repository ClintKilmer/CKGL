using static Retyped.dom; // DOM / WebGL Types
using WebGL_EXT = Retyped.dom.Literals; // WebGL Extensions

namespace CKGL.WebGL
{
	internal class WebGLGraphics : GraphicsBase
	{
		#region Extensions
		internal static class Extensions
		{
			private static WEBGL_debug_renderer_info _WEBGL_debug_renderer_info;
			internal static WEBGL_debug_renderer_info WEBGL_debug_renderer_info
			{
				get
				{
					if (_WEBGL_debug_renderer_info == null)
					{
						_WEBGL_debug_renderer_info = GL.getExtension(WebGL_EXT.WEBGL_debug_renderer_info);
						Output.WriteLine("WebGL 1.0 Extension \"WEBGL_debug_renderer_info\" acquired.");

						if (_WEBGL_debug_renderer_info == null)
							throw new CKGLException("WebGL 1.0 \"WEBGL_debug_renderer_info\" extension was requested, but is not supported in this browser.");
					}

					return _WEBGL_debug_renderer_info;
				}
			}

			private static OES_vertex_array_object _OES_vertex_array_object;
			internal static OES_vertex_array_object OES_vertex_array_object
			{
				get
				{
					if (_OES_vertex_array_object == null)
					{
						_OES_vertex_array_object = GL.getExtension(WebGL_EXT.OES_vertex_array_object);
						Output.WriteLine("WebGL 1.0 Extension \"OES_vertex_array_object\" acquired.");

						if (_OES_vertex_array_object == null)
							throw new CKGLException("WebGL 1.0 \"OES_vertex_array_object\" extension was requested, but is not supported in this browser.");
					}

					return _OES_vertex_array_object;
				}
			}

			private static EXT_blend_minmax _EXT_blend_minmax;
			internal static EXT_blend_minmax EXT_blend_minmax
			{
				get
				{
					if (_EXT_blend_minmax == null)
					{
						_EXT_blend_minmax = GL.getExtension(WebGL_EXT.EXT_blend_minmax);
						Output.WriteLine("WebGL 1.0 Extension \"EXT_blend_minmax\" acquired.");

						if (_EXT_blend_minmax == null)
							throw new CKGLException("WebGL 1.0 \"EXT_blend_minmax\" extension was requested, but is not supported in this browser.");
					}

					return _EXT_blend_minmax;
				}
			}

			private static ANGLE_instanced_arrays _ANGLE_instanced_arrays;
			internal static ANGLE_instanced_arrays ANGLE_instanced_arrays
			{
				get
				{
					if (_ANGLE_instanced_arrays == null)
					{
						_ANGLE_instanced_arrays = GL.getExtension(WebGL_EXT.ANGLE_instanced_arrays);
						Output.WriteLine("WebGL 1.0 Extension \"ANGLE_instanced_arrays\" acquired.");

						if (_ANGLE_instanced_arrays == null)
							throw new CKGLException("WebGL 1.0 \"ANGLE_instanced_arrays\" extension was requested, but is not supported in this browser.");
					}

					return _ANGLE_instanced_arrays;
				}
			}

			private static WEBGL_depth_texture _WEBGL_depth_texture;
			internal static WEBGL_depth_texture WEBGL_depth_texture
			{
				get
				{
					if (_WEBGL_depth_texture == null)
					{
						_WEBGL_depth_texture = GL.getExtension(WebGL_EXT.WEBGL_depth_texture);
						Output.WriteLine("WebGL 1.0 Extension \"WEBGL_depth_texture\" acquired.");

						if (_WEBGL_depth_texture == null)
							throw new CKGLException("WebGL 1.0 \"WEBGL_depth_texture\" extension was requested, but is not supported in this browser.");
					}

					return _WEBGL_depth_texture;
				}
			}

			private static OES_element_index_uint _OES_element_index_uint;
			internal static OES_element_index_uint OES_element_index_uint
			{
				get
				{
					if (_OES_element_index_uint == null)
					{
						_OES_element_index_uint = GL.getExtension(WebGL_EXT.OES_element_index_uint);
						Output.WriteLine("WebGL 1.0 Extension \"OES_element_index_uint\" acquired.");

						if (_OES_element_index_uint == null)
							throw new CKGLException("WebGL 1.0 \"OES_element_index_uint\" extension was requested, but is not supported in this browser.");
					}

					return _OES_element_index_uint;
				}
			}

			// Currently widely unsupported
			//private static WEBGL_draw_buffers _WEBGL_draw_buffers;
			//internal static WEBGL_draw_buffers WEBGL_draw_buffers
			//{
			//	get
			//	{
			//		if (_WEBGL_draw_buffers == null)
			//		{
			//			_WEBGL_draw_buffers = GL.getExtension(WebGL_EXT.WEBGL_draw_buffers);
			//			Output.WriteLine("WebGL 1.0 Extension \"WEBGL_draw_buffers\" acquired.");

			//			if (_WEBGL_draw_buffers == null)
			//				throw new CKGLException("WebGL 1.0 \"WEBGL_draw_buffers\" extension was requested, but is not supported in this browser.");
			//		}

			//		return _WEBGL_draw_buffers;
			//	}
			//}
		}
		#endregion

		internal static WebGLRenderingContext GL = null;

		internal static bool IsLittleEndian = new Retyped.es5.Uint8Array(new Retyped.es5.Uint32Array(new uint[] { 0x12345678 }).buffer)[0] == 0x78;

		internal override void Init()
		{
			GL = Platform.WebGLContext;

			if (GL == null)
				throw new CKGLException("Couldn't create WebGL 1.0 context");

			// Debug
			Output.WriteLine($"GraphicsBackend - WebGL 1.0 Initialized");
			var attributes = GL.getContextAttributes();
			Output.WriteLine($"WebGL Context - Attributes: alpha[{attributes.alpha}] premultipliedAlpha[{attributes.premultipliedAlpha}] depth[{attributes.depth}] stencil[{attributes.stencil}] antialias[{attributes.antialias}] preserveDrawingBuffer[{attributes.preserveDrawingBuffer}] failIfMajorPerformanceCaveat[{attributes.failIfMajorPerformanceCaveat}]");
			Output.WriteLine($"WebGL Context - GLSL Version: {GL.getParameter(GL.SHADING_LANGUAGE_VERSION)}");
			Output.WriteLine($"WebGL Context - VERSION: {GL.getParameter(GL.VERSION)}");
			Output.WriteLine($"WebGL Context - VENDOR: {GL.getParameter(GL.VENDOR)}");
			Output.WriteLine($"WebGL Context - RENDERER: {GL.getParameter(GL.RENDERER)}");
			try
			{
				Output.WriteLine($"WebGL Context - WEBGL_debug_renderer_info.UNMASKED_VENDOR_WEBGL: {GL.getParameter(Extensions.WEBGL_debug_renderer_info.UNMASKED_VENDOR_WEBGL)}");
				Output.WriteLine($"WebGL Context - WEBGL_debug_renderer_info.UNMASKED_RENDERER_WEBGL: {GL.getParameter(Extensions.WEBGL_debug_renderer_info.UNMASKED_RENDERER_WEBGL)}");
			}
			catch { }
			Output.WriteLine($"WebGL Context - MaxColourAttachments: 1 (WebGL 1.0 restriction)");
			Output.WriteLine($"WebGL Context - MaxCubeMapTextureSize: {GL.getParameter(GL.MAX_CUBE_MAP_TEXTURE_SIZE)}");
			Output.WriteLine($"WebGL Context - MaxDrawBuffers: 1 (WebGL 1.0 restriction)");
			Output.WriteLine($"WebGL Context - MaxElementIndices: undefined");
			Output.WriteLine($"WebGL Context - MaxElementVertices: undefined");
			Output.WriteLine($"WebGL Context - MaxRenderbufferSize: {GL.getParameter(GL.MAX_RENDERBUFFER_SIZE)}");
			Output.WriteLine($"WebGL Context - MaxSamples: undefined");
			Output.WriteLine($"WebGL Context - MaxTextureImageUnits: {GL.getParameter(GL.MAX_TEXTURE_IMAGE_UNITS)}");
			Output.WriteLine($"WebGL Context - MaxTextureSize: {GL.getParameter(GL.MAX_TEXTURE_SIZE)}");
			//Output.WriteLine($"WebGL - Extensions: \n{string.Join("\n", GL.GetSupportedExtensions())}");
		}

		#region Resources
		internal override VertexBuffer CreateVertexBuffer(BufferUsage bufferUsage)
		{
			return new WebGLVertexBuffer(bufferUsage);
		}

		internal override IndexBuffer CreateIndexBuffer(BufferUsage bufferUsage)
		{
			return new WebGLIndexBuffer(bufferUsage);
		}

		internal override GeometryInput CreateGeometryInput(IndexBuffer indexBuffer, VertexStream[] vertexStreams)
		{
			return new WebGLGeometryInput(indexBuffer, vertexStreams);
		}

		internal override Texture CreateTexture2D(byte[] data, int width, int height, TextureFormat textureFormat, TextureFilter minFilter, TextureFilter magFilter, TextureWrap wrapX, TextureWrap wrapY)
		{
			return new WebGLTexture(data, TextureType.Texture2D, width, height, 1, textureFormat, minFilter, magFilter, wrapX, wrapY);
		}

		internal override Texture CreateTexture2DFromFile(string file, TextureFormat textureFormat, TextureFilter minFilter, TextureFilter magFilter, TextureWrap wrapX, TextureWrap wrapY)
		{
			return new WebGLTexture(file, TextureType.Texture2D, textureFormat, minFilter, magFilter, wrapX, wrapY);
		}

		internal override Framebuffer CreateDefaultFramebuffer()
		{
			return WebGLFramebuffer.CreateDefault();
		}

		internal override Framebuffer CreateFramebuffer(int width, int height, int colourTextures, TextureFormat textureColourFormat, TextureFormat? textureDepthFormat = null)
		{
			return new WebGLFramebuffer(width, height, colourTextures, textureColourFormat, textureDepthFormat);
		}

		internal override Shader CreateShader(string source)
		{
			return new WebGLShader(source);
		}

		internal override Shader CreateShaderFromFile(string file)
		{
			throw new CKGLException("WebGL doesn't support creating shaders from file.");
		}
		#endregion

		#region Viewport
		internal override void SetViewport() => SetViewport(Framebuffer.Current);
		internal override void SetViewport(Framebuffer framebuffer)
			=> SetViewport(0, 0, (framebuffer ?? Framebuffer.Default).Width, (framebuffer ?? Framebuffer.Default).Height);
		internal override void SetViewport(int x, int y, int width, int height)
			=> GL.viewport(x, y, width, height);
		#endregion

		#region ScissorTest
		internal override void SetScissorTest() => SetScissorTest(Framebuffer.Current);
		internal override void SetScissorTest(Framebuffer framebuffer)
			=> SetScissorTest(0, 0, (framebuffer ?? Framebuffer.Default).Width, (framebuffer ?? Framebuffer.Default).Height);
		internal override void SetScissorTest(int x, int y, int width, int height)
		{
			GL.enable(GL.SCISSOR_TEST);
			GL.scissor(x, y, width, height);
		}

		internal override void SetScissorTest(bool enabled)
		{
			if (enabled)
				GL.enable(GL.SCISSOR_TEST);
			else
				GL.disable(GL.SCISSOR_TEST);
		}
		#endregion

		#region DepthRange
		internal override void SetDepthRange(double near, double far)
		{
			GL.depthRange(near.Clamp(0d, 1d), far.Clamp(0d, 1d));
		}
		#endregion

		#region Clear
		internal override void Clear(Colour colour)
		{
			SetClearColour(colour);
			GL.clear((int)GL.COLOR_BUFFER_BIT | (int)GL.DEPTH_BUFFER_BIT);
		}

		internal override void Clear(double depth)
		{
			SetClearDepth(depth);
			GL.clear(GL.DEPTH_BUFFER_BIT);
		}

		internal override void Clear(Colour colour, double depth)
		{
			SetClearColour(colour);
			SetClearDepth(depth);
			GL.clear((int)GL.COLOR_BUFFER_BIT | (int)GL.DEPTH_BUFFER_BIT);
		}

		private Colour clearColour = new Colour(0, 0, 0, 0);
		private void SetClearColour(Colour colour)
		{
			if (clearColour != colour)
			{
				GL.clearColor(colour.R, colour.G, colour.B, colour.A);
				clearColour = colour;
			}
		}

		private double clearDepth = 1d;
		private void SetClearDepth(double depth)
		{
			if (clearDepth != depth)
			{
				GL.clearDepth(depth);
				clearDepth = depth;
			}
		}
		#endregion

		#region State Setters
		internal override void SetFrontFace(FrontFaceState frontFaceState)
		{
			GL.frontFace(frontFaceState.FrontFace.ToWebGL());
		}

		internal override void SetCullMode(CullModeState cullModeState)
		{
			if (cullModeState.Enabled)
				GL.enable(GL.CULL_FACE);
			else
				GL.disable(GL.CULL_FACE);

			GL.cullFace(cullModeState.Face.ToWebGL());
		}

		internal override void SetPolygonMode(PolygonModeState polygonModeState)
		{
			//GL.polygonMode(polygonModeState.PolygonMode.ToWebGL());
			throw new CKGLException("glPolygonMode is not available in WebGL.");
		}

		internal override void SetColourMask(ColourMaskState colourMaskState)
		{
			GL.colorMask(colourMaskState.R, colourMaskState.G, colourMaskState.B, colourMaskState.A);
		}

		internal override void SetDepthMask(DepthMaskState depthMaskState)
		{
			GL.depthMask(depthMaskState.Depth);
		}

		internal override void SetDepth(DepthState depthState)
		{
			if (depthState.Enabled)
				GL.enable(GL.DEPTH_TEST);
			else
				GL.disable(GL.DEPTH_TEST);

			GL.depthFunc(depthState.DepthFunction.ToWebGL());
		}

		internal override void SetBlend(BlendState blendState)
		{
			if (blendState.Enabled)
				GL.enable(GL.BLEND);
			else
				GL.disable(GL.BLEND);

			if (blendState.ColourSource == blendState.AlphaSource && blendState.ColourDestination == blendState.AlphaDestination)
				GL.blendFunc(blendState.ColourSource.ToWebGL(), blendState.ColourDestination.ToWebGL());
			else
				GL.blendFuncSeparate(blendState.ColourSource.ToWebGL(), blendState.ColourDestination.ToWebGL(), blendState.AlphaSource.ToWebGL(), blendState.AlphaDestination.ToWebGL());

			if (blendState.ColourEquation == blendState.AlphaEquation)
				GL.blendEquation(blendState.ColourEquation.ToWebGL());
			else
				GL.blendEquationSeparate(blendState.ColourEquation.ToWebGL(), blendState.AlphaEquation.ToWebGL());
		}
		#endregion

		#region Draw
		internal override void DrawVertexArrays(PrimitiveTopology primitiveTopology, int offset, int count)
		{
			GL.drawArrays(primitiveTopology.ToWebGL(), offset, count);
		}

		internal override void DrawIndexedVertexArrays(PrimitiveTopology primitiveTopology, int offset, int count, IndexType indexType)
		{
			GL.drawElements(primitiveTopology.ToWebGL(), count, indexType.ToWebGL(), offset);
		}

		internal override void DrawVertexArraysInstanced(PrimitiveTopology primitiveTopology, int offset, int count, int primitiveCount)
		{
			Extensions.ANGLE_instanced_arrays.drawArraysInstancedANGLE(primitiveTopology.ToWebGL(), offset, count, primitiveCount);
		}

		internal override void DrawIndexedVertexArraysInstanced(PrimitiveTopology primitiveTopology, int offset, int count, int primitiveCount, IndexType indexType)
		{
			Extensions.ANGLE_instanced_arrays.drawElementsInstancedANGLE(primitiveTopology.ToWebGL(), count, indexType.ToWebGL(), offset, primitiveCount);
		}
		#endregion
	}
}