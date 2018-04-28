namespace CKGL
{
	public struct BlendState
	{
		public bool On;
		public BlendFactor ColourSource;
		public BlendFactor AlphaSource;
		public BlendFactor ColourDestination;
		public BlendFactor AlphaDestination;
		public BlendEquation ColourEquation;
		public BlendEquation AlphaEquation;

		public static BlendState Default { get { return Off; } }

		#region Static Constructors
		public static readonly BlendState Off = new BlendState(false);
		public static readonly BlendState Opaque = new BlendState(true, BlendFactor.One, BlendFactor.Zero);
		public static readonly BlendState AlphaBlend = new BlendState(true, BlendFactor.SrcAlpha, BlendFactor.OneMinusSrcAlpha);
		public static readonly BlendState Additive = new BlendState(true, BlendFactor.SrcAlpha, BlendFactor.One);
		public static readonly BlendState Subtractive = new BlendState(true, BlendFactor.Zero, BlendFactor.OneMinusSrcColour);
		#endregion

		#region Constructors
		public BlendState(
			bool on,
			BlendFactor colourSource,
			BlendFactor alphaSource,
			BlendFactor colourDestination,
			BlendFactor alphaDestination,
			BlendEquation colourEquation,
			BlendEquation alphaEquation)
		{
			On = on;
			ColourSource = colourSource;
			AlphaSource = alphaSource;
			ColourDestination = colourDestination;
			AlphaDestination = alphaDestination;
			ColourEquation = colourEquation;
			AlphaEquation = alphaEquation;
		}
		public BlendState(
			bool on,
			BlendFactor colourSource,
			BlendFactor colourDestination,
			BlendFactor alphaSource,
			BlendFactor alphaDestination)
		: this(
			on,
			colourSource,
			alphaSource,
			colourDestination,
			alphaDestination,
			BlendEquation.Add,
			BlendEquation.Add)
		{ }
		public BlendState(
			bool on,
			BlendFactor source,
			BlendFactor destination)
		: this(
			on,
			source,
			source,
			destination,
			destination,
			BlendEquation.Add,
			BlendEquation.Add)
		{ }
		public BlendState(
			bool on)
		: this(
			on,
			BlendFactor.One,
			BlendFactor.One,
			BlendFactor.Zero,
			BlendFactor.Zero,
			BlendEquation.Add,
			BlendEquation.Add)
		{ }
		#endregion

		#region Overrides
		public override string ToString()
		{
			return $"BlendState: [Enabled: {On}, ColourSource: {ColourSource}, AlphaSource: {AlphaSource}, ColourDestination: {ColourDestination}, AlphaDestination: {AlphaDestination}, ColourEquation: {ColourEquation}, AlphaEquation: {AlphaEquation}]";
		}
		#endregion

		#region Operators
		public static bool operator ==(BlendState a, BlendState b)
		{
			return a.On == b.On &&
				   a.ColourSource == b.ColourSource &&
				   a.AlphaSource == b.AlphaSource &&
				   a.ColourDestination == b.ColourDestination &&
				   a.AlphaDestination == b.AlphaDestination &&
				   a.ColourEquation == b.ColourEquation &&
				   a.AlphaEquation == b.AlphaEquation;
		}
		public static bool operator !=(BlendState a, BlendState b)
		{
			return a.On != b.On ||
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