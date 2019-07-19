namespace CKGL
{
	public class VertexStream
	{
		public readonly VertexBuffer VertexBuffer;
		public readonly VertexFormat VertexFormat;

		public VertexStream(VertexBuffer vertexBuffer, VertexFormat vertexFormat)
		{
			VertexBuffer = vertexBuffer;
			VertexFormat = vertexFormat;
		}
	}

	public abstract class GeometryInput
	{
		public VertexStream[] VertexStreams { get; protected set; }
		public int VertexStreamCount => VertexStreams.Length;
		public IndexBuffer IndexBuffer { get; protected set; } = null;

		//public static GeometryInput Default { get; private set; }
		//public static GeometryInput Current { get; private set; }

		public static GeometryInput Create(params VertexStream[] vertexStreams) => Create(null, vertexStreams);
		public static GeometryInput Create(IndexBuffer IndexBuffer, params VertexStream[] vertexStreams)
		{
			if (vertexStreams.Length < 1)
				throw new CKGLException("GeometryInput constructor requires at least 1 VertexStream");

			return Graphics.CreateGeometryInput(IndexBuffer, vertexStreams);
		}

		public abstract void Destroy();

		public abstract void Bind();

		//#region Methods
		//public void Set()
		//{
		//	Set(this);
		//}

		//public void SetDefault()
		//{
		//	SetDefault(this);
		//}
		//#endregion

		//#region Static Methods
		//public static void Set(GeometryInput geometryInput)
		//{
		//	if (Current != geometryInput)
		//	{
		//		Graphics.State.OnStateChanging?.Invoke();
		//		Graphics.SetGeometryInput(geometryInput);
		//		Current = geometryInput;
		//		Graphics.State.OnStateChanged?.Invoke();
		//	}
		//}
		//public static void Reset() => Set(Default);
		//public static void SetDefault(GeometryInput geometryInput) => Default = geometryInput;
		//#endregion
	}
}