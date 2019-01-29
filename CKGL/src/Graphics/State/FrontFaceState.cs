using System;

namespace CKGL
{
	#region Enums
	public enum FrontFace : byte
	{
		Clockwise,
		CounterClockwise
	}

	internal static class FrontFaceExt
	{
		internal static OpenGLBindings.FrontFace ToOpenGL(this FrontFace frontFace)
		{
			switch (frontFace)
			{
				case FrontFace.Clockwise:
					return OpenGLBindings.FrontFace.Clockwise;
				case FrontFace.CounterClockwise:
					return OpenGLBindings.FrontFace.CounterClockwise;
				default:
					throw new NotImplementedException();
			}
		}
	}
	#endregion

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
				Graphics.SetFrontFace(frontFaceState.FrontFace);
				Current = frontFaceState;
				OnStateChanged?.Invoke();
			}
		}
		public static void Reset() => Set(Default);
		public static void SetDefault(FrontFaceState frontFaceState) => Default = frontFaceState;

		public static void Set(FrontFace frontFace)
		{
			Set(frontFace);
		}
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