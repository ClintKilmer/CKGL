namespace CKGL
{
	public abstract class VertexBuffer
	{
		public BufferUsage BufferUsage { get; protected set; }

		public static VertexBuffer Create(BufferUsage bufferUsage)
		{
			return Graphics.CreateVertexBuffer(bufferUsage);
		}

		public abstract void Destroy();

		internal abstract void Bind();

		public abstract void LoadData(byte[] data);

#if WEBGL
		public abstract void LoadData(Retyped.es5.ArrayBuffer data);

		public abstract void LoadData<T>(T[] data, VertexFormat vertexFormat) where T : struct;
#else
		public abstract void LoadData<T>(T[] data) where T : struct;
#endif
	}
}