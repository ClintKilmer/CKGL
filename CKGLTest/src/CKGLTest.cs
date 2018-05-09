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
		//private static int width = 1778;
		//private static int height = 1000;
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

		Vector3 cameraPosition = new Vector3(0f, 0f, -10f);
		Vector3 cameraLookat = Vector3.Forward;
		Vector3 cameraScale = Vector3.One;
		float cameraRotationZ = 0f; // 2d only
		float cameraYaw = 0f;
		float cameraPitch = 0f;
		Matrix cameraRotationMatrix = Matrix.Identity;
		Matrix cameraTranslationMatrix = Matrix.Identity;

		Matrix ViewMatrix = Matrix.Identity;
		Matrix ProjectionMatrix = Matrix.Identity;

		RenderTarget surface;

		public override void Init()
		{
			Platform.RelativeMouseMode = true;
			//Platform.ShowCursor = false; // Default true
			//Platform.ScreensaverAllowed = true; // Default false

			//ProjectionMatrix = Matrix.CreateOrthographic(Window.Size, -10000f, 10000f);
			ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(Math.DegreesToRadians(75f), width / (float)height, 0.1f, 1000f);

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
				cameraRotationZ -= 0.01f;
			if (Input.Keyboard.Down(KeyCode.C))
				cameraRotationZ += 0.01f;
			Vector3 direction = Vector3.Zero;
			if (Input.Keyboard.Down(KeyCode.A))
				direction += Vector3.Cross(Vector3.Up, cameraLookat).Normalized;
			if (Input.Keyboard.Down(KeyCode.D))
				direction += Vector3.Cross(cameraLookat, Vector3.Up).Normalized;
			Vector3 cameraLookatNoVertical = new Vector3(cameraLookat.X, 0f, cameraLookat.Z).Normalized;
			if (Input.Keyboard.Down(KeyCode.W))
				direction += cameraLookatNoVertical;
			if (Input.Keyboard.Down(KeyCode.S))
				direction -= cameraLookatNoVertical;
			//cameraTranslationMatrix = Matrix.CreateTranslation(-cameraPosition);
			if (Input.Keyboard.Down(KeyCode.Q))
				direction += Vector3.Down;
			//cameraScale -= 0.03f * cameraScale;
			if (Input.Keyboard.Down(KeyCode.E))
				direction += Vector3.Up;
			//cameraScale += 0.03f * cameraScale;

			cameraPosition += direction.Normalized * speed * Time.DeltaTime;

			if (Platform.RelativeMouseMode)
			{
				var mouseSpeed = 0.0005f;
				cameraYaw = Math.Clamp(cameraYaw - (Input.Mouse.PositionRelative.Y) * mouseSpeed, -0.249f, 0.249f);
				cameraPitch = cameraPitch + (Input.Mouse.PositionRelative.X) * mouseSpeed;
			}

			cameraLookat = Vector3.Forward * (Matrix.CreateRotationX(cameraYaw) * Matrix.CreateRotationY(cameraPitch));

			//ViewMatrix = cameraRotationMatrix * cameraTranslationMatrix;
			ViewMatrix = Matrix.CreateLookAt(cameraPosition, cameraPosition + cameraLookat, Vector3.Up);
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

			InternalShaders.Renderer.MVP = Matrix.Model * ViewMatrix * ProjectionMatrix;

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
					   new Vector2(0f, 1f) * Matrix2D.CreateRotationZ(0.66666f),
					   new Vector2(0f, 1f) * Matrix2D.CreateRotationZ(0.33333f),
					   Colour.Red,
					   Colour.Green,
					   Colour.Blue,
					   null,
					   null,
					   null,
					   Time.TotalSeconds * 0.5f,
					   Vector2.Zero);

			// Right
			Renderer.Draw3D.Triangle(new Vector3(20f, 10f, 0f),
									 new Vector3(20f, 10f, 0f) * Quaternion.CreateRotationX(0.66666f),
									 new Vector3(20f, 10f, 0f) * Quaternion.CreateRotationX(0.33333f),
									 Colour.Red,
									 Colour.Green,
									 Colour.Blue);
			Renderer.Draw3D.Triangle(new Vector3(50f, 50f, 0f),
									 new Vector3(50f, 50f, 0f) * Quaternion.CreateRotationX(0.66666f),
									 new Vector3(50f, 50f, 0f) * Quaternion.CreateRotationX(0.33333f),
									 Colour.Red,
									 Colour.Green,
									 Colour.Blue);

			// Left
			Renderer.Draw3D.Triangle(new Vector3(-20f, 10f, 0f),
									 new Vector3(-20f, 10f, 0f) * Quaternion.CreateRotationX(0.33333f),
									 new Vector3(-20f, 10f, 0f) * Quaternion.CreateRotationX(0.66666f),
									 Colour.Red,
									 Colour.Green,
									 Colour.Blue);
			Renderer.Draw3D.Triangle(new Vector3(-50f, 50f, 0f),
									 new Vector3(-50f, 50f, 0f) * Quaternion.CreateFromAxisAngle(Vector3.Left, 0.66666f),
									 new Vector3(-50f, 50f, 0f) * Quaternion.CreateFromAxisAngle(Vector3.Left, 0.33333f),
									 Colour.Red,
									 Colour.Green,
									 Colour.Blue);

			// Forward
			Renderer.Draw3D.Triangle(new Vector3(0f, 10f, 20f),
									 new Vector3(0f, 10f, 20f) * Quaternion.CreateRotationZ(0.66666f),
									 new Vector3(0f, 10f, 20f) * Quaternion.CreateRotationZ(0.33333f),
									 Colour.Red,
									 Colour.Green,
									 Colour.Blue);
			Renderer.Draw3D.Triangle(new Vector3(0f, 50f, 50f),
									 new Vector3(0f, 50f, 50f) * Quaternion.CreateRotationZ(0.66666f),
									 new Vector3(0f, 50f, 50f) * Quaternion.CreateRotationZ(0.33333f),
									 Colour.Red,
									 Colour.Green,
									 Colour.Blue);

			// Backward
			Renderer.Draw3D.Triangle(new Vector3(0f, 10f, -20f),
									 new Vector3(0f, 10f, -20f) * Quaternion.CreateRotationZ(0.33333f),
									 new Vector3(0f, 10f, -20f) * Quaternion.CreateRotationZ(0.66666f),
									 Colour.Red,
									 Colour.Green,
									 Colour.Blue);
			Renderer.Draw3D.Triangle(new Vector3(0f, 50f, -50f),
									 new Vector3(0f, 50f, -50f) * Quaternion.CreateFromAxisAngle(Vector3.Backward, 0.66666f),
									 new Vector3(0f, 50f, -50f) * Quaternion.CreateFromAxisAngle(Vector3.Backward, 0.33333f),
									 Colour.Red,
									 Colour.Green,
									 Colour.Blue);

			// Up
			Renderer.Draw3D.Triangle(new Vector3(0f, 20f, -10f),
									 new Vector3(0f, 20f, -10f) * Quaternion.CreateRotationY(0.66666f),
									 new Vector3(0f, 20f, -10f) * Quaternion.CreateRotationY(0.33333f),
									 Colour.Red,
									 Colour.Green,
									 Colour.Blue);
			Renderer.Draw3D.Triangle(new Vector3(0f, 50f, -50f),
									 new Vector3(0f, 50f, -50f) * Quaternion.CreateRotationY(0.66666f),
									 new Vector3(0f, 50f, -50f) * Quaternion.CreateRotationY(0.33333f),
									 Colour.Red,
									 Colour.Green,
									 Colour.Blue);

			// Down
			Renderer.Draw3D.Triangle(new Vector3(0f, -20f, -10f),
									 new Vector3(0f, -20f, -10f) * Quaternion.CreateRotationY(0.33333f),
									 new Vector3(0f, -20f, -10f) * Quaternion.CreateRotationY(0.66666f),
									 Colour.Red,
									 Colour.Green,
									 Colour.Blue);
			Renderer.Draw3D.Triangle(new Vector3(0f, -50f, -50f),
									 new Vector3(0f, -50f, -50f) * Quaternion.CreateFromAxisAngle(Vector3.Down, 0.66666f),
									 new Vector3(0f, -50f, -50f) * Quaternion.CreateFromAxisAngle(Vector3.Down, 0.33333f),
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
									Time.TotalSeconds * 0.5f,
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
							   "|:shadow=0,-1,0.01,1,1,1,0.5:|ABCDEFGHIJKLMNOPQRSTUVWXYZ\nabcdefghijklmnopqrstuvwxyz\n1234567890\n_-+=(){}[]<>\\|/;:'\"?.,!@#$%^&*~`",
							   new Vector2(0f, 4f),
							   Vector2.One / 7f,
							   Colour.White,
							   HAlign.Center,
							   VAlign.Bottom);

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

			//for (int i = 0; i < 100; i++)
			//	Renderer.Draw.Pixel(new Vector3(Random.Range(-40f, 40f), Random.Range(-20f, 20f), 0.0f),
			//						new Colour(Random.Range(1f), Random.Range(1f), Random.Range(1f), 1f));

			//RenderTarget.Bind(null);
			//Renderer.Draw.RenderTarget(surface, 0,
			//						   -5f, 5f, 0.1f,
			//						   Colour.White);

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
		}
	}
}