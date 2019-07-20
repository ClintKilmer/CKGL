using CKGL;
using System.Runtime.InteropServices;

namespace CKGLExampleCube
{
	#region Shaders
	public static class Shaders
	{
		public static InternalShaders.RendererShader Renderer = new InternalShaders.RendererShader();
		public static PointLightShader PointLightShader = new PointLightShader();
	}

	#region Cube
	public class PointLightShader : Shader
	{
		#region GLSL
		private static string glsl = @"
#vertex

layout(location = 0) in vec3 position;
layout(location = 1) in vec3 normal;
layout(location = 2) in vec4 colour;
layout(location = 3) in vec2 uv;
layout(location = 4) in float textured;

uniform mat4 M;
uniform mat4 VP;

out vec3 vFragPosition;
out vec3 vNormal;
out vec4 vColour;
out vec2 vUV;
out float vTextured;

void main()
{
	gl_Position = vec4(position, 1.0) * M * VP;
    vFragPosition = vec3(vec4(position, 1.0) * M);
	vNormal = normal * mat3(transpose(inverse(M)));
	vColour = colour;
	vUV = uv;
	vTextured = textured;
}


#fragment

layout(location = 0) out vec4 colour;

uniform sampler2D Texture;
uniform vec3 CameraPosition;
uniform vec3 LightPosition;
uniform vec4 LightColour;

in vec3 vFragPosition;
in vec3 vNormal;
in vec4 vColour;
in vec2 vUV;
in float vTextured;

void main()
{
	// Directional Light
	//float intensity = max(dot(vNormal, -normalize(DirectionalLight)), 0.0);
	//colour = mix(vColour, texture(Texture, vUV) * vColour, vTextured);
	//colour = vec4(colour.rgb * max(intensity, 0.1), colour.a);
	
	//Point Light
	float specularStrength = 0.5;

	vec3 lightDirection = normalize(vFragPosition - LightPosition);

	vec3 viewDirection = normalize(CameraPosition - vFragPosition);
	vec3 reflectDirection = reflect(lightDirection, vNormal);

	float intensity = max(dot(vNormal, -lightDirection), 0.0);

	float spec = pow(max(dot(viewDirection, reflectDirection), 0.0), 32.0);
	vec3 specular = specularStrength * spec * LightColour.rgb;

	colour = mix(vColour, texture(Texture, vUV) * vColour, vTextured);
	colour = vec4(colour.rgb * max(intensity * LightColour.rgb + specular, 0.1), colour.a);
}";
		#endregion

		public Matrix M { set { SetUniform("M", value); } }
		public Matrix VP { set { SetUniform("VP", value); } }
		public Vector3 CameraPosition { set { SetUniform("CameraPosition", value); } }
		public Vector3 LightPosition { set { SetUniform("LightPosition", value); } }
		public Colour LightColour { set { SetUniform("LightColour", value); } }

		public PointLightShader() : base(glsl) { }
	}
	#endregion

	#endregion

	public class CKGLExampleCube : Game
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

		public CKGLExampleCube()
			: base(windowTitle: "CKGL Example - Cube",
				   windowWidth: width * scale,
				   windowHeight: height * scale,
				   windowVSync: true,
				   windowFullscreen: false,
				   windowResizable: true,
				   windowBorderless: false,
				   msaa: 0)
		{ }

		string debugString = "";

		// Variable for moving window on mouse click and drag
		Point2 windowDraggingPosition = Point2.Zero;

		Camera Camera = new Camera();
		Camera2D Camera2D = new Camera2D();
		float cameraYaw = 0f;
		float cameraPitch = 0f;
		Vector3 cameraLookat = Vector3.Forward;
		Vector3 cameraLookatNoVertical = Vector3.Forward;

		RenderTarget surface;

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct Vertex
		{
			public Vector3 Position;
			public Vector3 Normal;
			public Colour Colour;
			public UV UV;
			public byte Textured;

			public Vertex(Vector3 position, Vector3 normal, Colour colour, UV uv, bool textured)
			{
				Position = position;
				Normal = normal;
				Colour = colour;
				UV = uv;
				Textured = (byte)(textured ? 255 : 0);
			}

			public Vertex(Vector3 position, Vector3 normal, Colour? colour, UV? uv)
				: this(position, normal, colour ?? Colour.White, uv ?? UV.Zero, uv != null) { }
		}

		private VertexFormat vertexFormat;

		private VertexBuffer cubeVertexBuffer;
		private IndexBuffer cubeIndexBuffer;
		private GeometryInput cubeGeometryInput;

		private VertexBuffer planeVertexBuffer;
		private IndexBuffer planeIndexBuffer;
		private GeometryInput planeGeometryInput;

		private readonly Vertex[] cubeVertices = new Vertex[] {
			// Front
			new Vertex(new Vector3(-1f,  1f, -1f), Vector3.Backward, Colour.White, UV.TopLeft, false),
			new Vertex(new Vector3( 1f,  1f, -1f), Vector3.Backward, Colour.White, UV.TopRight, false),
			new Vertex(new Vector3(-1f, -1f, -1f), Vector3.Backward, Colour.White, UV.BottomLeft, false),
			new Vertex(new Vector3( 1f, -1f, -1f), Vector3.Backward, Colour.White, UV.BottomRight, false),
			// Back
			new Vertex(new Vector3( 1f,  1f,  1f), Vector3.Forward, Colour.White, UV.TopLeft, false),
			new Vertex(new Vector3(-1f,  1f,  1f), Vector3.Forward, Colour.White, UV.TopRight, false),
			new Vertex(new Vector3( 1f, -1f,  1f), Vector3.Forward, Colour.White, UV.BottomLeft, false),
			new Vertex(new Vector3(-1f, -1f,  1f), Vector3.Forward, Colour.White, UV.BottomRight, false),
			// Top
			new Vertex(new Vector3(-1f,  1f,  1f), Vector3.Up, Colour.White, UV.TopLeft, false),
			new Vertex(new Vector3( 1f,  1f,  1f), Vector3.Up, Colour.White, UV.TopRight, false),
			new Vertex(new Vector3(-1f,  1f, -1f), Vector3.Up, Colour.White, UV.BottomLeft, false),
			new Vertex(new Vector3( 1f,  1f, -1f), Vector3.Up, Colour.White, UV.BottomRight, false),
			// Bottom
			new Vertex(new Vector3( 1f, -1f,  1f), Vector3.Down, Colour.White, UV.TopLeft, false),
			new Vertex(new Vector3(-1f, -1f,  1f), Vector3.Down, Colour.White, UV.TopRight, false),
			new Vertex(new Vector3( 1f, -1f, -1f), Vector3.Down, Colour.White, UV.BottomLeft, false),
			new Vertex(new Vector3(-1f, -1f, -1f), Vector3.Down, Colour.White, UV.BottomRight, false),
			// Left
			new Vertex(new Vector3(-1f,  1f,  1f), Vector3.Left, Colour.White, UV.TopLeft, false),
			new Vertex(new Vector3(-1f,  1f, -1f), Vector3.Left, Colour.White, UV.TopRight, false),
			new Vertex(new Vector3(-1f, -1f,  1f), Vector3.Left, Colour.White, UV.BottomLeft, false),
			new Vertex(new Vector3(-1f, -1f, -1f), Vector3.Left, Colour.White, UV.BottomRight, false),
			// Right
			new Vertex(new Vector3( 1f,  1f, -1f), Vector3.Right, Colour.White, UV.TopLeft, false),
			new Vertex(new Vector3( 1f,  1f,  1f), Vector3.Right, Colour.White, UV.TopRight, false),
			new Vertex(new Vector3( 1f, -1f, -1f), Vector3.Right, Colour.White, UV.BottomLeft, false),
			new Vertex(new Vector3( 1f, -1f,  1f), Vector3.Right, Colour.White, UV.BottomRight, false),
		};

		private readonly ushort[] cubeIndices = new ushort[] {
			// Front
			0, 2, 1,
			2, 3, 1,
			// Back
			0 + 4, 2 + 4, 1 + 4,
			2 + 4, 3 + 4, 1 + 4,
			// Top
			0 + 4 * 2, 2 + 4 * 2, 1 + 4 * 2,
			2 + 4 * 2, 3 + 4 * 2, 1 + 4 * 2,
			// Bottom
			0 + 4 * 3, 2 + 4 * 3, 1 + 4 * 3,
			2 + 4 * 3, 3 + 4 * 3, 1 + 4 * 3,
			// Left
			0 + 4 * 4, 2 + 4 * 4, 1 + 4 * 4,
			2 + 4 * 4, 3 + 4 * 4, 1 + 4 * 4,
			// Right
			0 + 4 * 5, 2 + 4 * 5, 1 + 4 * 5,
			2 + 4 * 5, 3 + 4 * 5, 1 + 4 * 5,
		};

		private readonly Vertex[] planeVertices = new Vertex[] {
			new Vertex(new Vector3(-1f,  0f,  1f), Vector3.Up, Colour.White, UV.TopLeft, false),
			new Vertex(new Vector3( 1f,  0f,  1f), Vector3.Up, Colour.White, UV.TopRight, false),
			new Vertex(new Vector3(-1f,  0f, -1f), Vector3.Up, Colour.White, UV.BottomLeft, false),
			new Vertex(new Vector3( 1f,  0f, -1f), Vector3.Up, Colour.White, UV.BottomRight, false),
		};

		private readonly ushort[] planeIndices = new ushort[] {
			// Front
			0, 2, 1,
			2, 3, 1,
		};

		Colour lightColour = new Colour(0.95f, 1f, 0.75f, 1f);
		Transform lightParentTransform = new Transform();
		Transform lightTransform = new Transform { Position = new Vector3(2f, 3f, -1f), Scale = new Vector3(0.1f, 0.1f, 0.1f) };
		Transform cubeTransform = new Transform { Position = new Vector3(0f, 2f, 0f) };
		Transform planeTransform = new Transform { Scale = new Vector3(100f, 1f, 100f) };

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

			surface = new RenderTarget(width, height, 1, TextureFormat.RGB8, TextureFormat.Depth);

			vertexFormat = new VertexFormat(
				Marshal.SizeOf(typeof(Vertex)),                       // Dynamic Stride - For larger StructLayout Pack sizes
				new VertexAttribute(DataType.Float, 3, false),        // Position
				new VertexAttribute(DataType.Float, 3, false),        // Normal
				new VertexAttribute(DataType.UnsignedByte, 4, true),  // Colour
				new VertexAttribute(DataType.UnsignedShort, 2, true), // UV
				new VertexAttribute(DataType.UnsignedByte, 1, true)   // Textured
			);
			cubeVertexBuffer = VertexBuffer.Create(BufferUsage.Static);
			cubeIndexBuffer = IndexBuffer.Create(BufferUsage.Static);
			planeVertexBuffer = VertexBuffer.Create(BufferUsage.Static);
			planeIndexBuffer = IndexBuffer.Create(BufferUsage.Static);
			cubeGeometryInput = GeometryInput.Create(cubeIndexBuffer, new VertexStream(cubeVertexBuffer, vertexFormat));
			planeGeometryInput = GeometryInput.Create(planeIndexBuffer, new VertexStream(planeVertexBuffer, vertexFormat));
			cubeVertexBuffer.LoadData(in cubeVertices);
			cubeIndexBuffer.LoadData(in cubeIndices);
			planeVertexBuffer.LoadData(in planeVertices);
			planeIndexBuffer.LoadData(in planeIndices);

			lightTransform.Parent = lightParentTransform;
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

			cubeTransform.Rotation = Quaternion.CreateFromEuler(new Vector3(-Time.TotalSeconds * 0.3f, -Time.TotalSeconds * 0.25f, -Time.TotalSeconds * 0.09f));
			lightParentTransform.Rotation = Quaternion.CreateFromEuler(new Vector3(0f, -Time.TotalSeconds * 0.25f, 0f));

			debugString = $"|:outline=1,0.01,0,0,0,1:|Cam Pos: {Camera.Position.X:n1}, {Camera.Position.Y:n1}, {Camera.Position.Z:n1}\nCam Rot: {Camera.Rotation.Euler.X:n2}, {Camera.Rotation.Euler.Y:n2}, {Camera.Rotation.Euler.Z:n2}\nMem: {RAM:n1}MB\nVSync: {Window.GetVSyncMode()}\n{Time.UPS:n0}ups | {Time.FPSSmoothed:n0}fps\nDraw Calls: {Graphics.DrawCalls}\nState Changes: {Graphics.State.Changes}\nRenderTarget Swaps/Blits: {RenderTarget.Swaps}/{RenderTarget.Blits}\nTexture Swaps: {Texture.Swaps}\nShader/Uniform Swaps: {Shader.Swaps}/{Shader.UniformSwaps}\nWinPos: [{Window.X}, {Window.Y}]\nSize: [{Window.Size}]\nMouse Global: [{Input.Mouse.PositionDisplay}]\nMouse: [{Input.Mouse.Position}]\nMouse Relative: [{Input.Mouse.PositionRelative}]";
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
			CullModeState.Back.Set();
			DepthState.LessEqual.Set();

			Shaders.Renderer.Bind();
			Shaders.Renderer.MVP = Camera.Matrix;

			Renderer.Draw.ResetTransform();
			Renderer.Draw3D.ResetTransform();

			// Start Drawing
			Renderer.Draw3D.SetTransform(lightTransform);
			Renderer.Draw3D.Cube(lightColour);
			Renderer.Draw3D.ResetTransform();

			Shaders.PointLightShader.Bind();

			Shaders.PointLightShader.M = cubeTransform.Matrix;
			Shaders.PointLightShader.VP = Camera.Matrix;
			// Directional Light
			//Shaders.CubeShader.DirectionalLight = Vector3.Forward * Matrix.CreateRotationX(0.05f) * Matrix.CreateRotationY(-0.1f);
			// Point Light
			Shaders.PointLightShader.CameraPosition = Camera.Position;
			Shaders.PointLightShader.LightPosition = lightTransform.GlobalPosition;
			Shaders.PointLightShader.LightColour = lightColour;
			cubeGeometryInput.Bind();
			Graphics.DrawIndexedVertexArrays(PrimitiveTopology.TriangleList, 0, cubeIndices.Length, cubeIndexBuffer.IndexType);

			Shaders.PointLightShader.M = planeTransform.Matrix;
			planeGeometryInput.Bind();
			Graphics.DrawIndexedVertexArrays(PrimitiveTopology.TriangleList, 0, planeIndices.Length, planeIndexBuffer.IndexType);

			// Draw to Screen
			RenderTarget.Default.Bind();
			Graphics.Clear(new Colour(0.1f, 0.1f, 0.1f, 1f));
			Graphics.State.Reset();

			// Render RenderTarget
			Shaders.Renderer.Bind();
			Shaders.Renderer.MVP = RenderTarget.Default.Matrix;
			scale = Math.Max(1, Math.Min(Window.Width / width, Window.Height / height));
			Renderer.Draw.RenderTarget(surface, TextureSlot.Colour0, (Window.Width - width * scale) / 2, (Window.Height - height * scale) / 2, scale, Colour.White);

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
			//surface = new RenderTarget(width, height, 1, TextureFormat.RGB8, TextureFormat.Depth);

			Camera.AspectRatio = surface.AspectRatio;
			//Camera.AspectRatio = Window.AspectRatio;
		}
	}
}