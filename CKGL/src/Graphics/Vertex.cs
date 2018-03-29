using System.Runtime.InteropServices;

namespace CKGL
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct VertexPositionColourUVTextured : IVertexType
	{
		public Vector3 Position;
		public Colour Colour;
		public Vector2 UV;
		public Colour Textured;

		public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration
		(
			new VertexElement(VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
			new VertexElement(VertexElementFormat.Colour, VertexElementUsage.Colour, 0),
			new VertexElement(VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
			new VertexElement(VertexElementFormat.Colour, VertexElementUsage.Colour, 1)
		);

		public VertexPositionColourUVTextured(Vector3 position, Colour colour, Vector2 uv, Colour textured)
		{
			Position = position;
			Colour = colour;
			UV = uv;
			Textured = textured;
		}

		VertexDeclaration IVertexType.VertexDeclaration
		{
			get { return VertexDeclaration; }
		}
	}
}