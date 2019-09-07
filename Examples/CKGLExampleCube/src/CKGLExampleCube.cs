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

	#region PointLightShader
	public class PointLightShader : ShaderWrapper
	{
		public PointLightShader() : base(glsl) { }

		public Matrix M { set { SetUniform("M", value); } }
		public Matrix V { set { SetUniform("V", value); } }
		public Matrix P { set { SetUniform("P", value); } }
		public Matrix3x3 NormalMatrix { set { SetUniform("NormalMatrix", value); } }
		public Vector3 Light1Position { set { SetUniform("pointLights[0].position", value); } }
		public Vector3 Light2Position { set { SetUniform("pointLights[1].position", value); } }
		public Vector3 Light3Position { set { SetUniform("pointLights[2].position", value); } }
		public Colour Light1Colour { set { SetUniform("pointLights[0].colour", value); } }
		public Colour Light2Colour { set { SetUniform("pointLights[1].colour", value); } }
		public Colour Light3Colour { set { SetUniform("pointLights[2].colour", value); } }
		public float Light1Radius { set { SetUniform("pointLights[0].radius", value); } }
		public float Light2Radius { set { SetUniform("pointLights[1].radius", value); } }
		public float Light3Radius { set { SetUniform("pointLights[2].radius", value); } }

		#region GLSL
		private static string glsl = @"
#vertex

layout(location = 0) in vec3 position;
layout(location = 1) in vec3 normal;
layout(location = 2) in vec4 colour;
layout(location = 3) in vec2 uv;
layout(location = 4) in float textured;

uniform mat4 M;
uniform mat4 V;
uniform mat4 P;
uniform mat3 NormalMatrix;

out vec3 vFragPosition;
out vec3 vNormal;
out vec4 vColour;
out vec2 vUV;
out float vTextured;

void main()
{
	gl_Position = vec4(position, 1.0) * M * V * P;
    vFragPosition = vec3(vec4(position, 1.0) * M * V);
	//vec3 pos = vec3(position.x + sin(float(gl_InstanceID)) * 0.1 * float(gl_InstanceID),
	//				position.y + float(gl_InstanceID) * 0.1,
	//				position.z + cos(float(gl_InstanceID)) * 0.1 * float(gl_InstanceID));
	//gl_Position = vec4(pos, 1.0) * M * V * P;
    //vFragPosition = vec3(vec4(pos, 1.0) * M * V);
	//vNormal = normalize(normal * mat3(transpose(inverse(M * V)))); // 3x3 Normal Matrix - moved this to shader uniform for performance
	vNormal = normalize(normal * NormalMatrix);
	vColour = colour;
	vUV = uv;
	vTextured = textured;
}


#fragment

layout(location = 0) out vec4 colour;

uniform sampler2D Texture;

struct PointLight
{
	vec3 position;
	vec4 colour;

	float radius;
	
	vec3 diffuse;
	vec3 specular;
};
#define NR_POINT_LIGHTS 3
uniform PointLight pointLights[NR_POINT_LIGHTS];

in vec3 vFragPosition;
in vec3 vNormal;
in vec4 vColour;
in vec2 vUV;
in float vTextured;

vec3 CalculatePointLight(PointLight light, vec3 normal, vec3 fragPosition, vec3 viewDirection, float specularStrength)
{
	vec3 lightPosition = light.position - fragPosition;
	vec3 lightDirection = normalize(lightPosition);
	
	// Diffuse Shading
	float diffuseFactor = max(dot(normal, lightDirection), 0.0);
	
	// Specular Shading
	// Phong
	//vec3 reflectDirection = reflect(-lightDirection, normal);
	//float spec = pow(max(dot(viewDirection, reflectDirection), 0.0), 32.0);
	//vec3 specular = light.colour.rgb * spec * specularStrength;
	// Blinn-Phong
	vec3 halfwayDirection = normalize(lightDirection + viewDirection);
	float IsDiffuseFactorGreaterThanZero = max(sign(diffuseFactor), 0.0);
	float specularFactor = pow(max(dot(normal, halfwayDirection), 0.0), 32.0) * IsDiffuseFactorGreaterThanZero * specularStrength;
	
	// Attenuation
	float distance = length(lightPosition);
	//float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));
	//float attenuation = 1.0 / (constant + linear * distance + quadratic * (distance * distance)); // Quadratic
	float attenuation = 1.0 / (1.0 + (4.5 / light.radius) * distance + (75.0 / (light.radius * light.radius)) * (distance * distance)); // Quadratic
	//float attenuation = 1.0 / distance; // Linear
	//float attenuation = clamp(1.0 - distance / (light.radius), 0.0, 1.0); attenuation *= attenuation;
	//float attenuation = clamp(1.0 - (distance * distance) / (light.radius * light.radius), 0.0, 1.0); attenuation *= attenuation;
	
	// Combine
	//return light.colour.rgb * (diffuseFactor + specularFactor) * attenuation;
	return light.colour.rgb * diffuseFactor * attenuation + (light.colour.rgb + light.colour.rgb + vec3(1.0)) / 3.0 * specularFactor * attenuation;
	//return light.colour.rgb * (diffuseFactor + specularFactor * 0.0001) * attenuation; // Diffuse only
	//return light.colour.rgb * (specularFactor) * attenuation; // Specular only
}

void main()
{
	vec3 normal = normalize(vNormal);
	float specularStrength = 0.5;
	vec3 viewDirection = normalize(-vFragPosition);
	
	vec3 result = vec3(0.0);
	for(int i = 0; i < NR_POINT_LIGHTS; i++)
        result += CalculatePointLight(pointLights[i], normal, vFragPosition, viewDirection, specularStrength);
	result = max(result, vec3(0.1));
	
	colour = mix(vColour, texture(Texture, vUV) * vColour, vTextured);
	//colour = mix(vColour, texture(Texture, vUV * vec2(1.0, 1.0) + vec2(0.0, 0.5)) * vColour, vTextured); // Unity style texture offsets
	colour = vec4(colour.rgb * result, colour.a);
	
	// apply gamma correction
	//float gamma = 2.2;
	//colour.rgb = pow(colour.rgb, vec3(1.0 / gamma));
}";
		#endregion
	}
	#endregion

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

	public static class Textures
	{
		public static Texture2D UVTest = Texture2D.CreateFromFile("textures/UVTest.png", TextureFilter.Nearest, TextureWrap.Repeat);
	}
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
		//private static int width = 506;
		//private static int height = 253;
		//private static int scale = 1;

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

		CullModeState cullModeState = CullModeState.Back;
		PolygonModeState polygonModeState = PolygonModeState.Fill;

		[StructLayout(LayoutKind.Sequential, Pack = 4)]
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

		private VertexBuffer icosphereVertexBuffer;
		private IndexBuffer icosphereIndexBuffer;
		private GeometryInput icosphereGeometryInput;

		private VertexBuffer planeVertexBuffer;
		private IndexBuffer planeIndexBuffer;
		private GeometryInput planeGeometryInput;

		private Vertex[] cubeVertices;
		private ushort[] cubeIndices;

		private Vertex[] icosphereVertices;
		private ushort[] icosphereIndices;

		private Vertex[] planeVertices;
		private ushort[] planeIndices;

		Colour light1Colour = Colour.Red;
		Colour light2Colour = Colour.Green;
		Colour light3Colour = Colour.Blue;
		Transform light1ParentTransform = new Transform();
		Transform light2ParentTransform = new Transform();
		Transform light3ParentTransform = new Transform();
		Transform light1Transform = new Transform { Position = new Vector3(3f, 3f, 0f), Scale = new Vector3(0.1f) };
		Transform light2Transform = new Transform { Position = new Vector3(3f, 3f, 0f), Scale = new Vector3(0.1f) };
		Transform light3Transform = new Transform { Position = new Vector3(3f, 3f, 0f), Scale = new Vector3(0.1f) };
		Transform cubeTransform = new Transform { Position = new Vector3(0f, 2f, 0f), Scale = new Vector3(2f) };
		Transform cube2Transform = new Transform { Position = new Vector3(5f, 1f, 5f), Scale = new Vector3(2f) };
		Transform cube3Transform = new Transform { Position = new Vector3(-5f, 4f, -3f), Scale = new Vector3(2f), Rotation = Quaternion.CreateFromEuler(0.3f, 0.4f, 0.6f) };
		Transform icosphereTransform = new Transform { Position = new Vector3(-5f, 4f, 5f), Scale = new Vector3(4f) };
		Transform planeTransform = new Transform { Scale = new Vector3(10000f, 1f, 10000f) };

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
				4,                                                    // Pack
				new VertexAttribute(DataType.Float, 3, false),        // Position
				new VertexAttribute(DataType.Float, 3, false),        // Normal
				new VertexAttribute(DataType.UnsignedByte, 4, true),  // Colour
				new VertexAttribute(DataType.UnsignedShort, 2, true), // UV
				new VertexAttribute(DataType.UnsignedByte, 1, true)   // Textured
			);

			Geometry cubeGeometry = Geometry.Cube();
			cubeVertices = new Vertex[cubeGeometry.Vertices.Length];
			for (int i = 0; i < cubeGeometry.Vertices.Length; i++)
				cubeVertices[i] = new Vertex(cubeGeometry.Vertices[i].Position, cubeGeometry.Vertices[i].Normal, cubeGeometry.Vertices[i].Colour, cubeGeometry.Vertices[i].UV, false);
			cubeIndices = cubeGeometry.Indices16;

			Geometry icosphereGeometry = Geometry.Icosphere(0.5f, 4);
			icosphereVertices = new Vertex[icosphereGeometry.Vertices.Length];
			for (int i = 0; i < icosphereGeometry.Vertices.Length; i++)
				icosphereVertices[i] = new Vertex(icosphereGeometry.Vertices[i].Position, icosphereGeometry.Vertices[i].Normal, icosphereGeometry.Vertices[i].Colour, icosphereGeometry.Vertices[i].UV, false);
			icosphereIndices = icosphereGeometry.Indices16;

			Geometry planeGeometry = Geometry.Plane(1f, Geometry.Orientation.XZ, 10, 10, false);
			planeVertices = new Vertex[planeGeometry.Vertices.Length];
			for (int i = 0; i < planeGeometry.Vertices.Length; i++)
				planeVertices[i] = new Vertex(planeGeometry.Vertices[i].Position, planeGeometry.Vertices[i].Normal, planeGeometry.Vertices[i].Colour, planeGeometry.Vertices[i].UV, false);
			planeIndices = planeGeometry.Indices16;

			cubeVertexBuffer = VertexBuffer.Create(BufferUsage.Static);
			cubeIndexBuffer = IndexBuffer.Create(BufferUsage.Static);
			icosphereVertexBuffer = VertexBuffer.Create(BufferUsage.Static);
			icosphereIndexBuffer = IndexBuffer.Create(BufferUsage.Static);
			planeVertexBuffer = VertexBuffer.Create(BufferUsage.Static);
			planeIndexBuffer = IndexBuffer.Create(BufferUsage.Static);
			cubeGeometryInput = GeometryInput.Create(cubeIndexBuffer, new VertexStream(cubeVertexBuffer, vertexFormat));
			icosphereGeometryInput = GeometryInput.Create(icosphereIndexBuffer, new VertexStream(icosphereVertexBuffer, vertexFormat));
			planeGeometryInput = GeometryInput.Create(planeIndexBuffer, new VertexStream(planeVertexBuffer, vertexFormat));
			cubeVertexBuffer.LoadData(cubeVertices);
			cubeIndexBuffer.LoadData(cubeIndices);
			icosphereVertexBuffer.LoadData(icosphereVertices);
			icosphereIndexBuffer.LoadData(icosphereIndices);
			planeVertexBuffer.LoadData(planeVertices);
			planeIndexBuffer.LoadData(planeIndices);

			light1Transform.Parent = light1ParentTransform;
			light2Transform.Parent = light2ParentTransform;
			light3Transform.Parent = light3ParentTransform;
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

			cubeTransform.Rotation = Quaternion.CreateFromEuler(new Vector3(-Time.TotalSeconds * 0.3f, -Time.TotalSeconds * 0.25f, -Time.TotalSeconds * 0.09f));
			//icosphereTransform.Rotation = Quaternion.CreateFromEuler(new Vector3(-Time.TotalSeconds * -0.07f, -Time.TotalSeconds * 0.05f, -Time.TotalSeconds * 0.06f));
			light1ParentTransform.Rotation = Quaternion.CreateFromEuler(new Vector3(0f, -Time.TotalSeconds * 0.25f, 0f));
			light2ParentTransform.Rotation = Quaternion.CreateFromEuler(new Vector3(0f, -Time.TotalSeconds * 0.4f, 0f));
			light3ParentTransform.Rotation = Quaternion.CreateFromEuler(new Vector3(0f, -Time.TotalSeconds * 0.55f, 0f));
			light1Transform.Y = 2.25f + Math.Sin(Time.TotalSeconds * 0.65f) * 2f;
			light2Transform.Y = 2.25f + Math.Sin(Time.TotalSeconds * 0.8f) * 2f;
			light3Transform.Y = 2.25f + Math.Sin(Time.TotalSeconds * 0.95f) * 2f;

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
			DepthState.LessEqual.Set();
			CullModeState.Set(cullModeState);
			PolygonModeState.Set(polygonModeState);

			Shaders.Renderer.Bind();
			Shaders.Renderer.MVP = Camera.Matrix;

			Renderer.Draw.ResetTransform();
			Renderer.Draw3D.ResetTransform();

			// Start Drawing
			Renderer.Draw3D.SetTransform(light1Transform);
			Renderer.Draw3D.Cube(light1Colour);
			Renderer.Draw3D.SetTransform(light2Transform);
			Renderer.Draw3D.Cube(light2Colour);
			Renderer.Draw3D.SetTransform(light3Transform);
			Renderer.Draw3D.Cube(light3Colour);
			Renderer.Draw3D.ResetTransform();

			Shaders.PointLightShader.Bind();

			Shaders.PointLightShader.V = Camera.ViewMatrix;
			Shaders.PointLightShader.P = Camera.ProjectionMatrix;

			// Directional Light
			//Shaders.CubeShader.DirectionalLight = Vector3.Forward * Matrix.CreateRotationX(0.05f) * Matrix.CreateRotationY(-0.1f);

			// Point Light
			Shaders.PointLightShader.Light1Position = light1Transform.GlobalPosition * Camera.ViewMatrix;
			Shaders.PointLightShader.Light2Position = light2Transform.GlobalPosition * Camera.ViewMatrix;
			Shaders.PointLightShader.Light3Position = light3Transform.GlobalPosition * Camera.ViewMatrix;
			Shaders.PointLightShader.Light1Colour = light1Colour;
			Shaders.PointLightShader.Light2Colour = light2Colour;
			Shaders.PointLightShader.Light3Colour = light3Colour;
			Shaders.PointLightShader.Light1Radius = 75f;
			Shaders.PointLightShader.Light2Radius = 75f;
			Shaders.PointLightShader.Light3Radius = 75f;

			Shaders.PointLightShader.M = cubeTransform.Matrix;
			Shaders.PointLightShader.NormalMatrix = (cubeTransform.Matrix * Camera.ViewMatrix).ToMatrix3x3().Inverse().Transpose();
			cubeGeometryInput.Bind();
			Graphics.DrawIndexedVertexArrays(PrimitiveTopology.TriangleList, 0, cubeIndexBuffer.Count, cubeIndexBuffer.IndexType);

			Shaders.PointLightShader.M = cube2Transform.Matrix;
			Shaders.PointLightShader.NormalMatrix = (cube2Transform.Matrix * Camera.ViewMatrix).ToMatrix3x3().Inverse().Transpose();
			cubeGeometryInput.Bind(); // Redundant
			Graphics.DrawIndexedVertexArrays(PrimitiveTopology.TriangleList, 0, cubeIndexBuffer.Count, cubeIndexBuffer.IndexType);

			Shaders.PointLightShader.M = cube3Transform.Matrix;
			Shaders.PointLightShader.NormalMatrix = (cube3Transform.Matrix * Camera.ViewMatrix).ToMatrix3x3().Inverse().Transpose();
			cubeGeometryInput.Bind(); // Redundant
			Graphics.DrawIndexedVertexArrays(PrimitiveTopology.TriangleList, 0, cubeIndexBuffer.Count, cubeIndexBuffer.IndexType);

			Shaders.PointLightShader.M = icosphereTransform.Matrix;
			Shaders.PointLightShader.NormalMatrix = (icosphereTransform.Matrix * Camera.ViewMatrix).ToMatrix3x3().Inverse().Transpose();
			icosphereGeometryInput.Bind();
			Graphics.DrawIndexedVertexArrays(PrimitiveTopology.TriangleList, 0, icosphereIndexBuffer.Count, icosphereIndexBuffer.IndexType);

			Shaders.PointLightShader.M = planeTransform.Matrix;
			Shaders.PointLightShader.NormalMatrix = (planeTransform.Matrix * Camera.ViewMatrix).ToMatrix3x3().Inverse().Transpose();
			planeGeometryInput.Bind();
			Graphics.DrawIndexedVertexArrays(PrimitiveTopology.TriangleList, 0, planeIndexBuffer.Count, planeIndexBuffer.IndexType);

			// Draw to Screen
			RenderTarget.Default.Bind();
			Graphics.Clear(new Colour(0.1f, 0.1f, 0.1f, 1f));
			Graphics.State.Reset();

			// Render RenderTarget
			Shaders.Renderer.Bind();
			Shaders.Renderer.MVP = RenderTarget.Default.Matrix;
			scale = Math.Max(1, Math.Min(Window.Width / width, Window.Height / height));
			Renderer.Draw.RenderTarget(surface, TextureSlot.Colour0, (Window.Width - width * scale) / 2, (Window.Height - height * scale) / 2, scale, Colour.White);

			//Renderer.Draw.Text(SpriteFonts.Font,
			//				   debugString,
			//				   new Vector2(2, RenderTarget.Current.Height - 1),
			//				   Vector2.One * 3f,
			//				   Colour.White,
			//				   HAlign.Left,
			//				   VAlign.Top);

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