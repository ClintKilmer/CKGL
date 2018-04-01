namespace CKGL
{
	public struct BlendState
	{
		public static readonly BlendState None = new BlendState(false, BlendFactor.SrcAlpha, BlendFactor.OneMinusSrcAlpha);
		public static readonly BlendState Alpha = new BlendState(true, BlendFactor.SrcAlpha, BlendFactor.OneMinusSrcAlpha);
		public static readonly BlendState Premultiplied = new BlendState(true, BlendFactor.One, BlendFactor.OneMinusSrcAlpha);
		public static readonly BlendState Additive = new BlendState(true, BlendFactor.SrcAlpha, BlendFactor.One);
		public static readonly BlendState Multiply = new BlendState(true, BlendFactor.DstColour, BlendFactor.OneMinusSrcAlpha);
		public static readonly BlendState Screen = new BlendState(true, BlendFactor.One, BlendFactor.OneMinusSrcColour);

		public bool On;
		public BlendFactor Src;
		public BlendFactor Dst;
		public BlendEquation Eq;

		public BlendState(bool on, BlendFactor src, BlendFactor dst, BlendEquation eq)
		{
			On = on;
			Src = src;
			Dst = dst;
			Eq = eq;
		}
		public BlendState(bool on, BlendFactor src, BlendFactor dst)
			: this(on, src, dst, BlendEquation.Add)
		{

		}

		//public override bool Equals(object obj)
		//{
		//	return obj is BlendMode && Equals((BlendMode)obj);
		//}
		//public bool Equals(ref BlendMode other)
		//{
		//	return Src == other.Src && Dst == other.Dst;
		//}
		//public bool Equals(BlendMode other)
		//{
		//	return Equals(ref other);
		//}

		//public override int GetHashCode()
		//{
		//	unchecked
		//	{
		//		int hash = 17;
		//		hash = hash * 23 + Src.GetHashCode();
		//		hash = hash * 23 + Dst.GetHashCode();
		//		hash = hash * 23 + Eq.GetHashCode();
		//		return hash;
		//	}
		//}

		public override string ToString()
		{
			return $"{Eq.ToString()}, {Src.ToString()}, {Dst.ToString()}";
		}

		public static bool operator ==(BlendState a, BlendState b)
		{
			return a.On == b.On && a.Src == b.Src && a.Dst == b.Dst && a.Eq == b.Eq;
		}
		public static bool operator !=(BlendState a, BlendState b)
		{
			return a.On != b.On || a.Src != b.Src || a.Dst != b.Dst || a.Eq != b.Eq;
		}
	}
}