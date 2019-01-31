using System;

namespace CKGL
{
	#region Enums
	public enum Face : byte
	{
		Front,
		Back,
		FrontAndBack
	}

	internal static class FaceExt
	{
		internal static OpenGLBindings.Face ToOpenGL(this Face face)
		{
			switch (face)
			{
				case Face.Front:
					return OpenGLBindings.Face.Front;
				case Face.Back:
					return OpenGLBindings.Face.Back;
				case Face.FrontAndBack:
					return OpenGLBindings.Face.FrontAndBack;
				default:
					throw new NotImplementedException();
			}
		}
	}
	#endregion

	public struct CullModeState
	{
		public readonly bool Enabled;
		public readonly Face Face;

		public static CullModeState Default { get; private set; }
		public static CullModeState Current { get; private set; }

		#region Static Constructors
		static CullModeState()
		{
			Default = Off;
		}
		public static readonly CullModeState Off = new CullModeState(false, Face.Back);
		public static readonly CullModeState Back = new CullModeState(true, Face.Back);
		public static readonly CullModeState Front = new CullModeState(true, Face.Front);
		public static readonly CullModeState FrontAndBack = new CullModeState(true, Face.FrontAndBack);
		#endregion

		#region Constructors
		private CullModeState(bool enabled, Face cullFace)
		{
			Enabled = enabled;
			Face = cullFace;
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
		public static void Set(CullModeState cullModeState)
		{
			if (Current != cullModeState)
			{
				Graphics.State.OnStateChanging?.Invoke();
				Graphics.SetCullMode(cullModeState.Enabled, cullModeState.Face);
				Current = cullModeState;
				Graphics.State.OnStateChanged?.Invoke();
			}
		}
		public static void Reset() => Set(Default);
		public static void SetDefault(CullModeState cullModeState) => Default = cullModeState;
		#endregion

		#region Overrides
		public override string ToString()
		{
			return $"CullModeState: [Enabled: {Enabled}, Face: {Face}]";
		}
		#endregion

		#region Operators
		public static bool operator ==(CullModeState a, CullModeState b)
		{
			return a.Enabled == b.Enabled && a.Face == b.Face;
		}
		public static bool operator !=(CullModeState a, CullModeState b)
		{
			return a.Enabled != b.Enabled || a.Face != b.Face;
		}
		#endregion
	}
}