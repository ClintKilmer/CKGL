using System;
using System.IO;
using static SDL2.SDL;

namespace CKGL
{
	public static class Window
	{
		private static IntPtr IntPtr;
		private static IntPtr GL_Context;

		public static uint ID => SDL_GetWindowID(IntPtr);

		public static string Title { get => SDL_GetWindowTitle(IntPtr); set => SDL_SetWindowTitle(IntPtr, value); }

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

		public static int X => Position.X;
		public static int Y => Position.Y;

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

		public static int Width => Size.X;
		public static int Height => Size.Y;

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
			get { return SDL_GL_GetSwapInterval() != VSyncMode.Off; }
			set
			{
				if (value)
				{
					if (Platform.OS == OS.WinRT || Platform.OS == OS.Mac) // TODO - Check for Google ANGLE here, as it reports success, even though it doesn't seem to work
					{
						SDL_GL_SetSwapInterval(VSyncMode.On);
					}
					else
					{
						if (SDL_GL_SetSwapInterval(VSyncMode.Adaptive) == -1)
						{
							SDL_ClearError();
							SDL_GL_SetSwapInterval(VSyncMode.On);
							Output.WriteLine("Adaptive VSync Mode not supported. Standard VSync enabled instead.");
						}
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
			get => ((SDL_WindowFlags)SDL_GetWindowFlags(IntPtr) & (SDL_WindowFlags)FullscreenMode.Desktop) == (SDL_WindowFlags)FullscreenMode.Desktop;
			set => SDL_SetWindowFullscreen(IntPtr, value ? FullscreenMode.Desktop : FullscreenMode.Off);
		}

		public static void FullscreenReset() => Fullscreen = Fullscreen;

		public static bool Resizable
		{
			get => ((SDL_WindowFlags)SDL_GetWindowFlags(IntPtr) & SDL_WindowFlags.SDL_WINDOW_RESIZABLE) == SDL_WindowFlags.SDL_WINDOW_RESIZABLE;
			set => SDL_SetWindowResizable(IntPtr, value ? SDL_bool.SDL_TRUE : SDL_bool.SDL_FALSE);
		}

		public static bool Borderless
		{
			get => ((SDL_WindowFlags)SDL_GetWindowFlags(IntPtr) & SDL_WindowFlags.SDL_WINDOW_BORDERLESS) == SDL_WindowFlags.SDL_WINDOW_BORDERLESS;
			set => SDL_SetWindowBordered(IntPtr, value ? SDL_bool.SDL_FALSE : SDL_bool.SDL_TRUE);
		}

		public static bool Visible
		{
			get => ((SDL_WindowFlags)SDL_GetWindowFlags(IntPtr) & SDL_WindowFlags.SDL_WINDOW_SHOWN) == SDL_WindowFlags.SDL_WINDOW_SHOWN;
			set { if (value) SDL_ShowWindow(IntPtr); else SDL_HideWindow(IntPtr); }
		}

		public static void Center() => SDL_SetWindowPosition(IntPtr, SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED);

		public static void SwapBuffers() => SDL_GL_SwapWindow(IntPtr);

		public static float Opacity
		{
			get
			{
				SDL_GetWindowOpacity(IntPtr, out float opacity);
				return opacity;
			}
			set => SDL_SetWindowOpacity(IntPtr, value);
		}

		public static void Init(string title, int width, int height, bool vsync, bool fullscreen, bool resizable, bool borderless)
		{
			SDL_WindowFlags flags = SDL_WindowFlags.SDL_WINDOW_HIDDEN |
									SDL_WindowFlags.SDL_WINDOW_ALLOW_HIGHDPI;

			switch (Platform.GraphicsBackend)
			{
#if VULKAN
				case GraphicsBackend.Vulkan:
					flags |= SDL_WindowFlags.SDL_WINDOW_VULKAN;
					break;
#endif
#if OPENGL
				case GraphicsBackend.OpenGL:
#endif
#if OPENGLES
				case GraphicsBackend.OpenGLES:
#endif
#if OPENGL || OPENGLES
					flags |= SDL_WindowFlags.SDL_WINDOW_OPENGL;
					break;
#endif
				default:
					throw new NotSupportedException($"GraphicsBackend {Platform.GraphicsBackend} not supported.");
			}

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
			if (Platform.GraphicsBackend == GraphicsBackend.OpenGL || Platform.GraphicsBackend == GraphicsBackend.OpenGLES)
			{
				if ((GL_Context = SDL_GL_CreateContext(IntPtr)) == IntPtr.Zero)
				{
					Destroy();
					throw new Exception(SDL_GetError());
				}
				SDL_GL_MakeCurrent(IntPtr, GL_Context);
				Output.WriteLine($"OpenGL Context Initialized");
			}

			VSync = vsync;
		}

		public static void Destroy()
		{
			if (GL_Context != default)
			{
				SDL_GL_DeleteContext(GL_Context);
				GL_Context = default;

				SDL_SetHintWithPriority(SDL_HINT_VIDEO_MINIMIZE_ON_FOCUS_LOSS, "0", SDL_HintPriority.SDL_HINT_OVERRIDE);

				SDL_DestroyWindow(IntPtr);
				IntPtr = default;
			}
		}
	}
}