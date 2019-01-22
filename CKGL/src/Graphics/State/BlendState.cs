using System;
using OpenGL;

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
				OnStateChanging?.Invoke();
				if (blendState.Enabled)
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

		public static void SetEnabled(bool enabled)
		{
			Set(new BlendState(enabled, Current.ColourSource, Current.AlphaSource, Current.ColourDestination, Current.AlphaDestination, Current.ColourEquation, Current.AlphaEquation));
		}

		public static void SetBlendFactor(BlendFactor source, BlendFactor destination)
		{
			Set(new BlendState(Current.Enabled, source, source, destination, destination, Current.ColourEquation, Current.AlphaEquation));
		}

		public static void SetBlendFactor(
			BlendFactor colourSource,
			BlendFactor colourDestination,
			BlendFactor alphaSource,
			BlendFactor alphaDestination)
		{
			Set(new BlendState(Current.Enabled, colourSource, alphaSource, colourDestination, alphaDestination, Current.ColourEquation, Current.AlphaEquation));
		}

		public static void SetBlend(
			BlendFactor colourSource,
			BlendFactor colourDestination,
			BlendFactor alphaSource,
			BlendFactor alphaDestination,
			BlendEquation colourEquation,
			BlendEquation alphaEquation)
		{
			Set(new BlendState(Current.Enabled, colourSource, alphaSource, colourDestination, alphaDestination, colourEquation, alphaEquation));
		}
		#endregion

		#region Overrides
		public override string ToString()
		{
			return $"BlendState: [Enabled: {Enabled}, ColourSource: {ColourSource}, AlphaSource: {AlphaSource}, ColourDestination: {ColourDestination}, AlphaDestination: {AlphaDestination}, ColourEquation: {ColourEquation}, AlphaEquation: {AlphaEquation}]";
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