namespace CKGL
{
	public abstract class Game
	{
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

			Platform.OnWinFocusGained += () => { OnFocusGained(); };
			Platform.OnWinFocusLost += () => { OnFocusLost(); };
			Platform.OnWinResized += () => { OnWindowResized(); };
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
					Draw();
					Window.SwapBuffers();
					Time.Draw();
					//Platform.Delay(1);
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

		public abstract void Draw();

		public abstract void OnFocusGained();
		public abstract void OnFocusLost();
		public abstract void OnWindowResized();

		public abstract void Destroy();

		private void PostDestroy()
		{
			Renderer.Destroy();
			Audio.Destroy();
			Window.Destroy();
			Platform.Destroy();
		}
	}
}