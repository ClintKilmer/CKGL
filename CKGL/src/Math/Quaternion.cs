using System.Runtime.InteropServices;

namespace CKGL
{
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct Quaternion
	{
		public float X;
		public float Y;
		public float Z;
		public float W;

		#region Constructors
		private Quaternion(float x, float y, float z, float w)
		{
			X = x;
			Y = y;
			Z = z;
			W = w;
		}
		private Quaternion(Vector3 axis, float w)
		{
			X = axis.X;
			Y = axis.Y;
			Z = axis.Z;
			W = w;
		}
		#endregion

		#region Static Constructors
		public static readonly Quaternion Identity = new Quaternion(0f, 0f, 0f, 1f);
		#endregion

		#region Properties
		#endregion

		#region Methods
		public Matrix ToMatrix()
		{
			Matrix result = Matrix.Identity;

			float num9 = X * X;
			float num8 = Y * Y;
			float num7 = Z * Z;
			float num6 = X * Y;
			float num5 = Z * W;
			float num4 = Z * X;
			float num3 = Y * W;
			float num2 = Y * Z;
			float num = X * W;
			result.M11 = 1f - (2f * (num8 + num7));
			result.M12 = 2f * (num6 - num5);
			result.M13 = 2f * (num4 + num3);
			result.M14 = 0f;
			result.M21 = 2f * (num6 + num5);
			result.M22 = 1f - (2f * (num7 + num9));
			result.M23 = 2f * (num2 - num);
			result.M24 = 0f;
			result.M31 = 2f * (num4 - num3);
			result.M32 = 2f * (num2 + num);
			result.M33 = 1f - (2f * (num8 + num9));
			result.M34 = 0f;
			result.M41 = 0f;
			result.M42 = 0f;
			result.M43 = 0f;
			result.M44 = 1f;

			return result;
		}

		public Matrix2D ToMatrix2D()
		{
			Matrix2D result = Matrix2D.Identity;

			float num9 = X * X;
			float num8 = Y * Y;
			float num7 = Z * Z;
			float num6 = X * Y;
			float num5 = Z * W;
			float num4 = Z * X;
			float num3 = Y * W;
			float num2 = Y * Z;
			float num = X * W;
			result.M11 = 1f - (2f * (num8 + num7));
			result.M12 = 2f * (num6 - num5);
			result.M21 = 2f * (num6 + num5);
			result.M22 = 1f - (2f * (num7 + num9));
			result.M31 = 2f * (num4 - num3);
			result.M32 = 2f * (num2 + num);
			return result;
		}
		#endregion

		#region Static Methods
		public static Quaternion CreateFromEuler(float x, float y, float z)
		{
			Quaternion result = Identity;

			float halfX = x.RotationsToRadians() * 0.5f;
			float sinX = Math.Sin(halfX);
			float cosX = Math.Cos(halfX);
			float halfY = y.RotationsToRadians() * 0.5f;
			float sinY = Math.Sin(halfY);
			float cosY = Math.Cos(halfY);
			float halfZ = z.RotationsToRadians() * 0.5f;
			float sinZ = Math.Sin(halfZ);
			float cosZ = Math.Cos(halfZ);
			result.X = ((cosY * sinX) * cosZ) + ((sinY * cosX) * sinZ);
			result.Y = ((sinY * cosX) * cosZ) - ((cosY * sinX) * sinZ);
			result.Z = ((cosY * cosX) * sinZ) - ((sinY * sinX) * cosZ);
			result.W = ((cosY * cosX) * cosZ) + ((sinY * sinX) * sinZ);

			return result;
		}
		public static Quaternion CreateFromAxisAngle(Vector3 axis, float rotations)
		{
			Quaternion result = Identity;

			float half = rotations.RotationsToRadians() * 0.5f;
			float sin = Math.Sin(half);
			float cos = Math.Cos(half);
			result.X = axis.X * sin;
			result.Y = axis.Y * sin;
			result.Z = axis.Z * sin;
			result.W = cos;

			return result;
		}

		public static Quaternion CreateRotationX(float rotations)
		{
			return CreateFromAxisAngle(Vector3.Right, rotations);
		}

		public static Quaternion CreateRotationY(float rotations)
		{
			return CreateFromAxisAngle(Vector3.Up, rotations);
		}

		public static Quaternion CreateRotationZ(float rotations)
		{
			return CreateFromAxisAngle(Vector3.Forward, rotations);
		}

		public static Quaternion CreateFromRotationMatrix(Matrix matrix)
		{
			Quaternion result = Identity;

			float sqrt;
			float half;
			float scale = matrix.M11 + matrix.M22 + matrix.M33;

			if (scale > 0.0f)
			{
				sqrt = (float)Math.Sqrt(scale + 1.0f);
				result.W = sqrt * 0.5f;
				sqrt = 0.5f / sqrt;

				result.X = (matrix.M23 - matrix.M32) * sqrt;
				result.Y = (matrix.M31 - matrix.M13) * sqrt;
				result.Z = (matrix.M12 - matrix.M21) * sqrt;
			}
			else if ((matrix.M11 >= matrix.M22) && (matrix.M11 >= matrix.M33))
			{
				sqrt = (float)Math.Sqrt(1.0f + matrix.M11 - matrix.M22 - matrix.M33);
				half = 0.5f / sqrt;

				result.X = 0.5f * sqrt;
				result.Y = (matrix.M12 + matrix.M21) * half;
				result.Z = (matrix.M13 + matrix.M31) * half;
				result.W = (matrix.M23 - matrix.M32) * half;
			}
			else if (matrix.M22 > matrix.M33)
			{
				sqrt = (float)Math.Sqrt(1.0f + matrix.M22 - matrix.M11 - matrix.M33);
				half = 0.5f / sqrt;

				result.X = (matrix.M21 + matrix.M12) * half;
				result.Y = 0.5f * sqrt;
				result.Z = (matrix.M32 + matrix.M23) * half;
				result.W = (matrix.M31 - matrix.M13) * half;
			}
			else
			{
				sqrt = (float)Math.Sqrt(1.0f + matrix.M33 - matrix.M11 - matrix.M22);
				half = 0.5f / sqrt;

				result.X = (matrix.M31 + matrix.M13) * half;
				result.Y = (matrix.M32 + matrix.M23) * half;
				result.Z = 0.5f * sqrt;
				result.W = (matrix.M12 - matrix.M21) * half;
			}

			return result;
		}

		public static Quaternion Inverse(Quaternion quaternion)
		{
			Quaternion result = Identity;

			float num2 = (
				(quaternion.X * quaternion.X) +
				(quaternion.Y * quaternion.Y) +
				(quaternion.Z * quaternion.Z) +
				(quaternion.W * quaternion.W)
			);
			float num = 1f / num2;
			result.X = -quaternion.X * num;
			result.Y = -quaternion.Y * num;
			result.Z = -quaternion.Z * num;
			result.W = quaternion.W * num;

			return result;
		}

		public static Quaternion Lerp(Quaternion a, Quaternion b, float t)
		{
			Quaternion result = Identity;

			float num = t;
			float num2 = 1f - num;
			float num5 = (
				(a.X * b.X) +
				(a.Y * b.Y) +
				(a.Z * b.Z) +
				(a.W * b.W)
			);
			if (num5 >= 0f)
			{
				result.X = (num2 * a.X) + (num * b.X);
				result.Y = (num2 * a.Y) + (num * b.Y);
				result.Z = (num2 * a.Z) + (num * b.Z);
				result.W = (num2 * a.W) + (num * b.W);
			}
			else
			{
				result.X = (num2 * a.X) - (num * b.X);
				result.Y = (num2 * a.Y) - (num * b.Y);
				result.Z = (num2 * a.Z) - (num * b.Z);
				result.W = (num2 * a.W) - (num * b.W);
			}
			float num4 = (
				(result.X * result.X) +
				(result.Y * result.Y) +
				(result.Z * result.Z) +
				(result.W * result.W)
			);
			float num3 = 1f / Math.Sqrt(num4);
			result.X *= num3;
			result.Y *= num3;
			result.Z *= num3;
			result.W *= num3;

			return result;
		}

		public static Quaternion Slerp(Quaternion a, Quaternion b, float t)
		{
			Quaternion result = Identity;

			float num2;
			float num3;
			float num = t;
			float num4 = (
				(a.X * b.X) +
				(a.Y * b.Y) +
				(a.Z * b.Z) +
				(a.W * b.W)
			);
			float flag = 1.0f;
			if (num4 < 0f)
			{
				flag = -1.0f;
				num4 = -num4;
			}
			if (num4 > 0.999999f)
			{
				num3 = 1f - num;
				num2 = num * flag;
			}
			else
			{
				float num5 = Math.Acos(num4);
				float num6 = 1f / Math.Sin(num5);
				num3 = Math.Sin(((1f - num) * num5)) * num6;
				num2 = flag * Math.Sin((num * num5)) * num6;
			}
			result.X = (num3 * a.X) + (num2 * b.X);
			result.Y = (num3 * a.Y) + (num2 * b.Y);
			result.Z = (num3 * a.Z) + (num2 * b.Z);
			result.W = (num3 * a.W) + (num2 * b.W);

			return result;
		}
		#endregion

		#region Overrides
		public override string ToString()
		{
			return $"{X}, {Y}, {Z}, {W}";
		}
		#endregion

		#region Operators
		public static bool operator ==(Quaternion a, Quaternion b)
		{
			return a.X == b.X && a.Y == b.Y && a.Z == b.Z && a.W == b.W;
		}
		public static bool operator !=(Quaternion a, Quaternion b)
		{
			return a.X != b.X || a.Y != b.Y || a.Z != b.Z || a.W != b.W;
		}

		public static Quaternion operator +(Quaternion a, Quaternion b)
		{
			a.X += b.X;
			a.Y += b.Y;
			a.Z += b.Z;
			a.W += b.W;
			return a;
		}

		public static Quaternion operator -(Quaternion a, Quaternion b)
		{
			a.X -= b.X;
			a.Y -= b.Y;
			a.Z -= b.Z;
			a.W -= b.W;
			return a;
		}

		public static Quaternion operator *(Quaternion a, Quaternion b)
		{
			Quaternion result = new Quaternion(
				a.X * b.W + b.X * a.W + (a.Y * b.Z - a.Z * b.Y),
				a.Y * b.W + b.Y * a.W + (a.Z * b.X - a.X * b.Z),
				a.Z * b.W + b.Z * a.W + (a.X * b.Y - a.Y * b.X),
				a.W * b.W - (a.X * b.X + a.Y * b.Y + a.Z * b.Z));
			return result;
		}
		public static Quaternion operator *(Quaternion v, float n)
		{
			v.X *= n;
			v.Y *= n;
			v.Z *= n;
			v.W *= n;
			return v;
		}
		public static Quaternion operator *(float n, Quaternion v)
		{
			v.X *= n;
			v.Y *= n;
			v.Z *= n;
			v.W *= n;
			return v;
		}

		public static Quaternion operator /(Quaternion a, Quaternion b)
		{
			float x = a.X;
			float y = a.Y;
			float z = a.Z;
			float w = a.W;
			float num14 = (
				(b.X * b.X) +
				(b.Y * b.Y) +
				(b.Z * b.Z) +
				(b.W * b.W)
			);
			float num5 = 1f / num14;
			float num4 = -b.X * num5;
			float num3 = -b.Y * num5;
			float num2 = -b.Z * num5;
			float num = b.W * num5;
			float num13 = (y * num2) - (z * num3);
			float num12 = (z * num4) - (x * num2);
			float num11 = (x * num3) - (y * num4);
			float num10 = ((x * num4) + (y * num3)) + (z * num2);
			a.X = ((x * num) + (num4 * w)) + num13;
			a.Y = ((y * num) + (num3 * w)) + num12;
			a.Z = ((z * num) + (num2 * w)) + num11;
			a.W = (w * num) - num10;
			return a;
		}

		public static Quaternion operator /(Quaternion v, float n)
		{
			v.X /= n;
			v.Y /= n;
			v.Z /= n;
			v.W /= n;
			return v;
		}

		public static Quaternion operator -(Quaternion v)
		{
			v.X = -v.X;
			v.Y = -v.Y;
			v.Z = -v.Z;
			v.W = -v.W;
			return v;
		}
		#endregion
	}
}

#region ckgl.js example implementation
// From ckgl.js
/*
// ----- Quaternion ----- //
	var Quaternion = (function() {
		// ----- Constructor ----- //
		function Quaternion(x, y, z, w) {
			this.x = x || 0;
			this.y = y || 0;
			this.z = z || 0;
			this.w = w || 1;
		}
		Quaternion.prototype.toString = function() {
			return "{X: " + this.x + " Y:" + this.y + " Z:" + this.z + " W:" + this.w + "}";
		};

		// ----- Static Constructors ----- //
		Quaternion.identity = function() {
			return new Quaternion(0, 0, 0, 1);
		};

		// ----- Static Methods ----- //
		Quaternion.axisAngle = function(axis, angle) {
			var quaternion = new Quaternion();

			angle = angle * Math.PI * 2;

			var sin = Math.sin(angle / 2);

			quaternion.w = Math.cos(angle / 2);
			quaternion.x = axis.x * sin;
			quaternion.y = axis.y * sin;
			quaternion.z = axis.z * sin;

			return quaternion;
		};

		Quaternion.rotationEuler = function(euler) {
			var yaw = -euler.y * Math.PI * 2;
			var pitch = euler.x * Math.PI * 2;
			var roll = -euler.z * Math.PI * 2;

			var quaternion = new Quaternion();

			var halfRoll = roll * 0.5;
			var halfPitch = pitch * 0.5;
			var halfYaw = yaw * 0.5;

			var sinRoll = Math.sin(halfRoll);
			var cosRoll = Math.cos(halfRoll);
			var sinPitch = Math.sin(halfPitch);
			var cosPitch = Math.cos(halfPitch);
			var sinYaw = Math.sin(halfYaw);
			var cosYaw = Math.cos(halfYaw);

			quaternion.x = (cosYaw * sinPitch * cosRoll) + (sinYaw * cosPitch * sinRoll);
			quaternion.y = (sinYaw * cosPitch * cosRoll) - (cosYaw * sinPitch * sinRoll);
			quaternion.z = (cosYaw * cosPitch * sinRoll) - (sinYaw * sinPitch * cosRoll);
			quaternion.w = (cosYaw * cosPitch * cosRoll) + (sinYaw * sinPitch * sinRoll);

			return quaternion;
		};

		// ----- Operators ----- //
		Quaternion.prototype.multiply = function(q) {
			return this.clone().multiplyInPlace(q);
		};
		Quaternion.prototype.multiplyInPlace = function(q) {
			//this.x = this.x * q.w + this.y * q.z - this.z * q.y + this.w * q.x;
			//this.y = -this.x * q.z + this.y * q.w + this.z * q.x + this.w * q.y;
			//this.z = this.x * q.y - this.y * q.x + this.z * q.w + this.w * q.z;
			//this.w = -this.x * q.x - this.y * q.y - this.z * q.z + this.w * q.w;

			var qax = this.x, qay = this.y, qaz = this.z, qaw = this.w;
			var qbx = q.x, qby = q.y, qbz = q.z, qbw = q.w;

			this.x = qax * qbw + qaw * qbx + qay * qbz - qaz * qby;
			this.y = qay * qbw + qaw * qby + qaz * qbx - qax * qbz;
			this.z = qaz * qbw + qaw * qbz + qax * qby - qay * qbx;
			this.w = qaw * qbw - qax * qbx - qay * qby - qaz * qbz;

			return this;
		};

		Quaternion.prototype.rotateAround = function(axis, angle) {
			return this.multiply(Quaternion.axisAngle(axis, angle));
		};

		Quaternion.prototype.rotateAroundInPlace = function(axis, angle) {
			return this.multiplyInPlace(Quaternion.axisAngle(axis, angle));
		};

		// ----- Public Methods ----- //
		Quaternion.prototype.clone = function(q) {
			return new Quaternion(q.x, q.y, q.z, q.w);
		};

		Quaternion.prototype.toRotationMatrix = function() {
			var matrix = new Matrix();

			var xx = this.x * this.x;
			var yy = this.y * this.y;
			var zz = this.z * this.z;
			var xy = this.x * this.y;
			var zw = this.z * this.w;
			var zx = this.z * this.x;
			var yw = this.y * this.w;
			var yz = this.y * this.z;
			var xw = this.x * this.w;

			matrix.m[0] = 1.0 - (2.0 * (yy + zz));
			matrix.m[1] = 2.0 * (xy + zw);
			matrix.m[2] = 2.0 * (zx - yw);
			matrix.m[3] = 0;
			matrix.m[4] = 2.0 * (xy - zw);
			matrix.m[5] = 1.0 - (2.0 * (zz + xx));
			matrix.m[6] = 2.0 * (yz + xw);
			matrix.m[7] = 0;
			matrix.m[8] = 2.0 * (zx + yw);
			matrix.m[9] = 2.0 * (yz - xw);
			matrix.m[10] = 1.0 - (2.0 * (yy + xx));
			matrix.m[11] = 0;
			matrix.m[12] = 0;
			matrix.m[13] = 0;
			matrix.m[14] = 0;
			matrix.m[15] = 1.0;

			return matrix;
		};

		return Quaternion;
	})();
	window.Quaternion = Quaternion;
// ----- Quaternion ----- //
*/
#endregion