using System.Runtime.InteropServices;

namespace CKGL
{
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct Shear3D
	{
		public float XY;
		public float XZ;
		public float YX;
		public float YZ;
		public float ZX;
		public float ZY;

		#region Constructors
		public Shear3D(float xy, float xz, float yx, float yz, float zx, float zy)
		{
			XY = xy;
			XZ = xz;
			YX = yx;
			YZ = yz;
			ZX = zx;
			ZY = zy;
		}
		#endregion

		#region Static Constructors
		public static readonly Shear3D Identity = new Shear3D(0f, 0f, 0f, 0f, 0f, 0f);
		#endregion

		#region Overrides
		public override string ToString()
		{
			return $"XY: {XY}, XZ: {XZ}, YX: {YX}, YZ: {YZ}, ZX: {ZX}, ZY: {ZY}";
		}
		#endregion

		#region Operators
		public static bool operator ==(Shear3D a, Shear3D b)
		{
			return a.XY == b.XY && a.XZ == b.XZ && a.YX == b.YX && a.YZ == b.YZ && a.ZX == b.ZX && a.ZY == b.ZY;
		}
		public static bool operator !=(Shear3D a, Shear3D b)
		{
			return a.XY != b.XY || a.XZ != b.XZ || a.YX != b.YX || a.YZ != b.YZ || a.ZX != b.ZX || a.ZY != b.ZY;
		}

		public static Shear3D operator +(Shear3D a, Shear3D b)
		{
			a.XY += b.XY;
			a.XZ += b.XZ;
			a.YX += b.YX;
			a.YZ += b.YZ;
			a.ZX += b.ZX;
			a.ZY += b.ZY;
			return a;
		}

		public static Shear3D operator -(Shear3D a, Shear3D b)
		{
			a.XY -= b.XY;
			a.XZ -= b.XZ;
			a.YX -= b.YX;
			a.YZ -= b.YZ;
			a.ZX -= b.ZX;
			a.ZY -= b.ZY;
			return a;
		}

		public static Shear3D operator *(Shear3D a, Shear3D b)
		{
			a.XY *= b.XY;
			a.XZ *= b.XZ;
			a.YX *= b.YX;
			a.YZ *= b.YZ;
			a.ZX *= b.ZX;
			a.ZY *= b.ZY;
			return a;
		}
		public static Shear3D operator *(Shear3D s, float n)
		{
			s.XY *= n;
			s.XZ *= n;
			s.YX *= n;
			s.YZ *= n;
			s.ZX *= n;
			s.ZY *= n;
			return s;
		}
		public static Shear3D operator *(float n, Shear3D s)
		{
			s.XY *= n;
			s.XZ *= n;
			s.YX *= n;
			s.YZ *= n;
			s.ZX *= n;
			s.ZY *= n;
			return s;
		}

		public static Shear3D operator /(Shear3D a, Shear3D b)
		{
			a.XY /= b.XY;
			a.XZ /= b.XZ;
			a.YX /= b.YX;
			a.YZ /= b.YZ;
			a.ZX /= b.ZX;
			a.ZY /= b.ZY;
			return a;
		}
		public static Shear3D operator /(Shear3D s, float n)
		{
			s.XY /= n;
			s.XZ /= n;
			s.YX /= n;
			s.YZ /= n;
			s.ZX /= n;
			s.ZY /= n;
			return s;
		}

		public static Shear3D operator -(Shear3D s)
		{
			s.XY = -s.XY;
			s.XZ = -s.XZ;
			s.YX = -s.YX;
			s.YZ = -s.YZ;
			s.ZX = -s.ZX;
			s.ZY = -s.ZY;
			return s;
		}
		#endregion
	}
}