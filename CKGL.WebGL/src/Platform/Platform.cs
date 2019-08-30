using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Bridge.Html5; // HTML5 DOM Manipulation

namespace CKGL
{
	public static class Platform
	{
#if false // Temporary
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
#endif // Temporary

		public static GraphicsBackend GraphicsBackend { get; } = GraphicsBackend.WebGL;

		public static bool Running { get; private set; } = false;

#if false // Temporary
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
#endif // Temporary

		public static OS OS => OS.HTML5;

		public static GraphicsBackend GetPlatformDefaultGraphicsBackend()
		{
			return GraphicsBackend.WebGL;
		}

		////public static uint TotalMilliseconds { get { return SDL_GetTicks(); } }
		//public static ulong PerformanceCounter { get { return SDL_GetPerformanceCounter(); } }
		//public static ulong PerformanceFrequency { get { return SDL_GetPerformanceFrequency(); } }

		public static bool ShowCursor // Default true
		{
			get { return true; }
			set { }
		}

		public static string Clipboard
		{
			get { return ""; }
			set { }
		}

		// Handled by platform events
		private static bool ScreensaverAllowed // Default false
		{
			get { return true; }
			set { }
		}

		#region Init/Exit Methods
		internal static HTMLCanvasElement Canvas;
		public static void Init(string windowTitle, int windowWidth, int windowHeight, bool windowVSync, bool windowFullscreen, bool windowResizable, bool windowBorderless, int msaa)
		{
			//Input.Init();

			Running = true;

			Document.Title = windowTitle;

			Document.AddEventListener(EventType.MouseMove, handleMouseMove);

			Document.DocumentElement.Style.Overflow = Overflow.Hidden;
			Document.DocumentElement.Style.SetProperty("margin", "0", "important");
			Document.DocumentElement.Style.SetProperty("padding", "0", "important");
			Document.Body.Style.Overflow = Overflow.Hidden;
			Document.Body.Style.SetProperty("margin", "0", "important");
			Document.Body.Style.SetProperty("padding", "0", "important");

			Canvas = new HTMLCanvasElement
			{
				Width = windowFullscreen ? Window.InnerWidth : windowWidth,
				Height = windowFullscreen ? Window.InnerHeight : windowHeight
			};
			Document.Body.AppendChild(Canvas);

			// Disable selection
			Canvas.Style.SetProperty("user-select", "none");
			Canvas.Style.SetProperty("-webkit-user-select", "none");
			Canvas.Style.SetProperty("-moz-user-select", "none");
			Canvas.Style.SetProperty("-ms-user-select", "none");

			// Window
			//Window.Init(windowTitle, windowWidth, windowHeight, windowVSync, windowFullscreen, windowResizable, windowBorderless);
		}

		public static void Destroy()
		{
			//Window.Destroy();

			Document.Body.RemoveChild(Canvas);
		}
		#endregion

		internal static void Update()
		{
			//Input.Clear();

			//PollEvents();

			//Input.Update();
		}

#if false // Temporary
		#region PollEvents
		private static void PollEvents()
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
									if (OS == OS.Windows || OS == OS.WinRT)
										Window.FullscreenReset();
									Events.OnWinFocusGained?.Invoke();
									break;
								case SDL_WindowEventID.SDL_WINDOWEVENT_FOCUS_LOST:
									ScreensaverAllowed = true;
									if (OS == OS.Windows || OS == OS.WinRT)
										Window.FullscreenReset();
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
		#endregion
#endif // Temporary

		public static void Quit()
		{
			Running = false;
		}

		public static bool RelativeMouseMode // Default true
		{
			get { return false; } // todo
			set { if (value) Canvas.SetCapture(); else Document.ReleaseCapture(); }
		}

		private static int mouseX = 0;
		private static int mouseY = 0;
		private static int mouseScreenX = 0;
		private static int mouseScreenY = 0;
		private static int mouseRelativeX = 0;
		private static int mouseRelativeY = 0;
		private static void handleMouseMove(Event e)
		{
			//Output.WriteLine($"MouseMove: Type: {e.As<MouseEvent>().Type}, RelX: {e.As<MouseEvent>().MovementX}, RelY: {e.As<MouseEvent>().MovementY}");
			Output.WriteLine($"Event: MouseMove");
			mouseX = e.As<MouseEvent>().LayerX;
			mouseY = e.As<MouseEvent>().LayerY;
			mouseScreenX = e.As<MouseEvent>().ScreenX;
			mouseScreenY = e.As<MouseEvent>().ScreenY;
			mouseRelativeX = e.As<MouseEvent>().MovementX;
			mouseRelativeY = e.As<MouseEvent>().MovementY;
		}

		public static void GetGlobalMousePosition(out int x, out int y)
		{
			x = mouseScreenX;
			y = mouseScreenY;
		}

		public static void GetMousePosition(out int x, out int y)
		{
			x = mouseX;
			y = mouseY;
		}

		public static void GetRelativeMousePosition(out int x, out int y)
		{
			x = mouseRelativeX;
			y = mouseRelativeY;
		}

		public static void Delay(uint ms)
		{
		}

		#region Texture<->Image Load/Save Methods - Stub
		public static void LoadImage(string file, out int width, out int height, out byte[] data)
		{
			width = 0;
			height = 0;
			data = new byte[] { };
		}

		public static void SavePNG(string file, int destinationWidth, int destinationHeight, int sourceWidth, int sourceHeight, byte[] data)
		{
		}

		public static void SaveJPG(string file, int destinationWidth, int destinationHeight, int sourceWidth, int sourceHeight, byte[] data)
		{
		}
		#endregion
	}
}