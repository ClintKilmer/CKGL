using System;
using OpenGL;

namespace CKGL
{
	public struct PolygonModeState
	{
		public readonly PolygonMode FrontPolygonMode;
		public readonly PolygonMode BackPolygonMode;
		public readonly bool FrontAndBack;

		public static Action OnStateChanging;
		public static Action OnStateChanged;
		public static PolygonModeState Default { get; private set; }
		public static PolygonModeState Current { get; private set; }

		#region Static Constructors
		static PolygonModeState()
		{
			Default = FrontFillBackFill;
		}
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
		public static void Set(PolygonModeState polygonModeState)
		{
			if (Current != polygonModeState)
			{
				OnStateChanging?.Invoke();
				if (polygonModeState.FrontAndBack)
				{
					GL.PolygonMode(Face.FrontAndBack, polygonModeState.FrontPolygonMode);
				}
				else
				{
					GL.PolygonMode(Face.Front, polygonModeState.FrontPolygonMode);
					GL.PolygonMode(Face.Back, polygonModeState.BackPolygonMode);
				}
				Current = polygonModeState;
				OnStateChanged?.Invoke();
			}
		}
		public static void Reset() => Set(Default);
		public static void SetDefault(PolygonModeState polygonModeState) => Default = polygonModeState;

		public static void SetFront(PolygonMode polygonMode)
		{
			Set(new PolygonModeState(polygonMode, Current.BackPolygonMode));
		}

		public static void SetBack(PolygonMode polygonMode)
		{
			Set(new PolygonModeState(Current.FrontPolygonMode, polygonMode));
		}
		#endregion

		#region Overrides
		public override string ToString()
		{
			return $"PolygonModeState: [Front Face: {FrontPolygonMode}, Back Face: {BackPolygonMode}]";
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