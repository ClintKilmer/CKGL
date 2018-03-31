using OpenGL;
using CKGL;

using GLint = System.Int32;
using GLuint = System.UInt32;

namespace CKGLTest
{
	class Program
	{
		private static string Title = "CKGL Game!";
		private static int WindowWidth = 640;
		private static int WindowHeight = 360;
		private static bool VSync = true;
		private static bool Fullscreen = false;
		private static bool Resizable = true;
		private static bool Borderless = false;

		static void Main(string[] args)
		{
			Platform.Init(Title, WindowWidth, WindowHeight, VSync, Fullscreen, Resizable, Borderless);
			Graphics.Init();
			Audio.Init();
			Input.Init();

			//SDLPlatform.ShowCursor = false;

			// Load Shaders
			Shader shader = Shader.FromFile("Shaders/test.glsl");

			// Load Audio
			Audio.Buffer sndPop1 = new Audio.Buffer("snd/sndPop1.wav");
			Audio.Buffer sndPop2 = new Audio.Buffer("snd/sndPop2.wav");

			// Create Vertex Array Object
			GLuint vao = GL.GenVertexArray();
			GL.BindVertexArray(vao);

			// Create a Vertex Buffer Object and copy the vertex data to it
			//GLuint vbo = GL.GenBuffer();
			//GL.BindBuffer(BufferTarget.Array, vbo);
			//float[] interlacedBuffer = {
			//	-0.5f,-0.5f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, // Top-left
			//	 0.5f,-0.5f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, // Top-right
			//	 0.0f, 0.5f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f
			//};
			//GL.BufferData(BufferTarget.Array, sizeof(float) * interlacedBuffer.Length, interlacedBuffer, BufferUsage.StaticDraw);

			//Vertex[] vertices = new Vertex[3];
			//vertices[0].Position = new Vector3(-0.5f, -0.5f, 0.0f);
			//vertices[0].Colour = Colour.Red;
			//vertices[1].Position = new Vector3(0.5f, -0.5f, 0.0f);
			//vertices[1].Colour = Colour.Green;
			//vertices[2].Position = new Vector3(0.0f, 0.5f, 0.0f);
			//vertices[2].Colour = Colour.Blue;

			//GLuint vbo = GL.GenBuffer();
			//GL.BindBuffer(BufferTarget.Array, vbo);
			//GL.BufferData(BufferTarget.Array, sizeof(float) * vertices.Length * Vertex.FloatStride, Vertex.GetVBO(vertices), BufferUsage.StaticDraw);

			VertexBuffer vbo = new VertexBuffer();

			// Create an Index Buffer
			IndexBuffer ibo = new IndexBuffer();
			GLint[] indices = {
				0, 1, 2
			};
			ibo.LoadData(indices, BufferUsage.StaticDraw);

			// Shader - create/compile/link/set program
			shader.Set();

			// Specify the layout of the vertex data
			Vertex.SetAttributes();

			// Variable for moving window on mouse click and drag
			Point2 windowDraggingPosition = Point2.Zero;

			while (Platform.Running)
			{
				Time.Update();

				Input.Clear();

				Platform.PollEvents();

				Input.Update();

				Audio.Update();

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

				//--------------------

				Window.Title = $"{Title} - {Time.DeltaTime.ToString("n1")}ms - Info: {Platform.OS} | {Time.TotalSeconds.ToString("n1")} - Buffers: {Audio.BufferCount} - Sources: {Audio.SourceCount} - Position: [{Window.X}, {Window.Y}] - Size: [{Window.Width}, {Window.Height}] - Mouse: [{Input.Mouse.Position.X}, {Input.Mouse.Position.Y}]";

				// Clear the screen
				if (Input.Keyboard.Down(KeyCode.Space))
				{ }
				//Graphics.Clear(Colour.Grey * 0.25f);
				else
					Graphics.Clear(Colour.Black);

				// Set Shader uniforms
				//shader.SetUniform("offset", Time.TotalMilliseconds * 0.0016f, Time.TotalMilliseconds * 0.002f, Time.TotalMilliseconds * 0.0023f);

				int num = 3 * 5;
				Vertex[] vertices = new Vertex[num];
				//float[] rawvertices = new float[num * Vertex.FloatStride];
				for (int i = 0; i < num; i++)
				{
					vertices[i].Position = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0.0f);
					vertices[i].Colour = new Colour(Random.Range(1f), Random.Range(1f), Random.Range(1f), 1f);

					//rawvertices[i * Vertex.FloatStride + 0] = Random.Range(-1f, 1f);
					//rawvertices[i * Vertex.FloatStride + 1] = Random.Range(-1f, 1f);
					//rawvertices[i * Vertex.FloatStride + 2] = Random.Range(-1f, 1f);
					//rawvertices[i * Vertex.FloatStride + 3] = Random.Range(1f);
					//rawvertices[i * Vertex.FloatStride + 4] = Random.Range(1f);
					//rawvertices[i * Vertex.FloatStride + 5] = Random.Range(1f);
					//rawvertices[i * Vertex.FloatStride + 6] = 1f;
				}
				//vertices[0].Position = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0.0f);
				//vertices[0].Colour = Colour.Red;
				//vertices[1].Position = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0.0f);
				//vertices[1].Colour = Colour.Green;
				//vertices[2].Position = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0.0f);
				//vertices[2].Colour = Colour.Blue;

				vbo.Bind();
				vbo.LoadData(Vertex.GetVBO(vertices), BufferUsage.DynamicDraw);
				//vbo.Load(rawvertices, BufferUsage.DynamicDraw);

				//GL.DrawArrays(DrawMode.Triangles, 0, num);
				//int numDrawCalls = 1;
				//for (int i = 0; i < numDrawCalls; i++)
				//	GL.DrawArrays(DrawMode.Triangles, i * vertices.Length / numDrawCalls, vertices.Length / numDrawCalls);

				GL.DrawElements(DrawMode.Triangles, ibo.Count, ibo.IndexType, 0);

				// Swap buffers
				Window.SwapBuffers();
			}

			vbo.Destroy();

			Audio.Destroy();
			Platform.Destroy();
		}
	}
}