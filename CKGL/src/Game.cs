namespace CKGL
{
	public abstract class Game
	{
		public bool UnfocusedFrameLimiter = true;
		public uint UnfocusedFrameLimiterSleep = 33;
		private bool focused = true;

		public Game(string windowTitle, int windowWidth, int windowHeight, bool windowVSync, bool windowFullscreen, bool windowResizable, bool windowBorderless)
		{
			PreInit(windowTitle, windowWidth, windowHeight, windowVSync, windowFullscreen, windowResizable, windowBorderless);
			GameLoop();
		}

		private void PreInit(string windowTitle, int windowWidth, int windowHeight, bool windowVSync, bool windowFullscreen, bool windowResizable, bool windowBorderless)
		{
			Platform.Init();
			Window.Create(windowTitle, windowWidth, windowHeight, windowVSync, windowFullscreen, windowResizable, windowBorderless);
			Graphics.Init();
			Audio.Init();
			Input.Init();
			Renderer.Init();

			Platform.Events.OnWinFocusGained += () => { focused = true; OnFocusGained(); };
			Platform.Events.OnWinFocusLost += () => { focused = false; OnFocusLost(); };
			Platform.Events.OnWinResized += () => { OnWindowResized(); };
		}

		public void GameLoop()
		{
			Init();
			while (Platform.Running)
			{
				Time.Tick();

				while (Time.DoUpdate)
				{
					PreUpdate();
					Update();
					Time.Update();
				}

				if (Window.VSync || Time.DoDraw)
				{
					PreDraw();
					Draw();
					Window.SwapBuffers();
					Time.Draw();
					if (UnfocusedFrameLimiter && !focused)
						Platform.Delay(UnfocusedFrameLimiterSleep);
				}
			}
			Destroy();
			PostDestroy();
		}

		public abstract void Init();

		private void PreUpdate()
		{
			Input.Clear();
			Platform.PollEvents();
			Input.Update();
			Audio.Update();
		}

		public abstract void Update();

		private void PreDraw()
		{
			Graphics.PreDraw();
		}

		public abstract void Draw();

		public abstract void OnFocusGained();
		public abstract void OnFocusLost();
		public abstract void OnWindowResized();

		public abstract void Destroy();

		private void PostDestroy()
		{
			Platform.Events.OnWinFocusGained = null;
			Platform.Events.OnWinFocusLost = null;
			Platform.Events.OnWinResized = null;

			Renderer.Destroy();
			Audio.Destroy();
			Window.Destroy();
			Platform.Destroy();
		}
	}
}