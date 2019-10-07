using System;

namespace CKGL
{
	public static class Engine
	{
		public static float RAM => 0f;

		public static bool UnfocusedFrameLimiter = false;
		public static uint UnfocusedFrameLimiterSleep = 33;
		private static bool focused = true;

		private static Game game;

		public static void Init(string windowTitle, int windowWidth, int windowHeight, bool windowVSync, bool windowFullscreen, bool windowResizable, bool windowBorderless, int msaa)
		{
			try
			{
				Platform.Init(windowTitle, windowWidth, windowHeight, windowVSync, windowFullscreen, windowResizable, windowBorderless, msaa);
				Graphics.Init();
				//Audio.Init();
				Renderer.Init();

				//Platform.Events.OnWinFocusGained += () => { focused = true; OnFocusGained(); };
				//Platform.Events.OnWinFocusLost += () => { focused = false; OnFocusLost(); };
				//Platform.Events.OnWinResized += () => { OnWindowResized(); };
			}
			catch (Exception e)
			{
				Platform.DisplayException(e);
			}
		}

		public static void Run(Game game)
		{
			Engine.game = game;
			try
			{
				GameLoop();
			}
			catch (Exception e)
			{
				Platform.DisplayException(e);
			}
		}

		public static void GameLoop()
		{
			game.Init();

			//Window.Visible = true;
			InnerGameLoop(0);

			//game.Destroy();
			//PostDestroy();
		}

		public static void InnerGameLoop(double timestamp)
		{
			//while (Platform.Running)
			//{
			//	Time.Tick();

			//	while (Time.DoUpdate)
			//	{
			PreUpdate();
			game.Update();
			//		Time.Update();
			//	}

			//	if (Window.VSync || Time.DoDraw)
			//	{
			PreDraw();
			game.Draw();
			//		Window.SwapBuffers();
			//		Time.Draw();
			//		if (UnfocusedFrameLimiter && !focused)
			//			Platform.Delay(UnfocusedFrameLimiterSleep);
			//	}
			//}

			// Temporary
			Retyped.dom.window.requestAnimationFrame(InnerGameLoop);
			//Global.SetTimeout(InnerGameLoop, 1000 / 60);
		}

		private static void PreUpdate()
		{
			Platform.Update();
			//Audio.Update();
		}

		private static void PreDraw()
		{
			Graphics.PreDraw();
			Framebuffer.PreDraw();
			Texture.PreDraw();
			Shader.PreDraw();
			Window.Resize(); //  WebGL specific
		}

		public static void OnFocusGained() => game.OnFocusGained();
		public static void OnFocusLost() => game.OnFocusGained();
		public static void OnWindowResized() => game.OnFocusGained();

		private static void PostDestroy()
		{
			//Platform.Events.OnWinFocusGained = null;
			//Platform.Events.OnWinFocusLost = null;
			//Platform.Events.OnWinResized = null;

			Renderer.Destroy();
			//Audio.Destroy();
			Platform.Destroy();
		}
	}
}