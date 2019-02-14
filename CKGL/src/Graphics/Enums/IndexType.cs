namespace CKGL
{
	public enum IndexType : byte
	{
		UnsignedByte,
		UnsignedShort,
		UnsignedInt
	}

	internal static class IndexTypeExt
	{
		internal static OpenGLBindings.IndexType ToOpenGL(this IndexType indexType)
		{
			switch (indexType)
			{
				case IndexType.UnsignedByte:
					return OpenGLBindings.IndexType.UnsignedByte;
				case IndexType.UnsignedShort:
					return OpenGLBindings.IndexType.UnsignedShort;
				case IndexType.UnsignedInt:
					return OpenGLBindings.IndexType.UnsignedInt;
				default:
					throw new IllegalValueException(typeof(IndexType), indexType);
			}
		}
	}
}