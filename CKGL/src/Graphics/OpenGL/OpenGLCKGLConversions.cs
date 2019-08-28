namespace CKGL.OpenGL
{
	internal static class OpenGLCKGLConversions
	{
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

		internal static OpenGLBindings.PixelFormat ToOpenGL(this PixelFormat pixelFormat)
		{
			switch (pixelFormat)
			{
				case PixelFormat.Depth:
					return OpenGLBindings.PixelFormat.Depth;
				case PixelFormat.DepthStencil:
					return OpenGLBindings.PixelFormat.DepthStencil;
				case PixelFormat.R:
					return OpenGLBindings.PixelFormat.R;
				case PixelFormat.RG:
					return OpenGLBindings.PixelFormat.RG;
				case PixelFormat.RGB:
					return OpenGLBindings.PixelFormat.RGB;
				case PixelFormat.RGBA:
					return OpenGLBindings.PixelFormat.RGBA;
				default:
					throw new IllegalValueException(typeof(PixelFormat), pixelFormat);
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

		internal static OpenGLBindings.TexImageTarget ToOpenGL(this TexImageTarget texImageTarget)
		{
			switch (texImageTarget)
			{
				case TexImageTarget.Texture2D:
					return OpenGLBindings.TexImageTarget.Texture2D;
				case TexImageTarget.TextureCubeMapPosX:
					return OpenGLBindings.TexImageTarget.TextureCubeMapPosX;
				case TexImageTarget.TextureCubeMapNegX:
					return OpenGLBindings.TexImageTarget.TextureCubeMapNegX;
				case TexImageTarget.TextureCubeMapPosY:
					return OpenGLBindings.TexImageTarget.TextureCubeMapPosY;
				case TexImageTarget.TextureCubeMapNegY:
					return OpenGLBindings.TexImageTarget.TextureCubeMapNegY;
				case TexImageTarget.TextureCubeMapPosZ:
					return OpenGLBindings.TexImageTarget.TextureCubeMapPosZ;
				case TexImageTarget.TextureCubeMapNegZ:
					return OpenGLBindings.TexImageTarget.TextureCubeMapNegZ;
				default:
					throw new IllegalValueException(typeof(TexImageTarget), texImageTarget);
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
				// Depth textures
				case TextureFormat.Depth:
					return OpenGLBindings.TextureFormat.Depth;
				case TextureFormat.Depth16:
					return OpenGLBindings.TextureFormat.Depth16;
				case TextureFormat.Depth24:
					return OpenGLBindings.TextureFormat.Depth24;
				case TextureFormat.Depth32:
					return OpenGLBindings.TextureFormat.Depth32;
				case TextureFormat.Depth32F:
					return OpenGLBindings.TextureFormat.Depth32F;

				// Depth/Stencil textures
				case TextureFormat.DepthStencil:
					return OpenGLBindings.TextureFormat.DepthStencil;
				case TextureFormat.Depth24Stencil8:
					return OpenGLBindings.TextureFormat.Depth24Stencil8;

				// R textures
				case TextureFormat.R:
					return OpenGLBindings.TextureFormat.R;
				case TextureFormat.R8:
					return OpenGLBindings.TextureFormat.R8;

				// RG textures
				case TextureFormat.RG:
					return OpenGLBindings.TextureFormat.RG;
				case TextureFormat.RG8:
					return OpenGLBindings.TextureFormat.RG8;

				// RGB textures
				case TextureFormat.RGB:
					return OpenGLBindings.TextureFormat.RGB;
				case TextureFormat.RGB8:
					return OpenGLBindings.TextureFormat.RGB8;

				// RGBA textures
				case TextureFormat.RGBA:
					return OpenGLBindings.TextureFormat.RGBA;
				case TextureFormat.RGBA8:
					return OpenGLBindings.TextureFormat.RGBA8;
				default:
					throw new IllegalValueException(typeof(TextureFormat), textureFormat);
			}
		}

		internal static OpenGLBindings.TextureParam ToOpenGL(this TextureParam textureParam)
		{
			switch (textureParam)
			{
				case TextureParam.BaseLevel:
					return OpenGLBindings.TextureParam.BaseLevel;
				case TextureParam.CompareFunc:
					return OpenGLBindings.TextureParam.CompareFunc;
				case TextureParam.CompareMode:
					return OpenGLBindings.TextureParam.CompareMode;
				case TextureParam.MinFilter:
					return OpenGLBindings.TextureParam.MinFilter;
				case TextureParam.MagFilter:
					return OpenGLBindings.TextureParam.MagFilter;
				case TextureParam.MinLOD:
					return OpenGLBindings.TextureParam.MinLOD;
				case TextureParam.MaxLOD:
					return OpenGLBindings.TextureParam.MaxLOD;
				case TextureParam.MaxLevel:
					return OpenGLBindings.TextureParam.MaxLevel;
				case TextureParam.SwizzleR:
					return OpenGLBindings.TextureParam.SwizzleR;
				case TextureParam.SwizzleG:
					return OpenGLBindings.TextureParam.SwizzleG;
				case TextureParam.SwizzleB:
					return OpenGLBindings.TextureParam.SwizzleB;
				case TextureParam.SwizzleA:
					return OpenGLBindings.TextureParam.SwizzleA;
				case TextureParam.WrapS:
					return OpenGLBindings.TextureParam.WrapS;
				case TextureParam.WrapT:
					return OpenGLBindings.TextureParam.WrapT;
				case TextureParam.WrapR:
					return OpenGLBindings.TextureParam.WrapR;
				//case TextureParam.DepthTextureMode:
				//	return OpenGLBindings.TextureParam.DepthTextureMode;
				default:
					throw new IllegalValueException(typeof(TextureParam), textureParam);
			}
		}

		internal static OpenGLBindings.TextureTarget ToOpenGL(this TextureTarget textureTarget)
		{
			switch (textureTarget)
			{
				//case TextureTarget.Texture1D:
				//	return OpenGLBindings.TextureTarget.Texture1D;
				case TextureTarget.Texture2D:
					return OpenGLBindings.TextureTarget.Texture2D;
				case TextureTarget.Texture2DMultisample:
					return OpenGLBindings.TextureTarget.Texture2DMultisample;
				case TextureTarget.Texture3D:
					return OpenGLBindings.TextureTarget.Texture3D;
				//case TextureTarget.Texture1DArray:
				//	return OpenGLBindings.TextureTarget.Texture1DArray;
				case TextureTarget.Texture2DArray:
					return OpenGLBindings.TextureTarget.Texture2DArray;
				case TextureTarget.TextureCubeMap:
					return OpenGLBindings.TextureTarget.TextureCubeMap;
				default:
					throw new IllegalValueException(typeof(TextureTarget), textureTarget);
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