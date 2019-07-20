using CKGL;

namespace CKGLTest2D
{
	public class TransformTest : Entity
	{
		public Transform2D test1 = new Transform2D { Scale = new Vector2(3f) };
		public Transform2D test2 = new Transform2D { Position = new Vector3(12f, 0f, 0f), Scale = new Vector2(0.666f) };
		public Transform2D test3 = new Transform2D { Position = new Vector3(10f, 0f, 0f), Scale = new Vector2(0.666f) };
		public Transform2D test4 = new Transform2D { Position = new Vector3(8f, 0f, 0f), Scale = new Vector2(0.666f) };
		public Transform2D test5 = new Transform2D { Position = new Vector3(6f, 0f, 0f), Scale = new Vector2(0.666f) };

		public TransformTest()
		{
			Depth = Layer.TransformTest;

			test2.Parent = test1;
			test2.Child = test3;
			test4.Parent = test3;
			test4.Child = test5;
		}

		public override void Update()
		{
			test1.Rotation = -Time.TotalSeconds * 0.333f;
			test2.Rotation = Time.TotalSeconds * 0.666f;
			test3.Rotation = -Time.TotalSeconds * 1f;
			test4.Rotation = Time.TotalSeconds * 1.333f;
		}

		public override void Draw()
		{
			Renderer.Draw.SetTransform(test1);
			Renderer.Draw.Circle(Vector2.Zero, 2f, Colour.Blue);
			Renderer.Draw.SetTransform(test2);
			Renderer.Draw.Circle(Vector2.Zero, 2f, Colour.Green);
			Renderer.Draw.SetTransform(test3);
			Renderer.Draw.Circle(Vector2.Zero, 2f, Colour.Red);
			Renderer.Draw.SetTransform(test4);
			Renderer.Draw.Circle(Vector2.Zero, 2f, Colour.Cyan);
			Renderer.Draw.SetTransform(test5);
			Renderer.Draw.Circle(Vector2.Zero, 2f, Colour.Magenta);
			Renderer.Draw.ResetTransform();
		}
	}
}