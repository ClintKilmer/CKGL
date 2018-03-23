using System.Collections.Generic;

namespace CKGL
{
	public class Input
	{
		internal static void Clear()
		{
			Keyboard.Clear();
		}

		public static class Keyboard
		{
			public const int ScanCodeMask = SDL2.SDL.SDLK_SCANCODE_MASK;

			private static HashSet<KeyCode> downKeyCode = new HashSet<KeyCode>();
			private static HashSet<KeyCode> pressedKeyCode = new HashSet<KeyCode>();
			private static HashSet<KeyCode> releasedKeyCode = new HashSet<KeyCode>();
			private static HashSet<KeyCode> repeatedKeyCode = new HashSet<KeyCode>();

			private static HashSet<ScanCode> downScanCode = new HashSet<ScanCode>();
			private static HashSet<ScanCode> pressedScanCode = new HashSet<ScanCode>();
			private static HashSet<ScanCode> releasedScanCode = new HashSet<ScanCode>();
			private static HashSet<ScanCode> repeatedScanCode = new HashSet<ScanCode>();

			internal static void Clear()
			{
				System.Console.WriteLine("down:     " + string.Join(", ", downKeyCode));

				pressedKeyCode.Clear();
				releasedKeyCode.Clear();
				repeatedKeyCode.Clear();

				pressedScanCode.Clear();
				releasedScanCode.Clear();
				repeatedScanCode.Clear();
			}

			internal static void OnKeyDown(KeyCode keycode, ScanCode scancode, byte repeated)
			{
				downKeyCode.Add(keycode);
				downScanCode.Add(scancode);

				if(repeated == 0)
				{
					pressedKeyCode.Add(keycode);
					pressedScanCode.Add(scancode);

					//System.Console.WriteLine("pressed:  " + keycode);
				}
				else
				{
					repeatedKeyCode.Add(keycode);
					repeatedScanCode.Add(scancode);

					//System.Console.WriteLine("repeated: " + keycode);
				}
			}

			internal static void OnKeyUp(KeyCode keycode, ScanCode scancode)
			{
				downKeyCode.Remove(keycode);
				releasedKeyCode.Add(keycode);
				repeatedKeyCode.Add(keycode);

				downScanCode.Remove(scancode);
				releasedScanCode.Add(scancode);
				repeatedScanCode.Add(scancode);

				//System.Console.WriteLine("released: " + keycode);
			}

			public static bool Down(KeyCode key)
			{
				return downKeyCode.Contains(key);
			}
			public static bool Down(ScanCode key)
			{
				return downScanCode.Contains(key);
			}

			public static bool Pressed(KeyCode key)
			{
				return pressedKeyCode.Contains(key);
			}
			public static bool Pressed(ScanCode key)
			{
				return pressedScanCode.Contains(key);
			}

			public static bool Released(KeyCode key)
			{
				return releasedKeyCode.Contains(key);
			}
			public static bool Released(ScanCode key)
			{
				return releasedScanCode.Contains(key);
			}

			public static bool Repeated(KeyCode key)
			{
				return repeatedKeyCode.Contains(key);
			}
			public static bool Repeated(ScanCode key)
			{
				return repeatedScanCode.Contains(key);
			}

			public static bool PressedOrRepeated(KeyCode key)
			{
				return Pressed(key) || Repeated(key);
			}
			public static bool PressedOrRepeated(ScanCode key)
			{
				return Pressed(key) || Repeated(key);
			}
		}
	}
	
	public enum MouseButton
	{
		Left = 1,
		Middle = 2,
		Right = 3,
		X1 = 4,
		X2 = 5
	}
}