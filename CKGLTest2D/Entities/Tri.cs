using CKGL;

namespace CKGLTest2D
{
	public class Tri : Entity
	{
		float Height = 0f;

		public Tri()
		{
			Depth = Layer.Tri;
		}

		public override void Update()
		{
			Rotation += 0.25f * Time.DeltaTime;

			Height = (Math.Sin(Time.TotalSeconds * 2f) * 0.5f + 0.5f) * 10f;
		}

		public override void Draw()
		{

			Renderer.Draw.Triangle(new Vector2((X - 10f), (Y - 10f)),
								   new Vector2((X + 40f), (Y + 5f)),
								   new Vector2((X + 10f), (Y + 30f)),
								   Colour.Black.Alpha(0.5f),
								   Colour.Black.Alpha(0.5f),
								   Colour.Black.Alpha(0.5f),
								   null, null, null,
								   Rotation, Position + new Vector2(10f, 10f));

			Renderer.Draw.Triangle(new Vector2((X - 10f), (Y - 10f - Height)),
								   new Vector2((X + 40f), (Y + 5f - Height)),
								   new Vector2((X + 10f), (Y + 30f - Height)),
								   Colour.Red.Alpha(0.5f),
								   Colour.Green,
								   Colour.Blue,
								   null, null, null,
								   Rotation, Position + new Vector2(10f, 10f - Height));
		}
	}
}