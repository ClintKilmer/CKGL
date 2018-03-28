using System;
using System.IO;
using System.Runtime.InteropServices;
using static SDL2.SDL;

namespace CKGL
{
	public static class Platform
	{
		#region Events
		public delegate void KeyEvent(KeyCode keyCode, ScanCode scanCode, bool repeat);
		public delegate void MouseMoveEvent(int x, int y);
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
		#endregion

		public static bool Running = false;
		public static string OSVersion { get; private set; }
		public static IntPtr GL_Context;

		private static SDL_Event Event;

		public static bool ShowCursor
		{
			get
			{
				return SDL_ShowCursor(SDL_QUERY) == 1;
			}
			set
			{
				SDL_ShowCursor(value ? SDL_ENABLE : SDL_DISABLE);
			}
		}

		public static string Clipboard
		{
			get
			{
				return SDL_GetClipboardText();
			}
			set
			{
				SDL_SetClipboardText(value);
			}
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
		public static void Init(string windowTitle, int windowWidth, int windowHeight, bool windowFullscreen, bool windowResizeable, bool windowBorderless)
		{
			#region Flibit - shims
			SetDllDirectory();

			// Missing libs?
			try
			{
				OSVersion = SDL_GetPlatform();
				Console.WriteLine($"Platform: {OSVersion}");
			}
			catch (Exception e)
			{
				Console.WriteLine("SDL2 was not found! Do you have fnalibs?");
				throw e;
			}

			/* SDL2 might complain if an OS that uses SDL_main has not actually
			 * used SDL_main by the time you initialize SDL2.
			 * The only platform that is affected is Windows, but we can skip
			 * their WinMain. This was only added to prevent iOS from exploding.
			 * -flibit
			 */
			SDL_SetMainReady();

			// Also, Windows is an idiot. -flibit
			if (OSVersion.Equals("Windows") || OSVersion.Equals("WinRT"))
			{
				// Visual Studio is an idiot.
				if (System.Diagnostics.Debugger.IsAttached)
				{
					SDL_SetHint(
						SDL_HINT_WINDOWS_DISABLE_THREAD_NAMING,
						"1"
					);
				}

				// TODO
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

			// This _should_ be the first real SDL call we make...
			if (SDL_Init(SDL_INIT_VIDEO | SDL_INIT_TIMER | SDL_INIT_JOYSTICK | SDL_INIT_GAMECONTROLLER | SDL_INIT_HAPTIC) < 0)
			{
				SDL_Quit();
				throw new Exception(SDL_GetError());
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

			// Create Window
			Window.Create(windowTitle, windowWidth, windowHeight, windowFullscreen, windowResizeable, windowBorderless);

			//OpenGL attributes
			SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_CONTEXT_MAJOR_VERSION, 3);
			SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_CONTEXT_MINOR_VERSION, 1);
			SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_CONTEXT_PROFILE_MASK, (int)SDL_GLprofile.SDL_GL_CONTEXT_PROFILE_CORE);
			//SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_CONTEXT_FLAGS, (int)SDL_GLcontext.SDL_GL_CONTEXT_FORWARD_COMPATIBLE_FLAG);
			SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_DOUBLEBUFFER, 1);
			//SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_DEPTH_SIZE, 24);
			//SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_STENCIL_SIZE, 8);
			//SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_MULTISAMPLEBUFFERS, 1);
			//SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_MULTISAMPLESAMPLES, 3);

			// Create OpenGL Context
			if ((GL_Context = SDL_GL_CreateContext(Window.IntPtr)) == IntPtr.Zero)
			{
				SDL_Quit();
				throw new Exception(SDL_GetError());
			}
			SDL_GL_MakeCurrent(Window.IntPtr, GL_Context);
			VSync = true;

			Running = true;
		}

		public static void SwapBuffers()
		{
			SDL_GL_SwapWindow(Window.IntPtr);
		}

		public static void Delay(uint ms)
		{
			SDL_Delay(ms); //release the thread
		}

		public static void Exit()
		{
			SDL_GL_DeleteContext(GL_Context);
			// This _should_ be the last SDL call we make...
			SDL_Quit();
		}
		#endregion

		public static void PollEvents()
		{
			while (SDL_PollEvent(out Event) != 0)
			{
				//Console.WriteLine(Event.type.ToString());
				switch (Event.type)
				{
					case SDL_EventType.SDL_QUIT:
						Running = false;
						break;
					case SDL_EventType.SDL_KEYDOWN:
						OnKeyDown?.Invoke((KeyCode)Event.key.keysym.sym, (ScanCode)Event.key.keysym.scancode, Event.key.repeat != 0);
						break;
					case SDL_EventType.SDL_KEYUP:
						OnKeyUp?.Invoke((KeyCode)Event.key.keysym.sym, (ScanCode)Event.key.keysym.scancode, Event.key.repeat != 0);
						break;
					case SDL_EventType.SDL_MOUSEMOTION:
						OnMouseMove?.Invoke(Event.motion.x, Event.motion.y);
						break;
					case SDL_EventType.SDL_MOUSEBUTTONDOWN:
						OnMouseButtonDown?.Invoke(Event.button.button);
						break;
					case SDL_EventType.SDL_MOUSEBUTTONUP:
						OnMouseButtonUp?.Invoke(Event.button.button);
						break;
					case SDL_EventType.SDL_MOUSEWHEEL:
						OnMouseScroll?.Invoke(Event.wheel.x, Event.wheel.y);
						break;
					//case SDL_EventType.SDL_JOYDEVICEADDED:
					//	OnJoyDeviceAdd?.Invoke(Event.JDevice.Which);
					//	break;
					//case SDL_EventType.SDL_JOYDEVICEREMOVED:
					//	OnJoyDeviceRemove?.Invoke(Event.JDevice.Which);
					//	break;
					//case SDL_EventType.SDL_JOYBUTTONDOWN:
					//	OnJoyButtonDown?.Invoke(Event.JButton.Which, Event.JButton.Button);
					//	break;
					//case SDL_EventType.SDL_JOYBUTTONUP:
					//	OnJoyButtonDown?.Invoke(Event.JButton.Which, Event.JButton.Button);
					//	break;
					//case SDL_EventType.SDL_JOYAXISMOTION:
					//	OnJoyAxisMove?.Invoke(Event.JAxis.Which, Event.JAxis.Axis, Event.JAxis.Value / (float)short.MaxValue);
					//	break;
					//case SDL_EventType.SDL_JOYHATMOTION:
					//	OnJoyAxisMove?.Invoke(Event.JAxis.Which, Event.JAxis.Axis, Event.JAxis.Value / (float)short.MaxValue);
					//	break;
					//case SDL_EventType.SDL_JOYBALLMOTION:
					//	OnJoyAxisMove?.Invoke(Event.JAxis.Which, Event.JAxis.Axis, Event.JAxis.Value / (float)short.MaxValue);
					//	break;
					case SDL_EventType.SDL_WINDOWEVENT:
						if (Event.window.windowID == Window.ID)
						{
							//Console.WriteLine(Event.window.windowEvent.ToString());
							switch (Event.window.windowEvent)
							{
								case SDL_WindowEventID.SDL_WINDOWEVENT_CLOSE:
									OnWinClose?.Invoke();
									break;
								case SDL_WindowEventID.SDL_WINDOWEVENT_SHOWN:
									OnWinShown?.Invoke();
									break;
								case SDL_WindowEventID.SDL_WINDOWEVENT_HIDDEN:
									OnWinHidden?.Invoke();
									break;
								case SDL_WindowEventID.SDL_WINDOWEVENT_EXPOSED:
									OnWinExposed?.Invoke();
									break;
								case SDL_WindowEventID.SDL_WINDOWEVENT_MOVED:
									OnWinMoved?.Invoke();
									break;
								case SDL_WindowEventID.SDL_WINDOWEVENT_RESIZED:
									OnWinResized?.Invoke();
									break;
								case SDL_WindowEventID.SDL_WINDOWEVENT_MINIMIZED:
									OnWinMinimized?.Invoke();
									break;
								case SDL_WindowEventID.SDL_WINDOWEVENT_MAXIMIZED:
									OnWinMaximized?.Invoke();
									break;
								case SDL_WindowEventID.SDL_WINDOWEVENT_RESTORED:
									OnWinRestored?.Invoke();
									break;
								case SDL_WindowEventID.SDL_WINDOWEVENT_ENTER:
									OnWinEnter?.Invoke();
									break;
								case SDL_WindowEventID.SDL_WINDOWEVENT_LEAVE:
									OnWinLeave?.Invoke();
									break;
								case SDL_WindowEventID.SDL_WINDOWEVENT_FOCUS_GAINED:
									OnWinFocusGained?.Invoke();
									break;
								case SDL_WindowEventID.SDL_WINDOWEVENT_FOCUS_LOST:
									OnWinFocusLost?.Invoke();
									break;
								default:
									OnWinOtherEvent?.Invoke((int)Event.window.windowEvent);
									break;
							}
						}
						break;
				}
			}
		}

		public static void GetGlobalMousePosition(out int x, out int y)
		{
			SDL_GetGlobalMouseState(out x, out y);
		}

		public static IntPtr GetProcAddress(string proc)
		{
			return SDL_GL_GetProcAddress(proc);
		}

		public class VSyncMode // TODO
		{
			public static int LateSwapTearing { get; } = -1; // TODO
			public static int Off { get; } = 0;
			public static int On { get; } = 1;
		}
		private static bool vSync = false;
		public static bool VSync
		{
			get
			{
				return vSync;
			}
			set
			{
				if (vSync != value)
				{
					if (SDL_GL_SetSwapInterval(value ? VSyncMode.On : VSyncMode.Off) == 0)
						vSync = value;
				}
			}
		}

		// TODO
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