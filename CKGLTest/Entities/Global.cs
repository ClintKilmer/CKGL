using CKGL;

namespace CKGLTest
{
	class Global : Entity
	{
		public Global()
		{
			Depth = Layer.Global;
		}

		public override void Update()
		{
			if (Input.Exit)
				ExitGame();

			if (Input.BorderlessToggle.Down)
				ToggleBorderless();

			if (Input.FullscreenToggle.Down)
				ToggleFullscreen();

			//if (Input.Left)
			//	Camera.X -= 1f;
			//if (Input.Right)
			//	Camera.X += 1f;
			//if (Input.Up)
			//	Camera.Y -= 1f;
			//if (Input.Down)
			//	Camera.Y += 1f;
			if (Input.ZoomIn)
				Camera.Zoom += 0.01f * Camera.Zoom * 2;
			if (Input.ZoomOut)
				Camera.Zoom -= 0.01f * Camera.Zoom * 2;
			if (Input.ZoomIn && Input.ZoomOut)
				Camera.Zoom = 1f;
			if (Input.RotateLeft)
				Camera.Rotation -= 0.005f;
			if (Input.RotateRight)
				Camera.Rotation += 0.005f;
			if (Input.RotateLeft && Input.RotateRight)
				Camera.Rotation = 0f;
			if (Input.Keyboard.KeyDown(Keys.Space))
				TimeRate = TimeRate == 0f ? 1f : 0f;

#if !FNA
			if (!Fullscreen && Input.Mouse.LeftButton)
				Engine.GameWindow.Position = Input.Mouse.Position.Display.ToPoint() - Resolution.Window.Half.Point;
#endif

			Engine.GameWindow.Title = Title
				//+ $" | Res: ({Resolution.Scaled.Width}, {Resolution.Scaled.Height})"
				+ " | Scale: " + Resolution.Scale
				+ " | FPS: " + Metrics.FPS.ToString("N1").PadLeft(2, '0')
				//+ " | UFPS: " + Metrics.UpdateFPS.ToString("N1").PadLeft(2, '0')
#if DEBUG
				//+ $" | Mem: {Metrics.Memory.ToString("F")} MB"
				//+ $" | GC: {Metrics.GCMemory.ToString("F")} MB"
#endif
				+ " | Draw Calls: " + Metrics.DrawCalls
				+ " | Entities: " + Scene.Entities.Count
				//+ " | Cam Rotation: " + Camera.Rotation
				//+ " | Player: " + occlusion.player.Position.ToString()
				//+ " | Player Rotation: " + occlusion.player.Rotation
				//+ " | Mouse Display: " + Input.Mouse.Position.Display.ToString()
				//+ " | Mouse Window: " + Input.Mouse.Position.Window.ToString()
				//+ " | Mouse Viewport: " + Input.Mouse.Position.Viewport.ToString()
				//+ " | Mouse World: " + Input.Mouse.Position.World.ToString()
				;
		}

		public override void Draw()
		{
			Renderer.Draw.Text(SpriteFonts.Font, "|:shadow=0,1,0,0,0,0.5:|ABCDEFGHIJKLMNOPQRSTUVWXYZ\nabcdefghijklmnopqrstuvwxyz\n1234567890\n_-+=(){}[]<>\\|/;:'\"?.,!@#$%^&*~`", Vector2.Zero, Vector2.One, Colour.White, HAlign.Center, VAlign.Middle/*, Camera.Position.X * 0.01f, Vector2.Zero*/);

			// TriangleStrip
			Renderer.Draw.TriangleStrip.Begin();
			for (int i = Random.Range(30); i < 33; i++)
			{
				Renderer.Draw.TriangleStrip.AddVertex(new Vector2(-200 + Random.Range(200), -100 + Random.Range(200)), new Colour(Random.Range(255), Random.Range(255), Random.Range(255)).Alpha(0.5f));
			}
			Renderer.Draw.TriangleStrip.End();

			// LineStrip
			//Renderer.Draw.LineStrip.Begin();
			//for (int i = Random.Range(30); i < 33; i++)
			//{
			//	Renderer.Draw.LineStrip.AddVertex(new Vector2(Random.Range(200), -100 + Random.Range(200)), new Colour(Random.Range(255), Random.Range(255), Random.Range(255)).Alpha((float)Random.RangeDouble()));
			//}
			//Renderer.Draw.LineStrip.End();



			// Font stress-test
			//for (int i = 0; i < 1000; i++)
			//{
			//	Renderer.Draw.Text(SpriteFonts.Font, "ABCDEFGHIJKLMNOPQRSTUVWXYZ\nabcdefghijklmnopqrstuvwxyz\n1234567890\n_-+=(){}[]<>\\|/;:'\"?.,!@#$%^&*~`", new Vector2(-200 + Random.Range(200), -100 + Random.Range(200)), Vector2.One, Colour.White, HAlign.Center, VAlign.Middle/*, Camera.Position.X * 0.01f, Vector2.Zero*/);
			//}



			//for (int i = 0; i < 20; i++)
			//{
			//	nullsilence.Draw.Triangle(new Vector2((float)(i * 10f + Math.Sin(DeltaTime * 0.005f) * 20), (float)(i * 10f + Math.Sin(DeltaTime * 0.004f) * 20)),
			//							  new Vector2((float)(i * 10f + 50f + Math.Sin(DeltaTime * 0.003f) * 20), (float)(i * 10f + 15f + Math.Sin(DeltaTime * 0.002f) * 20)),
			//							  new Vector2((float)(i * 10f + 20f + Math.Sin(DateTime.Now.Millisecond * 0.007f) * 20), (float)(i * 10f + 40f + Math.Sin(DeltaTime * 0.008f) * 20)),
			//							  i,
			//							  Colour.Red.Alpha(0),
			//							  Colour.Green`,
			//							  Colour.Blue);
			//}
			//nullsilence.Draw.Line(new Vector2(-Global.Resolution.Width * 0.5f, -Global.Resolution.Height * 0.5f), new Vector2(Global.Resolution.Width * 0.5f, Global.Resolution.Height * 0.5f), Colour.White, Colour.Black, 1f);

			//nullsilence.Draw.Line(new Vector2(0f, 0f), new Vector2(Global.Resolution.Width - 1f, 0f), Colour.Black, Colour.Black, 1f);
			//nullsilence.Draw.Line(new Vector2(0f, Global.Resolution.Height - 1f), new Vector2(Global.Resolution.Width - 1f, Global.Resolution.Height - 1f), Colour.Black, Colour.Black, 1f);
			//nullsilence.Draw.Line(new Vector2(0f, 0f), new Vector2(0f, Global.Resolution.Height - 1f), Colour.Black, Colour.Black, 1f);
			//nullsilence.Draw.Line(new Vector2(Global.Resolution.Width - 1f, 0f), new Vector2(Global.Resolution.Width - 1f, Global.Resolution.Height - 1f), Colour.Black, Colour.Black, 1f);

			//nullsilence.Draw.Line(new Vector2(-2f, 0f), new Vector2(2f, 0f), Colour.Black, Colour.Black, 1f);
			//nullsilence.Draw.Line(new Vector2(0f, -2f), new Vector2(0f, 2f), Colour.Black, Colour.Black, 1f);

			//nullsilence.Draw.Line(new Vector2(-2f, 0f), new Vector2(3f, 0f), Colour.Black, Colour.Black, 1f);
			//nullsilence.Draw.Line(new Vector2(0f * i * 0.0001f, -2f), new Vector2(0f, 3f), Colour.Black, Colour.White, 1f);

			//nullsilence.Draw.RectangleOutline(new Vector2(1f, 1f), new Vector2(Global.Resolution.Width - 1f, Global.Resolution.Height - 1f), Colour.Orange);



			// Lines using GL_TRIANGLES
			// Debug lines from player to all boxes
			//foreach (Box box in Scene.Entities.FindAll<Box>())
			//{
			//	Renderer.Draw.Line(box.Position + box.Size * 0.5f, occlusion.player.Position, Colour.Red, Colour.Red.Alpha(0), 1f); Renderer.Draw.Line(Position + Size * 0.5f, occlusion.player.Position, Colour.Red, Colour.Red.Alpha(0), 1f);
			//}

			// GL_LINES
			// Debug lines from player to all boxes
			//foreach (Box box in Scene.Entities.FindAll<Box>())
			//{
			//	Renderer.Draw.Lines.Line(box.Position + box.Size * 0.5f, occlusion.player.Position, Colour.Red, Colour.Red.Alpha(0));
			//}
		}
	}
}