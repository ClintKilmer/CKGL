using OpenGL;

namespace CKGL
{
	public struct DepthState
	{
		public readonly bool On;
		public readonly DepthFunc DepthFunc;

		public static DepthState Default { get { return Off; } }

		#region Static Constructors
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
		private DepthState(bool on) : this(on, DepthFunc.Less) { }
		private DepthState(bool on, DepthFunc depthFunc)
		{
			On = on;
			DepthFunc = depthFunc;
		}
		#endregion

		#region Overrides
		public override string ToString()
		{
			return $"DepthState: [Enabled: {On}, Func: {DepthFunc}]";
		}
		#endregion

		#region Operators
		public static bool operator ==(DepthState a, DepthState b)
		{
			return a.On == b.On && a.DepthFunc == b.DepthFunc;
		}
		public static bool operator !=(DepthState a, DepthState b)
		{
			return a.On != b.On || a.DepthFunc != b.DepthFunc;
		}
		#endregion
	}
}