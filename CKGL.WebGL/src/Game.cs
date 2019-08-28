using System;
using Bridge;
using Bridge.Html5;
using Bridge.WebGL;

using static CKGL.WebGL.WebGLGraphics;

namespace CKGL
{
	public abstract class Game
	{
		public static float RAM
		{
			get
			{
				return 0f;
			}
		}

		public bool UnfocusedFrameLimiter = false;
		public uint UnfocusedFrameLimiterSleep = 33;
		private bool focused = true;

		public Game(string windowTitle, int windowWidth, int windowHeight, bool windowVSync, bool windowFullscreen, bool windowResizable, bool windowBorderless, int msaa)
		{
			Platform.Init(windowTitle, windowWidth, windowHeight, windowVSync, windowFullscreen, windowResizable, windowBorderless, msaa);
			Graphics.Init();
			//Audio.Init();
			//Renderer.Init();

			//Platform.Events.OnWinFocusGained += () => { focused = true; OnFocusGained(); };
			//Platform.Events.OnWinFocusLost += () => { focused = false; OnFocusLost(); };
			//Platform.Events.OnWinResized += () => { OnWindowResized(); };
		}

		WebGLProgram shader;
		WebGLBuffer buffer;
		int positionAttrib;
		int colourAttrib;
		public void Run()
		{
			WebGLShader vs = GL.CreateShader(GL.VERTEX_SHADER);
			WebGLShader fs = GL.CreateShader(GL.FRAGMENT_SHADER);
			GL.ShaderSource(vs, @"
attribute vec3 position;
attribute vec4 colour;
 
varying vec4 vColour;
 
void main(void)
{
	gl_Position = vec4(position, 1.0);
	vColour = colour;
}");
			GL.ShaderSource(fs, @"
precision mediump float;

varying vec4 vColour;

void main(void)
{
	gl_FragColor = vColour;
}");
			GL.CompileShader(vs);
			GL.CompileShader(fs);
			shader = (WebGLProgram)GL.CreateProgram();
			GL.AttachShader(shader, vs);
			GL.AttachShader(shader, fs);
			GL.LinkProgram(shader);
			GL.UseProgram(shader);

			positionAttrib = GL.GetAttribLocation(shader, "position");
			colourAttrib = GL.GetAttribLocation(shader, "colour");

			GL.EnableVertexAttribArray(positionAttrib);
			GL.EnableVertexAttribArray(colourAttrib);

			buffer = GL.CreateBuffer();
			GL.BindBuffer(GL.ARRAY_BUFFER, buffer);
			var vertexData = new Float32Array(new float[] {
				// X    Y     Z     R     G     B     A
				0.0f, 0.8f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f,
				// X    Y     Z     R     G     B     A
				-0.8f, -0.8f, 0.0f, 0.0f, 1.0f, 0.0f, 1.0f,
				// X    Y     Z     R     G     B     A
				0.8f, -0.8f, 0.0f, 0.0f, 0.0f, 1.0f, 1.0f});
			GL.BufferData(GL.ARRAY_BUFFER, vertexData, GL.STATIC_DRAW);

			GameLoop();
		}

		public void GameLoop()
		{
			Init();
			//Window.Visible = true;
			//while (Platform.Running)
			//{
			//	Time.Tick();

			//	while (Time.DoUpdate)
			//	{
			//		PreUpdate();
			//		Update();
			//		Time.Update();
			//	}

			//	if (Window.VSync || Time.DoDraw)
			//	{
			//		PreDraw();
			//		Draw();
			//		Window.SwapBuffers();
			//		Time.Draw();
			//		if (UnfocusedFrameLimiter && !focused)
			//			Platform.Delay(UnfocusedFrameLimiterSleep);
			//	}
			//}

			Update();
			Draw();


			// Temporary
			var vertexData = new Float32Array(new float[] {
				// X    Y     Z     R     G     B     A
				Math.Sin((float)Date.Now() * 0.003f), 0.8f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f,
				// X    Y     Z     R     G     B     A
				-0.8f, Math.Sin((float)Date.Now() * 0.003f), 0.0f, 0.0f, 1.0f, 0.0f, 1.0f,
				// X    Y     Z     R     G     B     A
				0.8f, -Math.Sin((float)Date.Now() * 0.003f), 0.0f, 0.0f, 0.0f, 1.0f, 1.0f});
			GL.BufferData(GL.ARRAY_BUFFER, vertexData, GL.STREAM_DRAW);

			GL.BindBuffer(GL.ARRAY_BUFFER, buffer);

			var stride = 7 * Float32Array.BYTES_PER_ELEMENT;
			// Set up position stream
			GL.VertexAttribPointer(positionAttrib, 3, GL.FLOAT, false, stride, 0);
			// Set up color stream
			GL.VertexAttribPointer(colourAttrib, 4, GL.FLOAT, false, stride, 3 * Float32Array.BYTES_PER_ELEMENT);

			GL.DrawArrays(GL.TRIANGLES, 0, 3);

			Window.RequestAnimationFrame(GameLoop);
			//Global.SetTimeout(GameLoop, 1000 / 60);

			Destroy();
			PostDestroy();
		}

		public abstract void Init();

		private void PreUpdate()
		{
			//Platform.Update();
			//Audio.Update();
		}

		public abstract void Update();

		private void PreDraw()
		{
			Graphics.PreDraw();
			//RenderTarget.PreDraw();
			//Texture.PreDraw();
			//Shader.PreDraw();
		}

		public abstract void Draw();

		public abstract void OnFocusGained();
		public abstract void OnFocusLost();
		public abstract void OnWindowResized();

		public abstract void Destroy();

		private void PostDestroy()
		{
			//Platform.Events.OnWinFocusGained = null;
			//Platform.Events.OnWinFocusLost = null;
			//Platform.Events.OnWinResized = null;

			//Renderer.Destroy();
			//Audio.Destroy();
			//Platform.Destroy();
		}
	}
}