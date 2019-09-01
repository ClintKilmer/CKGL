// Right-handed, Row-Major, 3x3 Matrix for representing a Normal Matrix

namespace CKGL
{
	public struct Matrix3x3
	{
		// Column-Major Storage Order
		public float M11; // X Scale		// Y/Z Rotation
		public float M12;                   // Z   Rotation		// X Axis - Y Shear
		public float M13;                   // Y   Rotation		// X Axis - Z Shear
		public float M21;                   // Z   Rotation		// Y Axis - X Shear
		public float M22; // Y Scale		// X/Z Rotation
		public float M23;                   // X   Rotation		// Y Axis - Z Shear
		public float M31;                   // Y   Rotation		// Z Axis - X Shear
		public float M32;                   // X   Rotation		// Z Axis - Y Shear
		public float M33; // Z Scale		// X/Y Rotation

		#region Constructors
		public Matrix3x3(float m11, float m12, float m13,
						 float m21, float m22, float m23,
						 float m31, float m32, float m33)
		{
			M11 = m11; M12 = m12; M13 = m13;
			M21 = m21; M22 = m22; M23 = m23;
			M31 = m31; M32 = m32; M33 = m33;
		}
		#endregion

		#region Static Constructors
		public static readonly Matrix3x3 Identity = new Matrix3x3(1f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 1f);
		#endregion

		#region Properties
		#endregion

		#region Methods
#if !WEBGL
		public float[] ToArrayRowMajor()
		{
			float[] array = {
				M11, M12, M13,
				M21, M22, M23,
				M31, M32, M33
			};
			return array;
		}
		public float[] ToArrayColumnMajor()
		{
			float[] array = {
				M11, M21, M31,
				M12, M22, M32,
				M13, M23, M33
			};
			return array;
		}
#elif WEBGL
		public double[] ToArrayRowMajor()
		{
			double[] array = {
				M11, M12, M13,
				M21, M22, M23,
				M31, M32, M33
			};
			return array;
		}
		public double[] ToArrayColumnMajor()
		{
			double[] array = {
				M11, M21, M31,
				M12, M22, M32,
				M13, M23, M33
			};
			return array;
		}
#endif

		public Matrix ToMatrix()
		{
			return new Matrix(M11, M12, M13, 0,
							  M21, M22, M23, 0,
							  M31, M32, M33, 0,
							  0, 0, 0, 1);
		}

		public Matrix3x3 Transpose()
		{
			return new Matrix3x3(M11, M21, M31,
								 M12, M22, M32,
								 M13, M23, M33);
		}

		public Matrix3x3 Inverse()
		{
			Matrix3x3 result = Identity;

			float d11 = M22 * M33 + M23 * -M32;
			float d12 = M21 * M33 + M23 * -M31;
			float d13 = M21 * M32 + M22 * -M31;

			float determinant = M11 * d11 - M12 * d12 + M13 * d13;

			if (Math.Abs(determinant) == 0f)
				return new Matrix3x3();

			determinant = 1f / determinant;

			float d21 = M12 * M33 + M13 * -M32;
			float d22 = M11 * M33 + M13 * -M31;
			float d23 = M11 * M32 + M12 * -M31;

			float d31 = (M12 * M23) - (M13 * M22);
			float d32 = (M11 * M23) - (M13 * M21);
			float d33 = (M11 * M22) - (M12 * M21);

			result.M11 = d11 * determinant;
			result.M12 = -d21 * determinant;
			result.M13 = d31 * determinant;

			result.M21 = -d12 * determinant;
			result.M22 = d22 * determinant;
			result.M23 = -d32 * determinant;

			result.M31 = d13 * determinant;
			result.M32 = -d23 * determinant;
			result.M33 = d33 * determinant;

			return result;
		}
		#endregion

		#region Static Methods
		#endregion

		#region Overrides
		public override string ToString()
		{
			return $"{{M11:{M11} M12:{M12} M13:{M13}}} {{M21:{M21} M22:{M22} M23:{M23}}} {{M31:{M31} M32:{M32} M33:{M33}}}";
		}

		public override bool Equals(object obj)
		{
			return obj is Matrix3x3 && Equals((Matrix3x3)obj);
		}
		public bool Equals(Matrix3x3 matrix3x3)
		{
			return this == matrix3x3;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hash = 17;
				hash = hash * 23 + M11.GetHashCode();
				hash = hash * 23 + M12.GetHashCode();
				hash = hash * 23 + M13.GetHashCode();
				hash = hash * 23 + M21.GetHashCode();
				hash = hash * 23 + M22.GetHashCode();
				hash = hash * 23 + M23.GetHashCode();
				hash = hash * 23 + M31.GetHashCode();
				hash = hash * 23 + M32.GetHashCode();
				hash = hash * 23 + M33.GetHashCode();
				return hash;
			}
		}
		#endregion

		#region Operators
		public static bool operator ==(Matrix3x3 a, Matrix3x3 b)
		{
			return
				a.M11 == b.M11 &&
				a.M12 == b.M12 &&
				a.M13 == b.M13 &&
				a.M21 == b.M21 &&
				a.M22 == b.M22 &&
				a.M23 == b.M23 &&
				a.M31 == b.M31 &&
				a.M32 == b.M32 &&
				a.M33 == b.M33
				;
		}

		public static bool operator !=(Matrix3x3 a, Matrix3x3 b)
		{
			return
				a.M11 != b.M11 ||
				a.M12 != b.M12 ||
				a.M13 != b.M13 ||
				a.M21 != b.M21 ||
				a.M22 != b.M22 ||
				a.M23 != b.M23 ||
				a.M31 != b.M31 ||
				a.M32 != b.M32 ||
				a.M33 != b.M33
				;
		}

		public static Matrix3x3 operator +(Matrix3x3 a, Matrix3x3 b)
		{
			a.M11 = a.M11 + b.M11;
			a.M12 = a.M12 + b.M12;
			a.M13 = a.M13 + b.M13;
			a.M21 = a.M21 + b.M21;
			a.M22 = a.M22 + b.M22;
			a.M23 = a.M23 + b.M23;
			a.M31 = a.M31 + b.M31;
			a.M32 = a.M32 + b.M32;
			a.M33 = a.M33 + b.M33;
			return a;
		}

		public static Matrix3x3 operator -(Matrix3x3 a, Matrix3x3 b)
		{
			a.M11 = a.M11 - b.M11;
			a.M12 = a.M12 - b.M12;
			a.M13 = a.M13 - b.M13;
			a.M21 = a.M21 - b.M21;
			a.M22 = a.M22 - b.M22;
			a.M23 = a.M23 - b.M23;
			a.M31 = a.M31 - b.M31;
			a.M32 = a.M32 - b.M32;
			a.M33 = a.M33 - b.M33;
			return a;
		}

		// Using matrix multiplication algorithm - see http://en.wikipedia.org/wiki/Matrix_multiplication
		public static Matrix3x3 operator *(Matrix3x3 a, Matrix3x3 b)
		{
			var m11 = (a.M11 * b.M11) + (a.M12 * b.M21) + (a.M13 * b.M31);
			var m12 = (a.M11 * b.M12) + (a.M12 * b.M22) + (a.M13 * b.M32);
			var m13 = (a.M11 * b.M13) + (a.M12 * b.M23) + (a.M13 * b.M33);
			var m21 = (a.M21 * b.M11) + (a.M22 * b.M21) + (a.M23 * b.M31);
			var m22 = (a.M21 * b.M12) + (a.M22 * b.M22) + (a.M23 * b.M32);
			var m23 = (a.M21 * b.M13) + (a.M22 * b.M23) + (a.M23 * b.M33);
			var m31 = (a.M31 * b.M11) + (a.M32 * b.M21) + (a.M33 * b.M31);
			var m32 = (a.M31 * b.M12) + (a.M32 * b.M22) + (a.M33 * b.M32);
			var m33 = (a.M31 * b.M13) + (a.M32 * b.M23) + (a.M33 * b.M33);
			a.M11 = m11;
			a.M12 = m12;
			a.M13 = m13;
			a.M21 = m21;
			a.M22 = m22;
			a.M23 = m23;
			a.M31 = m31;
			a.M32 = m32;
			a.M33 = m33;
			return a;
		}

		public static Matrix3x3 operator *(Matrix3x3 m, float n)
		{
			m.M11 = m.M11 * n;
			m.M12 = m.M12 * n;
			m.M13 = m.M13 * n;
			m.M21 = m.M21 * n;
			m.M22 = m.M22 * n;
			m.M23 = m.M23 * n;
			m.M31 = m.M31 * n;
			m.M32 = m.M32 * n;
			m.M33 = m.M33 * n;
			return m;
		}

		public static Matrix3x3 operator /(Matrix3x3 a, Matrix3x3 b)
		{
			a.M11 = a.M11 / b.M11;
			a.M12 = a.M12 / b.M12;
			a.M13 = a.M13 / b.M13;
			a.M21 = a.M21 / b.M21;
			a.M22 = a.M22 / b.M22;
			a.M23 = a.M23 / b.M23;
			a.M31 = a.M31 / b.M31;
			a.M32 = a.M32 / b.M32;
			a.M33 = a.M33 / b.M33;
			return a;
		}

		public static Matrix3x3 operator /(Matrix3x3 m, float n)
		{
			float inverseScalar = 1f / n;
			m.M11 = m.M11 * inverseScalar;
			m.M12 = m.M12 * inverseScalar;
			m.M13 = m.M13 * inverseScalar;
			m.M21 = m.M21 * inverseScalar;
			m.M22 = m.M22 * inverseScalar;
			m.M23 = m.M23 * inverseScalar;
			m.M31 = m.M31 * inverseScalar;
			m.M32 = m.M32 * inverseScalar;
			m.M33 = m.M33 * inverseScalar;
			return m;
		}

		public static Matrix3x3 operator -(Matrix3x3 m)
		{
			m.M11 = -m.M11;
			m.M12 = -m.M12;
			m.M13 = -m.M13;
			m.M21 = -m.M21;
			m.M22 = -m.M22;
			m.M23 = -m.M23;
			m.M31 = -m.M31;
			m.M32 = -m.M32;
			m.M33 = -m.M33;
			return m;
		}
		#endregion

		#region Implicit Convertion Operators
		public static implicit operator Matrix3x3(Matrix matrix)
		{
			return matrix.ToMatrix3x3();
		}
		#endregion
	}
}