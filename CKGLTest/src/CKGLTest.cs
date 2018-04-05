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
	public static class Sprites
	{
		public static Texture2D test1 = Texture2D.LoadTexture2DFromStream("Textures/Character1.png");
	}
	#endregion

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

		// Variable for moving window on mouse click and drag
		Point2 windowDraggingPosition = Point2.Zero;

		Vector3 cameraPosition = new Vector3(0f, 0f, -10f);
		Vector3 cameraLookat = Vector3.Forward;
		Vector3 cameraScale = Vector3.One;
		float cameraRotation = 0f;
		Matrix cameraRotationMatrix = Matrix.Identity;
		Matrix cameraTranslationMatrix = Matrix.Identity;

		Matrix WorldMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Up);
		Matrix ViewMatrix = Matrix.Identity;
		Matrix ProjectionMatrix = Matrix.Identity;

		public override void Init()
		{
			//Platform.ShowCursor = false; // Default true
			//Platform.ScreensaverAllowed = true; // Default false

			//ProjectionMatrix = Matrix.CreateOrthographic(Window.Size, -10000f, 10000f);
			ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(Math.DegreesToRadians(75f), Window.AspectRatio, 1f, 10000f);
		}

		public override void Update()
		{
			//Window.Title = $"{Time.DeltaTime.ToString("n1")}ms - Info: {Platform.OS} | {Time.TotalSeconds.ToString("n1")} - Buffers: {Audio.BufferCount} - Sources: {Audio.SourceCount} - Position: [{Window.X}, {Window.Y}] - Size: [{Window.Width}, {Window.Height}] - Mouse: [{Input.Mouse.Position.X}, {Input.Mouse.Position.Y}]";
			Window.Title = $"{Time.DeltaTime.ToString("n1")}ms | Position: [{Window.X}, {Window.Y}] | Size: [{Window.Width}, {Window.Height}] | Mouse: [{Input.Mouse.Position.X}, {Input.Mouse.Position.Y}]";

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

			if (Input.Keyboard.Pressed(KeyCode.Space) || Input.Mouse.LeftPressed)
				Sounds.sndPop1.Play();
			if (Input.Keyboard.Released(KeyCode.Space) || Input.Mouse.LeftReleased)
				Sounds.sndPop2.Play();

			var speed = 0.01f;
			if (Input.Keyboard.Down(KeyCode.Z))
				cameraRotation -= 0.01f;
			if (Input.Keyboard.Down(KeyCode.C))
				cameraRotation += 0.01f;
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
				cameraScale -= 0.03f * cameraScale;
			if (Input.Keyboard.Down(KeyCode.E))
				cameraScale += 0.03f * cameraScale;

			cameraPosition += direction.Normalized * speed * Time.DeltaTime;

			cameraLookat = cameraLookat * (Matrix.CreateRotationY(Math.DegreesToRadians((Input.Mouse.Position.X - Input.Mouse.LastPosition.X) * -0.3f)) *
										   Matrix.CreateRotationX(Math.DegreesToRadians((Input.Mouse.Position.Y - Input.Mouse.LastPosition.Y) * 0.3f)));

			//ViewMatrix = cameraRotationMatrix * cameraTranslationMatrix;
			ViewMatrix = Matrix.CreateLookAt(cameraPosition, cameraPosition + cameraLookat, Vector3.Up);
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
			CKGL.Shaders.Renderer.SetUniform("MVP", WorldMatrix * ViewMatrix * ProjectionMatrix);
			//Renderer.ResetShader();
			//Renderer.SetFrontFaceState(FrontFace.Clockwise);
			//Renderer.SetCullState(CullState.Back);
			//Renderer.SetBlendState(BlendState.AlphaBlend);
			Sprites.test1.MagFilter = TextureFilter.Nearest;
			Sprites.test1.MinFilter = TextureFilter.Nearest;
			Renderer.SetTexture(Sprites.test1);
			Renderer.Draw.Triangle(new Vector2(0f, 1f),
								   new Vector2(0f, 1f) * Matrix2D.CreateRotationZ(Math.RotationsToRadians(0.66666f)),
								   new Vector2(0f, 1f) * Matrix2D.CreateRotationZ(Math.RotationsToRadians(0.33333f)),
								   Colour.Red,
								   Colour.Green,
								   Colour.Blue,
								   false,
								   Vector2.Zero,
								   Vector2.One,
								   new Vector2(1f, 0f),
								   Time.TotalSeconds * 0.5f,
								   Vector2.Zero);
			Renderer.Draw.Rectangle(2f,
									1f,
									1f,
									1f,
									Colour.White,
									Colour.White,
									Colour.White,
									Colour.White,
									true,
									new Vector2(0f, 0f),
									new Vector2(1f, 0f),
									new Vector2(0f, 1f),
									new Vector2(1f, 1f));
			//Renderer.Draw.TriangleStrip.Begin();
			////int ii = Random.Range(1000, 10000);
			//for (int i = 0; i < 10000; i++)
			//	Renderer.Draw.TriangleStrip.AddVertex(new Vector3(Random.Range(-1f, 1f), Random.Range(10;, 1f), 0.0f),
			//										  new Colour(Random.Range(1f), Random.Range(1f), Random.Range(1f), Random.Range(1f)));
			//Renderer.Draw.TriangleStrip.End();

			//Renderer.Draw.LineStrip.Begin();
			//for (int i = 0; i < 10; i++)
			//	Renderer.Draw.LineStrip.AddVertex(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0.0f),
			//									  new Colour(Random.Range(1f), Random.Range(1f), Random.Range(1f), Random.Range(1f)));
			//Renderer.Draw.LineStrip.End();

			//for (int i = 0; i < 100; i++)
			//	Renderer.Draw.Pixel(new Vector3(Random.Range(-400f, 400f), Random.Range(-200f, 200f), 0.0f),
			//						new Colour(Random.Range(1f), Random.Range(1f), Random.Range(1f), 1f));
			Renderer.End();
		}

		public override void Destroy()
		{
		}

		public override void OnFocusGained()
		{
			Debug("Focus Gained");
		}

		public override void OnFocusLost()
		{
			Debug("Focus Lost");
		}

		public override void OnWindowResized()
		{
			//ProjectionMatrix = Matrix.CreateOrthographic(Window.Size, -10000f, 10000f);
			ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(Math.DegreesToRadians(75f), Window.AspectRatio, 1f, 10000f);
		}
	}
}