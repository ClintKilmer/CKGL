using OpenGL;

namespace CKGL
{
	public struct CullState
	{
		public readonly bool On;
		public readonly Face Face;

		public static CullState Default { get { return Off; } }

		#region Static Constructors
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