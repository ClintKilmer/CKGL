using CKGL;

namespace CKGLTest2D
{
	#region Layer
	public static class Layer
	{
		public static float Debug { get; } = -1;
		public static float Shadow { get; } = 0;
		public static float Box { get; } = 1;
		public static float Global { get; } = 2;
		public static float Player { get; } = 3;
		public static float Tri { get; } = 4;
	}
	#endregion

	#region Shaders
	public static class Shaders
	{
		public static InternalShaders.RendererShader Renderer = new InternalShaders.RendererShader();
	}
	#endregion

	#region Sounds
	public static class Sounds
	{
		//public static Audio.Buffer sndPop1 = new Audio.Buffer("snd/sndPop1.wav");
		//public static Audio.Buffer sndPop2 = new Audio.Buffer("snd/sndPop2.wav");
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
		public static Sprite Test1 = SpriteSheets.SpriteSheet.AddSprite(Texture2D.CreateFromFile($"textures/Character1.png"));
		public static Sprite Test2 = SpriteSheets.SpriteSheet.AddSprite(Texture2D.CreateFromFile($"textures/Character2.png"));
		public static Sprite Test3 = SpriteSheets.SpriteSheet.AddSprite(Texture2D.CreateFromFile($"textures/Character3.png"));
	}

	public static class Textures
	{
		public static Texture2D Test = Texture2D.CreateFromFile("textures/Character1.png");
	}
	#endregion

	public class CKGLTest2D : Game
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

		public CKGLTest2D()
			: base(windowTitle: "CKGL Game!",
				   windowWidth: width * scale,
				   windowHeight: height * scale,
				   windowVSync: true,
				   windowFullscreen: false,
				   windowResizable: true,
				   windowBorderless: false,
				   msaa: 0)
		{ }

		string debugString = "";

		public static Camera2D Camera = new Camera2D();

		RenderTarget surface;

		public static Player Player;

		public override void Init()
		{
			Window.SetIcon("textures/Character1.png");

			//Platform.RelativeMouseMode = true;
			//Platform.ShowCursor = false; // Default true

			Shadow shadow = new Shadow();

			Player = new Player { Position = new Vector2(2f, 2f) };

			for (int i = 0; i < 20; i++)
			{
				new Tri { X = i * 20, Y = -i * 20/*, Depth = -i * 20*/ };
			}

			for (int i = 0; i < 20; i++)
			{
				new Box { X = Random.Range(-width, width) / 8 * 8, Y = Random.Range(-height, height) / 8 * 8 };
			}

			if (Platform.GraphicsBackend == GraphicsBackend.OpenGLES)
				surface = new RenderTarget(width, height, 1, TextureFormat.RGB8, TextureFormat.Depth16);
			else
				surface = new RenderTarget(width, height, 1, TextureFormat.RGB8, TextureFormat.Depth24);
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

			debugString = $"|:outline=1,0.01,0,0,0,1:|Cam Pos: {Camera.Position.X:n1}, {Camera.Position.Y:n1}\nCam Rot: {Camera.Rotation:n2}\nMem: {RAM:n1}MB\nVSync: {Window.GetVSyncMode()}\n{Time.UPS:n0}ups | {Time.FPSSmoothed:n0}fps\nDraw Calls: {Graphics.DrawCalls}\nState Changes: {Graphics.State.Changes}\nRenderTarget Swaps/Blits: {RenderTarget.Swaps}/{RenderTarget.Blits}\nTexture Swaps: {Texture.Swaps}\nShader/Uniform Swaps: {Shader.Swaps}/{Shader.UniformSwaps}\nWinPos: [{Window.X}, {Window.Y}]\nSize: [{Window.Size}]\nMouse Global: [{Input.Mouse.PositionDisplay}]\nMouse: [{Input.Mouse.Position}]\nMouse Relative: [{Input.Mouse.PositionRelative}]";

			Scene?.BeforeUpdate();
			Scene?.Update();
			Scene?.AfterUpdate();
		}

		public override void Draw()
		{
			surface.Bind();

			// Clear the screen
			if (Input.Keyboard.Down(KeyCode.Space))
				Graphics.Clear(1d);
			else
				Graphics.Clear(new Colour(0.2f, 0.2f, 0.2f, 1f));

			// Reset
			Graphics.State.Reset();

			Camera.Width = RenderTarget.Current.Width;
			Camera.Height = RenderTarget.Current.Height;
			Shaders.Renderer.Bind();
			Shaders.Renderer.MVP = Camera.Matrix;

			Renderer.Draw.ResetTransform();
			Renderer.Draw3D.ResetTransform();

			// Start Drawing
			Scene?.Draw();

			//Renderer.Draw.Text(SpriteFonts.Font,
			//				   "|:shadow=0,-1,0.01,0,0,0,0.5:|Test Test\nStill testing...\nhhhheeeelllloooo",
			//				   new Vector2(2, height - 1),
			//				   Vector2.One * (1f + Math.SinNormalized(Time.TotalSeconds * 2f)),
			//				   Colour.White,
			//				   HAlign.Left,
			//				   VAlign.Top);

			//Renderer.Draw.Text(SpriteFonts.Font,
			//				   $"|:shadow=0,-1,0.01,0,0,0,0.5:|{Camera.Rotation}\nStill testing...",
			//				   new Vector2(2, 1),
			//				   Vector2.One * (1f + Math.SinNormalized(Time.TotalSeconds * 2f)),
			//				   Colour.White,
			//				   HAlign.Left,
			//				   VAlign.Bottom);

			//Renderer.Draw.Text(SpriteFonts.Font,
			//				   "|:shadow=0,-1,0.01,0,0,0,0.5:|Test Test",
			//				   new Vector2(width - 1, 1),
			//				   Vector2.One * (1f + Math.SinNormalized(Time.TotalSeconds * 2f)),
			//				   Colour.White,
			//				   HAlign.Right,
			//				   VAlign.Bottom);

			//Renderer.Draw.Text(SpriteFonts.Font,
			//				   "|:shadow=0,-1,0.01,0,0,0,0.5:|Test Test\nStill testing...\nhhhheeeelllloooo",
			//				   new Vector2(width - 1, height - 1),
			//				   Vector2.One * (1f + Math.SinNormalized(Time.TotalSeconds * 2f)),
			//				   Colour.White,
			//				   HAlign.Right,
			//				   VAlign.Top);

			//Renderer.Draw.Text(SpriteFonts.Font,
			//				   "|:shadow=0,-1,0.01,0,0,0,0.5:|Test Test\nStill testing...",
			//				   new Vector2(width / 2, height / 2),
			//				   Vector2.One * (1f + Math.SinNormalized(Time.TotalSeconds * 2f)),
			//				   Colour.White,
			//				   HAlign.Center,
			//				   VAlign.Middle);

			//Renderer.Draw.Text(SpriteFonts.Font,
			//				   "|:shadow=0,-1,0.01,0,0,0,0.5:|Test Test\nStill testing...",
			//				   new Vector2(width / 2, height / 2 + 50),
			//				   Vector2.One * (1f + Math.SinNormalized(Time.TotalSeconds * 2f) * 3f),
			//				   Colour.White,
			//				   HAlign.Center,
			//				   VAlign.Middle);

			//Renderer.Draw.Text(SpriteFonts.Font,
			//				   "|:shadow=0,-1,0.01,0,0,0,0.5:|Test Test\nStill testing...",
			//				   new Vector2(width / 2, height / 2 - 50),
			//				   Vector2.One * (1f + Math.SinNormalized(Time.TotalSeconds * 2f) * 2f),
			//				   Colour.White,
			//				   HAlign.Center,
			//				   VAlign.Middle);

			// Draw to Screen
			RenderTarget.Default.Bind();
			Graphics.Clear(new Colour(0.1f, 0.1f, 0.1f, 1f));
			Graphics.State.Reset();

			scale = Math.Max(1, Math.Min(Window.Width / width, Window.Height / height));

			// Render RenderTarget
			Shaders.Renderer.Bind();
			Shaders.Renderer.MVP = RenderTarget.Default.Matrix;
			Renderer.Draw.RenderTarget(surface, TextureSlot.Colour0, (Window.Width - width * scale) / 2, (Window.Height - height * scale) / 2, scale, Colour.White);
			//Renderer.Draw.RenderTarget(surface, TextureSlot.Colour0, (Window.Width - width * scale) / 2, (Window.Height - height * scale) / 2, scale, Math.Sin(Time.TotalSeconds) * 0.03f, new Vector2(Window.Width / 2f, Window.Height / 2f), Colour.White);

			// Blit RenderTarget
			//surface.BlitTextureTo(RenderTarget.Default, TextureSlot.Colour0, BlitFilter.Nearest, new RectangleI((Window.Width - width * scale) / 2, (Window.Height - height * scale) / 2, width * scale, height * scale));

			Renderer.Draw.Text(SpriteFonts.Font,
							   debugString,
							   new Vector2(2, RenderTarget.Current.Height - 1),
							   Vector2.One * 3f,
							   Colour.White,
							   HAlign.Left,
							   VAlign.Top);

			Renderer.Flush();

			// Screenshot
			if (Input.Keyboard.Pressed(KeyCode.F9))
			{
				string s = @"X:\Dropbox\Clint\Gamedev\2018-03-22 CKGL\screenshots\";
				if (!System.IO.Directory.Exists(s))
					s = @"C:\Users\Clint Kilmer\Dropbox\Clint\Gamedev\2018-03-22 CKGL\screenshots\";

				int sequentialNumber = 1;
				while (System.IO.File.Exists($@"{s}{System.DateTime.Now:yyyy-MM-dd HH.mm.ss}-{sequentialNumber} [CKGL].png"))
				{
					sequentialNumber++;
				}

				surface.textures[0].SavePNG($@"{s}{System.DateTime.Now:yyyy-MM-dd HH.mm.ss}-{sequentialNumber} [CKGL].png");

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

			//if (Platform.GraphicsBackend == GraphicsBackend.OpenGLES)
			//	surface = new RenderTarget(width, height, 1, TextureFormat.RGB8, TextureFormat.Depth16);
			//else
			//	surface = new RenderTarget(width, height, 1, TextureFormat.RGB8, TextureFormat.Depth24);
		}
	}
}