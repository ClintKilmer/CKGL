namespace CKGL
{
	public enum DataType : byte
	{
		Byte,
		UnsignedByte,
		Short,
		UnsignedShort,
		Int,
		UnsignedInt,
		Float,
		Double,
		HalfFloat,
		UnsignedInt_2_10_10_10_Rev,
		Int_2_10_10_10_Rev
	}

	internal static class DataTypeExt
	{
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
	}
}