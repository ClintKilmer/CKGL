using System;
using OpenGL;

namespace CKGL
{
	public struct CullState
	{
		public readonly bool On;
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
		private CullState(bool on, Face cullFace)
		{
			On = on;
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
				if (cullState.On)
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
		#endregion

		#region Overrides
		public override string ToString()
		{
			return $"CullState: [Enabled: {On}, Face: {Face}]";
		}
		#endregion

		#region Operators
		public static bool operator ==(CullState a, CullState b)
		{
			return a.On == b.On && a.Face == b.Face;
		}
		public static bool operator !=(CullState a, CullState b)
		{
			return a.On != b.On || a.Face != b.Face;
		}
		#endregion
	}
}