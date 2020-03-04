using System.Diagnostics;

namespace CKGL
{
	public static class Engine
	{
		private static Process process = Process.GetCurrentProcess();
		internal static Process Process
		{
			get
			{
				process.Refresh();
				return process;
			}
		}

		public static float RAM => process.WorkingSet64 * 0.000001f;

		public static bool UnfocusedFrameLimiter = false;
		public static uint UnfocusedFrameLimiterSleep = 33;
		private static bool focused = true;

		private static Game game;

		public static void Init(string windowTitle, int windowWidth, int windowHeight, bool windowVSync, bool windowFullscreen, bool windowResizable, bool windowBorderless, int msaa)
		{
			Platform.Init(windowTitle, windowWidth, windowHeight, windowVSync, windowFullscreen, windowResizable, windowBorderless, msaa);
			Graphics.Init();
			Audio.Init();
			Renderer.Init();

			Platform.Events.OnWinFocusGained += () => { focused = true; OnFocusGained(); };
			Platform.Events.OnWinFocusLost += () => { focused = false; OnFocusLost(); };
			Platform.Events.OnWinResized += () => { OnWindowResized(); };
		}

		public static void Run(Game game)
		{
			Engine.game = game;
			GameLoop();
		}

		public static void GameLoop()
		{
			game.Init();
			Window.Visible = true;
			while (Platform.Running)
			{
				Time.Tick();

				while (Time.DoUpdate)
				{
					PreUpdate();
					game.Update();
					Time.Update();
				}

				if (Window.VSync || Time.DoDraw)
				{
					PreDraw();
					game.Draw();
					Window.SwapBuffers();
					Time.Draw();
					if (UnfocusedFrameLimiter && !focused)
						Platform.Delay(UnfocusedFrameLimiterSleep);
				}
			}
			game.Destroy();
			PostDestroy();
		}

		private static void PreUpdate()
		{
			Platform.Update();
			Audio.Update();
		}

		private static void PreDraw()
		{
			Graphics.PreDraw();
			Framebuffer.PreDraw();
			Texture.PreDraw();
			Shader.PreDraw();
		}

		public static void OnFocusGained() => game.OnFocusGained();
		public static void OnFocusLost() => game.OnFocusLost();
		public static void OnWindowResized() => game.OnWindowResized();

		private static void PostDestroy()
		{
			Platform.Events.OnWinFocusGained = null;
			Platform.Events.OnWinFocusLost = null;
			Platform.Events.OnWinResized = null;

			Renderer.Destroy();
			Audio.Destroy();
			Platform.Destroy();
		}
	}
}