namespace CKGL
{
	public struct DepthState
	{
		public readonly bool Enabled;
		public readonly DepthFunction DepthFunction;

		public static DepthState Default { get; private set; }
		public static DepthState Current { get; private set; }

		#region Static Constructors
		static DepthState()
		{
			Default = Off;
		}
		public static readonly DepthState Off = new DepthState(false);
		public static readonly DepthState Never = new DepthState(true, DepthFunction.Never);
		public static readonly DepthState Less = new DepthState(true, DepthFunction.Less);
		public static readonly DepthState Equal = new DepthState(true, DepthFunction.Equal);
		public static readonly DepthState LessEqual = new DepthState(true, DepthFunction.LessEqual);
		public static readonly DepthState Greater = new DepthState(true, DepthFunction.Greater);
		public static readonly DepthState NotEqual = new DepthState(true, DepthFunction.NotEqual);
		public static readonly DepthState GreaterEqual = new DepthState(true, DepthFunction.GreaterEqual);
		public static readonly DepthState Always = new DepthState(true, DepthFunction.Always);
		#endregion

		#region Constructors
		private DepthState(bool enabled) : this(enabled, DepthFunction.Less) { }
		private DepthState(bool enabled, DepthFunction depthFunc)
		{
			Enabled = enabled;
			DepthFunction = depthFunc;
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
				Graphics.State.OnStateChanging?.Invoke();
				Graphics.SetDepth(depthState);
				Current = depthState;
				Graphics.State.OnStateChanged?.Invoke();
			}
		}
		public static void Reset() => Set(Default);
		public static void SetDefault(DepthState depthState) => Default = depthState;
		#endregion

		#region Overrides
		public override string ToString()
		{
			return $"DepthState: [Enabled: {Enabled}, Func: {DepthFunction}]";
		}

		public override bool Equals(object obj)
		{
			return obj is DepthState && Equals((DepthState)obj);
		}
		public bool Equals(DepthState depth)
		{
			return this == depth;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hash = 17;
				hash = hash * 23 + Enabled.GetHashCode();
				hash = hash * 23 + DepthFunction.GetHashCode();
				return hash;
			}
		}
		#endregion

		#region Operators
		public static bool operator ==(DepthState a, DepthState b)
		{
			return a.Enabled == b.Enabled && a.DepthFunction == b.DepthFunction;
		}

		public static bool operator !=(DepthState a, DepthState b)
		{
			return a.Enabled != b.Enabled || a.DepthFunction != b.DepthFunction;
		}
		#endregion
	}
}