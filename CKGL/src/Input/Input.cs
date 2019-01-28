using System;
using System.Collections.Generic;

namespace CKGL
{
	#region KeyCodes
	public enum KeyCode
	{
		Unknown = 0,
		Return = '\r',
		Escape = 27,
		Backspace = '\b',
		Tab = '\t',
		Space = ' ',
		Exclaim = '!',
		QuoteDbl = '"',
		Hash = '#',
		Percent = '%',
		Dollar = '$',
		Ampersand = '&',
		Quote = '\'',
		LeftParen = '(',
		RightParen = ')',
		Asterisk = '*',
		Plus = '+',
		Comma = ',',
		Minus = '-',
		Period = '.',
		Slash = '/',
		Num0 = '0',
		Num1 = '1',
		Num2 = '2',
		Num3 = '3',
		Num4 = '4',
		Num5 = '5',
		Num6 = '6',
		Num7 = '7',
		Num8 = '8',
		Num9 = '9',
		Colon = ':',
		Semicolon = ';',
		Less = '<',
		Equals = '=',
		Greater = '>',
		Question = '?',
		At = '@',
		LeftBracket = '[',
		BackSlash = '\\',
		RightBracket = ']',
		Caret = '^',
		Underscore = '_',
		BackQuote = '`',
		A = 'a',
		B = 'b',
		C = 'c',
		D = 'd',
		E = 'e',
		F = 'f',
		G = 'g',
		H = 'h',
		I = 'i',
		J = 'j',
		K = 'k',
		L = 'l',
		M = 'm',
		N = 'n',
		O = 'o',
		P = 'p',
		Q = 'q',
		R = 'r',
		S = 's',
		T = 't',
		U = 'u',
		V = 'v',
		W = 'w',
		X = 'x',
		Y = 'y',
		Z = 'z',
		CapsLock = ScanCode.CapsLock | Input.Keyboard.ScanCodeMask,
		F1 = ScanCode.F1 | Input.Keyboard.ScanCodeMask,
		F2 = ScanCode.F2 | Input.Keyboard.ScanCodeMask,
		F3 = ScanCode.F3 | Input.Keyboard.ScanCodeMask,
		F4 = ScanCode.F4 | Input.Keyboard.ScanCodeMask,
		F5 = ScanCode.F5 | Input.Keyboard.ScanCodeMask,
		F6 = ScanCode.F6 | Input.Keyboard.ScanCodeMask,
		F7 = ScanCode.F7 | Input.Keyboard.ScanCodeMask,
		F8 = ScanCode.F8 | Input.Keyboard.ScanCodeMask,
		F9 = ScanCode.F9 | Input.Keyboard.ScanCodeMask,
		F10 = ScanCode.F10 | Input.Keyboard.ScanCodeMask,
		F11 = ScanCode.F11 | Input.Keyboard.ScanCodeMask,
		F12 = ScanCode.F12 | Input.Keyboard.ScanCodeMask,
		PrintScreen = ScanCode.PrintScreen | Input.Keyboard.ScanCodeMask,
		ScrollLock = ScanCode.ScrollLock | Input.Keyboard.ScanCodeMask,
		Pause = ScanCode.Pause | Input.Keyboard.ScanCodeMask,
		Insert = ScanCode.Insert | Input.Keyboard.ScanCodeMask,
		Home = ScanCode.Home | Input.Keyboard.ScanCodeMask,
		PageUp = ScanCode.PageUp | Input.Keyboard.ScanCodeMask,
		Delete = 127,
		End = ScanCode.End | Input.Keyboard.ScanCodeMask,
		PageDown = ScanCode.PageDown | Input.Keyboard.ScanCodeMask,
		Right = ScanCode.Right | Input.Keyboard.ScanCodeMask,
		Left = ScanCode.Left | Input.Keyboard.ScanCodeMask,
		Down = ScanCode.Down | Input.Keyboard.ScanCodeMask,
		Up = ScanCode.Up | Input.Keyboard.ScanCodeMask,
		NumLockClear = ScanCode.NumLockClear | Input.Keyboard.ScanCodeMask,
		KPDivide = ScanCode.KPDivide | Input.Keyboard.ScanCodeMask,
		KPMultiply = ScanCode.KPMultiply | Input.Keyboard.ScanCodeMask,
		KPMinus = ScanCode.KPMinus | Input.Keyboard.ScanCodeMask,
		KPPlus = ScanCode.KPPlus | Input.Keyboard.ScanCodeMask,
		KPEnter = ScanCode.KPEnter | Input.Keyboard.ScanCodeMask,
		KP1 = ScanCode.KP1 | Input.Keyboard.ScanCodeMask,
		KP2 = ScanCode.KP2 | Input.Keyboard.ScanCodeMask,
		KP3 = ScanCode.KP3 | Input.Keyboard.ScanCodeMask,
		KP4 = ScanCode.KP4 | Input.Keyboard.ScanCodeMask,
		KP5 = ScanCode.KP5 | Input.Keyboard.ScanCodeMask,
		KP6 = ScanCode.KP6 | Input.Keyboard.ScanCodeMask,
		KP7 = ScanCode.KP7 | Input.Keyboard.ScanCodeMask,
		KP8 = ScanCode.KP8 | Input.Keyboard.ScanCodeMask,
		KP9 = ScanCode.KP9 | Input.Keyboard.ScanCodeMask,
		KP0 = ScanCode.KP0 | Input.Keyboard.ScanCodeMask,
		KPPeriod = ScanCode.KPPeriod | Input.Keyboard.ScanCodeMask,
		Application = ScanCode.Application | Input.Keyboard.ScanCodeMask,
		Power = ScanCode.Power | Input.Keyboard.ScanCodeMask,
		KPEquals = ScanCode.KPEquals | Input.Keyboard.ScanCodeMask,
		F13 = ScanCode.F13 | Input.Keyboard.ScanCodeMask,
		F14 = ScanCode.F14 | Input.Keyboard.ScanCodeMask,
		F15 = ScanCode.F15 | Input.Keyboard.ScanCodeMask,
		F16 = ScanCode.F16 | Input.Keyboard.ScanCodeMask,
		F17 = ScanCode.F17 | Input.Keyboard.ScanCodeMask,
		F18 = ScanCode.F18 | Input.Keyboard.ScanCodeMask,
		F19 = ScanCode.F19 | Input.Keyboard.ScanCodeMask,
		F20 = ScanCode.F20 | Input.Keyboard.ScanCodeMask,
		F21 = ScanCode.F21 | Input.Keyboard.ScanCodeMask,
		F22 = ScanCode.F22 | Input.Keyboard.ScanCodeMask,
		F23 = ScanCode.F23 | Input.Keyboard.ScanCodeMask,
		F24 = ScanCode.F24 | Input.Keyboard.ScanCodeMask,
		Execute = ScanCode.Execute | Input.Keyboard.ScanCodeMask,
		Help = ScanCode.Help | Input.Keyboard.ScanCodeMask,
		Menu = ScanCode.Menu | Input.Keyboard.ScanCodeMask,
		Select = ScanCode.Select | Input.Keyboard.ScanCodeMask,
		Stop = ScanCode.Stop | Input.Keyboard.ScanCodeMask,
		Again = ScanCode.Again | Input.Keyboard.ScanCodeMask,
		Undo = ScanCode.Undo | Input.Keyboard.ScanCodeMask,
		Cut = ScanCode.Cut | Input.Keyboard.ScanCodeMask,
		Copy = ScanCode.Copy | Input.Keyboard.ScanCodeMask,
		Paste = ScanCode.Paste | Input.Keyboard.ScanCodeMask,
		Find = ScanCode.Find | Input.Keyboard.ScanCodeMask,
		Mute = ScanCode.Mute | Input.Keyboard.ScanCodeMask,
		VolumeUp = ScanCode.VolumeUp | Input.Keyboard.ScanCodeMask,
		VolumeDown = ScanCode.VolumeDown | Input.Keyboard.ScanCodeMask,
		KPComma = ScanCode.KPComma | Input.Keyboard.ScanCodeMask,
		KPEqualsAs400 = ScanCode.KPEqualsAs400 | Input.Keyboard.ScanCodeMask,
		AltErase = ScanCode.AltErase | Input.Keyboard.ScanCodeMask,
		SysReq = ScanCode.SysReq | Input.Keyboard.ScanCodeMask,
		Cancel = ScanCode.Cancel | Input.Keyboard.ScanCodeMask,
		Clear = ScanCode.Clear | Input.Keyboard.ScanCodeMask,
		Prior = ScanCode.Prior | Input.Keyboard.ScanCodeMask,
		Return2 = ScanCode.Return2 | Input.Keyboard.ScanCodeMask,
		Separator = ScanCode.Separator | Input.Keyboard.ScanCodeMask,
		Out = ScanCode.Out | Input.Keyboard.ScanCodeMask,
		Oper = ScanCode.Oper | Input.Keyboard.ScanCodeMask,
		ClearAgain = ScanCode.ClearAgain | Input.Keyboard.ScanCodeMask,
		CRSel = ScanCode.CRSel | Input.Keyboard.ScanCodeMask,
		EXSel = ScanCode.EXSel | Input.Keyboard.ScanCodeMask,
		KP00 = ScanCode.KP00 | Input.Keyboard.ScanCodeMask,
		KP000 = ScanCode.KP000 | Input.Keyboard.ScanCodeMask,
		ThousandsSeparator = ScanCode.ThousandsSeparator | Input.Keyboard.ScanCodeMask,
		DecimalSeparator = ScanCode.DecimalSeparator | Input.Keyboard.ScanCodeMask,
		CurrencyUnit = ScanCode.CurrencyUnit | Input.Keyboard.ScanCodeMask,
		CurrencySubUnit = ScanCode.CurrencySubUnit | Input.Keyboard.ScanCodeMask,
		KPLeftParen = ScanCode.KPLeftParen | Input.Keyboard.ScanCodeMask,
		KPRightParen = ScanCode.KPRightParen | Input.Keyboard.ScanCodeMask,
		KPLeftBrace = ScanCode.KPLeftBrace | Input.Keyboard.ScanCodeMask,
		KPRightBrace = ScanCode.KPRightBrace | Input.Keyboard.ScanCodeMask,
		KPTab = ScanCode.KPTab | Input.Keyboard.ScanCodeMask,
		KPBackspace = ScanCode.KPBackspace | Input.Keyboard.ScanCodeMask,
		KPA = ScanCode.KPA | Input.Keyboard.ScanCodeMask,
		KPB = ScanCode.KPB | Input.Keyboard.ScanCodeMask,
		KPC = ScanCode.KPC | Input.Keyboard.ScanCodeMask,
		KPD = ScanCode.KPD | Input.Keyboard.ScanCodeMask,
		KPE = ScanCode.KPE | Input.Keyboard.ScanCodeMask,
		KPF = ScanCode.KPF | Input.Keyboard.ScanCodeMask,
		KPXor = ScanCode.KPXor | Input.Keyboard.ScanCodeMask,
		KPPower = ScanCode.KPPower | Input.Keyboard.ScanCodeMask,
		KPPercent = ScanCode.KPPercent | Input.Keyboard.ScanCodeMask,
		KPLess = ScanCode.KPLess | Input.Keyboard.ScanCodeMask,
		KPGreater = ScanCode.KPGreater | Input.Keyboard.ScanCodeMask,
		KPAmpersand = ScanCode.KPAmpersand | Input.Keyboard.ScanCodeMask,
		KPDblAmpersand = ScanCode.KPDblAmpersand | Input.Keyboard.ScanCodeMask,
		KPVerticalBar = ScanCode.KPVerticalBar | Input.Keyboard.ScanCodeMask,
		KPDblVerticalBar = ScanCode.KPDblVerticalBar | Input.Keyboard.ScanCodeMask,
		KPColon = ScanCode.KPColon | Input.Keyboard.ScanCodeMask,
		KPHash = ScanCode.KPHash | Input.Keyboard.ScanCodeMask,
		KPSpace = ScanCode.KPSpace | Input.Keyboard.ScanCodeMask,
		KPAt = ScanCode.KPAt | Input.Keyboard.ScanCodeMask,
		KPExclaim = ScanCode.KPExclaim | Input.Keyboard.ScanCodeMask,
		KPMemStore = ScanCode.KPMemStore | Input.Keyboard.ScanCodeMask,
		KPMemRecall = ScanCode.KPMemRecall | Input.Keyboard.ScanCodeMask,
		KPMemClear = ScanCode.KPMemClear | Input.Keyboard.ScanCodeMask,
		KPMemAdd = ScanCode.KPMemAdd | Input.Keyboard.ScanCodeMask,
		KPMemSubtract = ScanCode.KPMemSubtract | Input.Keyboard.ScanCodeMask,
		KPMemMultiply = ScanCode.KPMemMultiply | Input.Keyboard.ScanCodeMask,
		KPMemDivide = ScanCode.KPMemDivide | Input.Keyboard.ScanCodeMask,
		KPPlusMinus = ScanCode.KPPlusMinus | Input.Keyboard.ScanCodeMask,
		KPClear = ScanCode.KPClear | Input.Keyboard.ScanCodeMask,
		KPClearEntry = ScanCode.KPClearEntry | Input.Keyboard.ScanCodeMask,
		KPBinary = ScanCode.KPBinary | Input.Keyboard.ScanCodeMask,
		KPOctal = ScanCode.KPOctal | Input.Keyboard.ScanCodeMask,
		KPDecimal = ScanCode.KPDecimal | Input.Keyboard.ScanCodeMask,
		KPHexadecimal = ScanCode.KPHexadecimal | Input.Keyboard.ScanCodeMask,
		LCtrl = ScanCode.LCtrl | Input.Keyboard.ScanCodeMask,
		LShift = ScanCode.LShift | Input.Keyboard.ScanCodeMask,
		LAlt = ScanCode.LAlt | Input.Keyboard.ScanCodeMask,
		LGui = ScanCode.LGui | Input.Keyboard.ScanCodeMask,
		RCtrl = ScanCode.RCtrl | Input.Keyboard.ScanCodeMask,
		RShift = ScanCode.RShift | Input.Keyboard.ScanCodeMask,
		RAlt = ScanCode.RAlt | Input.Keyboard.ScanCodeMask,
		RGui = ScanCode.RGui | Input.Keyboard.ScanCodeMask,
		Mode = ScanCode.Mode | Input.Keyboard.ScanCodeMask,
		AudioNext = ScanCode.AudioNext | Input.Keyboard.ScanCodeMask,
		AudioPrev = ScanCode.AudioPrev | Input.Keyboard.ScanCodeMask,
		AudioStop = ScanCode.AudioStop | Input.Keyboard.ScanCodeMask,
		AudioPlay = ScanCode.AudioPlay | Input.Keyboard.ScanCodeMask,
		AudioMute = ScanCode.AudioMute | Input.Keyboard.ScanCodeMask,
		MediaSelect = ScanCode.MediaSelect | Input.Keyboard.ScanCodeMask,
		Www = ScanCode.Www | Input.Keyboard.ScanCodeMask,
		Mail = ScanCode.Mail | Input.Keyboard.ScanCodeMask,
		Calculator = ScanCode.Calculator | Input.Keyboard.ScanCodeMask,
		Computer = ScanCode.Computer | Input.Keyboard.ScanCodeMask,
		ACSearch = ScanCode.ACSearch | Input.Keyboard.ScanCodeMask,
		ACHome = ScanCode.ACHome | Input.Keyboard.ScanCodeMask,
		ACBack = ScanCode.ACBack | Input.Keyboard.ScanCodeMask,
		ACForward = ScanCode.ACForward | Input.Keyboard.ScanCodeMask,
		ACStop = ScanCode.ACStop | Input.Keyboard.ScanCodeMask,
		ACRefresh = ScanCode.ACRefresh | Input.Keyboard.ScanCodeMask,
		ACBookmarks = ScanCode.ACBookmarks | Input.Keyboard.ScanCodeMask,
		BrightnessDown = ScanCode.BrightnessDown | Input.Keyboard.ScanCodeMask,
		BrightnessUp = ScanCode.BrightnessUp | Input.Keyboard.ScanCodeMask,
		DisplaySwitch = ScanCode.DisplaySwitch | Input.Keyboard.ScanCodeMask,
		KBDillumToggle = ScanCode.KBDillumToggle | Input.Keyboard.ScanCodeMask,
		KBDillumDown = ScanCode.KBDillumDown | Input.Keyboard.ScanCodeMask,
		KBDillumUp = ScanCode.KBDillumUp | Input.Keyboard.ScanCodeMask,
		Eject = ScanCode.Eject | Input.Keyboard.ScanCodeMask,
		Sleep = ScanCode.Sleep | Input.Keyboard.ScanCodeMask
	}
	#endregion

	#region ScanCodes
	public enum ScanCode
	{
		Unknown = 0,
		A = 4,
		B = 5,
		C = 6,
		D = 7,
		E = 8,
		F = 9,
		G = 10,
		H = 11,
		I = 12,
		J = 13,
		K = 14,
		L = 15,
		M = 16,
		N = 17,
		O = 18,
		P = 19,
		Q = 20,
		R = 21,
		S = 22,
		T = 23,
		U = 24,
		V = 25,
		W = 26,
		X = 27,
		Y = 28,
		Z = 29,
		Num1 = 30,
		Num2 = 31,
		Num3 = 32,
		Num4 = 33,
		Num5 = 34,
		Num6 = 35,
		Num7 = 36,
		Num8 = 37,
		Num9 = 38,
		Num0 = 39,
		Return = 40,
		Escape = 41,
		Backspace = 42,
		Tab = 43,
		Space = 44,
		Minus = 45,
		Equals = 46,
		LeftBracket = 47,
		RightBracket = 48,
		BackSlash = 49,
		NonusHash = 50,
		Semicolon = 51,
		Apostrophe = 52,
		Grave = 53,
		Comma = 54,
		Period = 55,
		Slash = 56,
		CapsLock = 57,
		F1 = 58,
		F2 = 59,
		F3 = 60,
		F4 = 61,
		F5 = 62,
		F6 = 63,
		F7 = 64,
		F8 = 65,
		F9 = 66,
		F10 = 67,
		F11 = 68,
		F12 = 69,
		PrintScreen = 70,
		ScrollLock = 71,
		Pause = 72,
		Insert = 73,
		Home = 74,
		PageUp = 75,
		Delete = 76,
		End = 77,
		PageDown = 78,
		Right = 79,
		Left = 80,
		Down = 81,
		Up = 82,
		NumLockClear = 83,
		KPDivide = 84,
		KPMultiply = 85,
		KPMinus = 86,
		KPPlus = 87,
		KPEnter = 88,
		KP1 = 89,
		KP2 = 90,
		KP3 = 91,
		KP4 = 92,
		KP5 = 93,
		KP6 = 94,
		KP7 = 95,
		KP8 = 96,
		KP9 = 97,
		KP0 = 98,
		KPPeriod = 99,
		NonusBackslash = 100,
		Application = 101,
		Power = 102,
		KPEquals = 103,
		F13 = 104,
		F14 = 105,
		F15 = 106,
		F16 = 107,
		F17 = 108,
		F18 = 109,
		F19 = 110,
		F20 = 111,
		F21 = 112,
		F22 = 113,
		F23 = 114,
		F24 = 115,
		Execute = 116,
		Help = 117,
		Menu = 118,
		Select = 119,
		Stop = 120,
		Again = 121,
		Undo = 122,
		Cut = 123,
		Copy = 124,
		Paste = 125,
		Find = 126,
		Mute = 127,
		VolumeUp = 128,
		VolumeDown = 129,
		KPComma = 133,
		KPEqualsAs400 = 134,
		International1 = 135,
		International2 = 136,
		International3 = 137,
		International4 = 138,
		International5 = 139,
		International6 = 140,
		International7 = 141,
		International8 = 142,
		International9 = 143,
		Lang1 = 144,
		Lang2 = 145,
		Lang3 = 146,
		Lang4 = 147,
		Lang5 = 148,
		Lang6 = 149,
		Lang7 = 150,
		Lang8 = 151,
		Lang9 = 152,
		AltErase = 153,
		SysReq = 154,
		Cancel = 155,
		Clear = 156,
		Prior = 157,
		Return2 = 158,
		Separator = 159,
		Out = 160,
		Oper = 161,
		ClearAgain = 162,
		CRSel = 163,
		EXSel = 164,
		KP00 = 176,
		KP000 = 177,
		ThousandsSeparator = 178,
		DecimalSeparator = 179,
		CurrencyUnit = 180,
		CurrencySubUnit = 181,
		KPLeftParen = 182,
		KPRightParen = 183,
		KPLeftBrace = 184,
		KPRightBrace = 185,
		KPTab = 186,
		KPBackspace = 187,
		KPA = 188,
		KPB = 189,
		KPC = 190,
		KPD = 191,
		KPE = 192,
		KPF = 193,
		KPXor = 194,
		KPPower = 195,
		KPPercent = 196,
		KPLess = 197,
		KPGreater = 198,
		KPAmpersand = 199,
		KPDblAmpersand = 200,
		KPVerticalBar = 201,
		KPDblVerticalBar = 202,
		KPColon = 203,
		KPHash = 204,
		KPSpace = 205,
		KPAt = 206,
		KPExclaim = 207,
		KPMemStore = 208,
		KPMemRecall = 209,
		KPMemClear = 210,
		KPMemAdd = 211,
		KPMemSubtract = 212,
		KPMemMultiply = 213,
		KPMemDivide = 214,
		KPPlusMinus = 215,
		KPClear = 216,
		KPClearEntry = 217,
		KPBinary = 218,
		KPOctal = 219,
		KPDecimal = 220,
		KPHexadecimal = 221,
		LCtrl = 224,
		LShift = 225,
		LAlt = 226,
		LGui = 227,
		RCtrl = 228,
		RShift = 229,
		RAlt = 230,
		RGui = 231,
		Mode = 257,
		AudioNext = 258,
		AudioPrev = 259,
		AudioStop = 260,
		AudioPlay = 261,
		AudioMute = 262,
		MediaSelect = 263,
		Www = 264,
		Mail = 265,
		Calculator = 266,
		Computer = 267,
		ACSearch = 268,
		ACHome = 269,
		ACBack = 270,
		ACForward = 271,
		ACStop = 272,
		ACRefresh = 273,
		ACBookmarks = 274,
		BrightnessDown = 275,
		BrightnessUp = 276,
		DisplaySwitch = 277,
		KBDillumToggle = 278,
		KBDillumDown = 279,
		KBDillumUp = 280,
		Eject = 281,
		Sleep = 282,
		App1 = 283,
		App2 = 284,
		NumScancodes = 512
	}
	#endregion

	public static class Input
	{
		internal static void Init()
		{
			Keyboard.Init();
			Mouse.Init();
			Controllers.Init();
		}

		internal static void Clear()
		{
			Keyboard.Clear();
			Mouse.Clear();
			Controllers.Clear();
		}

		internal static void Update()
		{
			//Keyboard.Update();
			Mouse.Update();
			//Controllers.Update();
		}

		#region Keyboard
		public static class Keyboard
		{
			internal const int ScanCodeMask = Platform.ScanCodeMask;

			private static HashSet<KeyCode> downKeyCode = new HashSet<KeyCode>();
			private static HashSet<KeyCode> pressedKeyCode = new HashSet<KeyCode>();
			private static HashSet<KeyCode> releasedKeyCode = new HashSet<KeyCode>();
			private static HashSet<KeyCode> repeatedKeyCode = new HashSet<KeyCode>();

			private static HashSet<ScanCode> downScanCode = new HashSet<ScanCode>();
			private static HashSet<ScanCode> pressedScanCode = new HashSet<ScanCode>();
			private static HashSet<ScanCode> releasedScanCode = new HashSet<ScanCode>();
			private static HashSet<ScanCode> repeatedScanCode = new HashSet<ScanCode>();

			internal static void Init()
			{
				Platform.Events.OnKeyDown += (keycode, scancode, repeated) =>
				{
					downKeyCode.Add(keycode);
					downScanCode.Add(scancode);

					if (repeated)
					{
						repeatedKeyCode.Add(keycode);
						repeatedScanCode.Add(scancode);

						//Output.WriteLine("repeated: " + keycode); // Debug
					}
					else
					{
						pressedKeyCode.Add(keycode);
						pressedScanCode.Add(scancode);

						//Output.WriteLine("pressed:  " + keycode); // Debug
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

					//Output.WriteLine("released: " + keycode); // Debug
				};
			}

			internal static void Clear()
			{
				//Output.WriteLine("down:     " + string.Join(", ", downKeyCode)); // Debug

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

			private static readonly bool[] down = new bool[Enum.GetNames(typeof(MouseButton)).Length];
			private static readonly bool[] pressed = new bool[Enum.GetNames(typeof(MouseButton)).Length];
			private static readonly bool[] released = new bool[Enum.GetNames(typeof(MouseButton)).Length];

			internal static void Init()
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

			internal static void Clear()
			{
				Scroll = Point2.Zero;

				for (int i = 0; i < Enum.GetNames(typeof(MouseButton)).Length; i++)
				{
					pressed[i] = false;
					released[i] = false;
				}
			}

			internal static void Update()
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

			//public int DeviceIndex => Platform.GetController(ID).DeviceIndex; // SDL device index - apparently this can change, so not using
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

			private readonly bool[] down = new bool[Enum.GetNames(typeof(ControllerButton)).Length];
			private readonly bool[] pressed = new bool[Enum.GetNames(typeof(ControllerButton)).Length];
			private readonly bool[] released = new bool[Enum.GetNames(typeof(ControllerButton)).Length];

			internal Controller(int slot, int id)
			{
				Slot = slot;
				ID = id;
				Connected = true;
			}

			// Any/Dummy Controllers
			private Controller() { }
			internal static Controller Any { get; } = new Controller();
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
			private static Dictionary<int, Controller> controllers = new Dictionary<int, Controller>(); // Slot-based, starts at 1
			private static Dictionary<int, int> controllerIDs = new Dictionary<int, int>(); // Unique device ID's provided by SDL, dependant on order added
			private static Controller[] controllerPositions = new Controller[0]; // Position-based, recreated whenever a device is added or removed

			internal static void Init()
			{
				Platform.Events.OnControllerDeviceAdded += (id) =>
				{
					if (!controllerIDs.ContainsKey(id))
					{
						int slot = 1;
						while (controllers.ContainsKey(slot))
							slot++;
						controllers.Add(slot, new Controller(slot, id));
						controllerIDs.Add(id, slot);

						// debug
						Output.WriteLine($"Controller Added: {controllers[slot]}");
						Output.WriteLine($"Total Controllers: {controllers.Count}");

						UpdateControllerPositions();
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

						UpdateControllerPositions();
					}
				};

				Platform.Events.OnControllerDeviceRemapped += (id) =>
				{
					UpdateControllerPositions();

					Output.WriteLine($"OnControllerDeviceRemapped not implemented. (Controller ID {id})");
				};

				Platform.Events.OnControllerButtonDown += (id, button) =>
				{
					if (controllerIDs.ContainsKey(id))
					{
						controllers[controllerIDs[id]].OnControllerButtonDown(button);
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

			private static void UpdateControllerPositions()
			{
				int maxSlot = 0;
				foreach (int slot in controllers.Keys)
				{
					if (controllers.ContainsKey(slot))
						maxSlot = maxSlot.Max(slot);
				}

				controllerPositions = new Controller[controllers.Count];

				int position = 0;
				for (int slot = 1; slot <= maxSlot; slot++)
				{
					if (controllers.ContainsKey(slot))
					{
						controllerPositions[position] = controllers[slot];
						position++;
					}
				}

				// debug
				//Output.WriteLine($"controllerPositions.Length: {controllerPositions.Length}");
				//foreach (Controller controller in controllerPositions)
				//{
				//	Output.WriteLine($"    - {controller}");
				//}
			}

			public static Controller Slot(int slot) => controllers.ContainsKey(slot) ? controllers[slot] : Controller.Dummy;
			public static Controller Slot1 => Slot(1);
			public static Controller Slot2 => Slot(2);
			public static Controller Slot3 => Slot(3);
			public static Controller Slot4 => Slot(4);
			public static Controller Slot5 => Slot(5);
			public static Controller Slot6 => Slot(6);
			public static Controller Slot7 => Slot(7);
			public static Controller Slot8 => Slot(8);

			public static Controller Position(int position) => controllerPositions.Length >= position /* index + 1 hack */ ? controllerPositions[position - 1] : Controller.Dummy;
			public static Controller First => Position(1);
			public static Controller Second => Position(2);
			public static Controller Third => Position(3);
			public static Controller Fourth => Position(4);
			public static Controller Fifth => Position(5);
			public static Controller Sixth => Position(6);
			public static Controller Seventh => Position(7);
			public static Controller Eighth => Position(8);
		}
		#endregion
	}
}