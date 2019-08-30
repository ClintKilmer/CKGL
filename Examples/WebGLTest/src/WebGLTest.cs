using CKGL;

namespace WebGLTest
{
	//#region Shaders
	//public static class Shaders
	//{
	//	public static InternalShaders.RendererShader Renderer = new InternalShaders.RendererShader();
	//}
	//#endregion

	//#region Sprites
	//public static class SpriteSheets
	//{
	//	public static SpriteSheet SpriteSheet = new SpriteSheet(128, 1);
	//}

	//public static class SpriteFonts
	//{
	//	public static SpriteFont Font = new SpriteFont(SpriteSheets.SpriteSheet, "textures/font[5x7].png", 5, 7, '!', '~', 1, 3, 7, true);
	//}

	//public static class Textures
	//{
	//	public static Texture2D UVTest = Texture2D.CreateFromFile("textures/UVTest.png", TextureFilter.Nearest, TextureWrap.Repeat);
	//}
	//#endregion

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

		public WebGLTest()
			: base(windowTitle: "CKGL.WebGL Test",
				   windowWidth: width * scale,
				   windowHeight: height * scale,
				   windowVSync: true,
				   windowFullscreen: true,
				   windowResizable: true,
				   windowBorderless: false,
				   msaa: 0)
		{ }

		VertexFormat vertexFormat = new VertexFormat(
			new VertexAttribute(DataType.Float, 3, false),
			new VertexAttribute(DataType.UnsignedByte, 4, true)
		);
		VertexBuffer vertexBuffer;
		IndexBuffer indexBuffer;
		GeometryInput geometryInput;
		Vertex[] vertexData;

		public override void Init()
		{
			vertexBuffer = VertexBuffer.Create(BufferUsage.Dynamic);
			indexBuffer = IndexBuffer.Create(BufferUsage.Dynamic);
			geometryInput = GeometryInput.Create(indexBuffer, new VertexStream(vertexBuffer, vertexFormat));
		}

		public override void Update()
		{
			float totalMilliseconds = (float)(System.DateTime.Now - new System.DateTime(1970, 1, 1)).TotalMilliseconds;

			vertexData = new Vertex[] {
				new Vertex(new Vector3(Math.Sin(totalMilliseconds * 0.003f), 0.8f, 0f), Colour.Red),
				new Vertex(new Vector3(-0.8f,  Math.Sin(totalMilliseconds * 0.003f), 0f), Colour.Blue),
				new Vertex(new Vector3( 0.8f, -Math.Sin(totalMilliseconds * 0.003f), 0f), Colour.Green)};
#if WEBGL
			vertexBuffer.LoadData(vertexData, vertexFormat);
#else
			vertexBuffer.LoadData(vertexData);
#endif

			var indexData = new ushort[] { 0, 1, 2 };
			indexBuffer.LoadData(indexData);
		}

		public override void Draw()
		{
			// Clear the screen
			Graphics.Clear(Colour.Black);

			geometryInput.Bind();

			Graphics.DrawIndexedVertexArrays(PrimitiveTopology.TriangleList, 0, vertexData.Length, geometryInput.IndexBuffer.IndexType);
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