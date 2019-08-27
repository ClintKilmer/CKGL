using CKGL;

namespace CKGLTest
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

			Height = (Math.Sin(Time.TotalSeconds * 2f) * 0.5f + 0.5f);
		}

		public override void Draw()
		{
			Renderer.Draw.Triangle(new Vector2((X - 1f), (Y - 1f)),
								   new Vector2((X + 4f), (Y + 0.5f)),
								   new Vector2((X + 1f), (Y + 3f)),
								   Colour.Black.Alpha(0.5f),
								   Colour.Black.Alpha(0.5f),
								   Colour.Black.Alpha(0.5f),
								   null, null, null,
								   Rotation, Position + new Vector2(1f, 1f));

			Renderer.Draw.Triangle(new Vector2((X - 1f), (Y - 1f - Height)),
								   new Vector2((X + 4f), (Y + 0.5f - Height)),
								   new Vector2((X + 1f), (Y + 3f - Height)),
								   Colour.Red.Alpha(0.5f),
								   Colour.Green,
								   Colour.Blue,
								   null, null, null,
								   Rotation, Position + new Vector2(1f, 1f - Height));
		}
	}
}