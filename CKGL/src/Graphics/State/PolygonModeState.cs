using System;

namespace CKGL
{
	#region Enums
	public enum PolygonMode : byte
	{
		Fill,
		Line,
		Point
	}

	internal static class PolygonModeExt
	{
		internal static OpenGLBindings.PolygonMode ToOpenGL(this PolygonMode frontFace)
		{
			switch (frontFace)
			{
				case PolygonMode.Fill:
					return OpenGLBindings.PolygonMode.Fill;
				case PolygonMode.Line:
					return OpenGLBindings.PolygonMode.Line;
				case PolygonMode.Point:
					return OpenGLBindings.PolygonMode.Point;
				default:
					throw new NotImplementedException();
			}
		}
	}
	#endregion

	public struct PolygonModeState
	{
		public readonly PolygonMode PolygonMode;

		public static PolygonModeState Default { get; private set; }
		public static PolygonModeState Current { get; private set; }

		#region Static Constructors
		static PolygonModeState()
		{
			Default = Fill;
		}
		public static readonly PolygonModeState Fill = new PolygonModeState(PolygonMode.Fill);
		public static readonly PolygonModeState Line = new PolygonModeState(PolygonMode.Line);
		public static readonly PolygonModeState Point = new PolygonModeState(PolygonMode.Point);
		#endregion

		#region Constructors
		private PolygonModeState(PolygonMode polygonMode)
		{
			PolygonMode = polygonMode;
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
				Graphics.State.OnStateChanging?.Invoke();
				Graphics.SetPolygonMode(polygonModeState);
				Current = polygonModeState;
				Graphics.State.OnStateChanged?.Invoke();
			}
		}
		public static void Reset() => Set(Default);
		public static void SetDefault(PolygonModeState polygonModeState) => Default = polygonModeState;
		#endregion

		#region Overrides
		public override string ToString()
		{
			return $"PolygonModeState: [PolygonMode: {PolygonMode}]";
		}

		public override bool Equals(object obj)
		{
			return obj is PolygonModeState && Equals((PolygonModeState)obj);
		}
		public bool Equals(PolygonModeState polygonModeState)
		{
			return this == polygonModeState;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hash = 17;
				hash = hash * 23 + PolygonMode.GetHashCode();
				return hash;
			}
		}
		#endregion

		#region Operators
		public static bool operator ==(PolygonModeState a, PolygonModeState b)
		{
			return a.PolygonMode == b.PolygonMode;
		}

		public static bool operator !=(PolygonModeState a, PolygonModeState b)
		{
			return a.PolygonMode != b.PolygonMode;
		}
		#endregion
	}
}