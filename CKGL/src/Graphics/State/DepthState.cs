using System;
using CKGL.OpenGLBindings;

namespace CKGL
{
	public struct DepthState
	{
		public readonly bool Enabled;
		public readonly DepthFunc DepthFunc;

		public static Action OnStateChanging;
		public static Action OnStateChanged;
		public static DepthState Default { get; private set; }
		public static DepthState Current { get; private set; }

		#region Static Constructors
		static DepthState()
		{
			Default = Off;
		}
		public static readonly DepthState Off = new DepthState(false);
		public static readonly DepthState Never = new DepthState(true, DepthFunc.Never);
		public static readonly DepthState Less = new DepthState(true, DepthFunc.Less);
		public static readonly DepthState Equal = new DepthState(true, DepthFunc.Equal);
		public static readonly DepthState LessEqual = new DepthState(true, DepthFunc.LessEqual);
		public static readonly DepthState Greater = new DepthState(true, DepthFunc.Greater);
		public static readonly DepthState NotEqual = new DepthState(true, DepthFunc.NotEqual);
		public static readonly DepthState GreaterEqual = new DepthState(true, DepthFunc.GreaterEqual);
		public static readonly DepthState Always = new DepthState(true, DepthFunc.Always);
		#endregion

		#region Constructors
		private DepthState(bool enabled) : this(enabled, DepthFunc.Less) { }
		private DepthState(bool enabled, DepthFunc depthFunc)
		{
			Enabled = enabled;
			DepthFunc = depthFunc;
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
		public static void Set(DepthState depthState)
		{
			if (Current != depthState)
			{
				OnStateChanging?.Invoke();
				if (depthState.Enabled)
					GL.Enable(EnableCap.DepthTest);
				else
					GL.Disable(EnableCap.DepthTest);
				GL.DepthFunc(depthState.DepthFunc);
				Current = depthState;
				OnStateChanged?.Invoke();
			}
		}
		public static void Reset() => Set(Default);
		public static void SetDefault(DepthState depthState) => Default = depthState;

		public static void SetEnabled(bool enabled)
		{
			Set(new DepthState(enabled, Current.DepthFunc));
		}

		public static void SetDepthFunc(DepthFunc depthFunc)
		{
			Set(new DepthState(Current.Enabled, depthFunc));
		}
		#endregion

		#region Overrides
		public override string ToString()
		{
			return $"DepthState: [Enabled: {Enabled}, Func: {DepthFunc}]";
		}
		#endregion

		#region Operators
		public static bool operator ==(DepthState a, DepthState b)
		{
			return a.Enabled == b.Enabled && a.DepthFunc == b.DepthFunc;
		}
		public static bool operator !=(DepthState a, DepthState b)
		{
			return a.Enabled != b.Enabled || a.DepthFunc != b.DepthFunc;
		}
		#endregion
	}
}