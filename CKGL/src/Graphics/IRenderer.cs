namespace CKGL
{
	internal interface IRenderer
	{
		void Init();

		void Destroy();

		void Flush();

		void AddVertex(DrawMode type, Vector3 position, Colour? colour, UV? uv);
	}
}