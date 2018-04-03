using CKGL;

namespace CKGLTest
{
	public class CKGLTest : Game
	{
		public CKGLTest()
			: base(windowTitle: "CKGL Game!",
				   windowWidth: 640,
				   windowHeight: 360,
				   windowVSync: true,
				   windowFullscreen: false,
				   windowResizable: true,
				   windowBorderless: false)
		{ }

		// Load Shaders
		Shader test;

		// Load Audio
		Audio.Buffer sndPop1;
		Audio.Buffer sndPop2;

		// Variable for moving window on mouse click and drag
		Point2 windowDraggingPosition = Point2.Zero;

		public override void Init()
		{
			//Platform.ShowCursor = false; // Default true
			//Platform.ScreensaverAllowed = true; // Default false

			// Load Shaders
			test = Shader.FromFile("Shaders/test.glsl");

			// Load Audio
			sndPop1 = new Audio.Buffer("snd/sndPop1.wav");
			sndPop2 = new Audio.Buffer("snd/sndPop2.wav");
		}

		public override void Update()
		{
			Window.Title = $"{Time.DeltaTime.ToString("n1")}ms - Info: {Platform.OS} | {Time.TotalSeconds.ToString("n1")} - Buffers: {Audio.BufferCount} - Sources: {Audio.SourceCount} - Position: [{Window.X}, {Window.Y}] - Size: [{Window.Width}, {Window.Height}] - Mouse: [{Input.Mouse.Position.X}, {Input.Mouse.Position.Y}]";

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
		}

		public override void Draw()
		{
			// Clear the screen
			if (Input.Keyboard.Down(KeyCode.Space))
			{ }
			//Graphics.Clear(Colour.Grey * 0.25f);
			else
				Graphics.Clear(Colour.Black);

			// Set Shader uniforms
			//shader.SetUniform("offset", Time.TotalMilliseconds * 0.0016f, Time.TotalMilliseconds * 0.002f, Time.TotalMilliseconds * 0.0023f);

			Renderer.Start();
			//Renderer.SetShader(test);
			//Renderer.ResetShader();
			//Renderer.SetCullState(CullState.Back);
			Renderer.SetBlendState(BlendState.AlphaBlend);
			Renderer.Draw.Triangle(new Vector3(-0.5f, -0.5f, 0.0f),
								   new Vector3(0.5f, -0.5f, 0.0f),
								   new Vector2(0.0f, 0.5f) * Matrix2D.CreateScale(2f) * Matrix2D.CreateRotationZ(Input.Mouse.X / (float)Window.Size.X * 360 * Math.Rad) * Matrix2D.CreateTranslation(new Vector2(0.5f, 0f)),
								   Colour.Red,
								   Colour.Green,
								   Colour.Blue,
								   false,
								   Vector2.Zero,
								   Vector2.Zero,
								   Vector2.Zero,
								   Input.Mouse.X / (float)Window.Size.X,
								   Vector2.Zero);
			//Renderer.Draw.TriangleStrip.Begin();
			////int ii = Random.Range(1000, 10000);
			//for (int i = 0; i < 10000; i++)
			//	Renderer.Draw.TriangleStrip.AddVertex(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0.0f),
			//										  new Colour(Random.Range(1f), Random.Range(1f), Random.Range(1f), Random.Range(1f)));
			//Renderer.Draw.TriangleStrip.End();

			//Renderer.Draw.LineStrip.Begin();
			//for (int i = 0; i < 10; i++)
			//	Renderer.Draw.LineStrip.AddVertex(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0.0f),
			//									  new Colour(Random.Range(1f), Random.Range(1f), Random.Range(1f), Random.Range(1f)));
			//Renderer.Draw.LineStrip.End();

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
		}

		public override void Destroy()
		{
		}
	}
}