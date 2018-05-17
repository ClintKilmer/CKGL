using System;
using System.IO;
using System.Runtime.InteropServices;
using SDL2;
using static SDL2.SDL;

namespace CKGL
{
	public static class Platform
	{
		#region Events
		public static class Events
		{
			public delegate void KeyEvent(KeyCode keyCode, ScanCode scanCode, bool repeat);
			public delegate void MouseMoveEvent(int x, int y, int xRelative, int yRelative);
			public delegate void MouseButtonEvent(int buttonID);
			public delegate void MouseScrollEvent(int x, int y);
			public delegate void JoyDeviceEvent(int deviceID);
			public delegate void JoyButtonEvent(int deviceID, int buttonID);
			public delegate void JoyAxisEvent(int deviceID, int axisID, float value);
			public delegate void OtherEvent(int eventID);

			public static Action OnQuit;
			public static KeyEvent OnKeyDown;
			public static KeyEvent OnKeyRepeat;
			public static KeyEvent OnKeyUp;
			public static MouseMoveEvent OnMouseMove;
			public static MouseButtonEvent OnMouseButtonDown;
			public static MouseButtonEvent OnMouseButtonUp;
			public static MouseScrollEvent OnMouseScroll;
			public static JoyDeviceEvent OnJoyDeviceAdd;
			public static JoyDeviceEvent OnJoyDeviceRemove;
			public static JoyButtonEvent OnJoyButtonDown;
			public static JoyButtonEvent OnJoyButtonUp;
			public static JoyAxisEvent OnJoyAxisMove;
			public static Action OnWinClose;
			public static Action OnWinShown;
			public static Action OnWinHidden;
			public static Action OnWinExposed;
			public static Action OnWinMoved;
			public static Action OnWinResized;
			public static Action OnWinMinimized;
			public static Action OnWinMaximized;
			public static Action OnWinRestored;
			public static Action OnWinEnter;
			public static Action OnWinLeave;
			public static Action OnWinFocusGained;
			public static Action OnWinFocusLost;
			public static OtherEvent OnWinOtherEvent;
			public static OtherEvent OnOtherEvent;
		}
		#endregion

		public static bool Running { get; private set; } = false;

		private static SDL_Event Event;

		public const int ScanCodeMask = SDLK_SCANCODE_MASK;

		public static string OS { get { return SDL_GetPlatform(); } }
		//public static uint TotalMilliseconds { get { return SDL_GetTicks(); } }
		public static ulong PerformanceCounter { get { return SDL_GetPerformanceCounter(); } }
		public static ulong PerformanceFrequency { get { return SDL_GetPerformanceFrequency(); } }

		public static bool ShowCursor // Default true
		{
			get { return SDL_ShowCursor(SDL_QUERY) == 1; }
			set { SDL_ShowCursor(value ? SDL_ENABLE : SDL_DISABLE); }
		}

		public static string Clipboard
		{
			get { return SDL_GetClipboardText(); }
			set { SDL_SetClipboardText(value); }
		}

		public static bool ScreensaverAllowed // Default false
		{
			get { return SDL_IsScreenSaverEnabled() == SDL_bool.SDL_TRUE; }
			set { if (value) SDL_EnableScreenSaver(); else SDL_DisableScreenSaver(); }
		}

		#region Windows - Set Dll Directory - x64 | x86 - https://github.com/FNA-XNA/FNA/wiki/4:-FNA-and-Windows-API#64-bit-support
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool SetDllDirectory(string lpPathName);
		private static void SetDllDirectory()
		{
			if (Environment.OSVersion.Platform == PlatformID.Win32NT)
				SetDllDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Environment.Is64BitProcess ? "x64" : "x86"));
		}
		#endregion

		#region Init/Exit Methods
		public static void Init()
		{
			SetDllDirectory();

			#region Check for SDL2 libs
			try
			{
				string test = OS;
			}
			catch (Exception e)
			{
				Output.WriteLine("SDL2 was not found! Do you have fnalibs?");
				throw e;
			}
			#endregion

			#region Flibit - shims
			/* SDL2 might complain if an OS that uses SDL_main has not actually
			 * used SDL_main by the time you initialize SDL2.
			 * The only platform that is affected is Windows, but we can skip
			 * their WinMain. This was only added to prevent iOS from exploding.
			 * -flibit
			 */
			SDL_SetMainReady();

			// Also, Windows is an idiot. -flibit
			if (OS.Equals("Windows") || OS.Equals("WinRT"))
			{
				// Visual Studio is an idiot.
				if (System.Diagnostics.Debugger.IsAttached)
				{
					SDL_SetHint(
						SDL_HINT_WINDOWS_DISABLE_THREAD_NAMING,
						"1"
					);
				}

				// TODO - Win32 WM_PAINT Interop
				/* Windows has terrible event pumping and doesn't give us
				 * WM_PAINT events correctly. So we get to do this!
				 * -flibit
				 */
				//SDL_SetEventFilter(win32OnPaint, IntPtr.Zero);
			}
			#endregion

			// If available, load the SDL_GameControllerDB
			//string mappingsDB = Path.Combine(
			//	TitleLocation.Path,
			//	"gamecontrollerdb.txt"
			//);
			//if (File.Exists(mappingsDB))
			//{
			//	SDL_GameControllerAddMappingsFromFile(
			//		mappingsDB
			//	);
			//}

			// SDL Init
			if (SDL_Init(SDL_INIT_VIDEO | SDL_INIT_TIMER | SDL_INIT_JOYSTICK | SDL_INIT_GAMECONTROLLER | SDL_INIT_HAPTIC) < 0)
			{
				Destroy();
				throw new Exception(SDL_GetError());
			}

			// SDL Version Check
			if (SDLVersion != SDL2CSVersion)
			{
				Destroy();
				throw new Exception($"SDL2-CS was expecting v{SDL2CSVersion}, but found SDL DLL v{SDLVersion}");
			}

			//// We want to initialize the controllers ASAP!
			//SDL_Event[] evt = new SDL_Event[1];
			//SDL_PumpEvents();
			//while (SDL_PeepEvents(
			//	evt,
			//	1,
			//	SDL_eventaction.SDL_GETEVENT,
			//	SDL_EventType.SDL_CONTROLLERDEVICEADDED,
			//	SDL_EventType.SDL_CONTROLLERDEVICEADDED
			//) == 1)
			//{
			//	INTERNAL_AddInstance(evt[0].cdevice.which);
			//}

			// OpenGL attributes
			SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_CONTEXT_MAJOR_VERSION, 3);
			SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_CONTEXT_MINOR_VERSION, 1);
			SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_CONTEXT_PROFILE_MASK, (int)SDL_GLprofile.SDL_GL_CONTEXT_PROFILE_CORE);
			//SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_CONTEXT_FLAGS, (int)SDL_GLcontext.SDL_GL_CONTEXT_FORWARD_COMPATIBLE_FLAG);
			SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_DOUBLEBUFFER, 1);
			SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_RED_SIZE, 8);
			SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_GREEN_SIZE, 8);
			SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_BLUE_SIZE, 8);
			SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_ALPHA_SIZE, 8);
			SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_BUFFER_SIZE, 32);
			//SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_DEPTH_SIZE, 24);
			//SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_STENCIL_SIZE, 8);
			//SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_MULTISAMPLEBUFFERS, 1); // Handled in Window
			//SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_MULTISAMPLESAMPLES, 4); // Handled in Window

			Running = true;

			// Debug
			Output.WriteLine($"Platform SDL2 Initialized");
			Output.WriteLine($"SDL DLL Version: v{SDLVersion} | SDL2-CS Version: v{SDL2CSVersion}");
			Output.WriteLine($"Platform - OS: {OS}");
			Output.WriteLine($"Platform - Video Driver: {SDL_GetCurrentVideoDriver()}");
			Output.WriteLine($"Platform - Audio Driver: {SDL_GetCurrentAudioDriver()}");
			Output.WriteLine($"Platform - # of CPUs: {CPUCount}");
			Output.WriteLine($"Platform - Total RAM: {RAMTotalMB}MB");

			Input.Init();
		}

		public static void Destroy()
		{
			SDL_Quit();
		}
		#endregion

		public static void PollEvents()
		{
			while (SDL_PollEvent(out Event) != 0)
			{
				//Output.WriteLine(Event.type.ToString());
				switch (Event.type)
				{
					case SDL_EventType.SDL_QUIT:
						Events.OnQuit?.Invoke();
						Quit();
						break;
					case SDL_EventType.SDL_KEYDOWN:
						Events.OnKeyDown?.Invoke((KeyCode)Event.key.keysym.sym, (ScanCode)Event.key.keysym.scancode, Event.key.repeat != 0);
						break;
					case SDL_EventType.SDL_KEYUP:
						Events.OnKeyUp?.Invoke((KeyCode)Event.key.keysym.sym, (ScanCode)Event.key.keysym.scancode, Event.key.repeat != 0);
						break;
					case SDL_EventType.SDL_MOUSEMOTION:
						Events.OnMouseMove?.Invoke(Event.motion.x, Event.motion.y, Event.motion.xrel, Event.motion.yrel);
						break;
					case SDL_EventType.SDL_MOUSEBUTTONDOWN:
						Events.OnMouseButtonDown?.Invoke(Event.button.button);
						break;
					case SDL_EventType.SDL_MOUSEBUTTONUP:
						Events.OnMouseButtonUp?.Invoke(Event.button.button);
						break;
					case SDL_EventType.SDL_MOUSEWHEEL:
						Events.OnMouseScroll?.Invoke(Event.wheel.x, Event.wheel.y);
						break;
					//case SDL_EventType.SDL_JOYDEVICEADDED:
					//	Events.OnJoyDeviceAdd?.Invoke(Event.JDevice.Which);
					//	break;
					//case SDL_EventType.SDL_JOYDEVICEREMOVED:
					//	Events.OnJoyDeviceRemove?.Invoke(Event.JDevice.Which);
					//	break;
					//case SDL_EventType.SDL_JOYBUTTONDOWN:
					//	Events.OnJoyButtonDown?.Invoke(Event.JButton.Which, Event.JButton.Button);
					//	break;
					//case SDL_EventType.SDL_JOYBUTTONUP:
					//	Events.OnJoyButtonDown?.Invoke(Event.JButton.Which, Event.JButton.Button);
					//	break;
					//case SDL_EventType.SDL_JOYAXISMOTION:
					//	Events.OnJoyAxisMove?.Invoke(Event.JAxis.Which, Event.JAxis.Axis, Event.JAxis.Value / (float)short.MaxValue);
					//	break;
					//case SDL_EventType.SDL_JOYHATMOTION:
					//	Events.OnJoyAxisMove?.Invoke(Event.JAxis.Which, Event.JAxis.Axis, Event.JAxis.Value / (float)short.MaxValue);
					//	break;
					//case SDL_EventType.SDL_JOYBALLMOTION:
					//	Events.OnJoyAxisMove?.Invoke(Event.JAxis.Which, Event.JAxis.Axis, Event.JAxis.Value / (float)short.MaxValue);
					//	break;
					case SDL_EventType.SDL_WINDOWEVENT:
						if (Event.window.windowID == Window.ID)
						{
							//Output.WriteLine(Event.window.windowEvent.ToString());
							switch (Event.window.windowEvent)
							{
								case SDL_WindowEventID.SDL_WINDOWEVENT_CLOSE:
									Events.OnWinClose?.Invoke();
									break;
								case SDL_WindowEventID.SDL_WINDOWEVENT_SHOWN:
									Events.OnWinShown?.Invoke();
									break;
								case SDL_WindowEventID.SDL_WINDOWEVENT_HIDDEN:
									Events.OnWinHidden?.Invoke();
									break;
								case SDL_WindowEventID.SDL_WINDOWEVENT_EXPOSED:
									Events.OnWinExposed?.Invoke();
									break;
								case SDL_WindowEventID.SDL_WINDOWEVENT_MOVED:
									Events.OnWinMoved?.Invoke();
									break;
								case SDL_WindowEventID.SDL_WINDOWEVENT_RESIZED:
									Events.OnWinResized?.Invoke();
									break;
								case SDL_WindowEventID.SDL_WINDOWEVENT_MINIMIZED:
									Events.OnWinMinimized?.Invoke();
									break;
								case SDL_WindowEventID.SDL_WINDOWEVENT_MAXIMIZED:
									Events.OnWinMaximized?.Invoke();
									break;
								case SDL_WindowEventID.SDL_WINDOWEVENT_RESTORED:
									Events.OnWinRestored?.Invoke();
									break;
								case SDL_WindowEventID.SDL_WINDOWEVENT_ENTER:
									Events.OnWinEnter?.Invoke();
									break;
								case SDL_WindowEventID.SDL_WINDOWEVENT_LEAVE:
									Events.OnWinLeave?.Invoke();
									break;
								case SDL_WindowEventID.SDL_WINDOWEVENT_FOCUS_GAINED:
									Events.OnWinFocusGained?.Invoke();
									break;
								case SDL_WindowEventID.SDL_WINDOWEVENT_FOCUS_LOST:
									Events.OnWinFocusLost?.Invoke();
									break;
								default:
									Events.OnWinOtherEvent?.Invoke((int)Event.window.windowEvent);
									break;
							}
						}
						break;
				}
			}
		}

		public static void Quit()
		{
			Running = false;
		}

		public static bool RelativeMouseMode // Default true
		{
			get { return SDL_GetRelativeMouseMode() == SDL_bool.SDL_TRUE; }
			set { SDL_SetRelativeMouseMode(value ? SDL_bool.SDL_TRUE : SDL_bool.SDL_FALSE); }
		}

		public static string SDLVersion
		{
			get
			{
				SDL_GetVersion(out SDL_version ver);
				return $"{ver.major}.{ver.minor}.{ver.patch}";
			}
		}

		public static string SDL2CSVersion
		{
			get
			{
				return $"{SDL_MAJOR_VERSION}.{SDL_MINOR_VERSION}.{SDL_PATCHLEVEL}";
			}
		}

		public static int CPUCount => SDL_GetCPUCount();

		public static int RAMTotalMB => SDL_GetSystemRAM();

		public static void GetGlobalMousePosition(out int x, out int y)
		{
			SDL_GetGlobalMouseState(out x, out y);
		}

		public static void GetMousePosition(out int x, out int y)
		{
			SDL_GetMouseState(out x, out y);
		}

		public static void GetRelativeMousePosition(out int x, out int y)
		{
			SDL_GetRelativeMouseState(out x, out y);
		}

		public static IntPtr GetProcAddress(string proc)
		{
			return SDL_GL_GetProcAddress(proc);
		}

		public static void Delay(uint ms)
		{
			SDL_Delay(ms); //release the thread
		}

		#region Texture<->Image Load/Save Methods | Derived From FNA - SDL2_FNAPlatform.cs - https://github.com/FNA-XNA/FNA
		private static byte[] FlipImageData(int width, int height, PixelFormat pixelFormat, byte[] data)
		{
			byte[] result = new byte[data.Length];

			int stride = width * pixelFormat.Components();
			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < stride; x++)
				{
					result[x + y * stride] = data[x + (height - y - 1) * stride];
				}
			}

			return result;
		}

		public static void GetImageData(string file, out int width, out int height, out byte[] data)
		{
			IntPtr surfaceID = SDL_image.IMG_Load(file);
			if (surfaceID == IntPtr.Zero)
				throw new FileNotFoundException($"TextureDataFromStream: {SDL_GetError()}", file);

			unsafe
			{
				SDL_Surface* surface = (SDL_Surface*)surfaceID;
				width = surface->w;
				height = surface->h;
				data = new byte[width * height * PixelFormat.RGBA.Components()];
				Marshal.Copy(surface->pixels, data, 0, data.Length);
			}
			SDL_FreeSurface(surfaceID);

			// Enforce alpha
			for (int i = 0; i < data.Length; i += 4)
			{
				if (data[i + 3] == 0)
				{
					data[i] = 0;
					data[i + 1] = 0;
					data[i + 2] = 0;
				}
			}

			data = FlipImageData(width, height, PixelFormat.RGBA, data);
		}

		public static void SavePNG(string file, int destinationWidth, int destinationHeight, int sourceWidth, int sourceHeight, byte[] data)
		{
			IntPtr surface = INTERNAL_getScaledSurface(
				FlipImageData(sourceWidth, sourceHeight, PixelFormat.RGBA, data),
				sourceWidth,
				sourceHeight,
				destinationWidth,
				destinationHeight
			);
			SDL_image.IMG_SavePNG(surface, file);
			SDL_FreeSurface(surface);
		}

		public static void SaveJPG(string file, int destinationWidth, int destinationHeight, int sourceWidth, int sourceHeight, byte[] data)
		{
			IntPtr surface = INTERNAL_getScaledSurface(
				FlipImageData(sourceWidth, sourceHeight, PixelFormat.RGBA, data),
				sourceWidth,
				sourceHeight,
				destinationWidth,
				destinationHeight
			);

			//FIXME: Hack for Bugzilla #3972
			IntPtr temp = SDL_ConvertSurfaceFormat(surface, SDL_PIXELFORMAT_RGB24, 0);
			SDL_FreeSurface(surface);
			surface = temp;

			SDL_image.IMG_SaveJPG(surface, file, 100);
			SDL_FreeSurface(surface);
		}

		private static IntPtr INTERNAL_getScaledSurface(byte[] data, int srcW, int srcH, int dstW, int dstH)
		{
			// Create an SDL_Surface*, write the pixel data
			IntPtr surface = SDL_CreateRGBSurface(
				0,
				srcW,
				srcH,
				32,
				0x000000FF,
				0x0000FF00,
				0x00FF0000,
				0xFF000000
			);
			SDL_LockSurface(surface);
			unsafe
			{
				SDL_Surface* surPtr = (SDL_Surface*)surface;
				Marshal.Copy(data, 0, surPtr->pixels, data.Length);
			}
			SDL_UnlockSurface(surface);

			// Blit to a scaled surface of the size we want, if needed.
			if (srcW != dstW || srcH != dstH)
			{
				IntPtr scaledSurface = SDL_CreateRGBSurface(
					0,
					dstW,
					dstH,
					32,
					0x000000FF,
					0x0000FF00,
					0x00FF0000,
					0xFF000000
				);
				SDL_SetSurfaceBlendMode(surface, SDL_BlendMode.SDL_BLENDMODE_NONE);
				SDL_BlitScaled(surface, IntPtr.Zero, scaledSurface, IntPtr.Zero);
				SDL_FreeSurface(surface);
				surface = scaledSurface;
			}

			return surface;
		}

		private static unsafe IntPtr INTERNAL_convertSurfaceFormat(IntPtr surface)
		{
			IntPtr result = surface;
			unsafe
			{
				SDL_Surface* surPtr = (SDL_Surface*)surface;
				SDL_PixelFormat* pixelFormatPtr = (SDL_PixelFormat*)surPtr->format;

				// SurfaceFormat.Color is SDL_PIXELFORMAT_ABGR8888
				if (pixelFormatPtr->format != SDL_PIXELFORMAT_ABGR8888)
				{
					// Create a properly formatted copy, free the old surface
					result = SDL_ConvertSurfaceFormat(surface, SDL_PIXELFORMAT_ABGR8888, 0);
					SDL_FreeSurface(surface);
				}
			}
			return result;
		}
		#endregion

		// TODO - Win32 WM_PAINT Interop
		#region Private Static Win32 WM_PAINT Interop
		//private static SDL_EventFilter win32OnPaint = Win32OnPaint;
		//private static unsafe int Win32OnPaint(IntPtr func, IntPtr evtPtr)
		//{
		//	SDL_Event* evt = (SDL_Event*)evtPtr;
		//	if (evt->type == SDL_EventType.SDL_WINDOWEVENT &&
		//		evt->window.windowEvent == SDL_WindowEventID.SDL_WINDOWEVENT_EXPOSED)
		//	{
		//		if (evt->window.windowID == SDL_GetWindowID(Window.IntPtr))
		//		{
		//			game.RedrawWindow();
		//			return 0;
		//		}
		//	}
		//	return 1;
		//}
		#endregion
	}
}