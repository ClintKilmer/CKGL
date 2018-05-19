using System;
using OpenGL;

namespace CKGL
{
	public struct FrontFaceState
	{
		public readonly FrontFace FrontFace;

		public static Action OnStateChanging;
		public static Action OnStateChanged;
		public static FrontFaceState Default { get; private set; }
		public static FrontFaceState Current { get; private set; }

		#region Static Constructors
		static FrontFaceState()
		{
			Default = CounterClockwise;
		}
		public static readonly FrontFaceState Clockwise = new FrontFaceState(FrontFace.Clockwise);
		public static readonly FrontFaceState CounterClockwise = new FrontFaceState(FrontFace.CounterClockwise);
		#endregion

		#region Constructors
		private FrontFaceState(FrontFace frontFace)
		{
			FrontFace = frontFace;
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
		public static void Set(FrontFaceState frontFaceState)
		{
			if (Current != frontFaceState)
			{
				OnStateChanging?.Invoke();
				GL.FrontFace(frontFaceState.FrontFace);
				Current = frontFaceState;
				OnStateChanged?.Invoke();
			}
		}
		public static void Reset() => Set(Default);
		public static void SetDefault(FrontFaceState frontFaceState) => Default = frontFaceState;
		#endregion

		#region Overrides
		public override string ToString()
		{
			return $"FrontFaceState: [FrontFace: {FrontFace}]";
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