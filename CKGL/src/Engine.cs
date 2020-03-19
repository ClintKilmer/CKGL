using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

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

		static Engine()
		{
			// This is a workaround for Linux - Relative paths (sdl/gamecontrollerdb.txt) were not relative to the assembly directory
			Directory.SetCurrentDirectory(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location));

			#region NativeLibrary DLL Import Resolver
#if NETCOREAPP
			NativeLibrary.SetDllImportResolver(typeof(Engine).Assembly, (string libraryName, Assembly assembly, DllImportSearchPath? searchPath) =>
			{
				IntPtr libHandle;
				if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && Environment.Is64BitOperatingSystem)
					_ = libraryName switch
					{
						"SDL2" => NativeLibrary.TryLoad("libs/win-x64/SDL2.dll", out libHandle),
						"SDL2_image" => NativeLibrary.TryLoad("libs/win-x64/SDL2_image.dll", out libHandle),
						"soft_oal" => NativeLibrary.TryLoad("libs/win-x64/soft_oal.dll", out libHandle),
						"libEGL" => NativeLibrary.TryLoad("libs/win-x64/libEGL.dll", out libHandle),
						"libGLESv2" => NativeLibrary.TryLoad("libs/win-x64/libGLESv2.dll", out libHandle),
						_ => throw new CKGLException($"Invalid native library name: {libraryName}")
					};
				else
				if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && !Environment.Is64BitOperatingSystem)
					_ = libraryName switch
					{
						"SDL2" => NativeLibrary.TryLoad("libs/win-x86/SDL2.dll", out libHandle),
						"SDL2_image" => NativeLibrary.TryLoad("libs/win-x86/SDL2_image.dll", out libHandle),
						"soft_oal" => NativeLibrary.TryLoad("libs/win-x86/soft_oal.dll", out libHandle),
						"libEGL" => NativeLibrary.TryLoad("libs/win-x86/libEGL.dll", out libHandle),
						"libGLESv2" => NativeLibrary.TryLoad("libs/win-x86/libGLESv2.dll", out libHandle),
						_ => throw new CKGLException($"Invalid native library name: {libraryName}")
					};
				else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && Environment.Is64BitOperatingSystem)
					_ = libraryName switch
					{
						"SDL2" => NativeLibrary.TryLoad("libs/linux-x64/libSDL2-2.0.so.0", out libHandle),
						"SDL2_image" => NativeLibrary.TryLoad("libs/linux-x64/libSDL2_image-2.0.so.0", out libHandle),
						"soft_oal" => NativeLibrary.TryLoad("libs/linux-x64/libopenal.so.1", out libHandle),
						_ => throw new CKGLException($"Invalid native library name: {libraryName}")
					};
				else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) && Environment.Is64BitOperatingSystem)
					_ = libraryName switch
					{
						"SDL2" => NativeLibrary.TryLoad("libs/osx-x64/libSDL2-2.0.0.dylib", out libHandle),
						"SDL2_image" => NativeLibrary.TryLoad("libs/osx-x64/libSDL2_image-2.0.0.dylib", out libHandle),
						"soft_oal" => NativeLibrary.TryLoad("libs/osx-x64/libopenal.1.dylib", out libHandle),
						_ => throw new CKGLException($"Invalid native library name: {libraryName}")
					};
				else
					throw new CKGLException("Unsupported Operating System");
				return libHandle;
			});
#endif
			#endregion
		}

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