namespace CKGL
{
	public struct BlendState
	{
		public readonly bool Enabled;
		public readonly BlendFactor ColourSource;
		public readonly BlendFactor AlphaSource;
		public readonly BlendFactor ColourDestination;
		public readonly BlendFactor AlphaDestination;
		public readonly BlendEquation ColourEquation;
		public readonly BlendEquation AlphaEquation;

		public static BlendState Default { get; private set; }
		public static BlendState Current { get; private set; }

		#region Static Constructors
		static BlendState()
		{
			Default = AlphaBlend;
		}
		public static readonly BlendState Off = new BlendState(false);
		public static readonly BlendState Opaque = new BlendState(true, BlendFactor.One, BlendFactor.Zero);
		public static readonly BlendState AlphaBlend = new BlendState(true, BlendFactor.SrcAlpha, BlendFactor.OneMinusSrcAlpha);
		public static readonly BlendState Additive = new BlendState(true, BlendFactor.SrcAlpha, BlendFactor.One);
		public static readonly BlendState Subtractive = new BlendState(true, BlendFactor.Zero, BlendFactor.OneMinusSrcColour);
		#endregion

		#region Constructors
		public BlendState(
			bool enabled,
			BlendFactor colourSource,
			BlendFactor alphaSource,
			BlendFactor colourDestination,
			BlendFactor alphaDestination,
			BlendEquation colourEquation,
			BlendEquation alphaEquation)
		{
			Enabled = enabled;
			ColourSource = colourSource;
			AlphaSource = alphaSource;
			ColourDestination = colourDestination;
			AlphaDestination = alphaDestination;
			ColourEquation = colourEquation;
			AlphaEquation = alphaEquation;
		}
		public BlendState(
			bool enabled,
			BlendFactor colourSource,
			BlendFactor colourDestination,
			BlendFactor alphaSource,
			BlendFactor alphaDestination)
		: this(
			enabled,
			colourSource,
			alphaSource,
			colourDestination,
			alphaDestination,
			BlendEquation.Add,
			BlendEquation.Add)
		{ }
		public BlendState(
			bool enabled,
			BlendFactor source,
			BlendFactor destination)
		: this(
			enabled,
			source,
			source,
			destination,
			destination,
			BlendEquation.Add,
			BlendEquation.Add)
		{ }
		public BlendState(
			bool enabled)
		: this(
			enabled,
			BlendFactor.One,
			BlendFactor.One,
			BlendFactor.Zero,
			BlendFactor.Zero,
			BlendEquation.Add,
			BlendEquation.Add)
		{ }
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
		public static void Set(BlendState blendState)
		{
			if (Current != blendState)
			{
				Graphics.State.OnStateChanging?.Invoke();
				Graphics.SetBlend(blendState);
				Current = blendState;
				Graphics.State.OnStateChanged?.Invoke();
			}
		}
		public static void Reset() => Set(Default);
		public static void SetDefault(BlendState blendState) => Default = blendState;
		#endregion

		#region Overrides
		public override string ToString()
		{
			return $"BlendState: [Enabled: {Enabled}, ColourSource: {ColourSource}, AlphaSource: {AlphaSource}, ColourDestination: {ColourDestination}, AlphaDestination: {AlphaDestination}, ColourEquation: {ColourEquation}, AlphaEquation: {AlphaEquation}]";
		}

		public override bool Equals(object obj)
		{
			return obj is BlendState && Equals((BlendState)obj);
		}
		public bool Equals(BlendState blendState)
		{
			return this == blendState;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hash = 17;
				hash = hash * 23 + Enabled.GetHashCode();
				hash = hash * 23 + ColourSource.GetHashCode();
				hash = hash * 23 + AlphaSource.GetHashCode();
				hash = hash * 23 + ColourDestination.GetHashCode();
				hash = hash * 23 + AlphaDestination.GetHashCode();
				hash = hash * 23 + ColourEquation.GetHashCode();
				hash = hash * 23 + AlphaEquation.GetHashCode();
				return hash;
			}
		}
		#endregion

		#region Operators
		public static bool operator ==(BlendState a, BlendState b)
		{
			return a.Enabled == b.Enabled &&
				   a.ColourSource == b.ColourSource &&
				   a.AlphaSource == b.AlphaSource &&
				   a.ColourDestination == b.ColourDestination &&
				   a.AlphaDestination == b.AlphaDestination &&
				   a.ColourEquation == b.ColourEquation &&
				   a.AlphaEquation == b.AlphaEquation;
		}

		public static bool operator !=(BlendState a, BlendState b)
		{
			return a.Enabled != b.Enabled ||
				   a.ColourSource != b.ColourSource ||
				   a.AlphaSource != b.AlphaSource ||
				   a.ColourDestination != b.ColourDestination ||
				   a.AlphaDestination != b.AlphaDestination ||
				   a.ColourEquation != b.ColourEquation ||
				   a.AlphaEquation != b.AlphaEquation;
		}
		#endregion
	}
}