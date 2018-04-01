namespace CKGL
{
	public struct BlendMode
	{
		public static readonly BlendMode Alpha = new BlendMode(BlendFactor.SrcAlpha, BlendFactor.OneMinusSrcAlpha);
		public static readonly BlendMode Premultiplied = new BlendMode(BlendFactor.One, BlendFactor.OneMinusSrcAlpha);
		public static readonly BlendMode Additive = new BlendMode(BlendFactor.SrcAlpha, BlendFactor.One);
		public static readonly BlendMode Multiply = new BlendMode(BlendFactor.DstColour, BlendFactor.OneMinusSrcAlpha);
		public static readonly BlendMode Screen = new BlendMode(BlendFactor.One, BlendFactor.OneMinusSrcColour);

		public BlendFactor Src;
		public BlendFactor Dst;
		public BlendEquation Eq;

		public BlendMode(BlendFactor src, BlendFactor dst, BlendEquation eq)
		{
			Src = src;
			Dst = dst;
			Eq = eq;
		}
		public BlendMode(BlendFactor src, BlendFactor dst)
			: this(src, dst, BlendEquation.Add)
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

		public static bool operator ==(BlendMode a, BlendMode b)
		{
			return a.Src == b.Src && a.Dst == b.Dst;
		}
		public static bool operator !=(BlendMode a, BlendMode b)
		{
			return a.Src != b.Src || a.Dst != b.Dst;
		}
	}
}