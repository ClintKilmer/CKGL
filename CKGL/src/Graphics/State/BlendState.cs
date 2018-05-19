using System;
using OpenGL;

namespace CKGL
{
	public struct BlendState
	{
		public readonly bool On;
		public readonly BlendFactor ColourSource;
		public readonly BlendFactor AlphaSource;
		public readonly BlendFactor ColourDestination;
		public readonly BlendFactor AlphaDestination;
		public readonly BlendEquation ColourEquation;
		public readonly BlendEquation AlphaEquation;

		public static Action OnStateChanging;
		public static Action OnStateChanged;
		public static BlendState Default { get; private set; }
		public static BlendState Current { get; private set; }

		#region Static Constructors
		static BlendState()
		{
			Default = Off;
		}
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
				OnStateChanging?.Invoke();
				if (blendState.On)
					GL.Enable(EnableCap.Blend);
				else
					GL.Disable(EnableCap.Blend);
				GL.BlendFuncSeparate(blendState.ColourSource, blendState.ColourDestination, blendState.AlphaSource, blendState.AlphaDestination);
				GL.BlendEquationSeparate(blendState.ColourEquation, blendState.AlphaEquation);
				Current = blendState;
				OnStateChanged?.Invoke();
			}
		}
		public static void Reset() => Set(Default);
		public static void SetDefault(BlendState blendState) => Default = blendState;
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