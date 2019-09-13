using System;
using static Retyped.dom; // DOM / WebGL Types

namespace CKGL
{
	public static class Window
	{
		internal static uint ID => 0;

		public static string Title { get => document.title; set => document.title = value; }

		public static bool HighDPI { get; set; } = true; // WebGL only - can be toggled at runtime

		//public static void SetIcon(string file)
		//{
		//	if (!File.Exists(file))
		//	{
		//		Output.WriteLine($"Window.SetIcon() Error - The file \"{file}\" does not exist.");
		//		return;
		//	}

		//	if (file.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
		//	{
		//		try
		//		{
		//			IntPtr iconIntPtr = SDL2.SDL_image.IMG_Load(file);
		//			SDL_SetWindowIcon(IntPtr, iconIntPtr);
		//			SDL_FreeSurface(iconIntPtr);
		//		}
		//		catch (DllNotFoundException)
		//		{
		//			Output.WriteLine($"Window.SetIcon() Error - To load a .png icon, SDL_image must be supported.");
		//		}
		//	}
		//	else if (file.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase))
		//	{
		//		IntPtr iconIntPtr = SDL_LoadBMP(file);
		//		SDL_SetWindowIcon(IntPtr, iconIntPtr);
		//		SDL_FreeSurface(iconIntPtr);
		//	}
		//	else
		//	{
		//		Output.WriteLine($"Window.SetIcon() Error - Unsupported file extension. Please use .png or .bmp");
		//	}
		//}

		public static int X => Position.X;
		public static int Y => Position.Y;

		public static Point2 Position
		{
			get
			{
				return Point2.Zero;
			}
			set { }
		}

		public static int Width => (int)Platform.Canvas.width;
		public static int Height => (int)Platform.Canvas.height;

		public static Point2 Size
		{
			get
			{
				return new Point2((int)Platform.Canvas.width, (int)Platform.Canvas.height);
			}
			set
			{
				Platform.Canvas.width = (uint)value.X;
				Platform.Canvas.height = (uint)value.Y;
			}
		}

		public static float AspectRatio => Width / (float)Height;

		#region VSync
		private class VSyncMode
		{
			// Late Swap Tearing - https://wiki.libsdl.org/SDL_GL_SetSwapInterval
			// Adaptive VSync - https://www.khronos.org/opengl/wiki/Swap_Interval
			public static int Adaptive { get; } = -1;
			public static int Off { get; } = 0;
			public static int On { get; } = 1;
		}
		public static bool VSync
		{
			get => true;
			set
			{
				if (!value)
				{
					Output.WriteLine("WebGL does not support disabling VSync.");
				}
			}
		}

		public enum VSyncModeEnum
		{
			Adaptive = -1,
			Off = 0,
			On = 1
		};
		public static VSyncModeEnum GetVSyncMode()
		{
			switch (1)
			{
				//case -1:
				//	return VSyncModeEnum.Adaptive;
				//case 0:
				//	return VSyncModeEnum.Off;
				case 1:
					return VSyncModeEnum.On;
				default:
					throw new NotImplementedException("Unknown VSync Mode");
			}
		}
		#endregion

		//public static class FullscreenMode
		//{
		//	public static uint Off { get; } = 0;
		//	public static uint On { get; } = (uint)SDL_WindowFlags.SDL_WINDOW_FULLSCREEN;
		//	public static uint Desktop { get; } = (uint)SDL_WindowFlags.SDL_WINDOW_FULLSCREEN_DESKTOP;
		//}
		//public static bool Fullscreen
		//{
		//	get => ((SDL_WindowFlags)SDL_GetWindowFlags(IntPtr) & (SDL_WindowFlags)FullscreenMode.Desktop) == (SDL_WindowFlags)FullscreenMode.Desktop;
		//	set => SDL_SetWindowFullscreen(IntPtr, value ? FullscreenMode.Desktop : FullscreenMode.Off);
		//}

		//public static void FullscreenReset() => Fullscreen = Fullscreen;

		//public static bool Resizable
		//{
		//	get => ((SDL_WindowFlags)SDL_GetWindowFlags(IntPtr) & SDL_WindowFlags.SDL_WINDOW_RESIZABLE) == SDL_WindowFlags.SDL_WINDOW_RESIZABLE;
		//	set => SDL_SetWindowResizable(IntPtr, value ? SDL_bool.SDL_TRUE : SDL_bool.SDL_FALSE);
		//}

		//public static bool Borderless
		//{
		//	get => ((SDL_WindowFlags)SDL_GetWindowFlags(IntPtr) & SDL_WindowFlags.SDL_WINDOW_BORDERLESS) == SDL_WindowFlags.SDL_WINDOW_BORDERLESS;
		//	set => SDL_SetWindowBordered(IntPtr, value ? SDL_bool.SDL_FALSE : SDL_bool.SDL_TRUE);
		//}

		//public static bool Visible
		//{
		//	get => ((SDL_WindowFlags)SDL_GetWindowFlags(IntPtr) & SDL_WindowFlags.SDL_WINDOW_SHOWN) == SDL_WindowFlags.SDL_WINDOW_SHOWN;
		//	set { if (value) SDL_ShowWindow(IntPtr); else SDL_HideWindow(IntPtr); }
		//}

		//public static void Center() => SDL_SetWindowPosition(IntPtr, SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED);

		//public static void SwapBuffers() => SDL_GL_SwapWindow(IntPtr);

		public static float Opacity
		{
			get
			{
				return 1f;
			}
			set { }
		}

		public static void Init(string title, int width, int height, bool vsync, bool fullscreen, bool resizable, bool borderless)
		{
			document.documentElement.style.overflow = "hidden";
			document.documentElement.style.setProperty("margin", "0", "important");
			document.documentElement.style.setProperty("padding", "0", "important");
			document.body.style.overflow = "hidden";
			document.body.style.setProperty("margin", "0", "important");
			document.body.style.setProperty("padding", "0", "important");

			Platform.Canvas = new HTMLCanvasElement
			{
				//width = fullscreen ? (uint)window.innerWidth : (uint)width,
				//height = fullscreen ? (uint)window.innerHeight : (uint)height,
				textContent = "<b>Either the browser doesn't support WebGL 2.0 or it is disabled.<br>Please follow the instructions at: <a href=\"https://get.webgl.org/webgl2/\" > get.webgl.org</a>.</b>"
			};
			Platform.Canvas.style.setProperty("width", "100vw");
			Platform.Canvas.style.setProperty("height", "100vh");
			Platform.Canvas.style.setProperty("display", "block");
			document.body.appendChild(Platform.Canvas);

			// Disable selection
			Platform.Canvas.style.setProperty("user-select", "none");
			Platform.Canvas.style.setProperty("-webkit-user-select", "none");
			Platform.Canvas.style.setProperty("-moz-user-select", "none");
			Platform.Canvas.style.setProperty("-ms-user-select", "none");

			Output.WriteLine($"Window Initialized");

			//VSync = vsync;
		}

		public static void Destroy()
		{
			document.body.removeChild(Platform.Canvas);
		}

		public static void Resize()
		{
			if (Platform.Canvas.width != Platform.Canvas.clientWidth || Platform.Canvas.height != Platform.Canvas.clientHeight)
			{
				if (HighDPI)
				{
					Platform.Canvas.width = (uint)Math.FloorToInt(Platform.Canvas.clientWidth * (float)window.devicePixelRatio);
					Platform.Canvas.height = (uint)Math.FloorToInt(Platform.Canvas.clientHeight * (float)window.devicePixelRatio);
				}
				else
				{
					Platform.Canvas.width = (uint)Math.FloorToInt(Platform.Canvas.clientWidth);
					Platform.Canvas.height = (uint)Math.FloorToInt(Platform.Canvas.clientHeight);
				}
			}
		}
	}
}