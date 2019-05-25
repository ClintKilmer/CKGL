namespace CKGL
{
	public struct ColourMaskState
	{
		public readonly bool R;
		public readonly bool G;
		public readonly bool B;
		public readonly bool A;

		public static ColourMaskState Default { get; private set; }
		public static ColourMaskState Current { get; private set; }

		#region Static Constructors
		static ColourMaskState()
		{
			Default = RGBA;
		}
		public static readonly ColourMaskState RGBA = new ColourMaskState(true, true, true, true);
		public static readonly ColourMaskState RGB_ = new ColourMaskState(true, true, true, false);
		public static readonly ColourMaskState RG_A = new ColourMaskState(true, true, false, true);
		public static readonly ColourMaskState RG__ = new ColourMaskState(true, true, false, false);
		public static readonly ColourMaskState R_BA = new ColourMaskState(true, false, true, true);
		public static readonly ColourMaskState R_B_ = new ColourMaskState(true, false, true, false);
		public static readonly ColourMaskState R__A = new ColourMaskState(true, false, false, true);
		public static readonly ColourMaskState R___ = new ColourMaskState(true, false, false, false);
		public static readonly ColourMaskState _GBA = new ColourMaskState(false, true, true, true);
		public static readonly ColourMaskState _GB_ = new ColourMaskState(false, true, true, false);
		public static readonly ColourMaskState _G_A = new ColourMaskState(false, true, false, true);
		public static readonly ColourMaskState _G__ = new ColourMaskState(false, true, false, false);
		public static readonly ColourMaskState __BA = new ColourMaskState(false, false, true, true);
		public static readonly ColourMaskState __B_ = new ColourMaskState(false, false, true, false);
		public static readonly ColourMaskState ___A = new ColourMaskState(false, false, false, true);
		public static readonly ColourMaskState ____ = new ColourMaskState(false, false, false, false);
		#endregion

		#region Constructors
		private ColourMaskState(bool r, bool g, bool b, bool a)
		{
			R = r;
			G = g;
			B = b;
			A = a;
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
		public static void Set(ColourMaskState colourMaskState)
		{
			if (Current != colourMaskState)
			{
				Graphics.State.OnStateChanging?.Invoke();
				Graphics.SetColourMask(colourMaskState);
				Current = colourMaskState;
				Graphics.State.OnStateChanged?.Invoke();
			}
		}
		public static void Reset() => Set(Default);
		public static void SetDefault(ColourMaskState colourMaskState) => Default = colourMaskState;
		#endregion

		#region Overrides
		public override string ToString()
		{
			return $"ColourMaskState: [R: {R}, G: {G}, B: {B}, A: {A}]";
		}
		#endregion

		#region Operators
		public static bool operator ==(ColourMaskState a, ColourMaskState b)
		{
			return a.R == b.R && a.G == b.G && a.B == b.B && a.A == b.A;
		}

		public static bool operator !=(ColourMaskState a, ColourMaskState b)
		{
			return a.R != b.R || a.G != b.G || a.B != b.B || a.A != b.A;
		}
		#endregion
	}
}