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

		public abstract void LoadData(in byte[] data);

		public abstract void LoadData<T>(in T[] data) where T : struct;
	}
}