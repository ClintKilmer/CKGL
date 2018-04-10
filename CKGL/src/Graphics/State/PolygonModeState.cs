using OpenGL;

namespace CKGL
{
	public struct PolygonModeState
	{
		public readonly PolygonMode FrontPolygonMode;
		public readonly PolygonMode BackPolygonMode;
		public readonly bool FrontAndBack;

		#region Static Constructors
		public static readonly PolygonModeState Default = FrontFillBackFill;
		public static readonly PolygonModeState FrontPointBackPoint = new PolygonModeState(PolygonMode.Point, PolygonMode.Point);
		public static readonly PolygonModeState FrontPointBackLine = new PolygonModeState(PolygonMode.Point, PolygonMode.Line);
		public static readonly PolygonModeState FrontPointBackFill = new PolygonModeState(PolygonMode.Point, PolygonMode.Fill);
		public static readonly PolygonModeState FrontLineBackPoint = new PolygonModeState(PolygonMode.Line, PolygonMode.Point);
		public static readonly PolygonModeState FrontLineBackLine = new PolygonModeState(PolygonMode.Line, PolygonMode.Line);
		public static readonly PolygonModeState FrontLineBackFill = new PolygonModeState(PolygonMode.Line, PolygonMode.Fill);
		public static readonly PolygonModeState FrontFillBackPoint = new PolygonModeState(PolygonMode.Fill, PolygonMode.Point);
		public static readonly PolygonModeState FrontFillBackLine = new PolygonModeState(PolygonMode.Fill, PolygonMode.Line);
		public static readonly PolygonModeState FrontFillBackFill = new PolygonModeState(PolygonMode.Fill, PolygonMode.Fill);
		#endregion

		#region Constructors
		private PolygonModeState(PolygonMode frontPolygonMode, PolygonMode backPolygonMode)
		{
			FrontPolygonMode = frontPolygonMode;
			BackPolygonMode = backPolygonMode;
			FrontAndBack = FrontPolygonMode == BackPolygonMode;
		}
		#endregion

		#region Overrides
		public override string ToString()
		{
			return $"PolygonModeState: [Front Face: {FrontPolygonMode.ToString()}, Back Face: {BackPolygonMode.ToString()}]";
		}
		#endregion

		#region Operators
		public static bool operator ==(PolygonModeState a, PolygonModeState b)
		{
			return a.FrontPolygonMode == b.FrontPolygonMode && a.BackPolygonMode == b.BackPolygonMode;
		}
		public static bool operator !=(PolygonModeState a, PolygonModeState b)
		{
			return a.FrontPolygonMode != b.FrontPolygonMode || a.BackPolygonMode != b.BackPolygonMode;
		}
		#endregion
	}
}