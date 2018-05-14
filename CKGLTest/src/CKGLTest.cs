using CKGL;

namespace CKGLTest
{
	#region Shaders
	public static class Shaders
	{
		#region Test
		public static Shader Test = new Shader(@"
#version 330 core
uniform vec3 offset;
layout(location = 0) in vec3 position;
layout(location = 1) in vec4 colour;
out vec4 vertexColour;
/*mat4 rotationMatrix(vec3 axis, float angle)
{
    axis = normalize(axis);
    float s = sin(angle);
    float c = cos(angle);
    float oc = 1.0 - c;
    
    return mat4(oc * axis.x * axis.x + c,           oc * axis.x * axis.y - axis.z * s,  oc * axis.z * axis.x + axis.y * s,  0.0,
                oc * axis.x * axis.y + axis.z * s,  oc * axis.y * axis.y + c,           oc * axis.y * axis.z - axis.x * s,  0.0,
                oc * axis.z * axis.x - axis.y * s,  oc * axis.y * axis.z + axis.x * s,  oc * axis.z * axis.z + c,           0.0,
                0.0,                                0.0,                                0.0,                                1.0);
}*/
void main()
{
	vertexColour = colour;
	gl_Position = vec4(position.xyz, 1.0);
	//gl_Position = vec4(position.xyz, 1.0) * rotationMatrix(vec3(1.0, 0.0, 0.0), offset.x) * rotationMatrix(vec3(0.0, 1.0, 0.0), offset.y) * rotationMatrix(vec3(0.0, 0.0, 1.0), offset.z);
}
...
#version 330 core
in vec4 vertexColour;
layout(location = 0) out vec4 colour;
void main()
{
	colour = vertexColour;
}");
		#endregion
	}
	#endregion

	#region Sounds
	public static class Sounds
	{
		public static Audio.Buffer sndPop1 = new Audio.Buffer("snd/sndPop1.wav");
		public static Audio.Buffer sndPop2 = new Audio.Buffer("snd/sndPop2.wav");
	}
	#endregion

	#region Sprites
	public static class SpriteSheets
	{
		public static SpriteSheet SpriteSheet;
		//public static SpriteSheet SpriteSheet = new SpriteSheet(128);
	}

	public static class SpriteFonts
	{
		public static SpriteFont Font;
		//public static SpriteFont Font = new SpriteFont(SpriteSheets.SpriteSheet, "Sprites/font[5x7].png", 5, 7, '!', '~', 1, 3, 7, true);
	}

	public static class Sprites
	{
		public static Sprite Test1;
		public static Sprite Test2;
		public static Sprite Test3;
		//public static Sprite Test1 = SpriteSheets.SpriteSheet.AddSprite(Texture2D.CreateFromFile($"Sprites/Character1.png"));
		//public static Sprite Test2 = SpriteSheets.SpriteSheet.AddSprite(Texture2D.CreateFromFile($"Sprites/Character2.png"));
		//public static Sprite Test3 = SpriteSheets.SpriteSheet.AddSprite(Texture2D.CreateFromFile($"Sprites/Character3.png"));
	}

	public static class Textures
	{
		public static Texture2D Test;
		//public static Texture2D Test = Texture2D.CreateFromFile("Sprites/Character1.png");
	}
	#endregion

	public class CKGLTest : Game
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

		public CKGLTest()
			: base(windowTitle: "CKGL Game!",
				   windowWidth: width * scale,
				   windowHeight: height * scale,
				   windowVSync: true,
				   windowFullscreen: false,
				   windowResizable: true,
				   windowBorderless: false)
		{ }

		// Variable for moving window on mouse click and drag
		Point2 windowDraggingPosition = Point2.Zero;

		Camera Camera = new Camera();
		Camera2D Camera2D = new Camera2D();
		float cameraYaw = 0f;
		float cameraPitch = 0f;
		Vector3 cameraLookat = Vector3.Forward;
		Vector3 cameraLookatNoVertical = Vector3.Forward;

		RenderTarget surface;

		public override void Init()
		{
			Platform.RelativeMouseMode = true;
			//Platform.ShowCursor = false; // Default true
			//Platform.ScreensaverAllowed = true; // Default false

			Camera.FoV = 75f;
			Camera.AspectRatio = width / (float)height;
			Camera.Position = new Vector3(0f, 2f, -10f);

			// LoadContent()
			SpriteSheets.SpriteSheet = new SpriteSheet(128);
			SpriteFonts.Font = new SpriteFont(SpriteSheets.SpriteSheet, "Sprites/font[5x7].png", 5, 7, '!', '~', 1, 3, 7, true);
			Sprites.Test1 = SpriteSheets.SpriteSheet.AddSprite(Texture2D.CreateFromFile($"Sprites/Character1.png"));
			Sprites.Test2 = SpriteSheets.SpriteSheet.AddSprite(Texture2D.CreateFromFile($"Sprites/Character2.png"));
			Sprites.Test3 = SpriteSheets.SpriteSheet.AddSprite(Texture2D.CreateFromFile($"Sprites/Character3.png"));
			Textures.Test = Texture2D.CreateFromFile("Sprites/Character1.png");

			// Debug, test spritesheet
			//SpriteSheets.SpriteSheet.Texture.SavePNG($@"{System.IO.Directory.GetCurrentDirectory()}/SpriteSheet.png");
			//SpriteSheets.SpriteSheet.Texture.SavePNG("SpriteSheet.png");

			surface = new RenderTarget(width, height, 1, TextureFormat.RGB8, TextureFormat.Depth24);
		}

		public override void Update()
		{
			//Window.Title = $"{Time.DeltaTime:n1}ms - Info: {Platform.OS} | {Time.TotalSeconds:n1} - Buffers: {Audio.BufferCount} - Sources: {Audio.SourceCount} - Position: [{Window.X}, {Window.Y}] - Size: [{Window.Width}, {Window.Height}] - Mouse: [{Input.Mouse.Position.X}, {Input.Mouse.Position.Y}]";
			//Window.Title = $"{Time.DeltaTime:n1}ms | Position: [{Window.X}, {Window.Y}] | Size: [{Window.Width}, {Window.Height}] | Mouse: [{Input.Mouse.Position.X}, {Input.Mouse.Position.Y}]";
			Window.Title = $"Mem: {RAM:n1}MB | VSync: {Window.GetVSyncMode()} | {Time.UPS:n0}ups | {Time.FPSSmoothed:n0}fps | Draw Calls: {Graphics.DrawCalls} | State Changes: {Graphics.State.Changes} | RenderTarget Swaps: {RenderTarget.Swaps} | Texture Swaps: {Texture.Swaps} | Shader Swaps: {Shader.Swaps} | WinPos: [{Window.X}, {Window.Y}] | Size: [{Window.Size}] | Mouse Global: [{Input.Mouse.PositionDisplay}] | Mouse: [{Input.Mouse.Position}] | Mouse Relative: [{Input.Mouse.PositionRelative}]";

			if (Input.Keyboard.Down(KeyCode.Backspace))
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

			if (!Platform.RelativeMouseMode)
			{
				if (Input.Mouse.LeftPressed)
					windowDraggingPosition = Input.Mouse.LastPosition;
				else if (Input.Mouse.LeftDown)
					Window.Position = Input.Mouse.PositionDisplay - windowDraggingPosition;
			}

			if (Input.Keyboard.Pressed(KeyCode.Space) || Input.Mouse.LeftPressed)
				Sounds.sndPop1.Play();
			if (Input.Keyboard.Released(KeyCode.Space) || Input.Mouse.LeftReleased)
				Sounds.sndPop2.Play();

			var speed = 10f;
			if (Input.Keyboard.Down(KeyCode.Z))
				Camera2D.Rotation += 0.01f;
			if (Input.Keyboard.Down(KeyCode.C))
				Camera2D.Rotation -= 0.01f;
			if (Input.Keyboard.Down(KeyCode.Z) && Input.Keyboard.Down(KeyCode.C))
				Camera2D.Rotation = 0f;
			Vector3 direction = Vector3.Zero;
			if (Input.Keyboard.Down(KeyCode.A))
				direction += Vector3.Cross(cameraLookatNoVertical, Vector3.Up).Normalized;
			//direction += Vector3.Left;
			if (Input.Keyboard.Down(KeyCode.D))
				direction += Vector3.Cross(Vector3.Up, cameraLookatNoVertical).Normalized;
			//direction += Vector3.Right;
			if (Input.Keyboard.Down(KeyCode.W))
				direction += cameraLookatNoVertical;
			//direction += Vector3.Forward;
			if (Input.Keyboard.Down(KeyCode.S))
				direction -= cameraLookatNoVertical;
			//direction += Vector3.Backward;
			//cameraTranslationMatrix = Matrix.CreateTranslation(-cameraPosition);
			if (Input.Keyboard.Down(KeyCode.Q))
				direction += Vector3.Down;
			//cameraScale -= 0.03f * cameraScale;
			if (Input.Keyboard.Down(KeyCode.E))
				direction += Vector3.Up;
			//cameraScale += 0.03f * cameraScale;
			if (Input.Mouse.ScrollY != 0)
				Camera.FoV -= Input.Mouse.ScrollY;

			Camera.Position += direction.Normalized * speed * Time.DeltaTime;

			if (Platform.RelativeMouseMode)
			{
				var mouseSpeed = 0.0005f;
				cameraYaw = Math.Clamp(cameraYaw + (Input.Mouse.PositionRelative.Y) * mouseSpeed, -0.249f, 0.249f);
				cameraPitch += (Input.Mouse.PositionRelative.X) * mouseSpeed;
			}

			//Camera.Rotation = Quaternion.CreateLookAt(cameraLookat, Vector3.Up);
			Camera.Rotation = Quaternion.CreateRotationY(cameraPitch) * Quaternion.CreateRotationX(cameraYaw);

			cameraLookat = Vector3.Forward * Camera.Rotation;
			cameraLookatNoVertical = new Vector3(cameraLookat.X, 0f, cameraLookat.Z).Normalized;
		}

		public override void Draw()
		{
			RenderTarget.Default.Bind();
			Graphics.Clear(new Colour(0.1f, 0.1f, 0.1f, 1f));

			surface.Bind();

			// Clear the screen
			if (Input.Keyboard.Down(KeyCode.Space))
				Graphics.ClearDepth();
			else
				Graphics.Clear(Colour.Black);

			//Graphics.State.SetFrontFaceState(FrontFaceState.Clockwise);
			//Graphics.State.SetCullState(CullState.Back);
			Graphics.State.SetPolygonModeState(PolygonModeState.FrontFillBackLine);
			Graphics.State.SetBlendState(BlendState.AlphaBlend);
			Graphics.State.SetDepthState(DepthState.LessEqual);

			InternalShaders.Renderer.MVP = Camera.Matrix;

			Renderer.Draw.ResetTransform();
			Renderer.Draw3D.ResetTransform();

			// Start Drawing

			Transform2D t2D = new Transform2D();
			t2D.Rotation = Math.Sin(Time.TotalSeconds) * 0.03f;
			Renderer.Draw.SetTransform(t2D);

			Transform t = new Transform();
			t.Rotation = Quaternion.CreateRotationZ(Math.Sin(Time.TotalSeconds) * 0.01f);
			Renderer.Draw3D.SetTransform(t);

			//Graphics.State.SetBlendState(BlendState.Additive);
			Colour gridColour = Colour.Grey.Alpha(0.3f);
			int length = 100;
			//for (int yy = -length; yy <= length; yy++)
			//for (int yy = 0; yy <= 0; yy++)
			//{
			//float yy = cameraPosition.Y - 2f;
			float yy = -5f;
			for (int i = 0; i <= length; i++)
			{
				Renderer.Draw3D.Line(new Vector3(-length, yy, i),
									 new Vector3(length, yy, i),
									 gridColour);
				if (i != 0)
					Renderer.Draw3D.Line(new Vector3(-length, yy, -i),
										 new Vector3(length, yy, -i),
										 gridColour);

				Renderer.Draw3D.Line(new Vector3(i, yy, -length),
									 new Vector3(i, yy, length),
									 gridColour);
				if (i != 0)
					Renderer.Draw3D.Line(new Vector3(-i, yy, -length),
									 new Vector3(-i, yy, length),
									 gridColour);
			}
			Renderer.Draw3D.Line(new Vector3(-length, yy, -length),
								 new Vector3(-length, yy + length, -length),
								 gridColour);
			Renderer.Draw3D.Line(new Vector3(length, yy, -length),
								 new Vector3(length, yy + length, -length),
								 gridColour);
			Renderer.Draw3D.Line(new Vector3(-length, yy, length),
								 new Vector3(-length, yy + length, length),
								 gridColour);
			Renderer.Draw3D.Line(new Vector3(length, yy, length),
								 new Vector3(length, yy + length, length),
								 gridColour);
			//}
			//for (int y = -length; y <= length; y++)
			//{
			//	for (int x = -length; x <= length; x++)
			//	{
			//		for (int z = -length; z <= length; z++)
			//		{
			//			Renderer.Draw.Points.Point(new Vector3(x, y, z), gridColour);
			//		}
			//	}
			//}

			Renderer.Draw.Triangle(new Vector2(0f, 1f),
					   new Vector2(0f, 1f) * Matrix2D.CreateRotationZ(Rotation.Third),
					   new Vector2(0f, 1f) * Matrix2D.CreateRotationZ(Rotation.TwoThirds),
					   Colour.Red,
					   Colour.Green,
					   Colour.Blue,
					   null,
					   null,
					   null,
					   -Time.TotalSeconds * 0.5f,
					   Vector2.Zero);

			// Right
			Renderer.Draw3D.Triangle(new Vector3(20f, 10f, 0f),
									 new Vector3(20f, 10f, 0f) * Quaternion.CreateRotationX(Rotation.Third),
									 new Vector3(20f, 10f, 0f) * Quaternion.CreateRotationX(Rotation.TwoThirds),
									 Colour.Red,
									 Colour.Green,
									 Colour.Blue);
			Renderer.Draw3D.Triangle(new Vector3(50f, 50f, 0f),
									 new Vector3(50f, 50f, 0f) * Quaternion.CreateRotationX(Rotation.Third),
									 new Vector3(50f, 50f, 0f) * Quaternion.CreateRotationX(Rotation.TwoThirds),
									 Colour.Red,
									 Colour.Green,
									 Colour.Blue);

			// Left
			Renderer.Draw3D.Triangle(new Vector3(-20f, 10f, 0f),
									 new Vector3(-20f, 10f, 0f) * Quaternion.CreateRotationX(Rotation.TwoThirds),
									 new Vector3(-20f, 10f, 0f) * Quaternion.CreateRotationX(Rotation.Third),
									 Colour.Red,
									 Colour.Green,
									 Colour.Blue);
			Renderer.Draw3D.Triangle(new Vector3(-50f, 50f, 0f),
									 new Vector3(-50f, 50f, 0f) * Quaternion.CreateFromAxisAngle(Vector3.Left, Rotation.Third),
									 new Vector3(-50f, 50f, 0f) * Quaternion.CreateFromAxisAngle(Vector3.Left, Rotation.TwoThirds),
									 Colour.Red,
									 Colour.Green,
									 Colour.Blue);

			// Forward
			//Renderer.Draw3D.Triangle(new Vector3(0f, 2f, 10f),
			//						 new Vector3(0f, 2f, 10f) * Quaternion.CreateRotationZ(Rotation.Third),
			//						 new Vector3(0f, 2f, 10f) * Quaternion.CreateRotationZ(Rotation.TwoThirds),
			//						 Colour.Red,
			//						 Colour.Green,
			//						 Colour.Blue);
			//Renderer.Draw3D.Triangle(new Vector3(0f, 10f, 20f),
			//						 new Vector3(0f, 10f, 20f) * Quaternion.CreateRotationZ(Rotation.Third),
			//						 new Vector3(0f, 10f, 20f) * Quaternion.CreateRotationZ(Rotation.TwoThirds),
			//						 Colour.Red,
			//						 Colour.Green,
			//						 Colour.Blue);
			//Renderer.Draw3D.Triangle(new Vector3(0f, 50f, 50f),
			//						 new Vector3(0f, 50f, 50f) * Quaternion.CreateRotationZ(Rotation.Third).Matrix,
			//						 new Vector3(0f, 50f, 50f) * Quaternion.CreateRotationZ(Rotation.TwoThirds).Matrix,
			//						 Colour.Red,
			//						 Colour.Green,
			//						 Colour.Blue);
			for (int i = 0; i < 500; i++)
				Renderer.Draw3D.Triangle(new Vector3(0f, i * 0.1f, i * 0.1f) * Quaternion.CreateRotationZ(Rotation.Zero + i * 0.001f),
										 new Vector3(0f, i * 0.1f, i * 0.1f) * Quaternion.CreateRotationZ(Rotation.Third + i * 0.001f),
										 new Vector3(0f, i * 0.1f, i * 0.1f) * Quaternion.CreateRotationZ(Rotation.TwoThirds + i * 0.001f),
										 Colour.Red,
										 Colour.Green,
										 Colour.Blue);

			// Backward
			Renderer.Draw3D.Triangle(new Vector3(0f, 10f, -20f),
									 new Vector3(0f, 10f, -20f) * Quaternion.CreateRotationZ(Rotation.TwoThirds),
									 new Vector3(0f, 10f, -20f) * Quaternion.CreateRotationZ(Rotation.Third),
									 Colour.Red,
									 Colour.Green,
									 Colour.Blue);
			Renderer.Draw3D.Triangle(new Vector3(0f, 50f, -50f),
									 new Vector3(0f, 50f, -50f) * Quaternion.CreateFromAxisAngle(Vector3.Backward, Rotation.Third),
									 new Vector3(0f, 50f, -50f) * Quaternion.CreateFromAxisAngle(Vector3.Backward, Rotation.TwoThirds),
									 Colour.Red,
									 Colour.Green,
									 Colour.Blue);

			// Up
			Renderer.Draw3D.Triangle(new Vector3(0f, 20f, -10f),
									 new Vector3(0f, 20f, -10f) * Quaternion.CreateRotationY(Rotation.Third),
									 new Vector3(0f, 20f, -10f) * Quaternion.CreateRotationY(Rotation.TwoThirds),
									 Colour.Red,
									 Colour.Green,
									 Colour.Blue);
			Renderer.Draw3D.Triangle(new Vector3(0f, 50f, -50f),
									 new Vector3(0f, 50f, -50f) * Quaternion.CreateRotationY(Rotation.Third),
									 new Vector3(0f, 50f, -50f) * Quaternion.CreateRotationY(Rotation.TwoThirds),
									 Colour.Red,
									 Colour.Green,
									 Colour.Blue);

			// Down
			Renderer.Draw3D.Triangle(new Vector3(0f, -20f, -10f),
									 new Vector3(0f, -20f, -10f) * Quaternion.CreateRotationY(Rotation.TwoThirds),
									 new Vector3(0f, -20f, -10f) * Quaternion.CreateRotationY(Rotation.Third),
									 Colour.Red,
									 Colour.Green,
									 Colour.Blue);
			Renderer.Draw3D.Triangle(new Vector3(0f, -50f, -50f),
									 new Vector3(0f, -50f, -50f) * Quaternion.CreateFromAxisAngle(Vector3.Down, Rotation.Third),
									 new Vector3(0f, -50f, -50f) * Quaternion.CreateFromAxisAngle(Vector3.Down, Rotation.TwoThirds),
									 Colour.Red,
									 Colour.Green,
									 Colour.Blue);

			Textures.Test.Bind();
			Renderer.Draw.Rectangle(2f,
									-0.5f,
									1f,
									1f,
									Colour.White,
									Colour.White,
									Colour.White,
									Colour.White,
									new Vector2(0f, 0f),
									new Vector2(1f, 0f),
									new Vector2(0f, 1f),
									new Vector2(1f, 1f),
									-Time.TotalSeconds * 0.5f,
									new Vector2(2.5f, 0f));

			Renderer.Draw.Sprite(Sprites.Test1,
								 new Vector2(4f, -0.5f),
								 Vector2.One / 8f,
								 Colour.White);

			Renderer.Draw.Sprite(Sprites.Test2,
								 new Vector2(6f, -0.5f),
								 Vector2.One / 8f,
								 Colour.White);

			Renderer.Draw.Sprite(Sprites.Test3,
								 new Vector2(8f, -0.5f),
								 Vector2.One / 8f,
								 Colour.White);

			Renderer.Draw.Text(SpriteFonts.Font,
							   "|:shadow=0,-1,0.01,0,0,0,0.5:|ABCDEFGHIJKLMNOPQRSTUVWXYZ\nabcdefghijklmnopqrstuvwxyz\n1234567890\n_-+=(){}[]<>\\|/;:'\"?.,!@#$%^&*~`",
							   new Vector2(0f, 4f),
							   Vector2.One / 7f,
							   Colour.White,
							   HAlign.Center,
							   VAlign.Middle);

			Renderer.Draw.ResetTransform();

			Renderer.Flush();

			Graphics.State.SetDepthState(DepthState.Off);

			Camera2D.Width = RenderTarget.Current.Width;
			Camera2D.Height = RenderTarget.Current.Height;
			InternalShaders.Renderer.MVP = Camera2D.Matrix;

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

			//Renderer.Draw.TriangleListStrip.Begin();
			////int ii = Random.Range(1000, 10000);
			//for (int i = 0; i < 10000; i++)
			//	Renderer.Draw.TriangleListStrip.AddVertex(new Vector3(Random.Range(-100f, 100f), Random.Range(-100f, 100f), 0.0f),
			//										  new Colour(Random.Range(1f), Random.Range(1f), Random.Range(1f), Random.Range(1f)));
			//Renderer.Draw.TriangleListStrip.End();

			// TODO - LineListStrip
			//Renderer.Draw.LineListStrip.Begin();
			//for (int i = 0; i < 10; i++)
			//	Renderer.Draw.LineListStrip.AddVertex(new Vector3(Random.Range(-6f, -2f), Random.Range(-2f, 2f), 0.0f),
			//									  new Colour(Random.Range(1f), Random.Range(1f), Random.Range(1f), Random.Range(1f)));
			//Renderer.Draw.LineListStrip.End();

			//Renderer.Draw.Circle(new Vector2(0f, -8f), 10f, Colour.Green, Colour.Green.Alpha(0.1f), (int)Math.Lerp(8f, 64f, Math.SinNormalized(Time.TotalSeconds)));

			//for (int i = 0; i < 10000; i++)
			//	Renderer.Draw.PolyPoint(new Vector2(Random.Range(0, width), Random.Range(0, height)),
			//							new Colour(Random.Range(1f), Random.Range(1f), Random.Range(1f), 1f));

			//RenderTarget.Bind(null);
			//Renderer.Draw.RenderTarget(surface, 0,
			//						   -5f, 5f, 0.1f,
			//						   Colour.White);

			Renderer.Draw.Text(SpriteFonts.Font,
							   $"|:shadow=0,-1,0.01,0,0,0,0.5:|Cam Pos: {Camera.Position.X:n1}, {Camera.Position.Y:n1}, {Camera.Position.Z:n1}\nCam Rot: {Camera.Rotation.Euler.X:n2}, {Camera.Rotation.Euler.Y:n2}, {Camera.Rotation.Euler.Z:n2}\nMem: {RAM:n1}MB\nVSync: {Window.GetVSyncMode()}\n{Time.UPS:n0}ups | {Time.FPSSmoothed:n0}fps\nDraw Calls: {Graphics.DrawCalls}\nState Changes: {Graphics.State.Changes}\nRenderTarget Swaps: {RenderTarget.Swaps}\nTexture Swaps: {Texture.Swaps}\nShader Swaps: {Shader.Swaps}\nWinPos: [{Window.X}, {Window.Y}]\nSize: [{Window.Size}]\nMouse Global: [{Input.Mouse.PositionDisplay}]\nMouse: [{Input.Mouse.Position}]\nMouse Relative: [{Input.Mouse.PositionRelative}]",
							   new Vector2(2, RenderTarget.Current.Height - 1),
							   Vector2.One,
							   Colour.White,
							   HAlign.Left,
							   VAlign.Top);

			Renderer.Flush();

			scale = Math.Max(1, Math.Min(Window.Width / width, Window.Height / height));
			surface.BlitTextureTo(RenderTarget.Default, 0, BlitFilter.Nearest, new RectangleI((Window.Width - width * scale) / 2, (Window.Height - height * scale) / 2, width * scale, height * scale));

			// Screenshot
			if (Input.Keyboard.Pressed(KeyCode.F9))
			{
				string s = @"X:\Dropbox\Clint\Gamedev\2018-03-22 CKGL\screenshots\";
				//string s = @"C:\Users\Clint Kilmer\Dropbox\Clint\Gamedev\2018-03-22 CKGL\screenshots\";

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
			//ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(Math.DegreesToRadians(75f), Window.AspectRatio, 0.1f, 1000f);
		}
	}
}