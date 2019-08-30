using static Retyped.webgl2.WebGL2RenderingContext; // WebGL Enums

namespace CKGL.WebGL
{
	internal static class WebGLCKGLConversions
	{
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

		internal static double ToWebGL(this DataType dataType)
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

		internal static double ToWebGL(this IndexType indexType)
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

		//internal static WebGLBindings.PixelFormat ToWebGL(this PixelFormat pixelFormat)
		//{
		//	switch (pixelFormat)
		//	{
		//		case PixelFormat.Depth:
		//			return WebGLBindings.PixelFormat.Depth;
		//		case PixelFormat.DepthStencil:
		//			return WebGLBindings.PixelFormat.DepthStencil;
		//		case PixelFormat.R:
		//			return WebGLBindings.PixelFormat.R;
		//		case PixelFormat.RG:
		//			return WebGLBindings.PixelFormat.RG;
		//		case PixelFormat.RGB:
		//			return WebGLBindings.PixelFormat.RGB;
		//		case PixelFormat.RGBA:
		//			return WebGLBindings.PixelFormat.RGBA;
		//		default:
		//			throw new IllegalValueException(typeof(PixelFormat), pixelFormat);
		//	}
		//}

		internal static double ToWebGL(this PrimitiveTopology primitiveTopology)
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

		//internal static WebGLBindings.TexImageTarget ToWebGL(this TexImageTarget texImageTarget)
		//{
		//	switch (texImageTarget)
		//	{
		//		case TexImageTarget.Texture2D:
		//			return WebGLBindings.TexImageTarget.Texture2D;
		//		case TexImageTarget.TextureCubeMapPosX:
		//			return WebGLBindings.TexImageTarget.TextureCubeMapPosX;
		//		case TexImageTarget.TextureCubeMapNegX:
		//			return WebGLBindings.TexImageTarget.TextureCubeMapNegX;
		//		case TexImageTarget.TextureCubeMapPosY:
		//			return WebGLBindings.TexImageTarget.TextureCubeMapPosY;
		//		case TexImageTarget.TextureCubeMapNegY:
		//			return WebGLBindings.TexImageTarget.TextureCubeMapNegY;
		//		case TexImageTarget.TextureCubeMapPosZ:
		//			return WebGLBindings.TexImageTarget.TextureCubeMapPosZ;
		//		case TexImageTarget.TextureCubeMapNegZ:
		//			return WebGLBindings.TexImageTarget.TextureCubeMapNegZ;
		//		default:
		//			throw new IllegalValueException(typeof(TexImageTarget), texImageTarget);
		//	}
		//}

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
		//		// Depth textures
		//		case TextureFormat.Depth:
		//			return WebGLBindings.TextureFormat.Depth;
		//		case TextureFormat.Depth16:
		//			return WebGLBindings.TextureFormat.Depth16;
		//		case TextureFormat.Depth24:
		//			return WebGLBindings.TextureFormat.Depth24;
		//		case TextureFormat.Depth32:
		//			return WebGLBindings.TextureFormat.Depth32;
		//		case TextureFormat.Depth32F:
		//			return WebGLBindings.TextureFormat.Depth32F;

		//		// Depth/Stencil textures
		//		case TextureFormat.DepthStencil:
		//			return WebGLBindings.TextureFormat.DepthStencil;
		//		case TextureFormat.Depth24Stencil8:
		//			return WebGLBindings.TextureFormat.Depth24Stencil8;

		//		// R textures
		//		case TextureFormat.R:
		//			return WebGLBindings.TextureFormat.R;
		//		case TextureFormat.R8:
		//			return WebGLBindings.TextureFormat.R8;

		//		// RG textures
		//		case TextureFormat.RG:
		//			return WebGLBindings.TextureFormat.RG;
		//		case TextureFormat.RG8:
		//			return WebGLBindings.TextureFormat.RG8;

		//		// RGB textures
		//		case TextureFormat.RGB:
		//			return WebGLBindings.TextureFormat.RGB;
		//		case TextureFormat.RGB8:
		//			return WebGLBindings.TextureFormat.RGB8;

		//		// RGBA textures
		//		case TextureFormat.RGBA:
		//			return WebGLBindings.TextureFormat.RGBA;
		//		case TextureFormat.RGBA8:
		//			return WebGLBindings.TextureFormat.RGBA8;
		//		default:
		//			throw new IllegalValueException(typeof(TextureFormat), textureFormat);
		//	}
		//}

		//internal static WebGLBindings.TextureParam ToWebGL(this TextureParam textureParam)
		//{
		//	switch (textureParam)
		//	{
		//		case TextureParam.BaseLevel:
		//			return WebGLBindings.TextureParam.BaseLevel;
		//		case TextureParam.CompareFunc:
		//			return WebGLBindings.TextureParam.CompareFunc;
		//		case TextureParam.CompareMode:
		//			return WebGLBindings.TextureParam.CompareMode;
		//		case TextureParam.MinFilter:
		//			return WebGLBindings.TextureParam.MinFilter;
		//		case TextureParam.MagFilter:
		//			return WebGLBindings.TextureParam.MagFilter;
		//		case TextureParam.MinLOD:
		//			return WebGLBindings.TextureParam.MinLOD;
		//		case TextureParam.MaxLOD:
		//			return WebGLBindings.TextureParam.MaxLOD;
		//		case TextureParam.MaxLevel:
		//			return WebGLBindings.TextureParam.MaxLevel;
		//		case TextureParam.SwizzleR:
		//			return WebGLBindings.TextureParam.SwizzleR;
		//		case TextureParam.SwizzleG:
		//			return WebGLBindings.TextureParam.SwizzleG;
		//		case TextureParam.SwizzleB:
		//			return WebGLBindings.TextureParam.SwizzleB;
		//		case TextureParam.SwizzleA:
		//			return WebGLBindings.TextureParam.SwizzleA;
		//		case TextureParam.WrapS:
		//			return WebGLBindings.TextureParam.WrapS;
		//		case TextureParam.WrapT:
		//			return WebGLBindings.TextureParam.WrapT;
		//		case TextureParam.WrapR:
		//			return WebGLBindings.TextureParam.WrapR;
		//		//case TextureParam.DepthTextureMode:
		//		//	return WebGLBindings.TextureParam.DepthTextureMode;
		//		default:
		//			throw new IllegalValueException(typeof(TextureParam), textureParam);
		//	}
		//}

		//internal static WebGLBindings.TextureTarget ToWebGL(this TextureTarget textureTarget)
		//{
		//	switch (textureTarget)
		//	{
		//		//case TextureTarget.Texture1D:
		//		//	return WebGLBindings.TextureTarget.Texture1D;
		//		case TextureTarget.Texture2D:
		//			return WebGLBindings.TextureTarget.Texture2D;
		//		case TextureTarget.Texture2DMultisample:
		//			return WebGLBindings.TextureTarget.Texture2DMultisample;
		//		case TextureTarget.Texture3D:
		//			return WebGLBindings.TextureTarget.Texture3D;
		//		//case TextureTarget.Texture1DArray:
		//		//	return WebGLBindings.TextureTarget.Texture1DArray;
		//		case TextureTarget.Texture2DArray:
		//			return WebGLBindings.TextureTarget.Texture2DArray;
		//		case TextureTarget.TextureCubeMap:
		//			return WebGLBindings.TextureTarget.TextureCubeMap;
		//		default:
		//			throw new IllegalValueException(typeof(TextureTarget), textureTarget);
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