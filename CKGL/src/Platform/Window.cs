using System;
using static SDL2.SDL;

namespace CKGL
{
	public class Window
	{
		public static IntPtr IntPtr = IntPtr.Zero;
		public static explicit operator IntPtr(Window Window)
		{
			return IntPtr;
		}

		public static uint ID
		{
			get
			{
				return SDL_GetWindowID(IntPtr);
			}
		}

		public static string Title
		{
			get
			{
				return SDL_GetWindowTitle(IntPtr);
			}
			set
			{
				SDL_SetWindowTitle(IntPtr, value);
			}
		}

		public static int X
		{
			get
			{
				return Position.X;
			}
		}
		public static int Y
		{
			get
			{
				return Position.Y;
			}
		}
		public static Point2 Position
		{
			get
			{
				Point2 position = Point2.Zero;
				SDL_GetWindowPosition(IntPtr, out position.X, out position.Y);
				return position;
			}
			set
			{
				SDL_SetWindowPosition(IntPtr, value.X, value.Y);
			}
		}

		public static int Width
		{
			get
			{
				return Size.X;
			}
		}
		public static int Height
		{
			get
			{
				return Size.Y;
			}
		}
		public static Point2 Size
		{
			get
			{
				Point2 size = Point2.Zero;
				SDL_GetWindowSize(IntPtr, out size.X, out size.Y);
				return size;
			}
			set
			{
				SDL_SetWindowSize(IntPtr, value.X, value.Y);
			}
		}
		public static string Platform
		{
			get
			{
				return $"{SDL_GetPlatform()} | {(SDL_GetTicks() * 0.001f).ToString("n0")}";
			}
		}

		public static class FullscreenMode // TODO
		{
			public static uint Off { get; } = 0;
			public static uint On { get; } = (uint)SDL_WindowFlags.SDL_WINDOW_FULLSCREEN;
			public static uint Desktop { get; } = (uint)SDL_WindowFlags.SDL_WINDOW_FULLSCREEN_DESKTOP;
		}
		private static bool fullscreen = false;
		public static bool Fullscreen
		{
			get
			{
				return fullscreen;
			}
			set
			{
				if (fullscreen != value)
				{
					if (SDL_SetWindowFullscreen(IntPtr, value ? FullscreenMode.Desktop : FullscreenMode.Off) == 0)
						fullscreen = value;
				}
			}
		}

		private static bool resizeable = false;
		public static bool Resizeable
		{
			get
			{
				return resizeable;
			}
			set
			{
				SDL_SetWindowResizable(IntPtr, value ? SDL_bool.SDL_TRUE : SDL_bool.SDL_FALSE);
				resizeable = value;
			}
		}

		private static bool borderless = false;
		public static bool Borderless
		{
			get
			{
				return borderless;
			}
			set
			{
				SDL_SetWindowBordered(IntPtr, value ? SDL_bool.SDL_FALSE : SDL_bool.SDL_TRUE);
				borderless = value;
			}
		}

		public static void Center()
		{
			SDL_SetWindowPosition(IntPtr, SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED);
		}

		public static void Create(string title, int width, int height, bool resizeable, bool borderless)
		{
			SDL_WindowFlags flags = SDL_WindowFlags.SDL_WINDOW_SHOWN |
									SDL_WindowFlags.SDL_WINDOW_OPENGL |
									SDL_WindowFlags.SDL_WINDOW_ALLOW_HIGHDPI;

			if (resizeable)
				flags |= SDL_WindowFlags.SDL_WINDOW_RESIZABLE;

			if (borderless)
				flags |= SDL_WindowFlags.SDL_WINDOW_BORDERLESS;

			if ((IntPtr = SDL_CreateWindow(title, SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED, width, height, flags)) == IntPtr.Zero)
			{
				SDL_Quit();
				throw new Exception(SDL_GetError());
			}
		}
	}
}