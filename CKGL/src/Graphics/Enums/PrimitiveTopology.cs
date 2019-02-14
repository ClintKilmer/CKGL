namespace CKGL
{
	public enum PrimitiveTopology : byte
	{
		PointList,
		LineList,
		LineLoop,
		LineStrip,
		TriangleList,
		TriangleStrip,
		TriangleFan
	}

	internal static class PrimitiveTopologyExt
	{
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
	}
}