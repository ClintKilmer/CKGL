using System;

namespace CKGL
{
	public struct MaskState
	{
		public readonly bool R;
		public readonly bool G;
		public readonly bool B;
		public readonly bool A;
		public readonly bool Depth;

		public static Action OnStateChanging;
		public static Action OnStateChanged;
		public static MaskState Default { get; private set; }
		public static MaskState Current { get; private set; }

		#region Static Constructors
		static MaskState()
		{
			Default = All;
		}
		public static readonly MaskState All = new MaskState(true, true, true, true, true);
		public static readonly MaskState None = new MaskState(false, false, false, false, false);
		public static readonly MaskState ColourOff = new MaskState(false, false, false, true, true);
		public static readonly MaskState ColourOnly = new MaskState(true, true, true, false, false);
		public static readonly MaskState AlphaOff = new MaskState(true, true, true, false, true);
		public static readonly MaskState AlphaOnly = new MaskState(false, false, false, true, false);
		public static readonly MaskState DepthOff = new MaskState(true, true, true, true, false);
		public static readonly MaskState DepthOnly = new MaskState(false, false, false, false, true);
		#endregion

		#region Constructors
		public MaskState(bool r, bool g, bool b, bool a, bool depth)
		{
			R = r;
			G = g;
			B = b;
			A = a;
			Depth = depth;
		}
		#endregion

		#region Methods
		public void Set()
		{
			Set(this);
		}

		public void SetDefault()
		{
			SetDefault(this);
		}
		#endregion

		#region Static Methods
		public static void Set(MaskState maskState)
		{
			if (Current != maskState)
			{
				OnStateChanging?.Invoke();
				Graphics.SetMask(maskState.R, maskState.G, maskState.B, maskState.A, maskState.Depth);
				Current = maskState;
				OnStateChanged?.Invoke();
			}
		}
		public static void Reset() => Set(Default);
		public static void SetDefault(MaskState MaskState) => Default = MaskState;

		public static void Set(bool r, bool g, bool b, bool a, bool depth)
		{
			Set(new MaskState(r, g, b, a, depth));
		}

		public static void SetRed(bool enabled)
		{
			Set(new MaskState(enabled, Current.G, Current.B, Current.A, Current.Depth));
		}

		public static void SetGreen(bool enabled)
		{
			Set(new MaskState(Current.R, enabled, Current.B, Current.A, Current.Depth));
		}

		public static void SetBlue(bool enabled)
		{
			Set(new MaskState(Current.R, Current.G, enabled, Current.A, Current.Depth));
		}

		public static void SetColour(bool r, bool g, bool b)
		{
			Set(new MaskState(r, g, b, Current.A, Current.Depth));
		}

		public static void SetColourAlpha(bool r, bool g, bool b, bool a)
		{
			Set(new MaskState(r, g, b, a, Current.Depth));
		}

		public static void SetAlpha(bool enabled)
		{
			Set(new MaskState(Current.R, Current.G, Current.B, enabled, Current.Depth));
		}

		public static void SetDepth(bool enabled)
		{
			Set(new MaskState(Current.R, Current.G, Current.B, Current.A, enabled));
		}
		#endregion

		#region Overrides
		public override string ToString()
		{
			return $"MaskState: [R: {R}, G: {G}, B: {B}, A: {A}, Depth: {Depth}]";
		}
		#endregion

		#region Operators
		public static bool operator ==(MaskState a, MaskState b)
		{
			return a.R == b.R && a.G == b.G && a.B == b.B && a.A == b.A && a.Depth == b.Depth;
		}
		public static bool operator !=(MaskState a, MaskState b)
		{
			return a.R != b.R || a.G != b.G || a.B != b.B || a.A != b.A || a.Depth != b.Depth;
		}
		#endregion
	}
}