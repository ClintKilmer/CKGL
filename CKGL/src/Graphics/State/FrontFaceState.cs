using OpenGL;

namespace CKGL
{
	public struct FrontFaceState
	{
		public readonly FrontFace FrontFace;

		#region Static Constructors
		public static readonly FrontFaceState Default = CounterClockwise;
		public static readonly FrontFaceState Clockwise = new FrontFaceState(FrontFace.Clockwise);
		public static readonly FrontFaceState CounterClockwise = new FrontFaceState(FrontFace.CounterClockwise);
		#endregion

		#region Constructors
		private FrontFaceState(FrontFace frontFace)
		{
			FrontFace = frontFace;
		}
		#endregion

		#region Overrides
		public override string ToString()
		{
			return $"FrontFaceState: [FrontFace: {FrontFace.ToString()}]";
		}
		#endregion

		#region Operators
		public static bool operator ==(FrontFaceState a, FrontFaceState b)
		{
			return a.FrontFace == b.FrontFace;
		}
		public static bool operator !=(FrontFaceState a, FrontFaceState b)
		{
			return a.FrontFace != b.FrontFace;
		}
		#endregion
	}
}