namespace CKGL
{
	public struct CullState
	{
		public bool On;
		public CullFace CullFace;

		#region Static Constructors
		public static readonly CullState Default = Off;
		public static readonly CullState Off = new CullState(false, CullFace.Back);
		public static readonly CullState Back = new CullState(true, CullFace.Back);
		public static readonly CullState Front = new CullState(true, CullFace.Front);
		public static readonly CullState FrontAndBack = new CullState(true, CullFace.FrontAndBack);
		#endregion

		#region Constructors
		private CullState(bool on, CullFace cullFace)
		{
			On = on;
			CullFace = cullFace;
		}
		#endregion

		#region Overrides
		public override string ToString()
		{
			return $"{On.ToString()}, {CullFace.ToString()}";
		}
		#endregion

		#region Operators
		public static bool operator ==(CullState a, CullState b)
		{
			return a.On == b.On && a.CullFace == b.CullFace;
		}
		public static bool operator !=(CullState a, CullState b)
		{
			return a.On != b.On || a.CullFace != b.CullFace;
		}
		#endregion
	}
}