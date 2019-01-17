using System;
using System.Collections.Generic;

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
			public static Point2 LastPosition { get; private set; }
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

			#region Shortcuts
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
			#endregion
		}
		#endregion

		#region Controller
		public enum ControllerButton
		{
			// Digital Buttons
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
			// Virtual Digital Buttons (Analog to Digital shortcuts)
			L2 = 15, // If LeftTrigger is pressed passed the XInput threshold
			R2 = 16, // If RightTrigger is pressed passed the XInput threshold
			LeftStickDigitalUp = 17, // If LeftStick.Up is pressed passed the XInput threshold
			LeftStickDigitalDown = 18, // If LeftStick.Down is pressed passed the XInput threshold
			LeftStickDigitalLeft = 19, // If LeftStick.Left is pressed passed the XInput threshold
			LeftStickDigitalRight = 20, // If LeftStick.Right is pressed passed the XInput threshold
			RightStickDigitalUp = 21, // If RightStick.Up is pressed passed the XInput threshold
			RightStickDigitalDown = 22, // If RightStick.Down is pressed passed the XInput threshold
			RightStickDigitalLeft = 23, // If RightStick.Left is pressed passed the XInput threshold
			RightStickDigitalRight = 24 // If RightStick.Right is pressed passed the XInput threshold
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
			public int Slot { get; private set; } = -1;
			public int ID { get; private set; } = -1;
			public bool Connected { get; private set; } = false;

			//public int DeviceIndex => Platform.GetController(ID).DeviceIndex;
			public string Name => Platform.GetController(ID).Name;
			public string GUID => Platform.GetController(ID).GUID;
			public ushort Vendor => Platform.GetController(ID).Vendor;
			public ushort Product => Platform.GetController(ID).Product;
			public ushort ProductVersion => Platform.GetController(ID).ProductVersion;
			public bool Rumble => Platform.GetController(ID).Rumble;

			public float LeftTrigger { get; private set; }
			public float RightTrigger { get; private set; }
			public Vector2 LeftStick { get; private set; }
			public Vector2 RightStick { get; private set; }

			private const float LeftDeadZone = 7849f / 32768f; // XInput Constant
			private const float RightDeadZone = 8689f / 32768f; // XInput Constant
			private const float TriggerThreshold = 30f / 255f; // XInput Constant
			private const float LeftDeadZoneHigh = LeftDeadZone * 0.25f; // CKGL
			private const float RightDeadZoneHigh = RightDeadZone * 0.25f; // CKGL
			private const float TriggerThresholdHigh = TriggerThreshold * 0.25f; // CKGL

			private bool[] down = new bool[Enum.GetNames(typeof(ControllerButton)).Length];
			private bool[] pressed = new bool[Enum.GetNames(typeof(ControllerButton)).Length];
			private bool[] released = new bool[Enum.GetNames(typeof(ControllerButton)).Length];

			internal Controller(int slot, int id)
			{
				Slot = slot;
				ID = id;
				Connected = true;
			}

			// Dummy Controller
			private Controller() { }
			internal static Controller Dummy { get; } = new Controller();

			internal void OnControllerButtonDown(ControllerButton button)
			{
				down[(int)button] = true;
				pressed[(int)button] = true;

				// debug
				Output.WriteLine($"Controller {this} - Button {button} - Down");
			}

			internal void OnControllerButtonUp(ControllerButton button)
			{
				down[(int)button] = false;
				released[(int)button] = true;

				// debug
				Output.WriteLine($"Controller {this} - Button {button} - Up");
			}

			internal void OnControllerAxisMove(ControllerAxis axis, float value)
			{
				// Triggers
				if (axis == ControllerAxis.LeftTrigger || axis == ControllerAxis.RightTrigger)
				{
					if (axis == ControllerAxis.LeftTrigger)
					{
						LeftTrigger = DeadZoneMapper(value, TriggerThreshold, TriggerThresholdHigh);

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
						RightTrigger = DeadZoneMapper(value, TriggerThreshold, TriggerThresholdHigh);

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
					//Output.WriteLine($"Controller: {this} - Axis: {axis} - Value: {LeftTrigger}");
					//Output.WriteLine($"Controller: {this} - Axis: {axis} - Value: {RightTrigger}");
				}
				else
				{
					if (axis == ControllerAxis.LeftX)
					{
						LeftStick = new Vector2(DeadZoneMapper(value, LeftDeadZone, LeftDeadZoneHigh), LeftStick.Y);

						if (LeftStick.X < -LeftDeadZone)
						{
							if (!Down(ControllerButton.LeftStickDigitalLeft))
								OnControllerButtonDown(ControllerButton.LeftStickDigitalLeft);
						}
						else if (LeftStick.X > LeftDeadZone)
						{
							if (!Down(ControllerButton.LeftStickDigitalRight))
								OnControllerButtonDown(ControllerButton.LeftStickDigitalRight);
						}
						else
						{
							if (Down(ControllerButton.LeftStickDigitalLeft))
								OnControllerButtonUp(ControllerButton.LeftStickDigitalLeft);
							if (Down(ControllerButton.LeftStickDigitalRight))
								OnControllerButtonUp(ControllerButton.LeftStickDigitalRight);
						}

						// debug
						//Output.WriteLine($"Controller: {this} - Axis: {axis} - Value: {LeftStick.X}");
					}
					else if (axis == ControllerAxis.LeftY)
					{
						// Flip Y axis so forward is positive
						LeftStick = new Vector2(LeftStick.X, DeadZoneMapper(-value, LeftDeadZone, LeftDeadZoneHigh));

						if (LeftStick.Y < -LeftDeadZone)
						{
							if (!Down(ControllerButton.LeftStickDigitalDown))
								OnControllerButtonDown(ControllerButton.LeftStickDigitalDown);
						}
						else if (LeftStick.Y > LeftDeadZone)
						{
							if (!Down(ControllerButton.LeftStickDigitalUp))
								OnControllerButtonDown(ControllerButton.LeftStickDigitalUp);
						}
						else
						{
							if (Down(ControllerButton.LeftStickDigitalDown))
								OnControllerButtonUp(ControllerButton.LeftStickDigitalDown);
							if (Down(ControllerButton.LeftStickDigitalUp))
								OnControllerButtonUp(ControllerButton.LeftStickDigitalUp);
						}

						// debug
						//Output.WriteLine($"Controller: {this} - Axis: {axis} - Value: {LeftStick.Y}");
					}
					else if (axis == ControllerAxis.RightX)
					{
						RightStick = new Vector2(DeadZoneMapper(value, RightDeadZone, RightDeadZoneHigh), RightStick.Y);

						if (RightStick.X < -RightDeadZone)
						{
							if (!Down(ControllerButton.RightStickDigitalLeft))
								OnControllerButtonDown(ControllerButton.RightStickDigitalLeft);
						}
						else if (RightStick.X > RightDeadZone)
						{
							if (!Down(ControllerButton.RightStickDigitalRight))
								OnControllerButtonDown(ControllerButton.RightStickDigitalRight);
						}
						else
						{
							if (Down(ControllerButton.RightStickDigitalLeft))
								OnControllerButtonUp(ControllerButton.RightStickDigitalLeft);
							if (Down(ControllerButton.RightStickDigitalRight))
								OnControllerButtonUp(ControllerButton.RightStickDigitalRight);
						}

						// debug
						//Output.WriteLine($"Controller: {this} - Axis: {axis} - Value: {RightStick.X}");
					}
					else if (axis == ControllerAxis.RightY)
					{
						// Flip Y axis so forward is positive
						RightStick = new Vector2(RightStick.X, DeadZoneMapper(-value, RightDeadZone, RightDeadZoneHigh));

						if (RightStick.Y < -RightDeadZone)
						{
							if (!Down(ControllerButton.RightStickDigitalDown))
								OnControllerButtonDown(ControllerButton.RightStickDigitalDown);
						}
						else if (RightStick.Y > RightDeadZone)
						{
							if (!Down(ControllerButton.RightStickDigitalUp))
								OnControllerButtonDown(ControllerButton.RightStickDigitalUp);
						}
						else
						{
							if (Down(ControllerButton.RightStickDigitalDown))
								OnControllerButtonUp(ControllerButton.RightStickDigitalDown);
							if (Down(ControllerButton.RightStickDigitalUp))
								OnControllerButtonUp(ControllerButton.RightStickDigitalUp);
						}

						// debug
						//Output.WriteLine($"Controller: {this} - Axis: {axis} - Value: {RightStick.Y}");
					}
				}

				// debug
				//Output.WriteLine($"Controller: {this} - Axis: {axis} - Value: {value}");
			}

			private float DeadZoneMapper(float value, float deadZoneLow, float deadZoneHigh)
			{
				if (value < -deadZoneLow)
					value += deadZoneLow;
				else if (value > deadZoneLow)
					value -= deadZoneLow;
				else
					return 0.0f;

				return Math.Clamp(value / (1.0f - deadZoneLow - deadZoneHigh), -1f, 1f);
			}

			internal void Clear()
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

			#region Shortcuts
			public bool ADown
			{
				get { return Down(ControllerButton.A); }
			}

			public bool APressed
			{
				get { return Pressed(ControllerButton.A); }
			}

			public bool AReleased
			{
				get { return Released(ControllerButton.A); }
			}

			public bool BDown
			{
				get { return Down(ControllerButton.B); }
			}

			public bool BPressed
			{
				get { return Pressed(ControllerButton.B); }
			}

			public bool BReleased
			{
				get { return Released(ControllerButton.B); }
			}

			public bool XDown
			{
				get { return Down(ControllerButton.X); }
			}

			public bool XPressed
			{
				get { return Pressed(ControllerButton.X); }
			}

			public bool XReleased
			{
				get { return Released(ControllerButton.X); }
			}

			public bool YDown
			{
				get { return Down(ControllerButton.Y); }
			}

			public bool YPressed
			{
				get { return Pressed(ControllerButton.Y); }
			}

			public bool YReleased
			{
				get { return Released(ControllerButton.Y); }
			}

			public bool SelectDown
			{
				get { return Down(ControllerButton.Select); }
			}

			public bool SelectPressed
			{
				get { return Pressed(ControllerButton.Select); }
			}

			public bool SelectReleased
			{
				get { return Released(ControllerButton.Select); }
			}

			public bool HomeDown
			{
				get { return Down(ControllerButton.Home); }
			}

			public bool HomePressed
			{
				get { return Pressed(ControllerButton.Home); }
			}

			public bool HomeReleased
			{
				get { return Released(ControllerButton.Home); }
			}

			public bool StartDown
			{
				get { return Down(ControllerButton.Start); }
			}

			public bool StartPressed
			{
				get { return Pressed(ControllerButton.Start); }
			}

			public bool StartReleased
			{
				get { return Released(ControllerButton.Start); }
			}

			public bool L1Down
			{
				get { return Down(ControllerButton.L1); }
			}

			public bool L1Pressed
			{
				get { return Pressed(ControllerButton.L1); }
			}

			public bool L1Released
			{
				get { return Released(ControllerButton.L1); }
			}

			public bool R1Down
			{
				get { return Down(ControllerButton.R1); }
			}

			public bool R1Pressed
			{
				get { return Pressed(ControllerButton.R1); }
			}

			public bool R1Released
			{
				get { return Released(ControllerButton.R1); }
			}

			public bool L2Down
			{
				get { return Down(ControllerButton.L2); }
			}

			public bool L2Pressed
			{
				get { return Pressed(ControllerButton.L2); }
			}

			public bool L2Released
			{
				get { return Released(ControllerButton.L2); }
			}

			public bool R2Down
			{
				get { return Down(ControllerButton.R2); }
			}

			public bool R2Pressed
			{
				get { return Pressed(ControllerButton.R2); }
			}

			public bool R2Released
			{
				get { return Released(ControllerButton.R2); }
			}

			public bool L3Down
			{
				get { return Down(ControllerButton.L3); }
			}

			public bool L3Pressed
			{
				get { return Pressed(ControllerButton.L3); }
			}

			public bool L3Released
			{
				get { return Released(ControllerButton.L3); }
			}

			public bool R3Down
			{
				get { return Down(ControllerButton.R3); }
			}

			public bool R3Pressed
			{
				get { return Pressed(ControllerButton.R3); }
			}

			public bool R3Released
			{
				get { return Released(ControllerButton.R3); }
			}

			public bool DownDown
			{
				get { return Down(ControllerButton.Down); }
			}

			public bool DownPressed
			{
				get { return Pressed(ControllerButton.Down); }
			}

			public bool DownReleased
			{
				get { return Released(ControllerButton.Down); }
			}

			public bool UpDown
			{
				get { return Down(ControllerButton.Up); }
			}

			public bool UpPressed
			{
				get { return Pressed(ControllerButton.Up); }
			}

			public bool UpReleased
			{
				get { return Released(ControllerButton.Up); }
			}

			public bool LeftDown
			{
				get { return Down(ControllerButton.Left); }
			}

			public bool LeftPressed
			{
				get { return Pressed(ControllerButton.Left); }
			}

			public bool LeftReleased
			{
				get { return Released(ControllerButton.Left); }
			}

			public bool RightDown
			{
				get { return Down(ControllerButton.Right); }
			}

			public bool RightPressed
			{
				get { return Pressed(ControllerButton.Right); }
			}

			public bool RightReleased
			{
				get { return Released(ControllerButton.Right); }
			}

			public bool LeftStickDigitalDownDown
			{
				get { return Down(ControllerButton.LeftStickDigitalDown); }
			}

			public bool LeftStickDigitalDownPressed
			{
				get { return Pressed(ControllerButton.LeftStickDigitalDown); }
			}

			public bool LeftStickDigitalDownReleased
			{
				get { return Released(ControllerButton.LeftStickDigitalDown); }
			}

			public bool LeftStickDigitalUpDown
			{
				get { return Down(ControllerButton.LeftStickDigitalUp); }
			}

			public bool LeftStickDigitalUpPressed
			{
				get { return Pressed(ControllerButton.LeftStickDigitalUp); }
			}

			public bool LeftStickDigitalUpReleased
			{
				get { return Released(ControllerButton.LeftStickDigitalUp); }
			}

			public bool LeftStickDigitalLeftDown
			{
				get { return Down(ControllerButton.LeftStickDigitalLeft); }
			}

			public bool LeftStickDigitalLeftPressed
			{
				get { return Pressed(ControllerButton.LeftStickDigitalLeft); }
			}

			public bool LeftStickDigitalLeftReleased
			{
				get { return Released(ControllerButton.LeftStickDigitalLeft); }
			}

			public bool LeftStickDigitalRightDown
			{
				get { return Down(ControllerButton.LeftStickDigitalRight); }
			}

			public bool LeftStickDigitalRightPressed
			{
				get { return Pressed(ControllerButton.LeftStickDigitalRight); }
			}

			public bool LeftStickDigitalRightReleased
			{
				get { return Released(ControllerButton.LeftStickDigitalRight); }
			}

			public float LeftStickX
			{
				get { return LeftStick.X; }
			}

			public float LeftStickY
			{
				get { return LeftStick.Y; }
			}

			public float RightStickX
			{
				get { return RightStick.X; }
			}

			public float RightStickY
			{
				get { return RightStick.Y; }
			}
			#endregion

			public override string ToString()
			{
				return $"Slot: {Slot}, ID: {ID}";
			}
		}

		public static class Controllers
		{
			private static Dictionary<int, Controller> controllers = new Dictionary<int, Controller>();
			private static Dictionary<int, int> controllerIDs = new Dictionary<int, int>();

			internal static void Init()
			{
				Platform.Events.OnControllerDeviceAdded += (id) =>
				{
					if (!controllerIDs.ContainsKey(id))
					{
						int slot = 0;
						while (controllers.ContainsKey(slot))
							slot++;
						controllers.Add(slot, new Controller(slot, id));
						controllerIDs.Add(id, slot);

						// debug
						Output.WriteLine($"Controller Added: {controllers[slot]}");
						Output.WriteLine($"Total Controllers: {controllers.Count}");
					}
				};

				Platform.Events.OnControllerDeviceRemoved += (id) =>
				{
					if (controllerIDs.ContainsKey(id))
					{
						// debug
						Output.WriteLine($"Controller Removed: {controllers[controllerIDs[id]]}");

						controllers.Remove(controllerIDs[id]);
						controllerIDs.Remove(id);

						// debug
						Output.WriteLine($"Total Controllers: {controllers.Count}");
					}
				};

				Platform.Events.OnControllerDeviceRemapped += (id) =>
				{
					Output.WriteLine($"OnControllerDeviceRemapped not implemented. (Controller ID {id})");
				};

				Platform.Events.OnControllerButtonDown += (id, button) =>
				{
					if (controllerIDs.ContainsKey(id))
					{
						controllers[controllerIDs[id]].OnControllerButtonDown(button);
					}
				};

				Platform.Events.OnControllerButtonUp += (id, button) =>
				{
					if (controllerIDs.ContainsKey(id))
					{
						controllers[controllerIDs[id]].OnControllerButtonUp(button);
					}
				};

				Platform.Events.OnControllerAxisMove += (id, axis, value) =>
				{
					if (controllerIDs.ContainsKey(id))
					{
						controllers[controllerIDs[id]].OnControllerAxisMove(axis, value);
					}
				};
			}

			internal static void Clear()
			{
				foreach (Controller controller in controllers.Values)
				{
					controller.Clear();
				}
			}

			public static Controller First()
			{
				if (controllers.Count > 0)
				{
					int slot = 0;
					while (!controllers.ContainsKey(slot))
						slot++;

					return controllers[slot];
				}

				return Controller.Dummy;
			}

			public static Controller Slot(int slot)
			{
				if (controllers.ContainsKey(slot))
					return controllers[slot];

				return Controller.Dummy;
			}

			public static Controller Slot1()
			{
				return Slot(0);
			}

			public static Controller Slot2()
			{
				return Slot(1);
			}

			public static Controller Slot3()
			{
				return Slot(2);
			}

			public static Controller Slot4()
			{
				return Slot(3);
			}
		}
		#endregion
	}
}