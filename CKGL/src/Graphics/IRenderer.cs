namespace CKGL
{
	internal abstract class RendererBase
	{
		internal abstract void Init();
		internal abstract void Destroy();
		public abstract void Flush();
		internal abstract void AddVertex(DrawMode type, Vector3 position, Colour? colour, UV? uv);
	}
}