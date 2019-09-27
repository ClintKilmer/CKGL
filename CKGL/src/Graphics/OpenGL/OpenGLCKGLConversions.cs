namespace CKGL.OpenGL
{
	internal static class OpenGLCKGLConversions
	{
		#region State
		internal static OpenGLBindings.BlendEquation ToOpenGL(this BlendEquation blendEquation)
		{
			switch (blendEquation)
			{
				case BlendEquation.Add:
					return OpenGLBindings.BlendEquation.Add;
				case BlendEquation.Subtract:
					return OpenGLBindings.BlendEquation.Subtract;
				case BlendEquation.ReverseSubtract:
					return OpenGLBindings.BlendEquation.ReverseSubtract;
				case BlendEquation.Max:
					return OpenGLBindings.BlendEquation.Max;
				case BlendEquation.Min:
					return OpenGLBindings.BlendEquation.Min;
				default:
					throw new IllegalValueException(typeof(BlendEquation), blendEquation);
			}
		}

		internal static OpenGLBindings.BlendFactor ToOpenGL(this BlendFactor blendFactor)
		{
			switch (blendFactor)
			{
				case BlendFactor.Zero:
					return OpenGLBindings.BlendFactor.Zero;
				case BlendFactor.One:
					return OpenGLBindings.BlendFactor.One;
				case BlendFactor.SrcColour:
					return OpenGLBindings.BlendFactor.SrcColour;
				case BlendFactor.OneMinusSrcColour:
					return OpenGLBindings.BlendFactor.OneMinusSrcColour;
				case BlendFactor.SrcAlpha:
					return OpenGLBindings.BlendFactor.SrcAlpha;
				case BlendFactor.OneMinusSrcAlpha:
					return OpenGLBindings.BlendFactor.OneMinusSrcAlpha;
				case BlendFactor.DstAlpha:
					return OpenGLBindings.BlendFactor.DstAlpha;
				case BlendFactor.OneMinusDstAlpha:
					return OpenGLBindings.BlendFactor.OneMinusDstAlpha;
				case BlendFactor.DstColour:
					return OpenGLBindings.BlendFactor.DstColour;
				case BlendFactor.OneMinusDstcolour:
					return OpenGLBindings.BlendFactor.OneMinusDstcolour;
				case BlendFactor.SrcAlphaSaturate:
					return OpenGLBindings.BlendFactor.SrcAlphaSaturate;
				case BlendFactor.ConstantColour:
					return OpenGLBindings.BlendFactor.ConstantColour;
				case BlendFactor.OneMinusConstantColour:
					return OpenGLBindings.BlendFactor.OneMinusConstantColour;
				case BlendFactor.ConstantAlpha:
					return OpenGLBindings.BlendFactor.ConstantAlpha;
				case BlendFactor.OneMinusConstantAlpha:
					return OpenGLBindings.BlendFactor.OneMinusConstantAlpha;
				default:
					throw new IllegalValueException(typeof(BlendFactor), blendFactor);
			}
		}

		internal static OpenGLBindings.Face ToOpenGL(this Face face)
		{
			switch (face)
			{
				case Face.Front:
					return OpenGLBindings.Face.Front;
				case Face.Back:
					return OpenGLBindings.Face.Back;
				case Face.FrontAndBack:
					return OpenGLBindings.Face.FrontAndBack;
				default:
					throw new IllegalValueException(typeof(Face), face);
			}
		}

		internal static OpenGLBindings.DepthFunc ToOpenGL(this DepthFunction depthFunction)
		{
			switch (depthFunction)
			{
				case DepthFunction.Never:
					return OpenGLBindings.DepthFunc.Never;
				case DepthFunction.Less:
					return OpenGLBindings.DepthFunc.Less;
				case DepthFunction.Equal:
					return OpenGLBindings.DepthFunc.Equal;
				case DepthFunction.LessEqual:
					return OpenGLBindings.DepthFunc.LessEqual;
				case DepthFunction.Greater:
					return OpenGLBindings.DepthFunc.Greater;
				case DepthFunction.NotEqual:
					return OpenGLBindings.DepthFunc.NotEqual;
				case DepthFunction.GreaterEqual:
					return OpenGLBindings.DepthFunc.GreaterEqual;
				case DepthFunction.Always:
					return OpenGLBindings.DepthFunc.Always;
				default:
					throw new IllegalValueException(typeof(DepthFunction), depthFunction);
			}
		}

		internal static OpenGLBindings.FrontFace ToOpenGL(this FrontFace frontFace)
		{
			switch (frontFace)
			{
				case FrontFace.Clockwise:
					return OpenGLBindings.FrontFace.Clockwise;
				case FrontFace.CounterClockwise:
					return OpenGLBindings.FrontFace.CounterClockwise;
				default:
					throw new IllegalValueException(typeof(FrontFace), frontFace);
			}
		}

		internal static OpenGLBindings.PolygonMode ToOpenGL(this PolygonMode polygonMode)
		{
			switch (polygonMode)
			{
				case PolygonMode.Fill:
					return OpenGLBindings.PolygonMode.Fill;
				case PolygonMode.Line:
					return OpenGLBindings.PolygonMode.Line;
				case PolygonMode.Point:
					return OpenGLBindings.PolygonMode.Point;
				default:
					throw new IllegalValueException(typeof(PolygonMode), polygonMode);
			}
		}
		#endregion

		internal static OpenGLBindings.BlitFilter ToOpenGL(this BlitFilter blitFilter)
		{
			switch (blitFilter)
			{
				case BlitFilter.Nearest:
					return OpenGLBindings.BlitFilter.Nearest;
				case BlitFilter.Linear:
					return OpenGLBindings.BlitFilter.Linear;
				default:
					throw new IllegalValueException(typeof(BlitFilter), blitFilter);
			}
		}

		internal static OpenGLBindings.BufferUsage ToOpenGL(this BufferUsage bufferUsage)
		{
			switch (bufferUsage)
			{
				case BufferUsage.Static:
					return OpenGLBindings.BufferUsage.StaticDraw;
				case BufferUsage.Default:
					return OpenGLBindings.BufferUsage.DynamicDraw;
				case BufferUsage.Dynamic:
					return OpenGLBindings.BufferUsage.StreamDraw;
				case BufferUsage.Copy:
					return OpenGLBindings.BufferUsage.DynamicRead;
				default:
					throw new IllegalValueException(typeof(BufferUsage), bufferUsage);
			}
		}

		internal static OpenGLBindings.DataType ToOpenGL(this DataType dataType)
		{
			switch (dataType)
			{
				case DataType.Byte:
					return OpenGLBindings.DataType.Byte;
				case DataType.UnsignedByte:
					return OpenGLBindings.DataType.UnsignedByte;
				case DataType.Short:
					return OpenGLBindings.DataType.Short;
				case DataType.UnsignedShort:
					return OpenGLBindings.DataType.UnsignedShort;
				case DataType.Int:
					return OpenGLBindings.DataType.Int;
				case DataType.UnsignedInt:
					return OpenGLBindings.DataType.UnsignedInt;
				case DataType.Float:
					return OpenGLBindings.DataType.Float;
				case DataType.Double:
					return OpenGLBindings.DataType.Double;
				case DataType.HalfFloat:
					return OpenGLBindings.DataType.HalfFloat;
				case DataType.UnsignedInt_2_10_10_10_Rev:
					return OpenGLBindings.DataType.UnsignedInt_2_10_10_10_Rev;
				case DataType.Int_2_10_10_10_Rev:
					return OpenGLBindings.DataType.Int_2_10_10_10_Rev;
				default:
					throw new IllegalValueException(typeof(DataType), dataType);
			}
		}

		internal static OpenGLBindings.IndexType ToOpenGL(this IndexType indexType)
		{
			switch (indexType)
			{
				//case IndexType.UnsignedByte:
				//	return OpenGLBindings.IndexType.UnsignedByte;
				case IndexType.UnsignedShort:
					return OpenGLBindings.IndexType.UnsignedShort;
				case IndexType.UnsignedInt:
					return OpenGLBindings.IndexType.UnsignedInt;
				default:
					throw new IllegalValueException(typeof(IndexType), indexType);
			}
		}

		internal static OpenGLBindings.PrimitiveMode ToOpenGL(this PrimitiveTopology primitiveTopology)
		{
			switch (primitiveTopology)
			{
				case PrimitiveTopology.PointList:
					return OpenGLBindings.PrimitiveMode.Points;
				case PrimitiveTopology.LineList:
					return OpenGLBindings.PrimitiveMode.Lines;
				case PrimitiveTopology.LineLoop:
					return OpenGLBindings.PrimitiveMode.LineLoop;
				case PrimitiveTopology.LineStrip:
					return OpenGLBindings.PrimitiveMode.LineStrip;
				case PrimitiveTopology.TriangleList:
					return OpenGLBindings.PrimitiveMode.Triangles;
				case PrimitiveTopology.TriangleStrip:
					return OpenGLBindings.PrimitiveMode.TriangleStrip;
				case PrimitiveTopology.TriangleFan:
					return OpenGLBindings.PrimitiveMode.TriangleFan;
				default:
					throw new IllegalValueException(typeof(PrimitiveTopology), primitiveTopology);
			}
		}

		internal static OpenGLBindings.TextureAttachment ToOpenGL(this TextureAttachment textureAttachment)
		{
			switch (textureAttachment)
			{
				case TextureAttachment.Depth:
					return OpenGLBindings.TextureAttachment.Depth;
				case TextureAttachment.DepthStencil:
					return OpenGLBindings.TextureAttachment.DepthStencil;
				case TextureAttachment.Colour0:
					return OpenGLBindings.TextureAttachment.Colour0;
				case TextureAttachment.Colour1:
					return OpenGLBindings.TextureAttachment.Colour1;
				case TextureAttachment.Colour2:
					return OpenGLBindings.TextureAttachment.Colour2;
				case TextureAttachment.Colour3:
					return OpenGLBindings.TextureAttachment.Colour3;
				case TextureAttachment.Colour4:
					return OpenGLBindings.TextureAttachment.Colour4;
				case TextureAttachment.Colour5:
					return OpenGLBindings.TextureAttachment.Colour5;
				case TextureAttachment.Colour6:
					return OpenGLBindings.TextureAttachment.Colour6;
				case TextureAttachment.Colour7:
					return OpenGLBindings.TextureAttachment.Colour7;
				case TextureAttachment.Colour8:
					return OpenGLBindings.TextureAttachment.Colour8;
				case TextureAttachment.Colour9:
					return OpenGLBindings.TextureAttachment.Colour9;
				case TextureAttachment.Colour10:
					return OpenGLBindings.TextureAttachment.Colour10;
				case TextureAttachment.Colour11:
					return OpenGLBindings.TextureAttachment.Colour11;
				case TextureAttachment.Colour12:
					return OpenGLBindings.TextureAttachment.Colour12;
				case TextureAttachment.Colour13:
					return OpenGLBindings.TextureAttachment.Colour13;
				case TextureAttachment.Colour14:
					return OpenGLBindings.TextureAttachment.Colour14;
				case TextureAttachment.Colour15:
					return OpenGLBindings.TextureAttachment.Colour15;
				default:
					throw new IllegalValueException(typeof(TextureAttachment), textureAttachment);
			}
		}

		internal static OpenGLBindings.TextureFilter ToOpenGL(this TextureFilter textureFilter)
		{
			switch (textureFilter)
			{
				case TextureFilter.Nearest:
					return OpenGLBindings.TextureFilter.Nearest;
				case TextureFilter.Linear:
					return OpenGLBindings.TextureFilter.Linear;
				case TextureFilter.NearestMipmapNearest:
					return OpenGLBindings.TextureFilter.NearestMipmapNearest;
				case TextureFilter.LinearMipmapNearest:
					return OpenGLBindings.TextureFilter.LinearMipmapNearest;
				case TextureFilter.NearestMipmapLinear:
					return OpenGLBindings.TextureFilter.NearestMipmapLinear;
				case TextureFilter.LinearMipmapLinear:
					return OpenGLBindings.TextureFilter.LinearMipmapLinear;
				default:
					throw new IllegalValueException(typeof(TextureFilter), textureFilter);
			}
		}

		internal static OpenGLBindings.TextureFormat ToOpenGL(this TextureFormat textureFormat)
		{
			switch (textureFormat)
			{
				case TextureFormat.R8:
					return OpenGLBindings.TextureFormat.R8;
				case TextureFormat.RG8:
					return OpenGLBindings.TextureFormat.RG8;
				case TextureFormat.RGB8:
					return OpenGLBindings.TextureFormat.RGB8;
				case TextureFormat.RGBA8:
					return OpenGLBindings.TextureFormat.RGBA8;
				case TextureFormat.Depth16:
					return OpenGLBindings.TextureFormat.Depth16;
				case TextureFormat.Depth24:
					return OpenGLBindings.TextureFormat.Depth24;
				case TextureFormat.Depth32F:
					return OpenGLBindings.TextureFormat.Depth32F;
				case TextureFormat.Depth24Stencil8:
					return OpenGLBindings.TextureFormat.Depth24Stencil8;
				case TextureFormat.Depth32FStencil8:
					return OpenGLBindings.TextureFormat.Depth32FStencil8;
				default:
					throw new IllegalValueException(typeof(TextureFormat), textureFormat);
			}
		}

		internal static OpenGLBindings.TextureWrap ToOpenGL(this TextureWrap textureWrap)
		{
			switch (textureWrap)
			{
				case TextureWrap.Clamp:
					return OpenGLBindings.TextureWrap.Clamp;
				case TextureWrap.Repeat:
					return OpenGLBindings.TextureWrap.Repeat;
				case TextureWrap.MirroredRepeat:
					return OpenGLBindings.TextureWrap.MirroredRepeat;
				default:
					throw new IllegalValueException(typeof(TextureWrap), textureWrap);
			}
		}
	}
}