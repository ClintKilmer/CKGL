using CKGL;

namespace CKGLTest
{
	public class Box : Entity
	{
		public Colour Colour = Colour.Grey;
		public Vector2 Size = new Vector2(8f, 8f);
		public new Vector2 Origin = new Vector2(0f, 0f);

		public Box()
		{
			Depth = Layer.Box;
		}

		public override void Update()
		{
		}

		public override void Draw()
		{
			Renderer.Draw.Rectangle(Position - Origin, Position - Origin + Size, Colour);
			//Renderer.Draw.Sprite(Sprites.Test, Position - Origin, Color.White);
		}
	}
}