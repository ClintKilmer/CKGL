using System;
using System.Collections.Generic;
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
			public delegate void MouseButtonEvent(Input.MouseButton button);
			public delegate void MouseScrollEvent(int x, int y);
			public delegate void TouchMoveEvent(int x, int y, int xRelative, int yRelative);
			public delegate void OnTouchEvent(long touchID, long fingerID, float x, float y, float xRelative, float yRelative, float pressure);
			public delegate void ControllerDeviceEvent(int id);
			public delegate void ControllerButtonEvent(int id, Input.ControllerButton button);
			public delegate void ControllerAxisEvent(int id, Input.ControllerAxis axis, float value);
			public delegate void OnDisplayOrientationEvent(uint id, int orientation);
			public delegate void OtherEvent(int eventID);

			public static Action OnQuit;
			public static KeyEvent OnKeyDown;
			public static KeyEvent OnKeyRepeat;
			public static KeyEvent OnKeyUp;
			public static MouseMoveEvent OnMouseMove;
			public static MouseButtonEvent OnMouseButtonDown;
			public static MouseButtonEvent OnMouseButtonUp;
			public static MouseScrollEvent OnMouseScroll;
			public static OnTouchEvent OnTouchDown;
			public static OnTouchEvent OnTouchUp;
			public static OnTouchEvent OnTouchMove;
			public static ControllerDeviceEvent OnControllerDeviceAdded;
			public static ControllerDeviceEvent OnControllerDeviceRemoved;
			public static ControllerDeviceEvent OnControllerDeviceRemapped;
			public static ControllerButtonEvent OnControllerButtonDown;
			public static ControllerButtonEvent OnControllerButtonUp;
			public static ControllerAxisEvent OnControllerAxisMove;
			public static OnDisplayOrientationEvent OnDisplayOrientationChanged;
			public static Action OnWinClose;
			public static Action OnWinShown;
			public static Action OnWinHidden;
			public static Action OnWinExposed;
			public static Action OnWinMoved;
			public static Action OnWinResized;
			public static Action OnWinSizeChanged;
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

		#region Controllers
		private static Dictionary<int, Controller> Controllers = new Dictionary<int, Controller>();

		internal static Controller GetController(int id)
		{
			if (Controllers.ContainsKey(id))
				return Controllers[id];

			return new Controller(-1, IntPtr.Zero);
		}

		public class Controller
		{
			public int DeviceIndex;
			public IntPtr IntPtr;

			public IntPtr JoystickIntPtr => SDL_GameControllerGetJoystick(IntPtr);
			public int InstanceID => SDL_JoystickInstanceID(JoystickIntPtr);
			public string Name => SDL_GameControllerName(IntPtr);
			public string GUID => (Vendor == 0x00 && Product == 0x00) ? "xinput" : string.Format("{0:x2}{1:x2}{2:x2}{3:x2}", Vendor & 0xFF, Vendor >> 8, Product & 0xFF, Product >> 8);
			public ushort Vendor => SDL_GameControllerGetVendor(IntPtr);
			public ushort Product => SDL_GameControllerGetProduct(IntPtr);
			public ushort ProductVersion => SDL_GameControllerGetProductVersion(IntPtr);
			public bool Rumble => SDL_GameControllerRumble(IntPtr, 0, 0, SDL_HAPTIC_INFINITY) == 0;

			public Controller(int deviceIndex, IntPtr intPtr)
			{
				DeviceIndex = deviceIndex;
				IntPtr = intPtr;

				// TODO: FNA PS4 Lightbar init goes here
			}

			public void Destroy()
			{
				SDL_GameControllerClose(IntPtr);
			}

			public override string ToString()
			{
				return $"Name: {Name}, DeviceIndex: {DeviceIndex}, InstanceID: {InstanceID}, GUID: {GUID}, Vendor: {Vendor}, Product: {Product}, Rumble: {Rumble}";
			}

			// Static

			public static void Added(int deviceIndex)
			{
				IntPtr controllerIntPtr = SDL_GameControllerOpen(deviceIndex);
				IntPtr joystickIntPtr = SDL_GameControllerGetJoystick(controllerIntPtr);
				int instanceID = SDL_JoystickInstanceID(joystickIntPtr);

				if (!Controllers.ContainsKey(instanceID))
				{
					Controller controller = new Controller(deviceIndex, controllerIntPtr);
					Controllers[controller.InstanceID] = controller;

					Events.OnControllerDeviceAdded?.Invoke(controller.InstanceID);

					// debug
					//Output.WriteLine($"Controller Added: {controller}");
					//Output.WriteLine($"Total Controllers: {Controllers.Count} | SDL_NumJoysticks: {SDL_NumJoysticks()}");
				}
			}

			public static void Removed(int instanceID)
			{
				if (Controllers.TryGetValue(instanceID, out Controller controller))
				{
					// debug
					//Output.WriteLine($"Controller Removed: {controller}");

					controller.Destroy();
					Controllers.Remove(instanceID);

					Events.OnControllerDeviceRemoved?.Invoke(instanceID);
				}

				// debug
				//Output.WriteLine($"Total Controllers: {Controllers.Count} | SDL_NumJoysticks: {SDL_NumJoysticks()}");
			}

			public static void Remapped(int instanceID)
			{
				Output.WriteLine($"SDL_CONTROLLERDEVICEREMAPPED not implemented. (SDL Controller Instance {instanceID})");

				Events.OnControllerDeviceRemapped?.Invoke(instanceID);
			}

			public static void ButtonDown(int instanceID, int buttonID)
			{
				Events.OnControllerButtonDown?.Invoke(instanceID, (Input.ControllerButton)buttonID);
			}

			public static void ButtonUp(int instanceID, int buttonID)
			{
				Events.OnControllerButtonUp?.Invoke(instanceID, (Input.ControllerButton)buttonID);
			}

			public static void AxisMotion(int instanceID, int axisID, short value)
			{
				if (axisID > -1 && axisID < 6)
				{
					Input.ControllerAxis axis;
					switch (axisID)
					{
						case 0:
							axis = Input.ControllerAxis.LeftX;
							break;
						case 1:
							axis = Input.ControllerAxis.LeftY;
							break;
						case 2:
							axis = Input.ControllerAxis.RightX;
							break;
						case 3:
							axis = Input.ControllerAxis.RightY;
							break;
						case 4:
							axis = Input.ControllerAxis.LeftTrigger;
							break;
						case 5:
							axis = Input.ControllerAxis.RightTrigger;
							break;
						default:
							throw new Exception("Invalid Axis");
					}
					// 32766f instead of 32767f or 32768f as it seems the SDL Y axes are skewed by +1
					Events.OnControllerAxisMove?.Invoke(instanceID, axis, Math.Clamp(value / 32766f, -1f, 1f));
				}
			}

			public static bool SetVibration(int instanceID, float leftMotor, float rightMotor)
			{
				IntPtr device = SDL_GameControllerFromInstanceID(instanceID);
				if (device == IntPtr.Zero)
				{
					return false;
				}

				return SDL_GameControllerRumble(
					device,
					(ushort)(Math.Clamp(leftMotor, 0.0f, 1.0f) * 0xFFFF),
					(ushort)(Math.Clamp(rightMotor, 0.0f, 1.0f) * 0xFFFF),
					SDL.SDL_HAPTIC_INFINITY // Oh dear...
				) == 0;
			}
		}
		#endregion

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
		static extern bool SetDefaultDllDirectories(int directoryFlags);
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		static extern void AddDllDirectory(string lpPathName);
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool SetDllDirectory(string lpPathName);
		const int LOAD_LIBRARY_SEARCH_DEFAULT_DIRS = 0x00001000;
		private static void SetDllDirectory()
		{
			if (Environment.OSVersion.Platform == PlatformID.Win32NT)
			{
				try
				{
					SetDefaultDllDirectories(LOAD_LIBRARY_SEARCH_DEFAULT_DIRS);
					AddDllDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Environment.Is64BitProcess ? "x64" : "x86"));
					Output.WriteLine($"{(Environment.Is64BitProcess ? "x64" : "x86")} executable detected, selecting proper DLLs.");
				}
				catch
				{
					// Pre-Windows 7, KB2533623 
					SetDllDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Environment.Is64BitProcess ? "x64" : "x86"));
					Output.WriteLine($"{(Environment.Is64BitProcess ? "x64" : "x86")} executable detected, selecting proper DLLs. (Pre-Windows 7, KB2533623)");
				}
			}
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
				Output.WriteLine("SDL2 libs were not found.");
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
					SDL_SetHint(SDL_HINT_WINDOWS_DISABLE_THREAD_NAMING, "1");
			}
			#endregion

			// If available, load the SDL_GameControllerDB/gamecontrollerdb.txt
			string gamecontrollerdb = "sdl/gamecontrollerdb.txt";
			if (File.Exists(gamecontrollerdb))
			{
				if (SDL_GameControllerAddMappingsFromFile(gamecontrollerdb) > -1)
					Output.WriteLine($"SDL - {gamecontrollerdb} loaded successfully.");
				else
					Output.WriteLine($"SDL - {gamecontrollerdb} was unsuccessful.");
			}
			else
			{
				Output.WriteLine($"SDL - {gamecontrollerdb} not found.");
			}
			// todo - Monogame gamecontrollerdb implementation - may be cleaner
			//using (var stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("gamecontrollerdb.txt"))
			//{
			//	if (stream != null)
			//	{
			//		using (var reader = new BinaryReader(stream))
			//		{
			//			try
			//			{
			//				//var src = Sdl.RwFromMem(reader.ReadBytes((int)stream.Length), (int)stream.Length);
			//				//Sdl.GameController.AddMappingFromRw(src, 1);
			//
			//				var src = SDL.SDL_RWFromMem(reader.ReadBytes((int)stream.Length), (int)stream.Length);
			//				SDL.SDL_GameControllerAddMappingsFromFile
			//			}
			//			catch { }
			//		}
			//	}
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
#if DEBUG
			SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_CONTEXT_FLAGS, (int)SDL_GLcontext.SDL_GL_CONTEXT_DEBUG_FLAG);
#endif

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
						Events.OnMouseButtonDown?.Invoke((Input.MouseButton)Event.button.button);
						break;
					case SDL_EventType.SDL_MOUSEBUTTONUP:
						Events.OnMouseButtonUp?.Invoke((Input.MouseButton)Event.button.button);
						break;
					case SDL_EventType.SDL_MOUSEWHEEL:
						Events.OnMouseScroll?.Invoke(Event.wheel.x, Event.wheel.y);
						break;
					case SDL_EventType.SDL_FINGERDOWN:
						Events.OnTouchDown?.Invoke(Event.tfinger.touchId, Event.tfinger.fingerId, Event.tfinger.x, Event.tfinger.y, Event.tfinger.dx, Event.tfinger.dy, Event.tfinger.pressure);
						Output.WriteLine($"Touch Down - TouchID: {Event.tfinger.touchId} - FingerID: {Event.tfinger.fingerId} - Position: ({Event.tfinger.x},{Event.tfinger.y})"); // Debug
						break;
					case SDL_EventType.SDL_FINGERUP:
						Events.OnTouchUp?.Invoke(Event.tfinger.touchId, Event.tfinger.fingerId, Event.tfinger.x, Event.tfinger.y, Event.tfinger.dx, Event.tfinger.dy, Event.tfinger.pressure);
						Output.WriteLine($"Touch Up - TouchID: {Event.tfinger.touchId} - FingerID: {Event.tfinger.fingerId} - Position: ({Event.tfinger.x},{Event.tfinger.y})"); // Debug
						break;
					case SDL_EventType.SDL_FINGERMOTION:
						Events.OnTouchMove?.Invoke(Event.tfinger.touchId, Event.tfinger.fingerId, Event.tfinger.x, Event.tfinger.y, Event.tfinger.dx, Event.tfinger.dy, Event.tfinger.pressure);
						Output.WriteLine($"Touch Move - TouchID: {Event.tfinger.touchId} - FingerID: {Event.tfinger.fingerId} - Position: ({Event.tfinger.x},{Event.tfinger.y}) - Relative: ({Event.tfinger.dx},{Event.tfinger.dy})"); // Debug
						break;
					case SDL_EventType.SDL_CONTROLLERDEVICEADDED:
						Controller.Added(Event.cdevice.which);
						break;
					case SDL_EventType.SDL_CONTROLLERDEVICEREMOVED:
						Controller.Removed(Event.cdevice.which);
						break;
					case SDL_EventType.SDL_CONTROLLERDEVICEREMAPPED:
						Controller.Remapped(Event.cdevice.which);
						break;
					case SDL_EventType.SDL_CONTROLLERBUTTONDOWN:
						Controller.ButtonDown(Event.cbutton.which, Event.cbutton.button);
						break;
					case SDL_EventType.SDL_CONTROLLERBUTTONUP:
						Controller.ButtonUp(Event.cbutton.which, Event.cbutton.button);
						break;
					case SDL_EventType.SDL_CONTROLLERAXISMOTION:
						Controller.AxisMotion(Event.caxis.which, Event.caxis.axis, Event.caxis.axisValue);
						break;
					case SDL_EventType.SDL_DISPLAYEVENT:
						if (Event.display.displayEvent == SDL_DisplayEventID.SDL_DISPLAYEVENT_ORIENTATION)
							Events.OnDisplayOrientationChanged?.Invoke(Event.display.display, Event.display.data1);
						break;
					case SDL_EventType.SDL_WINDOWEVENT:
						if (Event.window.windowID == Window.ID)
						{
							//Output.WriteLine(Event.window.windowEvent.ToString()); // Debug
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
								case SDL_WindowEventID.SDL_WINDOWEVENT_SIZE_CHANGED:
									Events.OnWinSizeChanged?.Invoke();
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
									ScreensaverAllowed = false;
									Events.OnWinEnter?.Invoke();
									break;
								case SDL_WindowEventID.SDL_WINDOWEVENT_LEAVE:
									ScreensaverAllowed = true;
									Events.OnWinLeave?.Invoke();
									break;
								case SDL_WindowEventID.SDL_WINDOWEVENT_FOCUS_GAINED:
									ScreensaverAllowed = false;
									Events.OnWinFocusGained?.Invoke();
									break;
								case SDL_WindowEventID.SDL_WINDOWEVENT_FOCUS_LOST:
									ScreensaverAllowed = true;
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
	}
}