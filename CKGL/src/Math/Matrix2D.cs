// Right-handed, Row-Major, 3x2 Matrix for 2D Transformations

/*
 * Partially Derived from:
 * Matrix2D.cs in the Nez Library: https://github.com/prime31/Nez/
 * Matrix2D.cs in the MonoGame.Extended Library: https://github.com/craftworkgames/MonoGame.Extended
 * Matrix3x2.cs in the Rise Library: https://github.com/ChevyRay/Rise
 */

using System.Runtime.CompilerServices;

namespace CKGL
{
	public struct Matrix2D
	{
		// Column-Major Storage Order
		public float M11; // X Scale		// Z Rotation
		public float M12;                   // Z Rotation	// Z Axis - Y Shear
		public float M21;                   // Z Rotation	// Z Axis - X Shear
		public float M22; // Y Scale		// Z Rotation
		public float M31; // X Translation
		public float M32; // Y Translation

		#region Constructors
		public Matrix2D(float m11, float m12, float m21, float m22, float m31, float m32)
		{
			M11 = m11;
			M12 = m12;

			M21 = m21;
			M22 = m22;

			M31 = m31;
			M32 = m32;
		}
		#endregion

		#region Static Constructors
		public static readonly Matrix2D Identity = new Matrix2D(1f, 0f, 0f, 1f, 0f, 0f);
		#endregion

		#region Properties
		public Vector2 Translation
		{
			get
			{
				return new Vector2(M31, M32);
			}
			set
			{
				M31 = value.X;
				M32 = value.Y;
			}
		}

		public Rotation Rotation
		{
			get { return Rotation.FromRadians(Math.Atan2(M21, M11)); }
			set
			{
				var cos = Math.Cos(value.Radians);
				var sin = Math.Sin(value.Radians);

				M11 = cos;
				M12 = -sin;
				M21 = sin;
				M22 = cos;
			}
		}

		public Vector2 Scale
		{
			get
			{
				return new Vector2(M11, M22);
			}
			set
			{
				M11 = value.X;
				M22 = value.Y;
			}
		}

		public Vector2 Left
		{
			get
			{
				return new Vector2(-M11, -M12);
			}
			set
			{
				M11 = -value.X;
				M12 = -value.Y;
			}
		}

		public Vector2 Right
		{
			get
			{
				return new Vector2(M11, M12);
			}
			set
			{
				M11 = value.X;
				M12 = value.Y;
			}
		}

		public Vector2 Up
		{
			get
			{
				return new Vector2(M21, M22);
			}
			set
			{
				M21 = value.X;
				M22 = value.Y;
			}
		}

		public Vector2 Down
		{
			get
			{
				return new Vector2(-M21, -M22);
			}
			set
			{
				M21 = -value.X;
				M22 = -value.Y;
			}
		}

		public Matrix2D Inverse
		{
			get { return Invert(); }
		}
		#endregion

		#region Methods
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float[] ToArrayRowMajor()
		{
			float[] array = {
				M11, M21, M31,
				M12, M22, M32
			};
			return array;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float[] ToArrayColumnMajor()
		{
			float[] array = {
				M11, M12,
				M21, M22,
				M31, M32
			};
			return array;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float Determinant()
		{
			return M11 * M22 - M12 * M21;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Matrix2D Invert()
		{
			Matrix2D result = Identity;

			var det = 1 / Determinant();

			result.M11 = M22 * det;
			result.M12 = -M12 * det;

			result.M21 = -M21 * det;
			result.M22 = M11 * det;

			result.M31 = (M32 * M21 - M31 * M22) * det;
			result.M32 = -(M32 * M11 - M31 * M12) * det;

			return result;
		}

		public Matrix ToMatrix(float depth = 0)
		{
			Matrix result = Matrix.Identity;

			result.M11 = M11;
			result.M12 = M12;
			result.M13 = 0;
			result.M14 = 0;

			result.M21 = M21;
			result.M22 = M22;
			result.M23 = 0;
			result.M24 = 0;

			result.M31 = 0;
			result.M32 = 0;
			result.M33 = 1;
			result.M34 = 0;

			result.M41 = M31;
			result.M42 = M32;
			result.M43 = depth;
			result.M44 = 1;

			return result;
		}
		#endregion

		#region Static Methods
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Matrix2D CreateTransform(Vector2 origin, Vector2 position, float rotations, Vector2 scale, Vector2 shear)
		{
			Matrix2D transformMatrix = Identity;

			if (origin != Vector2.Zero)
				transformMatrix *= CreateTranslation(-origin);

			if (shear != Vector2.Zero)
				transformMatrix *= CreateShear(shear);

			if (scale != Vector2.One)
				transformMatrix *= CreateScale(scale);

			if (rotations != 0f)
				transformMatrix *= CreateRotationZ(rotations);

			if (position != Vector2.Zero)
			{
				if (origin != Vector2.Zero)
					transformMatrix *= CreateTranslation(position - origin);
				else
					transformMatrix *= CreateTranslation(position);
			}

			if (origin != Vector2.Zero)
				transformMatrix *= CreateTranslation(origin);

			return transformMatrix;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Matrix2D CreateFrom(Vector2 position, float rotations, Vector2? scale = null, Vector2? shear = null, Vector2? origin = null)
		{
			Matrix2D transformMatrix = Identity;

			if (origin.HasValue)
			{
				transformMatrix.M31 = -origin.Value.X;
				transformMatrix.M32 = -origin.Value.Y;
			}

			if (shear.HasValue)
			{
				var shearMatrix = CreateShear(shear.Value);
				transformMatrix = transformMatrix * shearMatrix;
			}

			if (scale.HasValue)
			{
				var scaleMatrix = CreateScale(scale.Value);
				transformMatrix = transformMatrix * scaleMatrix;
			}

			if (rotations != 0f)
			{
				var rotationMatrix = CreateRotationZ(rotations.RotationsToRadians());
				transformMatrix = transformMatrix * rotationMatrix;
			}

			Matrix2D translationMatrix = CreateTranslation(position);
			transformMatrix = transformMatrix * translationMatrix;

			return transformMatrix;
		}

		public static Matrix2D CreateTranslation(Vector2 position)
		{
			return CreateTranslation(position.X, position.Y);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Matrix2D CreateTranslation(float positionX, float positionY)
		{
			Matrix2D result = Identity;

			result.M11 = 1;
			result.M12 = 0;

			result.M21 = 0;
			result.M22 = 1;

			result.M31 = positionX;
			result.M32 = positionY;

			return result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Matrix2D CreateRotationZ(float rotations)
		{
			Matrix2D result = Identity;

			float cos = Math.Cos(rotations.RotationsToRadians());
			float sin = Math.Sin(rotations.RotationsToRadians());

			result.M11 = cos;
			result.M12 = sin;

			result.M21 = -sin; // Handedness Switch
			result.M22 = cos; // Handedness Switch

			result.M31 = 0;
			result.M32 = 0;

			return result;
		}

		public static Matrix2D CreateScale(Vector2 scale)
		{
			return CreateScale(scale.X, scale.Y);
		}
		public static Matrix2D CreateScale(float scale)
		{
			return CreateScale(scale, scale);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Matrix2D CreateScale(float scaleX, float scaleY)
		{
			Matrix2D result = Identity;

			result.M11 = scaleX;
			result.M12 = 0;

			result.M21 = 0;
			result.M22 = scaleY;

			result.M31 = 0;
			result.M32 = 0;

			return result;
		}

		public static Matrix2D CreateShear(Vector2 shear)
		{
			return CreateShear(shear.X, shear.Y);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Matrix2D CreateShear(float shearX, float shearY)
		{
			Matrix2D result = Identity;

			result.M11 = 1;
			result.M12 = shearY;

			result.M21 = shearX;
			result.M22 = 1;

			result.M31 = 0;
			result.M32 = 0;

			return result;
		}
		#endregion

		#region Overrides
		public override string ToString()
		{
			return this == Identity ? "Identity" : $"T:({Translation.X:0.##}, {Translation.Y:0.##}), R:{Rotation:0.##}, S:({Scale.X:0.##},{Scale.Y:0.##})";
			//return $"{{M11:{M11} M12:{M12}}} {{M21:{M21} M22:{M22}}} {{M31:{M31} M32:{M32}}}";
		}

		public override bool Equals(object obj)
		{
			return obj is Matrix2D && Equals((Matrix2D)obj);
		}
		public bool Equals(Matrix2D matrix2D)
		{
			return this == matrix2D;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hash = 17;
				hash = hash * 23 + M11.GetHashCode();
				hash = hash * 23 + M12.GetHashCode();
				hash = hash * 23 + M21.GetHashCode();
				hash = hash * 23 + M22.GetHashCode();
				hash = hash * 23 + M31.GetHashCode();
				hash = hash * 23 + M32.GetHashCode();
				return hash;
			}
		}
		#endregion

		#region Operators
		public static bool operator ==(Matrix2D a, Matrix2D b)
		{
			return
				a.M11 == b.M11 &&
				a.M21 == b.M21 &&
				a.M31 == b.M31 &&
				a.M12 == b.M12 &&
				a.M22 == b.M22 &&
				a.M32 == b.M32
				;
		}

		public static bool operator !=(Matrix2D a, Matrix2D b)
		{
			return
				a.M11 != b.M11 ||
				a.M21 != b.M21 ||
				a.M31 != b.M31 ||
				a.M12 != b.M12 ||
				a.M22 != b.M22 ||
				a.M32 != b.M32
				;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Matrix2D operator +(Matrix2D a, Matrix2D b)
		{
			a.M11 = a.M11 + b.M11;
			a.M12 = a.M12 + b.M12;

			a.M21 = a.M21 + b.M21;
			a.M22 = a.M22 + b.M22;

			a.M31 = a.M31 + b.M31;
			a.M32 = a.M32 + b.M32;

			return a;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Matrix2D operator -(Matrix2D a, Matrix2D b)
		{
			a.M11 = a.M11 - b.M11;
			a.M12 = a.M12 - b.M12;

			a.M21 = a.M21 - b.M21;
			a.M22 = a.M22 - b.M22;

			a.M31 = a.M31 - b.M31;
			a.M32 = a.M32 - b.M32;

			return a;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Matrix2D operator *(Matrix2D a, Matrix2D b)
		{
			var m11 = a.M11 * b.M11 + a.M12 * b.M21;
			var m12 = a.M11 * b.M12 + a.M12 * b.M22;

			var m21 = a.M21 * b.M11 + a.M22 * b.M21;
			var m22 = a.M21 * b.M12 + a.M22 * b.M22;

			var m31 = a.M31 * b.M11 + a.M32 * b.M21 + b.M31;
			var m32 = a.M31 * b.M12 + a.M32 * b.M22 + b.M32;

			a.M11 = m11;
			a.M12 = m12;

			a.M21 = m21;
			a.M22 = m22;

			a.M31 = m31;
			a.M32 = m32;

			return a;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Matrix2D operator *(Matrix2D m, float n)
		{
			m.M11 = m.M11 * n;
			m.M12 = m.M12 * n;

			m.M21 = m.M21 * n;
			m.M22 = m.M22 * n;

			m.M31 = m.M31 * n;
			m.M32 = m.M32 * n;

			return m;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Matrix2D operator /(Matrix2D a, Matrix2D b)
		{
			a.M11 = a.M11 / b.M11;
			a.M12 = a.M12 / b.M12;

			a.M21 = a.M21 / b.M21;
			a.M22 = a.M22 / b.M22;

			a.M31 = a.M31 / b.M31;
			a.M32 = a.M32 / b.M32;

			return a;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Matrix2D operator /(Matrix2D m, float n)
		{
			float inverseScalar = 1f / n;
			m.M11 = m.M11 * inverseScalar;
			m.M12 = m.M12 * inverseScalar;

			m.M21 = m.M21 * inverseScalar;
			m.M22 = m.M22 * inverseScalar;

			m.M31 = m.M31 * inverseScalar;
			m.M32 = m.M32 * inverseScalar;

			return m;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Matrix2D operator -(Matrix2D m)
		{
			m.M11 = -m.M11;
			m.M12 = -m.M12;

			m.M21 = -m.M21;
			m.M22 = -m.M22;

			m.M31 = -m.M31;
			m.M32 = -m.M32;

			return m;
		}
		#endregion

		#region Implicit Convertion Operators
		public static implicit operator Matrix2D(Matrix matrix)
		{
			return new Matrix2D
			(
				matrix.M11, matrix.M12,
				matrix.M21, matrix.M22,
				matrix.M41, matrix.M42
			);
		}
		#endregion
	}
}