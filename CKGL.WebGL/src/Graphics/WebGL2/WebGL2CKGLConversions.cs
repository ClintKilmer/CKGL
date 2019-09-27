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

		//internal static WebGLBindings.TextureFilter ToWebGL2(this TextureFilter textureFilter)
		//{
		//	switch (textureFilter)
		//	{
		//		case TextureFilter.Nearest:
		//			return WebGLBindings.TextureFilter.Nearest;
		//		case TextureFilter.Linear:
		//			return WebGLBindings.TextureFilter.Linear;
		//		case TextureFilter.NearestMipmapNearest:
		//			return WebGLBindings.TextureFilter.NearestMipmapNearest;
		//		case TextureFilter.LinearMipmapNearest:
		//			return WebGLBindings.TextureFilter.LinearMipmapNearest;
		//		case TextureFilter.NearestMipmapLinear:
		//			return WebGLBindings.TextureFilter.NearestMipmapLinear;
		//		case TextureFilter.LinearMipmapLinear:
		//			return WebGLBindings.TextureFilter.LinearMipmapLinear;
		//		default:
		//			throw new IllegalValueException(typeof(TextureFilter), textureFilter);
		//	}
		//}

		//internal static WebGLBindings.TextureFormat ToWebGL2(this TextureFormat textureFormat)
		//{
		//	switch (textureFormat)
		//	{
		//		case TextureFormat.R8:
		//			return WebGLBindings.TextureFormat.R8;
		//		case TextureFormat.RG8:
		//			return WebGLBindings.TextureFormat.RG8;
		//		case TextureFormat.RGB8:
		//			return WebGLBindings.TextureFormat.RGB8;
		//		case TextureFormat.RGBA8:
		//			return WebGLBindings.TextureFormat.RGBA8;
		//		case TextureFormat.Depth16:
		//			return WebGLBindings.TextureFormat.Depth16;
		//		case TextureFormat.Depth24:
		//			return WebGLBindings.TextureFormat.Depth24;
		//		case TextureFormat.Depth32F:
		//			return WebGLBindings.TextureFormat.Depth32F;
		//		case TextureFormat.Depth24Stencil8:
		//			return WebGLBindings.TextureFormat.Depth24Stencil8;
		//		case TextureFormat.Depth32FStencil8:
		//			return WebGLBindings.TextureFormat.Depth32FStencil8;
		//		default:
		//			throw new IllegalValueException(typeof(TextureFormat), textureFormat);
		//	}
		//}

		//internal static WebGLBindings.TextureWrap ToWebGL2(this TextureWrap textureWrap)
		//{
		//	switch (textureWrap)
		//	{
		//		case TextureWrap.Clamp:
		//			return WebGLBindings.TextureWrap.Clamp;
		//		case TextureWrap.Repeat:
		//			return WebGLBindings.TextureWrap.Repeat;
		//		case TextureWrap.MirroredRepeat:
		//			return WebGLBindings.TextureWrap.MirroredRepeat;
		//		default:
		//			throw new IllegalValueException(typeof(TextureWrap), textureWrap);
		//	}
		//}
	}
}