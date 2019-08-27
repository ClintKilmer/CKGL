using System;
using Bridge;
using Bridge.Html5;
using Bridge.WebGL;

using static CKGL.GFX;

namespace CKGL
{
	// Temporary
	public static class GFX
	{
		public static WebGLRenderingContext gl = null;

		public enum WEBGL_debug_renderer_info
		{
			UNMASKED_VENDOR_WEBGL = 0x9245,
			UNMASKED_RENDERER_WEBGL = 0x9246
		}
	}

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
			//Platform.Init(windowTitle, windowWidth, windowHeight, windowVSync, windowFullscreen, windowResizable, windowBorderless, msaa);
			//Graphics.Init();
			//Audio.Init();
			//Renderer.Init();

			//Platform.Events.OnWinFocusGained += () => { focused = true; OnFocusGained(); };
			//Platform.Events.OnWinFocusLost += () => { focused = false; OnFocusLost(); };
			//Platform.Events.OnWinResized += () => { OnWindowResized(); };

			Document.Title = windowTitle;

			HTMLCanvasElement canvas = new HTMLCanvasElement
			{
				Width = windowWidth,
				Height = windowHeight
			};
			Document.Body.AppendChild(canvas);

			// Create 3D Context
			string[] names = new string[] {
				"webgl2",
				"experimental-webgl2",
				"webgl",
				"experimental-webgl",
				//"webkit-3d",
				//"moz-webgl"
			};

			foreach (string name in names)
			{
				try
				{
					gl = canvas.GetContext(name).As<WebGLRenderingContext>();
					//gl = (WebGLRenderingContext)canvas.GetContext(name);
				}
				catch { }

				if (gl != null)
					break;
			}

			if (gl == null)
				canvas.ParentElement.ReplaceChild(new HTMLParagraphElement { InnerHTML = "<b>Either the browser doesn't support WebGL or it is disabled.<br>Please follow <a href=\"http://get.webgl.com\">Get WebGL</a>.</b>" }, canvas);

			Console.WriteLine($"Window.Navigator - Platform: {Window.Navigator.Platform}");
			Console.WriteLine($"Window.Navigator - UserAgent: {Window.Navigator.UserAgent}");
			Console.WriteLine($"WebGL Context - GLSL Version: {gl.GetParameter(gl.SHADING_LANGUAGE_VERSION)}");
			Console.WriteLine($"WebGL Context - VERSION: {gl.GetParameter(gl.VERSION)}");
			Console.WriteLine($"WebGL Context - VENDOR: {gl.GetParameter(gl.VENDOR)}");
			Console.WriteLine($"WebGL Context - RENDERER: {gl.GetParameter(gl.RENDERER)}");
			var dbgRenderInfo = gl.GetExtension("WEBGL_debug_renderer_info");
			if (dbgRenderInfo != null)
			{
				Console.WriteLine($"WebGL Context - WEBGL_debug_renderer_info.UNMASKED_VENDOR_WEBGL: {gl.GetParameter(WEBGL_debug_renderer_info.UNMASKED_VENDOR_WEBGL)}");
				Console.WriteLine($"WebGL Context - WEBGL_debug_renderer_info.UNMASKED_RENDERER_WEBGL: {gl.GetParameter(WEBGL_debug_renderer_info.UNMASKED_RENDERER_WEBGL)}");
			}
			//Console.WriteLine($"WebGL - Extensions: \n{string.Join("\n", gl.GetSupportedExtensions())}");
		}

		WebGLProgram shader;
		WebGLBuffer buffer;
		int positionAttrib;
		int colourAttrib;
		public void Run()
		{
			WebGLShader vs = gl.CreateShader(gl.VERTEX_SHADER);
			WebGLShader fs = gl.CreateShader(gl.FRAGMENT_SHADER);
			gl.ShaderSource(vs, @"
attribute vec3 position;
attribute vec4 colour;
 
varying vec4 vColour;
 
void main(void)
{
	gl_Position = vec4(position, 1.0);
	vColour = colour;
}");
			gl.ShaderSource(fs, @"
precision mediump float;

varying vec4 vColour;

void main(void)
{
	gl_FragColor = vColour;
}");
			gl.CompileShader(vs);
			gl.CompileShader(fs);
			shader = (WebGLProgram)gl.CreateProgram();
			gl.AttachShader(shader, vs);
			gl.AttachShader(shader, fs);
			gl.LinkProgram(shader);
			gl.UseProgram(shader);

			positionAttrib = gl.GetAttribLocation(shader, "position");
			colourAttrib = gl.GetAttribLocation(shader, "colour");

			gl.EnableVertexAttribArray(positionAttrib);
			gl.EnableVertexAttribArray(colourAttrib);

			buffer = gl.CreateBuffer();
			gl.BindBuffer(gl.ARRAY_BUFFER, buffer);
			var vertexData = new Float32Array(new float[] {
				// X    Y     Z     R     G     B     A
				0.0f, 0.8f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f,
				// X    Y     Z     R     G     B     A
				-0.8f, -0.8f, 0.0f, 0.0f, 1.0f, 0.0f, 1.0f,
				// X    Y     Z     R     G     B     A
				0.8f, -0.8f, 0.0f, 0.0f, 0.0f, 1.0f, 1.0f});
			gl.BufferData(gl.ARRAY_BUFFER, vertexData, gl.STATIC_DRAW);

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
			//gl.ClearColor(Math.Sin(Date.Now() * 0.005f) * 0.5f + 0.5f,
			//			  Math.Sin(Date.Now() * 0.0075f) * 0.5f + 0.5f,
			//			  Math.Sin(Date.Now() * 0.01f) * 0.5f + 0.5f,
			//			  1.0);
			gl.ClearColor(0f, 0f, 0f, 1f);
			gl.Clear(gl.COLOR_BUFFER_BIT);

			var vertexData = new Float32Array(new float[] {
				// X    Y     Z     R     G     B     A
				(float)Math.Sin(Date.Now() * 0.003f), 0.8f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f,
				// X    Y     Z     R     G     B     A
				-0.8f, (float)Math.Sin(Date.Now() * 0.003f), 0.0f, 0.0f, 1.0f, 0.0f, 1.0f,
				// X    Y     Z     R     G     B     A
				0.8f, -(float)Math.Sin(Date.Now() * 0.003f), 0.0f, 0.0f, 0.0f, 1.0f, 1.0f});
			gl.BufferData(gl.ARRAY_BUFFER, vertexData, gl.STREAM_DRAW);

			gl.BindBuffer(gl.ARRAY_BUFFER, buffer);

			var stride = 7 * Float32Array.BYTES_PER_ELEMENT;
			// Set up position stream
			gl.VertexAttribPointer(positionAttrib, 3, gl.FLOAT, false, stride, 0);
			// Set up color stream
			gl.VertexAttribPointer(colourAttrib, 4, gl.FLOAT, false, stride, 3 * Float32Array.BYTES_PER_ELEMENT);

			gl.DrawArrays(gl.TRIANGLES, 0, 3);

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
			//Graphics.PreDraw();
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