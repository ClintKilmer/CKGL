using static CKGL.WebGL.WebGLGraphics; // WebGL Context Methods
using static Retyped.dom; // WebGL Types
using static Retyped.webgl2.WebGL2RenderingContext; // WebGL Enums

namespace CKGL
{
	public abstract class Game
	{
		public struct Vertex
		{
			public Vector3 Position;
			public Colour Colour;

			public Vertex(Vector3 position, Colour colour)
			{
				Position = position;
				Colour = colour;
			}
		}

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

		public void Run()
		{
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
			Bridge.Html5.Window.RequestAnimationFrame(GameLoop);
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