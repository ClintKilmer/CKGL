using System;
using System.IO;
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

		public static void SetIcon(string file)
		{
			if (!File.Exists(file))
			{
				Output.WriteLine($"Window.SetIcon() Error - The file \"{file}\" does not exist.");
				return;
			}

			if (file.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
			{
				try
				{
					IntPtr iconIntPtr = SDL2.SDL_image.IMG_Load(file);
					SDL_SetWindowIcon(IntPtr, iconIntPtr);
					SDL_FreeSurface(iconIntPtr);
				}
				catch (DllNotFoundException)
				{
					Output.WriteLine($"Window.SetIcon() Error - To load a .png icon, SDL_image must be supported.");
				}
			}
			else if (file.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase))
			{
				IntPtr iconIntPtr = SDL_LoadBMP(file);
				SDL_SetWindowIcon(IntPtr, iconIntPtr);
				SDL_FreeSurface(iconIntPtr);
			}
			else
			{
				Output.WriteLine($"Window.SetIcon() Error - Unsupported file extension. Please use .png or .bmp");
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
				//SDL_GetWindowSize(IntPtr, out size.X, out size.Y);
				SDL_GL_GetDrawableSize(IntPtr, out size.X, out size.Y);
				return size;
			}
			set
			{
				SDL_SetWindowSize(IntPtr, value.X, value.Y);
			}
		}

		public static float AspectRatio
		{
			get { return Width / (float)Height; }
		}

		#region VSync
		public class VSyncMode
		{
			// Late Swap Tearing - https://wiki.libsdl.org/SDL_GL_SetSwapInterval
			// Adaptive VSync - https://www.khronos.org/opengl/wiki/Swap_Interval
			public static int Adaptive { get; } = -1;
			public static int Off { get; } = 0;
			public static int On { get; } = 1;
		}
		public static bool VSync
		{
			get { return SDL_GL_GetSwapInterval() != VSyncMode.Off; }
			set
			{
				if (value)
				{
					if (SDL_GL_SetSwapInterval(VSyncMode.Adaptive) == -1)
					{
						SDL_GL_SetSwapInterval(VSyncMode.On);
						Output.WriteLine("Adaptive VSync Mode not supported. Standard VSync enabled instead.");
					}
				}
				else
				{
					SDL_GL_SetSwapInterval(VSyncMode.Off);
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
			switch (SDL_GL_GetSwapInterval())
			{
				case -1:
					return VSyncModeEnum.Adaptive;
				case 0:
					return VSyncModeEnum.Off;
				case 1:
					return VSyncModeEnum.On;
				default:
					throw new NotImplementedException("Unknown VSync Mode");

			}
		}
		#endregion

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

		public static void Create(string title, int width, int height, bool vsync, bool fullscreen, bool resizable, bool borderless, int msaa)
		{
			if (msaa < 0 || msaa > OpenGL.GL.MaxSamples)
				throw new ArgumentOutOfRangeException($"msaa out of range: (0 - {OpenGL.GL.MaxSamples})");

			SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_MULTISAMPLEBUFFERS, msaa > 0 ? 1 : 0);
			SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_MULTISAMPLESAMPLES, msaa);

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
			Output.WriteLine($"Window Initialized");

			// Create OpenGL Context
			if ((GL_Context = SDL_GL_CreateContext(IntPtr)) == IntPtr.Zero)
			{
				Destroy();
				throw new Exception(SDL_GetError());
			}
			SDL_GL_MakeCurrent(IntPtr, GL_Context);
			Output.WriteLine($"OpenGL Context Initialized");

			VSync = vsync;
		}

		public static void Destroy()
		{
			if (GL_Context != IntPtr.Zero)
				SDL_GL_DeleteContext(GL_Context);
		}
	}
}