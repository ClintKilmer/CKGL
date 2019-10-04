using static Retyped.webgl2.WebGL2RenderingContext; // WebGL 2.0 Enums

namespace CKGL.WebGL2
{
	internal static class WebGL2CKGLConversions
	{
		#region State
		internal static double ToWebGL2(this BlendEquation blendEquation)
		{
			switch (blendEquation)
			{
				case BlendEquation.Add:
					return FUNC_ADD;
				case BlendEquation.Subtract:
					return FUNC_SUBTRACT;
				case BlendEquation.ReverseSubtract:
					return FUNC_REVERSE_SUBTRACT;
				case BlendEquation.Max:
					return WebGL2Graphics.MAX;
				case BlendEquation.Min:
					return WebGL2Graphics.MIN;
				default:
					throw new IllegalValueException(typeof(BlendEquation), blendEquation);
			}
		}

		internal static double ToWebGL2(this BlendFactor blendFactor)
		{
			switch (blendFactor)
			{
				case BlendFactor.Zero:
					return ZERO;
				case BlendFactor.One:
					return ONE;
				case BlendFactor.SrcColour:
					return SRC_COLOR;
				case BlendFactor.OneMinusSrcColour:
					return ONE_MINUS_SRC_COLOR;
				case BlendFactor.SrcAlpha:
					return SRC_ALPHA;
				case BlendFactor.OneMinusSrcAlpha:
					return ONE_MINUS_SRC_ALPHA;
				case BlendFactor.DstAlpha:
					return DST_ALPHA;
				case BlendFactor.OneMinusDstAlpha:
					return ONE_MINUS_DST_ALPHA;
				case BlendFactor.DstColour:
					return DST_COLOR;
				case BlendFactor.OneMinusDstcolour:
					return ONE_MINUS_DST_COLOR;
				case BlendFactor.SrcAlphaSaturate:
					return SRC_ALPHA_SATURATE;
				case BlendFactor.ConstantColour:
					return CONSTANT_COLOR;
				case BlendFactor.OneMinusConstantColour:
					return ONE_MINUS_CONSTANT_COLOR;
				case BlendFactor.ConstantAlpha:
					return CONSTANT_ALPHA;
				case BlendFactor.OneMinusConstantAlpha:
					return ONE_MINUS_CONSTANT_ALPHA;
				default:
					throw new IllegalValueException(typeof(BlendFactor), blendFactor);
			}
		}

		internal static double ToWebGL2(this Face face)
		{
			switch (face)
			{
				case Face.Front:
					return FRONT;
				case Face.Back:
					return BACK;
				case Face.FrontAndBack:
					return FRONT_AND_BACK;
				default:
					throw new IllegalValueException(typeof(Face), face);
			}
		}

		internal static double ToWebGL2(this DepthFunction depthFunction)
		{
			switch (depthFunction)
			{
				case DepthFunction.Never:
					return NEVER;
				case DepthFunction.Less:
					return LESS;
				case DepthFunction.Equal:
					return EQUAL;
				case DepthFunction.LessEqual:
					return LEQUAL;
				case DepthFunction.Greater:
					return GREATER;
				case DepthFunction.NotEqual:
					return NOTEQUAL;
				case DepthFunction.GreaterEqual:
					return GEQUAL;
				case DepthFunction.Always:
					return ALWAYS;
				default:
					throw new IllegalValueException(typeof(DepthFunction), depthFunction);
			}
		}

		internal static double ToWebGL2(this FrontFace frontFace)
		{
			switch (frontFace)
			{
				case FrontFace.Clockwise:
					return CW;
				case FrontFace.CounterClockwise:
					return CCW;
				default:
					throw new IllegalValueException(typeof(FrontFace), frontFace);
			}
		}

		internal static double ToWebGL2(this PolygonMode polygonMode)
		{
			switch (polygonMode)
			{
				//case PolygonMode.Fill:
				//	return FILL;
				//case PolygonMode.Line:
				//	return LINE;
				//case PolygonMode.Point:
				//	return POINT;
				//default:
				//	throw new IllegalValueException(typeof(PolygonMode), polygonMode);
				default:
					throw new CKGLException("glPolygonMode is not available in WebGL.");
			}
		}
		#endregion

		//internal static WebGLBindings.BlitFilter ToWebGL2(this BlitFilter blitFilter)
		//{
		//	switch (blitFilter)
		//	{
		//		case BlitFilter.Nearest:
		//			return WebGLBindings.BlitFilter.Nearest;
		//		case BlitFilter.Linear:
		//			return WebGLBindings.BlitFilter.Linear;
		//		default:
		//			throw new IllegalValueException(typeof(BlitFilter), blitFilter);
		//	}
		//}

		internal static double ToWebGL2(this BufferUsage bufferUsage)
		{
			switch (bufferUsage)
			{
				case BufferUsage.Static:
					return STATIC_DRAW;
				case BufferUsage.Default:
					return DYNAMIC_DRAW;
				case BufferUsage.Dynamic:
					return STREAM_DRAW;
				case BufferUsage.Copy:
					return DYNAMIC_READ_Static;
				default:
					throw new IllegalValueException(typeof(BufferUsage), bufferUsage);
			}
		}

		internal static double ToWebGL2(this DataType dataType)
		{
			switch (dataType)
			{
				case DataType.Byte:
					return BYTE;
				case DataType.UnsignedByte:
					return UNSIGNED_BYTE;
				case DataType.Short:
					return SHORT;
				case DataType.UnsignedShort:
					return UNSIGNED_SHORT;
				case DataType.Int:
					return INT;
				case DataType.UnsignedInt:
					return UNSIGNED_INT;
				case DataType.Float:
					return FLOAT;
				//case DataType.Double:
				//	return DOUBLE;
				case DataType.HalfFloat:
					return HALF_FLOAT_Static;
				case DataType.UnsignedInt_2_10_10_10_Rev:
					return UNSIGNED_INT_2_10_10_10_REV_Static;
				case DataType.Int_2_10_10_10_Rev:
					return INT_2_10_10_10_REV_Static;
				default:
					throw new IllegalValueException(typeof(DataType), dataType);
			}
		}

		internal static double ToWebGL2(this IndexType indexType)
		{
			switch (indexType)
			{
				//case IndexType.UnsignedByte:
				//	return UNSIGNED_BYTE;
				case IndexType.UnsignedShort:
					return UNSIGNED_SHORT;
				case IndexType.UnsignedInt:
					return UNSIGNED_INT;
				default:
					throw new IllegalValueException(typeof(IndexType), indexType);
			}
		}

		internal static double ToWebGL2(this PrimitiveTopology primitiveTopology)
		{
			switch (primitiveTopology)
			{
				case PrimitiveTopology.PointList:
					return POINTS;
				case PrimitiveTopology.LineList:
					return LINES;
				case PrimitiveTopology.LineLoop:
					return LINE_LOOP;
				case PrimitiveTopology.LineStrip:
					return LINE_STRIP;
				case PrimitiveTopology.TriangleList:
					return TRIANGLES;
				case PrimitiveTopology.TriangleStrip:
					return TRIANGLE_STRIP;
				case PrimitiveTopology.TriangleFan:
					return TRIANGLE_FAN;
				default:
					throw new IllegalValueException(typeof(PrimitiveTopology), primitiveTopology);
			}
		}

		//internal static WebGLBindings.TextureAttachment ToWebGL2(this TextureAttachment textureAttachment)
		//{
		//	switch (textureAttachment)
		//	{
		//		case TextureAttachment.Depth:
		//			return WebGLBindings.TextureAttachment.Depth;
		//		case TextureAttachment.DepthStencil:
		//			return WebGLBindings.TextureAttachment.DepthStencil;
		//		case TextureAttachment.Colour0:
		//			return WebGLBindings.TextureAttachment.Colour0;
		//		case TextureAttachment.Colour1:
		//			return WebGLBindings.TextureAttachment.Colour1;
		//		case TextureAttachment.Colour2:
		//			return WebGLBindings.TextureAttachment.Colour2;
		//		case TextureAttachment.Colour3:
		//			return WebGLBindings.TextureAttachment.Colour3;
		//		case TextureAttachment.Colour4:
		//			return WebGLBindings.TextureAttachment.Colour4;
		//		case TextureAttachment.Colour5:
		//			return WebGLBindings.TextureAttachment.Colour5;
		//		case TextureAttachment.Colour6:
		//			return WebGLBindings.TextureAttachment.Colour6;
		//		case TextureAttachment.Colour7:
		//			return WebGLBindings.TextureAttachment.Colour7;
		//		case TextureAttachment.Colour8:
		//			return WebGLBindings.TextureAttachment.Colour8;
		//		case TextureAttachment.Colour9:
		//			return WebGLBindings.TextureAttachment.Colour9;
		//		case TextureAttachment.Colour10:
		//			return WebGLBindings.TextureAttachment.Colour10;
		//		case TextureAttachment.Colour11:
		//			return WebGLBindings.TextureAttachment.Colour11;
		//		case TextureAttachment.Colour12:
		//			return WebGLBindings.TextureAttachment.Colour12;
		//		case TextureAttachment.Colour13:
		//			return WebGLBindings.TextureAttachment.Colour13;
		//		case TextureAttachment.Colour14:
		//			return WebGLBindings.TextureAttachment.Colour14;
		//		case TextureAttachment.Colour15:
		//			return WebGLBindings.TextureAttachment.Colour15;
		//		default:
		//			throw new IllegalValueException(typeof(TextureAttachment), textureAttachment);
		//	}
		//}

		internal static double ToWebGL2(this TextureFilter textureFilter)
		{
			switch (textureFilter)
			{
				case TextureFilter.Nearest:
					return NEAREST;
				case TextureFilter.Linear:
					return LINEAR;
				case TextureFilter.NearestMipmapNearest:
					return NEAREST_MIPMAP_NEAREST;
				case TextureFilter.LinearMipmapNearest:
					return LINEAR_MIPMAP_NEAREST;
				case TextureFilter.NearestMipmapLinear:
					return NEAREST_MIPMAP_LINEAR;
				case TextureFilter.LinearMipmapLinear:
					return LINEAR_MIPMAP_LINEAR;
				default:
					throw new IllegalValueException(typeof(TextureFilter), textureFilter);
			}
		}

		internal static double ToWebGL2(this TextureFormat textureFormat)
		{
			switch (textureFormat)
			{
				case TextureFormat.R8:
					return R8_Static;
				case TextureFormat.RG8:
					return RG8_Static;
				case TextureFormat.RGB8:
					return RGB8_Static;
				case TextureFormat.RGBA8:
					return RGBA8_Static;
				case TextureFormat.Depth16:
					return DEPTH_COMPONENT16;
				case TextureFormat.Depth24:
					return DEPTH_COMPONENT24_Static;
				case TextureFormat.Depth32F:
					return DEPTH_COMPONENT32F_Static;
				case TextureFormat.Depth24Stencil8:
					return DEPTH24_STENCIL8_Static;
				case TextureFormat.Depth32FStencil8:
					return DEPTH32F_STENCIL8_Static;
				default:
					throw new IllegalValueException(typeof(TextureFormat), textureFormat);
			}
		}
		#region TextureFormat Extensions
		internal static double ToWebGL2PixelFormat(this TextureFormat textureFormat)
		{
			switch (textureFormat)
			{
				case TextureFormat.R8:
					return RED_Static;
				case TextureFormat.RG8:
					return 0x8227; // GL_RG
				case TextureFormat.RGB8:
					return RGB;
				case TextureFormat.RGBA8:
					return RGBA;
				case TextureFormat.Depth16:
					return DEPTH_COMPONENT;
				case TextureFormat.Depth24:
					return DEPTH_COMPONENT;
				case TextureFormat.Depth32F:
					return DEPTH_COMPONENT;
				case TextureFormat.Depth24Stencil8:
					return DEPTH_STENCIL;
				case TextureFormat.Depth32FStencil8:
					return DEPTH_STENCIL;
				default:
					throw new IllegalValueException(typeof(TextureFormat), textureFormat);
			}
		}

		public static double ToWebGL2PixelType(this TextureFormat textureFormat)
		{
			switch (textureFormat)
			{
				case TextureFormat.R8:
					return UNSIGNED_BYTE;
				case TextureFormat.RG8:
					return UNSIGNED_BYTE;
				case TextureFormat.RGB8:
					return UNSIGNED_BYTE;
				case TextureFormat.RGBA8:
					return UNSIGNED_BYTE;
				case TextureFormat.Depth16:
					return UNSIGNED_SHORT;
				case TextureFormat.Depth24:
					return UNSIGNED_INT;
				case TextureFormat.Depth32F:
					return FLOAT;
				case TextureFormat.Depth24Stencil8:
					return UNSIGNED_INT_24_8_Static;
				case TextureFormat.Depth32FStencil8:
					return FLOAT_32_UNSIGNED_INT_24_8_REV_Static;
				default:
					throw new IllegalValueException(typeof(TextureFormat), textureFormat);
			}
		}

		//public static TextureAttachment TextureAttachment(this TextureFormat textureFormat)
		//{
		//	switch (textureFormat)
		//	{
		//		case TextureFormat.Depth16:
		//		case TextureFormat.Depth24:
		//		case TextureFormat.Depth32F:
		//			return OpenGLBindings.TextureAttachment.Depth;
		//		case TextureFormat.Depth24Stencil8:
		//		case TextureFormat.Depth32FStencil8:
		//			return OpenGLBindings.TextureAttachment.DepthStencil;
		//		default:
		//			throw new IllegalValueException(typeof(TextureFormat), textureFormat);
		//	}
		//}

		public static int Components(this TextureFormat textureFormat)
		{
			switch (textureFormat)
			{
				//case TextureFormat.R8:
				//	return 1;
				//case TextureFormat.RG8:
				//	return 2;
				case TextureFormat.RGB8:
					return 3;
				case TextureFormat.RGBA8:
					return 4;
				case TextureFormat.Depth16:
					return 1;
				case TextureFormat.Depth24:
					return 1;
				//case TextureFormat.Depth32F:
				//	return 1;
				case TextureFormat.Depth24Stencil8:
					return 2;
				//case TextureFormat.Depth32FStencil8::
				//	return 2;
				default:
					throw new IllegalValueException(typeof(TextureFormat), textureFormat);
			}
		}
		#endregion

		internal static double ToWebGL2(this TextureWrap textureWrap)
		{
			switch (textureWrap)
			{
				case TextureWrap.Clamp:
					return CLAMP_TO_EDGE;
				case TextureWrap.Repeat:
					return REPEAT;
				case TextureWrap.MirroredRepeat:
					return MIRRORED_REPEAT;
				default:
					throw new IllegalValueException(typeof(TextureWrap), textureWrap);
			}
		}
	}
}