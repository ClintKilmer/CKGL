namespace CKGL
{
	public struct DepthMaskState
	{
		public readonly bool Depth;

		public static DepthMaskState Default { get; private set; }
		public static DepthMaskState Current { get; private set; }

		#region Static Constructors
		static DepthMaskState()
		{
			Default = Enabled;
		}
		public static readonly DepthMaskState Enabled = new DepthMaskState(true);
		public static readonly DepthMaskState Disabled = new DepthMaskState(false);
		#endregion

		#region Constructors
		private DepthMaskState(bool depth)
		{
			Depth = depth;
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
		public static void Set(DepthMaskState DepthMaskState)
		{
			if (Current != DepthMaskState)
			{
				Graphics.State.OnStateChanging?.Invoke();
				Graphics.SetDepthMask(DepthMaskState);
				Current = DepthMaskState;
				Graphics.State.OnStateChanged?.Invoke();
			}
		}
		public static void Reset() => Set(Default);
		public static void SetDefault(DepthMaskState depthMaskState) => Default = depthMaskState;
		#endregion

		#region Overrides
		public override string ToString()
		{
			return $"DepthMaskState: [Depth: {Depth}]";
		}
		#endregion

		#region Operators
		public static bool operator ==(DepthMaskState a, DepthMaskState b)
		{
			return a.Depth == b.Depth;
		}

		public static bool operator !=(DepthMaskState a, DepthMaskState b)
		{
			return a.Depth != b.Depth;
		}
		#endregion
	}
}