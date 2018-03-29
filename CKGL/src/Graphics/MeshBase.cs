namespace CKGL
{
	public abstract class MeshBase
	{
		public abstract int FloatNumber();

		public abstract void SetAttributes();

		public abstract float[] GetVBO(int vertexNumber);
	}
}