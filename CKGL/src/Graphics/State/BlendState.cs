using System;

namespace CKGL
{
	#region Enums
	public enum BlendEquation : byte
	{
		Add,
		Subtract,
		ReverseSubtract,
		Max,
		Min
	}

	internal static class BlendEquationExt
	{
		internal static OpenGLBindings.BlendEquation ToOpenGL(this BlendEquation blendEquation)
		{
			switch (blendEquation)
			{
				case BlendEquation.Add:
					return OpenGLBindings.BlendEquation.Add;
				case BlendEquation.Subtract:
					return OpenGLBindings.BlendEquation.Subtract;
				case BlendEquation.ReverseSubtract:
					return OpenGLBindings.BlendEquation.ReverseSubtract;
				case BlendEquation.Max:
					return OpenGLBindings.BlendEquation.Max;
				case BlendEquation.Min:
					return OpenGLBindings.BlendEquation.Min;
				default:
					throw new NotImplementedException();
			}
		}
	}

	public enum BlendFactor : byte
	{
		Zero,
		One,
		SrcColour,
		OneMinusSrcColour,
		SrcAlpha,
		OneMinusSrcAlpha,
		DstAlpha,
		OneMinusDstAlpha,
		DstColour,
		OneMinusDstcolour,
		SrcAlphaSaturate,
		ConstantColour,
		OneMinusConstantColour,
		ConstantAlpha,
		OneMinusConstantAlpha
	}

	internal static class BlendFactorExt
	{
		internal static OpenGLBindings.BlendFactor ToOpenGL(this BlendFactor blendFactor)
		{
			switch (blendFactor)
			{
				case BlendFactor.Zero:
					return OpenGLBindings.BlendFactor.Zero;
				case BlendFactor.One:
					return OpenGLBindings.BlendFactor.One;
				case BlendFactor.SrcColour:
					return OpenGLBindings.BlendFactor.SrcColour;
				case BlendFactor.OneMinusSrcColour:
					return OpenGLBindings.BlendFactor.OneMinusSrcColour;
				case BlendFactor.SrcAlpha:
					return OpenGLBindings.BlendFactor.SrcAlpha;
				case BlendFactor.OneMinusSrcAlpha:
					return OpenGLBindings.BlendFactor.OneMinusSrcAlpha;
				case BlendFactor.DstAlpha:
					return OpenGLBindings.BlendFactor.DstAlpha;
				case BlendFactor.OneMinusDstAlpha:
					return OpenGLBindings.BlendFactor.OneMinusDstAlpha;
				case BlendFactor.DstColour:
					return OpenGLBindings.BlendFactor.DstColour;
				case BlendFactor.OneMinusDstcolour:
					return OpenGLBindings.BlendFactor.OneMinusDstcolour;
				case BlendFactor.SrcAlphaSaturate:
					return OpenGLBindings.BlendFactor.SrcAlphaSaturate;
				case BlendFactor.ConstantColour:
					return OpenGLBindings.BlendFactor.ConstantColour;
				case BlendFactor.OneMinusConstantColour:
					return OpenGLBindings.BlendFactor.OneMinusConstantColour;
				case BlendFactor.ConstantAlpha:
					return OpenGLBindings.BlendFactor.ConstantAlpha;
				case BlendFactor.OneMinusConstantAlpha:
					return OpenGLBindings.BlendFactor.OneMinusConstantAlpha;
				default:
					throw new NotImplementedException();
			}
		}
	}
	#endregion

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