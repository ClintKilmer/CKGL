namespace CKGL
{
	public abstract class VertexBuffer
	{
		public static VertexBuffer Create(BufferUsage bufferUsage)
		{
			return Graphics.CreateVertexBuffer(bufferUsage);
		}

		public abstract void Destroy();

		internal abstract void Bind();

		public abstract void LoadData(ref byte[] data);

		public abstract void LoadData<T>(ref T[] data) where T : struct;
	}
}