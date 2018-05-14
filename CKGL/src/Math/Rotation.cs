using System.Runtime.InteropServices;

namespace CKGL
{
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct Rotation
	{
		private float r;

		#region Constructors
		public Rotation(float value)
		{
			r = value;
		}
		#endregion

		#region Static Constructors
		public static readonly Rotation Zero = 0f;
		public static readonly Rotation Quarter = 0.25f;
		public static readonly Rotation Half = 0.5f;
		public static readonly Rotation ThreeQuarters = 0.75f;
		public static readonly Rotation Third = 0.33333f;
		public static readonly Rotation TwoThirds = 0.66666f;
		#endregion

		#region Properties
		public float R
		{
			get { return r; }
			set
			{
				if (r != value)
				{
					r = value;
					if (r > 1f)
						r = r % 1f;
					else if (r < 0f)
						r = r % -1f + 1f;
				}
			}
		}

		public float Rotations
		{
			get { return r; }
			set
			{
				R = value;
			}
		}

		public float Radians
		{
			get { return r.RotationsToRadians(); }
			set
			{
				R = value.RadiansToRotations();
			}
		}

		public float Degrees
		{
			get { return r.RotationsToDegrees(); }
			set
			{
				R = value.DegreesToRotations();
			}
		}
		#endregion

		#region Methods
		public Rotation Lerp(Rotation r, float t)
		{
			return new Rotation(r.Lerp(r.r, t));
		}
		#endregion

		#region StaticMethods
		public static Rotation FromRadians(float radians)
		{
			return new Rotation(radians.RadiansToRotations());
		}

		public static Rotation FromDegrees(float degrees)
		{
			return new Rotation(degrees.DegreesToRotations());
		}
		#endregion

		#region Overrides
		public override string ToString()
		{
			return $"{r}";
		}
		#endregion

		#region Operators
		public static bool operator ==(Rotation a, Rotation b)
		{
			return a.r == b.r;
		}
		public static bool operator !=(Rotation a, Rotation b)
		{
			return a.r != b.r;
		}

		public static Rotation operator +(Rotation a, Rotation b)
		{
			a.R += b.r;
			return a;
		}

		public static Rotation operator -(Rotation a, Rotation b)
		{
			a.R -= b.r;
			return a;
		}

		public static Rotation operator *(Rotation a, Rotation b)
		{
			a.R *= b.r;
			return a;
		}
		public static Rotation operator *(Rotation r, float n)
		{
			r.R *= n;
			return r;
		}
		public static Rotation operator *(float n, Rotation r)
		{
			r.R *= n;
			return r;
		}

		public static Rotation operator /(Rotation a, Rotation b)
		{
			a.R /= b.r;
			return a;
		}
		public static Rotation operator /(Rotation r, float n)
		{
			r.R /= n;
			return r;
		}

		public static Rotation operator -(Rotation r)
		{
			r.R = -r.r;
			return r;
		}
		#endregion

		#region Implicit Convertion Operators
		public static implicit operator Rotation(float f)
		{
			return new Rotation(f);
		}

		public static implicit operator float(Rotation r)
		{
			return r.R;
		}
		#endregion
	}
}