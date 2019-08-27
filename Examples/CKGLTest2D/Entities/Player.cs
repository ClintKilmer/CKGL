using CKGL;

namespace CKGLTest2D
{
	public class Player : Entity
	{
		public Sprite Sprite = Sprites.Test1;
		public Colour Colour = Colour.White;
		public Colour ShadowColour = Colour.Black;
		public new Vector2 Scale = new Vector2(1f, 1f);
		public new Vector2 Origin { get { return Sprite.Size * Scale * 0.5f; } }

		public Player()
		{
			Depth = Layer.Player;
		}

		public override void Update()
		{
			if (Input.Keyboard.Down(KeyCode.A) || Input.Controllers.First.LeftStickDigitalLeftDown || Input.Controllers.First.LeftDown)
				X -= 1f;
			if (Input.Keyboard.Down(KeyCode.D) || Input.Controllers.First.LeftStickDigitalRightDown || Input.Controllers.First.RightDown)
				X += 1f;
			if (Input.Keyboard.Down(KeyCode.W) || Input.Controllers.First.LeftStickDigitalUpDown || Input.Controllers.First.UpDown)
				Y += 1f;
			if (Input.Keyboard.Down(KeyCode.S) || Input.Controllers.First.LeftStickDigitalDownDown || Input.Controllers.First.DownDown)
				Y -= 1f;

			//Vector2 direction = Vector2.Zero;
			//if (Input.Left)
			//	direction -= new Vector2(1f, 0f);
			//if (Input.Right)
			//	direction += new Vector2(1f, 0f);
			//if (Input.Up)
			//	direction -= new Vector2(0f, 1f);
			//if (Input.Down)
			//	direction += new Vector2(0f, 1);
			//if(direction != Vector2.Zero)
			//{
			//	direction.Normalize();
			//	Position = Position + direction * 64f * DeltaTime;
			//}

			CKGLTest2D.Camera.Position = Position - new Vector2(160, 90);

			//Engine.GameWindow.Position = (Position * Resolution.Scale).ToPoint();

			Rotation += 0.5f * Time.DeltaTime;

			//Depth = -Y;
		}

		public override void Draw()
		{
			Transform2D t = new Transform2D();
			t.Origin = Sprites.Test2.Size * 0.5f;
			t.Position = Position + new Vector2(9f, 0f);
			t.Scale = Scale;
			t.Rotation = Position.X * -0.01f;
			Renderer.Draw.Sprite(Sprites.Test2, t, Colour.White.Alpha(0.5f));

			//Renderer.SetBlendState(nsBlendState.Additive);
			//Renderer.Draw.Rectangle(Position - Origin, Position - Origin + Size, Colour);
			//Renderer.Draw.Sprite(Sprite, Position - Origin, Colour.White.Alpha(0.5f));
			Renderer.Draw.Sprite(Sprites.Test2, Position - Origin, Scale, Colour.White.Alpha(0.5f), Position.X * -0.01f, Position);
			//Renderer.Draw.Sprite(Sprites.Test2, Position - Origin + new Vector2(8, 8), Scale, Colour.White.Alpha(0.5f), Position.X * 0.01f, Position);
			//Renderer.Draw.Sprite(Sprites.Test3, Position - Origin + new Vector2(8, -8), Scale, Colour.White.Alpha(0.5f), Position.X * 0.01f, Position);
			//Renderer.Draw.Circle(Position, 15.5f, Colour.Blue.Alpha(0), Colour.Blue, 16);
			//Renderer.Draw.Line(Position - Origin, Position - Origin + Size - Vector2.One, Colour.Blue, Colour.Red, 1f);
			//Renderer.Draw.Lines.Line(Position - Origin + new Vector2(0f, Size.Y-0.5f), Position - Origin + Size + new Vector2(0f, -Size.Y-0.5f), Colour.White, Colour.White, true, new Vector2(0f, 0f), new Vector2(1f / 64f, 1f / 64f));
			//Renderer.ResetBlendState();

			//Renderer.Draw.Text(SpriteFonts.Font, $"|:colour=1,0,0,1:|Player 1\n|:colour=1,1,1,0.2:|Oh hello there! `", Position + new Vector2(0, 4), Vector2.One, Colour.White, HAlign.Center, VAlign.Top);
			//Renderer.Draw.Text(SpriteFonts.Font, $"|:shadow=0,1,0,0,0,0.5:||:colour={Random.Range(1f)},{Random.Range(1f)},{Random.Range(1f)},1:|P|:colour={Random.Range(1f)},{Random.Range(1f)},{Random.Range(1f)},1:|l|:colour={Random.Range(1f)},{Random.Range(1f)},{Random.Range(1f)},1:|a|:colour={Random.Range(1f)},{Random.Range(1f)},{Random.Range(1f)},1:|y|:colour={Random.Range(1f)},{Random.Range(1f)},{Random.Range(1f)},1:|e|:colour={Random.Range(1f)},{Random.Range(1f)},{Random.Range(1f)},1:|r |:colour={Random.Range(1f)},{Random.Range(1f)},{Random.Range(1f)},1:|1|:shadow=0,1,1,1,1,0.25:||:colour=0,0,0,1:|\nOh hello there! `", Position + new Vector2(0, 4), Vector2.One, Colour.White, HAlign.Center, VAlign.Top);
		}
	}
}