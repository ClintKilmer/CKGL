namespace CKGL
{
	internal abstract class RendererBase
	{
		internal abstract void Init();
		internal abstract void Destroy();
		internal abstract void Flush();
		internal abstract void AddVertex(PrimitiveTopology primitiveTopology, Vector3 position, Colour? colour, UV? uv);
	}
}