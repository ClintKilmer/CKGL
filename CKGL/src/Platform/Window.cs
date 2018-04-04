using System;
using static SDL2.SDL;

namespace CKGL
{
	public static class Window
	{
		private static IntPtr IntPtr = IntPtr.Zero;
		private static IntPtr GL_Context;

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

		public class VSyncMode
		{
			// TODO - VSyncMode - LateSwapTearing
			public static int LateSwapTearing { get; } = -1;
			public static int Off { get; } = 0;
			public static int On { get; } = 1;
		}
		public static bool VSync
		{
			get { return SDL_GL_GetSwapInterval() == VSyncMode.On; }
			set { SDL_GL_SetSwapInterval(value ? VSyncMode.On : VSyncMode.Off); }
		}

		public static class FullscreenMode
		{
			public static uint Off { get; } = 0;
			public static uint On { get; } = (uint)SDL_WindowFlags.SDL_WINDOW_FULLSCREEN;
			public static uint Desktop { get; } = (uint)SDL_WindowFlags.SDL_WINDOW_FULLSCREEN_DESKTOP;
		}
		public static bool Fullscreen
		{
			get
			{
				return ((SDL_WindowFlags)SDL_GetWindowFlags(IntPtr) & (SDL_WindowFlags)FullscreenMode.Desktop) == (SDL_WindowFlags)FullscreenMode.Desktop;
			}
			set
			{
				SDL_SetWindowFullscreen(IntPtr, value ? FullscreenMode.Desktop : FullscreenMode.Off);
			}
		}

		public static bool Resizable
		{
			get
			{
				return ((SDL_WindowFlags)SDL_GetWindowFlags(IntPtr) & SDL_WindowFlags.SDL_WINDOW_RESIZABLE) == SDL_WindowFlags.SDL_WINDOW_RESIZABLE;
			}
			set
			{
				SDL_SetWindowResizable(IntPtr, value ? SDL_bool.SDL_TRUE : SDL_bool.SDL_FALSE);
			}
		}

		public static bool Borderless
		{
			get
			{
				return ((SDL_WindowFlags)SDL_GetWindowFlags(IntPtr) & SDL_WindowFlags.SDL_WINDOW_BORDERLESS) == SDL_WindowFlags.SDL_WINDOW_BORDERLESS;
			}
			set
			{
				SDL_SetWindowBordered(IntPtr, value ? SDL_bool.SDL_FALSE : SDL_bool.SDL_TRUE);
			}
		}

		public static void Center()
		{
			SDL_SetWindowPosition(IntPtr, SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED);
		}

		public static void SwapBuffers()
		{
			SDL_GL_SwapWindow(IntPtr);
		}

		public static void Create(string title, int width, int height, bool vsync, bool fullscreen, bool resizable, bool borderless)
		{
			SDL_WindowFlags flags = SDL_WindowFlags.SDL_WINDOW_SHOWN |
									SDL_WindowFlags.SDL_WINDOW_OPENGL |
									SDL_WindowFlags.SDL_WINDOW_ALLOW_HIGHDPI;

			if (fullscreen)
				flags |= SDL_WindowFlags.SDL_WINDOW_FULLSCREEN_DESKTOP;

			if (resizable)
				flags |= SDL_WindowFlags.SDL_WINDOW_RESIZABLE;

			if (borderless)
				flags |= SDL_WindowFlags.SDL_WINDOW_BORDERLESS;

			if ((IntPtr = SDL_CreateWindow(title, SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED, width, height, flags)) == IntPtr.Zero)
			{
				SDL_Quit();
				throw new Exception(SDL_GetError());
			}
			// Create OpenGL Context
			if ((GL_Context = SDL_GL_CreateContext(IntPtr)) == IntPtr.Zero)
			{
				Destroy();
				throw new Exception(SDL_GetError());
			}
			SDL_GL_MakeCurrent(IntPtr, GL_Context);
			VSync = vsync;
		}

		public static void Destroy()
		{
			if (GL_Context != IntPtr.Zero)
				SDL_GL_DeleteContext(GL_Context);
		}
	}
}