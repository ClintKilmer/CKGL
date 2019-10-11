using CKGL;

namespace NETCoreTest
{
	#region Shaders
	public static class Shaders
	{
		public static InternalShaders.RendererShader Renderer = new InternalShaders.RendererShader();
	}
	#endregion

	#region Sprites
	public static class SpriteSheets
	{
		public static SpriteSheet SpriteSheet = new SpriteSheet(128, 1);
	}

	public static class SpriteFonts
	{
		public static SpriteFont Font = new SpriteFont(SpriteSheets.SpriteSheet, "textures/font[5x7].png", 5, 7, '!', '~', 1, 3, 7, true);
	}

	public static class Sprites
	{
		public static Sprite Test1 = SpriteSheets.SpriteSheet.AddSprite("textures/Character1.png");
		public static Sprite Test2 = SpriteSheets.SpriteSheet.AddSprite("textures/Character2.png");
		public static Sprite Test3 = SpriteSheets.SpriteSheet.AddSprite("textures/Character3.png");
	}

	public static class Textures
	{
		public static Texture Test = Texture.Create2DFromFile("textures/Character1.png");
	}
	#endregion

	public class NETCoreTest : Game
	{
		private static int width = 320;
		private static int height = 180;
		private static int scale = 3;
		//private static int width = 2560;
		//private static int height = 1440;
		//private static int scale = 1;
		//private static int width = 1366;
		//private static int height = 768;
		//private static int scale = 1;
		//private static int width = 16;
		//private static int height = 9;
		//private static int scale = 100;
		//private static int width = 506;
		//private static int height = 253;
		//private static int scale = 1;

		static void Main(string[] args)
		{
			Engine.Init(windowTitle: "NETCoreTest",
						windowWidth: width * scale,
						windowHeight: height * scale,
						windowVSync: true,
						windowFullscreen: false,
						windowResizable: true,
						windowBorderless: false,
						msaa: 0);
			Engine.Run(new NETCoreTest());
		}

		string debugString = "";

		// Variable for moving window on mouse click and drag
		Point2 windowDraggingPosition = Point2.Zero;

		Camera Camera = new Camera();
		Camera2D Camera2D = new Camera2D();
		float cameraYaw = 0f;
		float cameraPitch = 0f;
		Vector3 cameraLookat = Vector3.Forward;
		Vector3 cameraLookatNoVertical = Vector3.Forward;

		Framebuffer surface = Framebuffer.Create(width, height, 1, TextureFormat.RGB8, TextureFormat.Depth24);

		CullModeState cullModeState = CullModeState.Back;
		PolygonModeState polygonModeState = PolygonModeState.Fill;

		public override void Init()
		{
			//Window.SetIcon("textures/icon.png");

			Platform.RelativeMouseMode = true;
			//Platform.ShowCursor = false; // Default true

			Camera.FoV = 75f;
			Camera.AspectRatio = width / (float)height;
			Camera.Position = new Vector3(0f, 2f, -5f);
			//Camera.Projection = Projection.Orthographic;
			//Camera.Width = Window.Width / 10;
			//Camera.Height = Window.Height / 10;
			Camera.zNear = 0.1f;
			Camera.zFar = 150f;
		}

		public override void Update()
		{
			if (Input.Keyboard.Down(KeyCode.Backspace) || Input.Controllers.First.SelectDown)
				Platform.Quit();

			if (Input.Keyboard.Pressed(KeyCode.F11))
				Window.Fullscreen = !Window.Fullscreen;

			if (Input.Keyboard.Pressed(KeyCode.F10))
				Window.Borderless = !Window.Borderless;

			if (Input.Keyboard.Pressed(KeyCode.F8))
				Window.Resizable = !Window.Resizable;

			if (Input.Keyboard.Pressed(KeyCode.F7))
				Window.VSync = !Window.VSync;

			if (Input.Keyboard.Pressed(KeyCode.Escape))
				Platform.RelativeMouseMode = !Platform.RelativeMouseMode;

			if (Input.Keyboard.Pressed(KeyCode.F2))
			{
				if (polygonModeState == PolygonModeState.Fill)
					polygonModeState = PolygonModeState.Line;
				else if (polygonModeState == PolygonModeState.Line)
					polygonModeState = PolygonModeState.Point;
				else if (polygonModeState == PolygonModeState.Point)
					polygonModeState = PolygonModeState.Fill;
			}

			if (Input.Keyboard.Pressed(KeyCode.F3))
			{
				if (cullModeState == CullModeState.Off)
					cullModeState = CullModeState.Back;
				else if (cullModeState == CullModeState.Back)
					cullModeState = CullModeState.Front;
				else if (cullModeState == CullModeState.Front)
					cullModeState = CullModeState.FrontAndBack;
				else if (cullModeState == CullModeState.FrontAndBack)
					cullModeState = CullModeState.Off;
			}

			if (!Platform.RelativeMouseMode)
			{
				if (Input.Mouse.LeftPressed)
					windowDraggingPosition = Input.Mouse.LastPosition;
				else if (Input.Mouse.LeftDown)
					Window.Position = Input.Mouse.PositionDisplay - windowDraggingPosition;
			}

			float speed = 10f;
			if (Input.Keyboard.Down(KeyCode.Z) || Input.Controllers.First.L2Down)
				Camera2D.Rotation += 0.01f;
			if (Input.Keyboard.Down(KeyCode.C) || Input.Controllers.First.R2Down)
				Camera2D.Rotation -= 0.01f;
			if ((Input.Keyboard.Down(KeyCode.Z) && Input.Keyboard.Down(KeyCode.C)) || (Input.Controllers.First.L2Down && Input.Controllers.First.R2Down))
				Camera2D.Rotation = 0f;
			Vector3 direction = Vector3.Zero;
			if (Input.Keyboard.Down(KeyCode.A) || Input.Controllers.First.LeftStickDigitalLeftDown || Input.Controllers.First.LeftDown)
				direction += Vector3.Cross(cameraLookatNoVertical, Vector3.Up).Normalized;
			if (Input.Keyboard.Down(KeyCode.D) || Input.Controllers.First.LeftStickDigitalRightDown || Input.Controllers.First.RightDown)
				direction += Vector3.Cross(Vector3.Up, cameraLookatNoVertical).Normalized;
			if (Input.Keyboard.Down(KeyCode.W) || Input.Controllers.First.LeftStickDigitalUpDown || Input.Controllers.First.UpDown)
				direction += cameraLookatNoVertical;
			if (Input.Keyboard.Down(KeyCode.S) || Input.Controllers.First.LeftStickDigitalDownDown || Input.Controllers.First.DownDown)
				direction -= cameraLookatNoVertical;
			if (Input.Keyboard.Down(KeyCode.Q) || Input.Controllers.First.L1Down)
				direction += Vector3.Down;
			if (Input.Keyboard.Down(KeyCode.E) || Input.Controllers.First.R1Down)
				direction += Vector3.Up;
			if (Input.Mouse.ScrollY != 0)
				Camera.FoV -= Input.Mouse.ScrollY;

			Camera.Position += direction.Normalized * speed * Time.DeltaTime;

			//  Mouse look
			if (Platform.RelativeMouseMode)
			{
				float mouseSpeed = 0.0005f;
				cameraYaw = Math.Clamp(cameraYaw + (Input.Mouse.PositionRelative.Y) * mouseSpeed, -0.249f, 0.249f);
				cameraPitch += (Input.Mouse.PositionRelative.X) * mouseSpeed;
			}

			// Controller look
			float controllerSpeed = 0.01f;
			cameraYaw = Math.Clamp(cameraYaw + (-Input.Controllers.First.RightStickY * controllerSpeed), -0.249f, 0.249f);
			cameraPitch += Input.Controllers.First.RightStickX * controllerSpeed;

			//Camera.Rotation = Quaternion.CreateLookAt(cameraLookat, Vector3.Up);
			Camera.Rotation = Quaternion.CreateRotationY(cameraPitch) * Quaternion.CreateRotationX(cameraYaw);

			cameraLookat = Vector3.Forward * Camera.Rotation;
			cameraLookatNoVertical = new Vector3(cameraLookat.X, 0f, cameraLookat.Z).Normalized;

			debugString = $"|:outline=1,0.01,0,0,0,1:|Cam Pos: {Camera.Position.X:n1}, {Camera.Position.Y:n1}, {Camera.Position.Z:n1}\nCam Rot: {Camera.Rotation.Euler.X:n2}, {Camera.Rotation.Euler.Y:n2}, {Camera.Rotation.Euler.Z:n2}\nMem: {Engine.RAM:n1}MB\nVSync: {Window.GetVSyncMode()}\n{Time.UPS:n0}ups | {Time.FPSSmoothed:n0}fps\nDraw Calls: {Graphics.DrawCalls}\nState Changes: {Graphics.State.Changes}\nFramebuffer Swaps/Blits: {Framebuffer.Swaps}/{Framebuffer.Blits}\nTexture Swaps: {Texture.Swaps}\nShader/Uniform Swaps: {Shader.Swaps}/{Shader.UniformSwaps}\nWinPos: [{Window.X}, {Window.Y}]\nSize: [{Window.Size}]\nMouse Global: [{Input.Mouse.PositionDisplay}]\nMouse: [{Input.Mouse.Position}]\nMouse Relative: [{Input.Mouse.PositionRelative}]";
		}

		public override void Draw()
		{
			surface.Bind();

			// Clear the screen
			if (Input.Keyboard.Down(KeyCode.Space))
				Graphics.Clear(1d);
			else
				Graphics.Clear(Colour.Black);

			Graphics.State.Reset();
			DepthState.LessEqual.Set();
			CullModeState.Set(cullModeState);
			PolygonModeState.Set(polygonModeState);

			Shaders.Renderer.Bind();
			Shaders.Renderer.MVP = Camera.Matrix;

			Renderer.Draw.ResetTransform();
			Renderer.Draw3D.ResetTransform();

			// Start Drawing
			Renderer.Draw3D.Cube(Colour.Green);

			// Draw to Screen
			Framebuffer.Default.Bind();
			Graphics.Clear(new Colour(0.1f, 0.1f, 0.1f, 1f));
			Graphics.State.Reset();

			// Render Framebuffer
			Shaders.Renderer.Bind();
			Shaders.Renderer.MVP = Framebuffer.Default.Matrix;
			scale = Math.Max(1, Math.Min(Window.Width / width, Window.Height / height));
			Renderer.Draw.Framebuffer(surface, TextureAttachment.Colour0, (Window.Width - width * scale) / 2, (Window.Height - height * scale) / 2, scale, Colour.White);

			Renderer.Draw.Text(SpriteFonts.Font,
							   debugString,
							   new Vector2(2, Framebuffer.Current.Height - 1),
							   Vector2.One * 3f,
							   Colour.White,
							   HAlign.Left,
							   VAlign.Top);

			Renderer.Flush();

			// Screenshot
			if (Input.Keyboard.Pressed(KeyCode.F9))
			{
				string s = "X:/Dropbox/Clint/Gamedev/2018-03-22 CKGL/screenshots/";
				if (!System.IO.Directory.Exists(s))
					s = "C:/Users/Clint Kilmer/Dropbox/Clint/Gamedev/2018-03-22 CKGL/screenshots/";

				int sequentialNumber = 1;
				while (System.IO.File.Exists($@"{s}{System.DateTime.Now:yyyy-MM-dd HH.mm.ss}-{sequentialNumber} [CKGL].png"))
				{
					sequentialNumber++;
				}

				surface.SavePNG($@"{s}{System.DateTime.Now:yyyy-MM-dd HH.mm.ss}-{sequentialNumber} [CKGL].png");

				//System.GC.Collect();
			}
		}

		public override void Destroy()
		{
		}

		public override void OnFocusGained()
		{
		}

		public override void OnFocusLost()
		{
		}

		public override void OnWindowResized()
		{
			//surface.Destroy();
			//width = Window.Width;
			//height = Window.Height;
			//surface = Framebuffer.Create(width, height, 1, TextureFormat.RGB8, TextureFormat.Depth24);

			Camera.AspectRatio = surface.AspectRatio;
			//Camera.AspectRatio = Window.AspectRatio;
		}
	}
}