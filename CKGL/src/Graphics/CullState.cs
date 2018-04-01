namespace CKGL
{
	public struct CullState
	{
		public static readonly CullState Off = new CullState(false, CullFace.Back);
		public static readonly CullState Back = new CullState(true, CullFace.Back);
		public static readonly CullState Front = new CullState(true, CullFace.Front);
		public static readonly CullState FrontAndBack = new CullState(true, CullFace.FrontAndBack);

		public bool On;
		public CullFace CullFace;

		private CullState(bool on, CullFace cullFace)
		{
			On = on;
			CullFace = cullFace;
		}

		public override string ToString()
		{
			return $"{On.ToString()}, {CullFace.ToString()}";
		}

		public static bool operator ==(CullState a, CullState b)
		{
			return a.On == b.On && a.CullFace == b.CullFace;
		}
		public static bool operator !=(CullState a, CullState b)
		{
			return a.On != b.On || a.CullFace != b.CullFace;
		}
	}
}