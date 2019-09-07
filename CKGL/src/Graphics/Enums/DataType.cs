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
		Int_2_10_10_10_Rev,
		UnsignedInt_2_10_10_10_Rev
	}


	public static class DataTypeExt
	{
		public static int Size(this DataType dataType)
		{
			switch (dataType)
			{
				case DataType.Byte:
					return 1;
				case DataType.UnsignedByte:
					return 1;
				case DataType.Short:
					return 2;
				case DataType.UnsignedShort:
					return 2;
				case DataType.Int:
					return 4;
				case DataType.UnsignedInt:
					return 4;
				case DataType.Float:
					return 4;
				case DataType.Double:
					return 8;
				case DataType.HalfFloat:
					return 2;
				case DataType.Int_2_10_10_10_Rev:
					return 4;
				case DataType.UnsignedInt_2_10_10_10_Rev:
					return 4;
				default:
					throw new IllegalValueException(typeof(DataType), dataType);
			}
		}
	}
}