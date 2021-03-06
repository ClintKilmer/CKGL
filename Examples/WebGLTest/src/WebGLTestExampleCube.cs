using System.Runtime.InteropServices;
using CKGL;

namespace WebGLTestExampleCube
{
	#region Shaders
	public static class Shaders
	{
		public static InternalShaders.RendererShader Renderer = new InternalShaders.RendererShader();
		//public static InternalShaders.RendererFogShader RendererFog = new InternalShaders.RendererFogShader();
		//public static InternalShaders.LinearizeDepthShader LinearizeDepth = new InternalShaders.LinearizeDepthShader();
		public static PointLightShader PointLightShader = new PointLightShader();
	}

	#region PointLightShader
	public class PointLightShader : ShaderWrapper
	{
		public PointLightShader() : base(glsl) { }

		public Matrix M { set { SetUniform("M", value); } }
		public Matrix V { set { SetUniform("V", value); } }
		public Matrix P { set { SetUniform("P", value); } }
		public int Texture { set { SetUniform("Texture", value); } }
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
	
	vec4 baseColour = mix(vColour, texture(Texture, vUV) * vColour, vTextured);
	//colour = mix(vColour, texture(Texture, vUV * vec2(1.0, 1.0) + vec2(0.0, 0.5)) * vColour, vTextured); // Unity style texture offsets
	colour = vec4(baseColour.rgb * result, baseColour.a);
	
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
		//public static SpriteFont Font = new SpriteFont(SpriteSheets.SpriteSheet, "textures/font[5x7].png", 5, 7, '!', '~', 1, 3, 7, true);
		public static SpriteFont Font = new SpriteFont(SpriteSheets.SpriteSheet, "http://clintkilmer.com/games/webgltest/textures/font[5x7].png", 5, 7, '!', '~', 1, 3, 7, true); // Testing only - CORS fix
	}

	public static class Sprites
	{
		//public static Sprite Test1 = SpriteSheets.SpriteSheet.AddSprite("textures/Character1.png");
		//public static Sprite Test2 = SpriteSheets.SpriteSheet.AddSprite("textures/Character2.png");
		//public static Sprite Test3 = SpriteSheets.SpriteSheet.AddSprite("textures/Character3.png");
		public static Sprite Test1 = SpriteSheets.SpriteSheet.AddSprite("http://clintkilmer.com/games/webgltest/textures/Character1.png"); // Testing only - CORS fix
		public static Sprite Test2 = SpriteSheets.SpriteSheet.AddSprite("http://clintkilmer.com/games/webgltest/textures/Character2.png"); // Testing only - CORS fix
		public static Sprite Test3 = SpriteSheets.SpriteSheet.AddSprite("http://clintkilmer.com/games/webgltest/textures/Character3.png"); // Testing only - CORS fix
																																		   //public static Sprite Hoopoe = SpriteSheets.SpriteSheet.AddSprite("http://clintkilmer.com/games/webgltest/textures/Hoopoe.png"); // Testing only - CORS fix
	}

	public static class Textures
	{
		//public static Texture Hoopoe = Texture.Create2DFromFile("textures/Hoopoe.png");
		public static Texture Hoopoe = Texture.Create2DFromFile("http://clintkilmer.com/games/webgltest/textures/Hoopoe.png"); // Testing only - CORS fix
	}
	#endregion

	public class WebGLTest : Game
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
			Engine.Init(windowTitle: "CKGL.WebGL Test",
						windowWidth: width * scale,
						windowHeight: height * scale,
						windowVSync: true,
						windowFullscreen: true,
						windowResizable: true,
						windowBorderless: false,
						msaa: 0);
			Engine.Run(new WebGLTest());
		}

		Camera Camera = new Camera();

		Framebuffer surface = Framebuffer.Create(width, height, 1, TextureFormat.RGB8, TextureFormat.Depth24);

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
			Camera.FoV = 75f;
			Camera.AspectRatio = width / (float)height;
			Camera.Position = new Vector3(0f, 2f, -10f);
			Camera.zNear = 0.1f;
			Camera.zFar = 150f;

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
				cubeVertices[i] = new Vertex(cubeGeometry.Vertices[i].Position, cubeGeometry.Vertices[i].Normal, cubeGeometry.Vertices[i].Colour, cubeGeometry.Vertices[i].UV, true);
			cubeIndices = cubeGeometry.Indices16;

			Geometry icosphereGeometry = Geometry.Icosphere(0.5f, 2, true);
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
#if WEBGL
			cubeVertexBuffer.LoadData(cubeVertices, vertexFormat);
			icosphereVertexBuffer.LoadData(icosphereVertices, vertexFormat);
			planeVertexBuffer.LoadData(planeVertices, vertexFormat);
#else
			cubeVertexBuffer.LoadData(cubeVertices);
			icosphereVertexBuffer.LoadData(icosphereVertices);
			planeVertexBuffer.LoadData(planeVertices);
#endif
			cubeIndexBuffer.LoadData(cubeIndices);
			icosphereIndexBuffer.LoadData(icosphereIndices);
			planeIndexBuffer.LoadData(planeIndices);

			light1Transform.Parent = light1ParentTransform;
			light2Transform.Parent = light2ParentTransform;
			light3Transform.Parent = light3ParentTransform;
		}

		public override void Update()
		{
			float totalMilliseconds = (float)(System.DateTime.Now - new System.DateTime(1970, 1, 1)).TotalMilliseconds;
			float totalSeconds = (float)(System.DateTime.Now - new System.DateTime(1970, 1, 1)).TotalSeconds;

			cubeTransform.Rotation = Quaternion.CreateFromEuler(new Vector3(-totalSeconds * 0.3f, -totalSeconds * 0.25f, -totalSeconds * 0.09f));
			//icosphereTransform.Rotation = Quaternion.CreateFromEuler(new Vector3(-totalSeconds * -0.07f, -totalSeconds * 0.05f, -totalSeconds * 0.06f));
			light1ParentTransform.Rotation = Quaternion.CreateFromEuler(new Vector3(0f, -totalSeconds * 0.25f, 0f));
			light2ParentTransform.Rotation = Quaternion.CreateFromEuler(new Vector3(0f, -totalSeconds * 0.4f, 0f));
			light3ParentTransform.Rotation = Quaternion.CreateFromEuler(new Vector3(0f, -totalSeconds * 0.55f, 0f));
			light1Transform.Y = 2.25f + Math.Sin(totalSeconds * 0.65f) * 2f;
			light2Transform.Y = 2.25f + Math.Sin(totalSeconds * 0.8f) * 2f;
			light3Transform.Y = 2.25f + Math.Sin(totalSeconds * 0.95f) * 2f;
		}

		public override void Draw()
		{
			float totalMilliseconds = (float)(System.DateTime.Now - new System.DateTime(1970, 1, 1)).TotalMilliseconds;
			float totalSeconds = (float)(System.DateTime.Now - new System.DateTime(1970, 1, 1)).TotalSeconds;

			surface.Bind();

			// Clear the screen
			//if (Input.Keyboard.Down(KeyCode.Space))
			//	Graphics.Clear(1d);
			//else
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
			Shaders.PointLightShader.Texture = 0;
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

			//SpriteSheets.SpriteSheet.Texture.Bind();
			Textures.Hoopoe.Bind();

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

			DepthState.Off.Set();
			Shaders.Renderer.Bind();
			//Shaders.Renderer.MVP = Camera.Matrix;
			Shaders.Renderer.MVP = surface.Matrix;
			//Textures.Hoopoe.Bind();
			//Renderer.Draw.Rectangle(40f,
			//						40f,
			//						50f,
			//						50f,
			//						Colour.White,
			//						Colour.White,
			//						Colour.White,
			//						Colour.White,
			//						UV.BottomLeft,
			//						UV.BottomRight,
			//						UV.TopLeft,
			//						UV.TopRight,
			//						-totalSeconds * 0.5f,
			//						new Vector2(65f, 65f));

			Renderer.Draw.Sprite(Sprites.Test1,
								 new Vector2(1f, 35f),
								 Vector2.One * 2f,
								 Colour.White);

			Renderer.Draw.Sprite(Sprites.Test2,
								 new Vector2(18f, 35f),
								 Vector2.One * 2f,
								 Colour.White);

			Renderer.Draw.Sprite(Sprites.Test3,
								 new Vector2(35f, 35f),
								 Vector2.One * 2f,
								 Colour.White);

			//Renderer.Draw.Sprite(Sprites.Hoopoe,
			//					 new Vector2(100f, 20f),
			//					 Vector2.One / Sprites.Hoopoe.MaxLength * 8f * 2f,
			//					 Colour.White);

			//for (int i = 0; i < 1000; i++)
			//{
			//	Renderer.Draw.Sprite(Sprites.Test1,
			//						 new Vector2(40f + i * 0.5f + 100f * Math.Sin((totalMilliseconds + i * 10f) * 0.01f * 0.5f), 20f + i * 0.5f + 100f * Math.Sin((totalMilliseconds + i * 15f) * 0.01f * 0.5f)),
			//						 Vector2.One * 2f,
			//						 Colour.White);
			//}

			Renderer.Draw.Text(SpriteFonts.Font,
							   "|:shadow=0,-1,0.01,0,0,0,0.5:|Test Test\nStill testing...\nhhhheeeelllloooo",
							   new Vector2(2, height - 1),
							   Vector2.One * (1f + (Math.SinNormalized(totalSeconds * 2f) * 2f - 0.5f).Clamp(0.001f, 1f)),
							   Colour.White,
							   HAlign.Left,
							   VAlign.Top);

			//Renderer.Draw.Text(SpriteFonts.Font,
			//				   $"|:shadow=0,-1,0.01,0,0,0,0.5:|Still testing...",
			//				   new Vector2(2, 1),
			//				   Vector2.One * (1f + (Math.SinNormalized(totalSeconds * 2f) * 2f - 0.5f).Clamp(0.001f, 1f)),
			//				   Colour.White,
			//				   HAlign.Left,
			//				   VAlign.Bottom);

			//Renderer.Draw.Text(SpriteFonts.Font,
			//				   "|:shadow=0,-1,0.01,0,0,0,0.5:|Test Test",
			//				   new Vector2(width - 1, 1),
			//				   Vector2.One * (1f + (Math.SinNormalized(totalSeconds * 2f) * 2f - 0.5f).Clamp(0.001f, 1f)),
			//				   Colour.White,
			//				   HAlign.Right,
			//				   VAlign.Bottom);

			//Renderer.Draw.Text(SpriteFonts.Font,
			//				   "|:shadow=0,-1,0.01,0,0,0,0.5:|Test Test\nStill testing...\nhhhheeeelllloooo",
			//				   new Vector2(width - 1, height - 1),
			//				   Vector2.One * (1f + (Math.SinNormalized(totalSeconds * 2f) * 2f - 0.5f).Clamp(0.001f, 1f)),
			//				   Colour.White,
			//				   HAlign.Right,
			//				   VAlign.Top);

			//Renderer.Draw.Text(SpriteFonts.Font,
			//				   "|:outline=1,0.01,0,0,0,1:|Test Test Test Test Test Test Test Test Test\nStill testing... ... ...",
			//				   new Vector2(width / 2, height / 2),
			//				   Vector2.One * (1f + (Math.SinNormalized(totalSeconds * 2f) * 2f - 0.5f).Clamp(0.001f, 1f)),
			//				   Colour.White,
			//				   HAlign.Center,
			//				   VAlign.Middle);

			//Renderer.Draw.Text(SpriteFonts.Font,
			//				   "|:shadow=0,-1,0.01,0,0,0,0.5:|Test Test\nStill testing...",
			//				   new Vector2(width / 2, height / 2 + 50),
			//				   Vector2.One * (1f + (Math.SinNormalized(totalSeconds * 2f) * 2f - 0.5f).Clamp(0.001f, 1f) * 3f),
			//				   Colour.White,
			//				   HAlign.Center,
			//				   VAlign.Middle);

			//Renderer.Draw.Text(SpriteFonts.Font,
			//				   "|:shadow=0,-1,0.01,0,0,0,0.5:|Test Test\nStill testing...",
			//				   new Vector2(width / 2, height / 2 - 50),
			//				   Vector2.One * (1f + (Math.SinNormalized(totalSeconds * 2f) * 2f - 0.5f).Clamp(0.001f, 1f) * 2f),
			//				   Colour.White,
			//				   HAlign.Center,
			//				   VAlign.Middle);

			// Spritesheet test
			SpriteSheets.SpriteSheet.Texture.Bind();
			Renderer.Draw.Rectangle(0f,
									0f,
									SpriteSheets.SpriteSheet.Texture.Width,
									SpriteSheets.SpriteSheet.Texture.Height,
									Colour.White,
									Colour.White,
									Colour.White,
									Colour.White,
									UV.BottomLeft,
									UV.BottomRight,
									UV.TopLeft,
									UV.TopRight);

			// Draw to Screen
			Framebuffer.Default.Bind();
			Graphics.Clear(new Colour(0.1f, 0.1f, 0.1f, 1f));
			Graphics.State.Reset();

			// Render Framebuffer
			Shaders.Renderer.Bind();
			Shaders.Renderer.MVP = Framebuffer.Default.Matrix;
			Shaders.Renderer.Texture = 0;
			scale = Math.Max(1, Math.Min(Window.Width / width, Window.Height / height));
			Renderer.Draw.Framebuffer(surface, TextureAttachment.Colour0, (Window.Width - width * scale) / 2, (Window.Height - height * scale) / 2, scale, Colour.White);

			//Renderer.Draw.Text(SpriteFonts.Font,
			//				   debugString,
			//				   new Vector2(2, Framebuffer.Current.Height - 1),
			//				   Vector2.One * 3f,
			//				   Colour.White,
			//				   HAlign.Left,
			//				   VAlign.Top);

			Renderer.Flush();
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