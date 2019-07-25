namespace CKGL
{
	public abstract class IndexBuffer
	{
		public BufferUsage BufferUsage { get; protected set; }
		public IndexType IndexType { get; protected set; } = IndexType.UnsignedInt;
		public int Count { get; protected set; } = 0;

		public static IndexBuffer Create(BufferUsage bufferUsage)
		{
			return Graphics.CreateIndexBuffer(bufferUsage);
		}

		public abstract void Destroy();

		internal abstract void Bind();

		public abstract void LoadData(ushort[] indices);

		public abstract void LoadData(uint[] indices);
	}
}