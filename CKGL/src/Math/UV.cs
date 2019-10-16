using System.Runtime.InteropServices;

namespace CKGL
{
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct UV
	{
		public ushort u;
		public ushort v;

		public float U { get { return u / 65535f; } set { u = (ushort)(value * ushort.MaxValue); } }
		public float V { get { return v / 65535f; } set { v = (ushort)(value * ushort.MaxValue); } }

		#region Constructors
		public UV(float u, float v)
		{
			this.u = (ushort)(u * ushort.MaxValue);
			this.v = (ushort)(v * ushort.MaxValue);
		}
		public UV(float value)
		{
			u = (ushort)(value * ushort.MaxValue);
			v = (ushort)(value * ushort.MaxValue);
		}
		#endregion

		#region Static Constructors
		public static readonly UV Zero = new UV(0f, 0f);
		public static readonly UV One = new UV(1f, 1f);
		public static readonly UV BottomLeft = new UV(0f, 0f);
		public static readonly UV BottomRight = new UV(1f, 0f);
		public static readonly UV TopLeft = new UV(0f, 1f);
		public static readonly UV TopRight = new UV(1f, 1f);
		#endregion

		#region Methods
		public UV Lerp(UV b, float t)
		{
			return new UV(U.Lerp(b.U, t), V.Lerp(b.V, t));
		}

		public UV Eerp(UV b, float t)
		{
			return new UV(U.Eerp(b.U, t), V.Eerp(b.V, t));
		}
		#endregion

		#region Static Methods
		public static UV Lerp(UV a, UV b, float t)
		{
			return new UV(a.U.Lerp(b.U, t), a.V.Lerp(b.V, t));
		}

		public static UV Eerp(UV a, UV b, float t)
		{
			return new UV(a.U.Eerp(b.U, t), a.V.Eerp(b.V, t));
		}
		#endregion

		#region Overrides
		public override string ToString()
		{
			return $"{U}, {V}";
		}

		public override bool Equals(object obj)
		{
			return obj is UV && Equals((UV)obj);
		}
		public bool Equals(UV uv)
		{
			return this == uv;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hash = 17;
				hash = hash * 23 + u.GetHashCode();
				hash = hash * 23 + v.GetHashCode();
				return hash;
			}
		}
		#endregion

		#region Operators
		public static bool operator ==(UV a, UV b)
		{
			return a.U == b.U && a.V == b.V;
		}

		public static bool operator !=(UV a, UV b)
		{
			return a.U != b.U || a.V != b.V;
		}

		public static UV operator +(UV a, UV b)
		{
			a.U += b.U;
			a.V += b.V;
			return a;
		}

		public static UV operator -(UV a, UV b)
		{
			a.U -= b.U;
			a.V -= b.V;
			return a;
		}

		public static UV operator *(UV a, UV b)
		{
			a.U *= b.U;
			a.V *= b.V;
			return a;
		}
		public static UV operator *(UV uv, float n)
		{
			uv.U *= n;
			uv.V *= n;
			return uv;
		}
		public static UV operator *(float n, UV uv)
		{
			uv.U *= n;
			uv.V *= n;
			return uv;
		}

		public static UV operator /(UV a, UV b)
		{
			a.U /= b.U;
			a.V /= b.V;
			return a;
		}
		public static UV operator /(UV uv, float n)
		{
			uv.U /= n;
			uv.V /= n;
			return uv;
		}

		public static UV operator -(UV uv)
		{
			uv.U = -uv.U;
			uv.V = -uv.V;
			return uv;
		}
		#endregion

		#region Implicit Convertion Operators
		public static implicit operator UV(Vector2 v)
		{
			return new UV(v.X, v.Y);
		}
		#endregion
	}
}