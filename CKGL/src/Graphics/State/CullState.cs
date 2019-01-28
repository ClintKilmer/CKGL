using System;
using CKGL.OpenGLBindings;

namespace CKGL
{
	public struct CullState
	{
		public readonly bool Enabled;
		public readonly Face Face;

		public static Action OnStateChanging;
		public static Action OnStateChanged;
		public static CullState Default { get; private set; }
		public static CullState Current { get; private set; }

		#region Static Constructors
		static CullState()
		{
			Default = Off;
		}
		public static readonly CullState Off = new CullState(false, Face.Back);
		public static readonly CullState Back = new CullState(true, Face.Back);
		public static readonly CullState Front = new CullState(true, Face.Front);
		public static readonly CullState FrontAndBack = new CullState(true, Face.FrontAndBack);
		#endregion

		#region Constructors
		private CullState(bool enabled, Face cullFace)
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
		public static void Set(CullState cullState)
		{
			if (Current != cullState)
			{
				OnStateChanging?.Invoke();
				if (cullState.Enabled)
					GL.Enable(EnableCap.CullFace);
				else
					GL.Disable(EnableCap.CullFace);
				GL.CullFace(cullState.Face);
				Current = cullState;
				OnStateChanged?.Invoke();
			}
		}
		public static void Reset() => Set(Default);
		public static void SetDefault(CullState cullState) => Default = cullState;

		public static void SetEnabled(bool enabled)
		{
			Set(new CullState(enabled, Current.Face));
		}

		public static void SetFace(Face face)
		{
			Set(new CullState(Current.Enabled, face));
		}
		#endregion

		#region Overrides
		public override string ToString()
		{
			return $"CullState: [Enabled: {Enabled}, Face: {Face}]";
		}
		#endregion

		#region Operators
		public static bool operator ==(CullState a, CullState b)
		{
			return a.Enabled == b.Enabled && a.Face == b.Face;
		}
		public static bool operator !=(CullState a, CullState b)
		{
			return a.Enabled != b.Enabled || a.Face != b.Face;
		}
		#endregion
	}
}