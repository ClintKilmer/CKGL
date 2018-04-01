using CKGL;

namespace CKGLTest
{
	class Program
	{
		private static string Title = "CKGL Game!";
		private static int WindowWidth = 640;
		private static int WindowHeight = 360;
		private static bool VSync = true;
		private static bool Fullscreen = false;
		private static bool Resizable = true;
		private static bool Borderless = false;

		static void Main(string[] args)
		{
			Platform.Init(Title, WindowWidth, WindowHeight, VSync, Fullscreen, Resizable, Borderless);
			Graphics.Init();
			Audio.Init();
			Input.Init();
			Renderer.Init();

			//Platform.ShowCursor = false; // Default true
			//Platform.ScreensaverAllowed = true; // Default false

			// Load Shaders
			Shader shader = Shader.FromFile("Shaders/test.glsl");

			// Load Audio
			Audio.Buffer sndPop1 = new Audio.Buffer("snd/sndPop1.wav");
			Audio.Buffer sndPop2 = new Audio.Buffer("snd/sndPop2.wav");

			// Shader - create/compile/link/set program
			shader.Set();

			// Variable for moving window on mouse click and drag
			Point2 windowDraggingPosition = Point2.Zero;

			while (Platform.Running)
			{
				Time.Update();

				Input.Clear();

				Platform.PollEvents();

				Input.Update();

				Audio.Update();

				if (Input.Keyboard.Down(KeyCode.Backspace))
					Platform.Quit();

				if (Input.Keyboard.Pressed(KeyCode.F11))
					Window.Fullscreen = !Window.Fullscreen;

				if (Input.Keyboard.Pressed(KeyCode.F10))
					Window.Borderless = !Window.Borderless;

				if (Input.Keyboard.Pressed(KeyCode.F9))
					Window.Resizable = !Window.Resizable;

				if (Input.Mouse.LeftPressed)
					windowDraggingPosition = Input.Mouse.LastPosition;
				else if (Input.Mouse.LeftDown)
					Window.Position = Input.Mouse.PositionDisplay - windowDraggingPosition;

				Window.Position -= Input.Mouse.Scroll;

				if (Input.Keyboard.Pressed(KeyCode.Space) || Input.Mouse.LeftPressed)
					sndPop1.Play();
				if (Input.Keyboard.Released(KeyCode.Space) || Input.Mouse.LeftReleased)
					sndPop2.Play();

				//--------------------

				Window.Title = $"{Title} - {Time.DeltaTime.ToString("n1")}ms - Info: {Platform.OS} | {Time.TotalSeconds.ToString("n1")} - Buffers: {Audio.BufferCount} - Sources: {Audio.SourceCount} - Position: [{Window.X}, {Window.Y}] - Size: [{Window.Width}, {Window.Height}] - Mouse: [{Input.Mouse.Position.X}, {Input.Mouse.Position.Y}]";

				// Clear the screen
				if (Input.Keyboard.Down(KeyCode.Space))
				{ }
				//Graphics.Clear(Colour.Grey * 0.25f);
				else
					Graphics.Clear(Colour.Black);

				// Set Shader uniforms
				//shader.SetUniform("offset", Time.TotalMilliseconds * 0.0016f, Time.TotalMilliseconds * 0.002f, Time.TotalMilliseconds * 0.0023f);

				Renderer.Start();
				Renderer.Draw.Triangle(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0.0f),
									   new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0.0f),
									   new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0.0f),
									   new Colour(Random.Range(1f), Random.Range(1f), Random.Range(1f), 1f));

				Renderer.Draw.LineStrip.Begin();
				Renderer.Draw.LineStrip.AddVertex(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0.0f),
												  new Colour(Random.Range(1f), Random.Range(1f), Random.Range(1f), 1f));
				Renderer.Draw.LineStrip.AddVertex(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0.0f),
												  new Colour(Random.Range(1f), Random.Range(1f), Random.Range(1f), 1f));
				Renderer.Draw.LineStrip.AddVertex(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0.0f),
												  new Colour(Random.Range(1f), Random.Range(1f), Random.Range(1f), 1f));
				Renderer.Draw.LineStrip.AddVertex(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0.0f),
												  new Colour(Random.Range(1f), Random.Range(1f), Random.Range(1f), 1f));
				Renderer.Draw.LineStrip.End();

				//Renderer.Draw.Pixel(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0.0f),
				//					new Colour(Random.Range(1f), Random.Range(1f), Random.Range(1f), 1f));
				//Renderer.Draw.Pixel(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0.0f),
				//					new Colour(Random.Range(1f), Random.Range(1f), Random.Range(1f), 1f));
				//Renderer.Draw.Pixel(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0.0f),
				//					new Colour(Random.Range(1f), Random.Range(1f), Random.Range(1f), 1f));
				//Renderer.Draw.Pixel(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0.0f),
				//					new Colour(Random.Range(1f), Random.Range(1f), Random.Range(1f), 1f));
				//Renderer.Draw.Pixel(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0.0f),
				//					new Colour(Random.Range(1f), Random.Range(1f), Random.Range(1f), 1f));
				Renderer.End();

				// Swap buffers
				Window.SwapBuffers();
			}

			Renderer.Destroy();
			Audio.Destroy();
			Platform.Destroy();
		}
	}
}