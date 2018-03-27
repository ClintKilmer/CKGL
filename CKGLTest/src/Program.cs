using CKGL;

using GLint = System.Int32;
using GLuint = System.UInt32;
//using GLfloat = System.Float;

namespace CKGLTest
{
	class Program
	{
		private static string Title = "CKGL Game!";
		private static int WindowWidth = 640;
		private static int WindowHeight = 360;

		static void Main(string[] args)
		{
			SDL.Init(Title, WindowWidth, WindowHeight, true, false);
			Audio.Init();

			// Load Shaders
			Shader shader = Shader.FromFile("Shaders/test.glsl");

			// Load Audio
			Audio.Buffer sndPop1 = new Audio.Buffer("snd/sndPop1.wav");
			Audio.Buffer sndPop2 = new Audio.Buffer("snd/sndPop2.wav");
			Audio.Source source1 = new Audio.Source();

			// Create Vertex Array Object
			GLuint vao = GL.GenVertexArray();
			GL.BindVertexArray(vao);

			// Create a Vertex Buffer Object and copy the vertex data to it
			GLuint vbo = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.Array, vbo);
			float[] verticies = {
				-0.5f,  -0.5f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, // Top-left
				 0.5f,  -0.5f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, // Top-right
				 0.0f, 0.5f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f
			};
			GL.BufferData(BufferTarget.Array, sizeof(float) * verticies.Length, verticies, BufferUsage.StaticDraw);

			// Create an Index Buffer
			GLuint ibo = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ElementArray, ibo);
			GLuint[] indices = {
				0, 1, 2
			};
			GL.BufferData(BufferTarget.ElementArray, sizeof(GLuint) * indices.Length, indices, BufferUsage.StaticDraw);

			// Shader - create/compile/link/set program
			shader.Use();

			// Specify the layout of the vertex data
			GL.EnableVertexAttribArray(0);
			GL.EnableVertexAttribArray(1);
			GL.VertexAttribPointer(0, 3, VertexType.Float, false, 7 * sizeof(float), 0);
			GL.VertexAttribPointer(1, 4, VertexType.Float, false, 7 * sizeof(float), 3 * sizeof(float));

			while (SDL.Running)
			{
				if (Input.Keyboard.Down(KeyCode.Backspace))
					SDL.Running = false;

				if (Input.Keyboard.Pressed(KeyCode.Space))
				{
					source1.BindBuffer(sndPop1);
					source1.Play();
				}
				if (Input.Keyboard.Released(KeyCode.Space))
				{
					source1.BindBuffer(sndPop2);
					source1.Play();
				}

				//--------------------

				//give windows some time as well as capture SDL events
				SDL.PollEvents();

				Window.Title = $"{Title} - Info: {Window.Platform} - Position: [{Window.X}, {Window.Y}] - Size: [{Window.Width}, {Window.Height}]";

				GL.Viewport(0, 0, Window.Width, Window.Height);

				// Clear the screen
				if (Input.Keyboard.Down(KeyCode.Space))
					GL.ClearColour(Colour.Grey * 0.25f);
				else
					GL.ClearColour(Colour.Black);
				GL.Clear(BufferBit.Color | BufferBit.Depth);

				// Set Shader uniforms
				shader.SetUniform("offset", SDL2.SDL.SDL_GetTicks() * 0.0016f, SDL2.SDL.SDL_GetTicks() * 0.002f, SDL2.SDL.SDL_GetTicks() * 0.0023f);

				//GL.DrawArrays(DrawMode.Triangles, 0, 3);
				GL.DrawElements(DrawMode.Triangles, indices.Length, IndexType.UnsignedInt, 0);

				// Swap buffers
				SDL.SwapBuffers();

				//SDL.Delay(1);
			}

			Audio.Destroy();
			SDL.Exit();
		}
	}
}