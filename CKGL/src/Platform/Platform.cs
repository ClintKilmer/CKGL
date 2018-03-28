using System;

namespace CKGL
{
	public abstract class Platform
	{
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
	}
}