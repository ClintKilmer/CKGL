using static CKGL.WebGL.WebGLGraphics; // WebGL Context Methods

namespace CKGL.WebGL
{
	internal static class WebGLCKGLConversions
	{
		#region State
		internal static double ToWebGL(this BlendEquation blendEquation)
		{
			switch (blendEquation)
			{
				case BlendEquation.Add:
					return GL.FUNC_ADD;
				case BlendEquation.Subtract:
					return GL.FUNC_SUBTRACT;
				case BlendEquation.ReverseSubtract:
					return GL.FUNC_REVERSE_SUBTRACT;
				case BlendEquation.Max:
					return Extensions.EXT_blend_minmax.MAX_EXT;
				case BlendEquation.Min:
					return Extensions.EXT_blend_minmax.MIN_EXT;
				default:
					throw new IllegalValueException(typeof(BlendEquation), blendEquation);
			}
		}

		internal static double ToWebGL(this BlendFactor blendFactor)
		{
			switch (blendFactor)
			{
				case BlendFactor.Zero:
					return GL.ZERO;
				case BlendFactor.One:
					return GL.ONE;
				case BlendFactor.SrcColour:
					return GL.SRC_COLOR;
				case BlendFactor.OneMinusSrcColour:
					return GL.ONE_MINUS_SRC_COLOR;
				case BlendFactor.SrcAlpha:
					return GL.SRC_ALPHA;
				case BlendFactor.OneMinusSrcAlpha:
					return GL.ONE_MINUS_SRC_ALPHA;
				case BlendFactor.DstAlpha:
					return GL.DST_ALPHA;
				case BlendFactor.OneMinusDstAlpha:
					return GL.ONE_MINUS_DST_ALPHA;
				case BlendFactor.DstColour:
					return GL.DST_COLOR;
				case BlendFactor.OneMinusDstcolour:
					return GL.ONE_MINUS_DST_COLOR;
				case BlendFactor.SrcAlphaSaturate:
					return GL.SRC_ALPHA_SATURATE;
				case BlendFactor.ConstantColour:
					return GL.CONSTANT_COLOR;
				case BlendFactor.OneMinusConstantColour:
					return GL.ONE_MINUS_CONSTANT_COLOR;
				case BlendFactor.ConstantAlpha:
					return GL.CONSTANT_ALPHA;
				case BlendFactor.OneMinusConstantAlpha:
					return GL.ONE_MINUS_CONSTANT_ALPHA;
				default:
					throw new IllegalValueException(typeof(BlendFactor), blendFactor);
			}
		}

		internal static double ToWebGL(this Face face)
		{
			switch (face)
			{
				case Face.Front:
					return GL.FRONT;
				case Face.Back:
					return GL.BACK;
				case Face.FrontAndBack:
					return GL.FRONT_AND_BACK;
				default:
					throw new IllegalValueException(typeof(Face), face);
			}
		}

		internal static double ToWebGL(this DepthFunction depthFunction)
		{
			switch (depthFunction)
			{
				case DepthFunction.Never:
					return GL.NEVER;
				case DepthFunction.Less:
					return GL.LESS;
				case DepthFunction.Equal:
					return GL.EQUAL;
				case DepthFunction.LessEqual:
					return GL.LEQUAL;
				case DepthFunction.Greater:
					return GL.GREATER;
				case DepthFunction.NotEqual:
					return GL.NOTEQUAL;
				case DepthFunction.GreaterEqual:
					return GL.GEQUAL;
				case DepthFunction.Always:
					return GL.ALWAYS;
				default:
					throw new IllegalValueException(typeof(DepthFunction), depthFunction);
			}
		}

		internal static double ToWebGL(this FrontFace frontFace)
		{
			switch (frontFace)
			{
				case FrontFace.Clockwise:
					return GL.CW;
				case FrontFace.CounterClockwise:
					return GL.CCW;
				default:
					throw new IllegalValueException(typeof(FrontFace), frontFace);
			}
		}

		internal static double ToWebGL(this PolygonMode polygonMode)
		{
			switch (polygonMode)
			{
				//case PolygonMode.Fill:
				//	return GL.FILL;
				//case PolygonMode.Line:
				//	return GL.LINE;
				//case PolygonMode.Point:
				//	return GL.POINT;
				//default:
				//	throw new IllegalValueException(typeof(PolygonMode), polygonMode);
				default:
					throw new CKGLException("glPolygonMode is not available in WebGL.");
			}
		}
		#endregion

		//internal static WebGLBindings.BlitFilter ToWebGL(this BlitFilter blitFilter)
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

		internal static double ToWebGL(this BufferUsage bufferUsage)
		{
			switch (bufferUsage)
			{
				case BufferUsage.Static:
					return GL.STATIC_DRAW;
				case BufferUsage.Default:
					return GL.DYNAMIC_DRAW;
				case BufferUsage.Dynamic:
					return GL.STREAM_DRAW;
				//case BufferUsage.Copy:
				//	return GL.DYNAMIC_READ;
				default:
					throw new IllegalValueException(typeof(BufferUsage), bufferUsage);
			}
		}

		internal static double ToWebGL(this DataType dataType)
		{
			switch (dataType)
			{
				case DataType.Byte:
					return GL.BYTE;
				case DataType.UnsignedByte:
					return GL.UNSIGNED_BYTE;
				case DataType.Short:
					return GL.SHORT;
				case DataType.UnsignedShort:
					return GL.UNSIGNED_SHORT;
				case DataType.Int:
					return GL.INT;
				case DataType.UnsignedInt:
					return GL.UNSIGNED_INT;
				case DataType.Float:
					return GL.FLOAT;
				//case DataType.Double:
				//	return GL.DOUBLE;
				//case DataType.HalfFloat:
				//	return GL.HALF_FLOAT;
				//case DataType.UnsignedInt_2_10_10_10_Rev:
				//	return GL.UNSIGNED_INT_2_10_10_10_REV;
				//case DataType.Int_2_10_10_10_Rev:
				//	return GL.INT_2_10_10_10_REV;
				default:
					throw new IllegalValueException(typeof(DataType), dataType);
			}
		}

		internal static double ToWebGL(this IndexType indexType)
		{
			switch (indexType)
			{
				//case IndexType.UnsignedByte:
				//	return GL.UNSIGNED_BYTE;
				case IndexType.UnsignedShort:
					return GL.UNSIGNED_SHORT;
				case IndexType.UnsignedInt when Extensions.OES_element_index_uint != null:
					return GL.UNSIGNED_INT;
				default:
					throw new IllegalValueException(typeof(IndexType), indexType);
			}
		}

		internal static double ToWebGL(this PrimitiveTopology primitiveTopology)
		{
			switch (primitiveTopology)
			{
				case PrimitiveTopology.PointList:
					return GL.POINTS;
				case PrimitiveTopology.LineList:
					return GL.LINES;
				case PrimitiveTopology.LineLoop:
					return GL.LINE_LOOP;
				case PrimitiveTopology.LineStrip:
					return GL.LINE_STRIP;
				case PrimitiveTopology.TriangleList:
					return GL.TRIANGLES;
				case PrimitiveTopology.TriangleStrip:
					return GL.TRIANGLE_STRIP;
				case PrimitiveTopology.TriangleFan:
					return GL.TRIANGLE_FAN;
				default:
					throw new IllegalValueException(typeof(PrimitiveTopology), primitiveTopology);
			}
		}

		//internal static WebGLBindings.TextureAttachment ToWebGL(this TextureAttachment textureAttachment)
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

		//internal static WebGLBindings.TextureFilter ToWebGL(this TextureFilter textureFilter)
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

		//internal static WebGLBindings.TextureFormat ToWebGL(this TextureFormat textureFormat)
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

		//internal static WebGLBindings.TextureWrap ToWebGL(this TextureWrap textureWrap)
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