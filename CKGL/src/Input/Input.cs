using System;
using System.Collections.Generic;
using System.Linq;

namespace CKGL
{
	public static class Input
	{
		public static void Init()
		{
			Keyboard.Init();
			Mouse.Init();
			Controllers.Init();
		}

		public static void Clear()
		{
			Keyboard.Clear();
			Mouse.Clear();
			Controllers.Clear();
		}

		public static void Update()
		{
			//Keyboard.Update();
			Mouse.Update();
			//Controllers.Update();
		}

		#region Keyboard
		public static class Keyboard
		{
			public const int ScanCodeMask = Platform.ScanCodeMask;

			private static HashSet<KeyCode> downKeyCode = new HashSet<KeyCode>();
			private static HashSet<KeyCode> pressedKeyCode = new HashSet<KeyCode>();
			private static HashSet<KeyCode> releasedKeyCode = new HashSet<KeyCode>();
			private static HashSet<KeyCode> repeatedKeyCode = new HashSet<KeyCode>();

			private static HashSet<ScanCode> downScanCode = new HashSet<ScanCode>();
			private static HashSet<ScanCode> pressedScanCode = new HashSet<ScanCode>();
			private static HashSet<ScanCode> releasedScanCode = new HashSet<ScanCode>();
			private static HashSet<ScanCode> repeatedScanCode = new HashSet<ScanCode>();

			public static void Init()
			{
				Platform.Events.OnKeyDown += (keycode, scancode, repeated) =>
				{
					downKeyCode.Add(keycode);
					downScanCode.Add(scancode);

					if (repeated)
					{
						repeatedKeyCode.Add(keycode);
						repeatedScanCode.Add(scancode);

						//Output.WriteLine("repeated: " + keycode);
					}
					else
					{
						pressedKeyCode.Add(keycode);
						pressedScanCode.Add(scancode);

						//Output.WriteLine("pressed:  " + keycode);
					}
				};

				Platform.Events.OnKeyUp += (keycode, scancode, repeated) =>
				{
					downKeyCode.Remove(keycode);
					releasedKeyCode.Add(keycode);
					repeatedKeyCode.Add(keycode);

					downScanCode.Remove(scancode);
					releasedScanCode.Add(scancode);
					repeatedScanCode.Add(scancode);

					//Output.WriteLine("released: " + keycode);
				};
			}

			public static void Clear()
			{
				//Output.WriteLine("down:     " + string.Join(", ", downKeyCode));

				pressedKeyCode.Clear();
				releasedKeyCode.Clear();
				repeatedKeyCode.Clear();

				pressedScanCode.Clear();
				releasedScanCode.Clear();
				repeatedScanCode.Clear();
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
		#endregion

		#region Mouse
		public enum MouseButton
		{
			Left = 1,
			Middle = 2,
			Right = 3,
			X1 = 4,
			X2 = 5
		}

		public static class Mouse
		{
			public static Point2 LastPosition { get; set; }
			public static Point2 Position { get; private set; }
			public static Point2 LastPositionDisplay { get; private set; }
			public static Point2 PositionDisplay { get; private set; }
			public static Point2 PositionRelative { get; private set; }
			public static Point2 Scroll { get; private set; }

			private static bool[] down = new bool[Enum.GetNames(typeof(MouseButton)).Length];
			private static bool[] pressed = new bool[Enum.GetNames(typeof(MouseButton)).Length];
			private static bool[] released = new bool[Enum.GetNames(typeof(MouseButton)).Length];

			public static void Init()
			{
				Platform.Events.OnMouseButtonDown += (button) =>
				{
					down[(int)button] = true;
					pressed[(int)button] = true;
				};

				Platform.Events.OnMouseButtonUp += (button) =>
				{
					down[(int)button] = false;
					released[(int)button] = true;
				};

				Platform.Events.OnMouseScroll += (x, y) =>
				{
					Scroll = new Point2(x, y);
				};
			}

			public static void Clear()
			{
				Scroll = Point2.Zero;

				for (int i = 0; i < Enum.GetNames(typeof(MouseButton)).Length; i++)
				{
					pressed[i] = false;
					released[i] = false;
				}
			}

			public static void Update()
			{
				LastPositionDisplay = PositionDisplay;
				Platform.GetGlobalMousePosition(out int mouseDisplayX, out int mouseDisplayY);
				PositionDisplay = new Point2(mouseDisplayX, mouseDisplayY);

				LastPosition = Position;
				Platform.GetMousePosition(out int mouseX, out int mouseY);
				Position = new Point2(mouseX, mouseY);

				Platform.GetRelativeMousePosition(out int mouseRelativeX, out int mmouseDisplayY);
				PositionRelative = new Point2(mouseRelativeX, mmouseDisplayY);
			}

			public static bool Down(MouseButton button)
			{
				return down[(int)button];
			}

			public static bool Pressed(MouseButton button)
			{
				return pressed[(int)button];
			}

			public static bool Released(MouseButton button)
			{
				return released[(int)button];
			}

			public static bool LeftDown
			{
				get { return Down(MouseButton.Left); }
			}

			public static bool LeftPressed
			{
				get { return Pressed(MouseButton.Left); }
			}

			public static bool LeftReleased
			{
				get { return Released(MouseButton.Left); }
			}

			public static bool RightDown
			{
				get { return Down(MouseButton.Right); }
			}

			public static bool RightPressed
			{
				get { return Pressed(MouseButton.Right); }
			}

			public static bool RightReleased
			{
				get { return Released(MouseButton.Right); }
			}

			public static bool MiddleDown
			{
				get { return Down(MouseButton.Middle); }
			}

			public static bool MiddlePressed
			{
				get { return Pressed(MouseButton.Middle); }
			}

			public static bool MiddleReleased
			{
				get { return Released(MouseButton.Middle); }
			}

			public static int X
			{
				get { return Position.X; }
			}

			public static int Y
			{
				get { return Position.Y; }
			}

			public static int ScrollX
			{
				get { return Scroll.X; }
			}

			public static int ScrollY
			{
				get { return Scroll.Y; }
			}
		}
		#endregion

		#region Controller
		public enum ControllerButton
		{
			A = 0,
			B = 1,
			X = 2,
			Y = 3,
			Select = 4,
			Home = 5,
			Start = 6,
			L3 = 7,
			R3 = 8,
			L1 = 9,
			R1 = 10,
			Up = 11,
			Down = 12,
			Left = 13,
			Right = 14,
			L2 = 15, // If LeftTrigger is pressed passed the XInput threshold
			R2 = 16 // If RightTrigger is pressed passed the XInput threshold
		}

		public enum ControllerAxis
		{
			LeftX = 0,
			LeftY = 1,
			RightX = 2,
			RightY = 3,
			LeftTrigger = 4,
			RightTrigger = 5
		}

		public class Controller
		{
			public int ID;

			public float LeftTrigger { get; private set; }
			public float RightTrigger { get; private set; }
			public Vector2 LeftStick { get; private set; }
			public Vector2 RightStick { get; private set; }

			// TODO - ApplyRadialDeadZone - use this?
			public float AnalogStickDeadZoneLow { get; set; } = 0.1f;
			public float AnalogStickDeadZoneHigh { get; set; } = 0.1f;

			private const float LeftDeadZone = 7849f / 32768f; // XInput Constant
			private const float RightDeadZone = 8689f / 32768f; // XInput Constant
			private const float TriggerThreshold = 30f / 255f; // XInput Constant

			private bool[] down = new bool[Enum.GetNames(typeof(ControllerButton)).Length];
			private bool[] pressed = new bool[Enum.GetNames(typeof(ControllerButton)).Length];
			private bool[] released = new bool[Enum.GetNames(typeof(ControllerButton)).Length];

			public Controller(int id)
			{
				ID = id;
			}

			public void OnControllerButtonDown(ControllerButton button)
			{
				down[(int)button] = true;
				pressed[(int)button] = true;

				// debug
				Output.WriteLine($"Controller {ID} - Button {button} - Down");
			}

			public void OnControllerButtonUp(ControllerButton button)
			{
				down[(int)button] = false;
				released[(int)button] = true;

				// debug
				Output.WriteLine($"Controller {ID} - Button {button} - Up");
			}

			public void OnControllerAxisMove(ControllerAxis axis, float value)
			{
				// Triggers
				if (axis == ControllerAxis.LeftTrigger || axis == ControllerAxis.RightTrigger)
				{
					if (axis == ControllerAxis.LeftTrigger)
					{
						LeftTrigger = DeadZoneMapper(value, TriggerThreshold);

						if (LeftTrigger > TriggerThreshold)
						{
							if (!Down(ControllerButton.L2))
								OnControllerButtonDown(ControllerButton.L2);
						}
						else
						{
							if (Down(ControllerButton.L2))
								OnControllerButtonUp(ControllerButton.L2);
						}
					}
					else if (axis == ControllerAxis.RightTrigger)
					{
						RightTrigger = DeadZoneMapper(value, TriggerThreshold);

						if (RightTrigger > TriggerThreshold)
						{
							if (!Down(ControllerButton.R2))
								OnControllerButtonDown(ControllerButton.R2);
						}
						else
						{
							if (Down(ControllerButton.R2))
								OnControllerButtonUp(ControllerButton.R2);
						}
					}

					// debug
					//Output.WriteLine($"Controller: {ID} - Axis: {axis} - Value: {value}");
				}
				else
				{
					if (axis == ControllerAxis.LeftX)
					{
						LeftStick = new Vector2(DeadZoneMapper(value, LeftDeadZone), LeftStick.Y);

						// debug
						//Output.WriteLine($"Controller: {ID} - Axis: {axis} - Value: {LeftStick.X}");
					}
					else if (axis == ControllerAxis.LeftY)
					{
						// Flip Y axis so forward is positive
						LeftStick = new Vector2(LeftStick.X, DeadZoneMapper(-value, LeftDeadZone));

						// debug
						Output.WriteLine($"Controller: {ID} - Axis: {axis} - Value: {LeftStick.Y}");
					}
					else if (axis == ControllerAxis.RightX)
					{
						RightStick = new Vector2(DeadZoneMapper(value, RightDeadZone), RightStick.Y);

						// debug
						//Output.WriteLine($"Controller: {ID} - Axis: {axis} - Value: {RightStick.X}");
					}
					else if (axis == ControllerAxis.RightY)
					{
						// Flip Y axis so forward is positive
						RightStick = new Vector2(RightStick.X, DeadZoneMapper(-value, RightDeadZone));

						// debug
						Output.WriteLine($"Controller: {ID} - Axis: {axis} - Value: {RightStick.Y}");
					}
				}

				// debug
				//Output.WriteLine($"Controller: {ID} - Axis: {axis} - Value: {value}");
			}

			public void Clear()
			{
				for (int i = 0; i < Enum.GetNames(typeof(ControllerButton)).Length; i++)
				{
					pressed[i] = false;
					released[i] = false;
				}
			}

			public bool Down(ControllerButton button)
			{
				return down[(int)button];
			}

			public bool Pressed(ControllerButton button)
			{
				return pressed[(int)button];
			}

			public bool Released(ControllerButton button)
			{
				return released[(int)button];
			}

			public override string ToString()
			{
				return $"ID: {ID}";
			}

			private float DeadZoneMapper(float value, float deadZone)
			{
				if (value < -deadZone)
				{
					value += deadZone;
				}
				else if (value > deadZone)
				{
					value -= deadZone;
				}
				else
				{
					return 0.0f;
				}
				return Math.Clamp(value / (1.0f - deadZone), -1f, 1f);
			}

			// TODO - ApplyRadialDeadZone - use this?
			private Vector2 ApplyRadialDeadZone(Vector2 position)
			{
				float mag = position.Magnitude();

				if (mag > AnalogStickDeadZoneLow)
				{
					// scale such that output magnitude is in the range [0.0f, 1.0f]
					float legalRange = 1.0f - AnalogStickDeadZoneHigh - AnalogStickDeadZoneLow;
					float normalizedMag = Math.Min(1.0f, (mag - AnalogStickDeadZoneLow) / legalRange);
					float scale = normalizedMag / mag;
					return position * scale;
				}
				else
				{
					// stick is in the inner dead zone
					return Vector2.Zero;
				}
			}
		}

		public static class Controllers
		{
			private static Dictionary<int, Controller> controllers = new Dictionary<int, Controller>();

			public static void Init()
			{
				Platform.Events.OnControllerDeviceAdded += (id) =>
				{
					if (!controllers.ContainsKey(id))
					{
						Controller controller = new Controller(id);
						controllers.Add(id, controller);
					}
				};

				Platform.Events.OnControllerDeviceRemoved += (id) =>
				{
					if (controllers.ContainsKey(id))
						controllers.Remove(id);
				};

				Platform.Events.OnControllerDeviceRemapped += (id) =>
				{
					Output.WriteLine($"OnControllerDeviceRemapped not implemented. (Controller ID {id})");
				};

				Platform.Events.OnControllerButtonDown += (id, button) =>
				{
					if (controllers.ContainsKey(id))
					{
						controllers[id].OnControllerButtonDown(button);
					}
				};

				Platform.Events.OnControllerButtonUp += (id, button) =>
				{
					if (controllers.ContainsKey(id))
					{
						controllers[id].OnControllerButtonUp(button);
					}
				};

				Platform.Events.OnControllerAxisMove += (id, axis, value) =>
				{
					if (controllers.ContainsKey(id))
					{
						controllers[id].OnControllerAxisMove(axis, value);
					}
				};
			}

			public static void Clear()
			{
				foreach (Controller controller in controllers.Values)
				{
					controller.Clear();
				}
			}

			public static Controller First()
			{
				return controllers.First().Value;
			}
		}
		#endregion
	}
}