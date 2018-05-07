// Right-handed, Row-Major, 3x2 Matrix for 2D Transformations

/*
 * Partially Derived from:
 * Matrix2D.cs in the Nez Library: https://github.com/prime31/Nez/
 * Matrix2D.cs in the MonoGame.Extended Library: https://github.com/craftworkgames/MonoGame.Extended
 * Matrix3x2.cs in the Rise Library: https://github.com/ChevyRay/Rise
 */

using System;
using System.Runtime.CompilerServices;

namespace CKGL
{
	public struct Matrix2D
	{
		// Column-Major Storage Order
		public float M11; // X Scale		// Z Rotation
		public float M12;                   // Z Rotation
		public float M21;                   // Z Rotation
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

		public float Rotation
		{
			get { return Math.Atan2(M21, M11).RadiansToRotations(); }
			set
			{
				var cos = Math.Cos(value.RotationsToRadians());
				var sin = Math.Sin(value.RotationsToRadians());

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
		public static Matrix2D CreateTransform(Vector2 origin, Vector2 position, float rotations, Vector2 scale)
		{
			Matrix2D transformMatrix = Identity;

			if (origin != Vector2.Zero)
				transformMatrix *= CreateTranslation(-origin);

			if (scale != Vector2.One)
				transformMatrix *= CreateScale(scale);

			if (rotations != 0f)
				transformMatrix *= CreateRotationZ(rotations);

			if (position != Vector2.Zero)
				transformMatrix *= CreateTranslation(position);

			if (origin != Vector2.Zero)
				transformMatrix *= CreateTranslation(origin);

			return transformMatrix;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Matrix2D CreateFrom(Vector2 position, float rotations, Vector2? scale = null, Vector2? origin = null)
		{
			Matrix2D transformMatrix = Identity;

			if (origin.HasValue)
			{
				transformMatrix.M31 = -origin.Value.X;
				transformMatrix.M32 = -origin.Value.Y;
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
			result.M12 = -sin;

			result.M21 = sin;
			result.M22 = cos;

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
		#endregion

		#region Overrides
		public override string ToString()
		{
			return this == Identity ? "Identity" : $"T:({Translation.X:0.##}, {Translation.Y:0.##}), R:{Rotation:0.##}, S:({Scale.X:0.##},{Scale.Y:0.##})";
			//return $"{{M11:{M11} M12:{M12}}} {{M21:{M21} M22:{M22}}} {{M31:{M31} M32:{M32}}}";
		}
		#endregion

		#region Operators
		public static bool operator ==(Matrix2D matrix1, Matrix2D matrix2)
		{
			return matrix1.M11 == matrix2.M11 && matrix1.M21 == matrix2.M21 && matrix1.M31 == matrix2.M31 &&
				   matrix1.M12 == matrix2.M12 && matrix1.M22 == matrix2.M22 && matrix1.M32 == matrix2.M32;
		}

		public static bool operator !=(Matrix2D matrix1, Matrix2D matrix2)
		{
			return matrix1.M11 != matrix2.M11 || matrix1.M21 != matrix2.M21 || matrix1.M31 != matrix2.M31 ||
				   matrix1.M12 != matrix2.M12 || matrix1.M22 != matrix2.M22 || matrix1.M32 != matrix2.M32;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Matrix2D operator +(Matrix2D matrix1, Matrix2D matrix2)
		{
			matrix1.M11 = matrix1.M11 + matrix2.M11;
			matrix1.M12 = matrix1.M12 + matrix2.M12;

			matrix1.M21 = matrix1.M21 + matrix2.M21;
			matrix1.M22 = matrix1.M22 + matrix2.M22;

			matrix1.M31 = matrix1.M31 + matrix2.M31;
			matrix1.M32 = matrix1.M32 + matrix2.M32;

			return matrix1;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Matrix2D operator -(Matrix2D matrix1, Matrix2D matrix2)
		{
			matrix1.M11 = matrix1.M11 - matrix2.M11;
			matrix1.M12 = matrix1.M12 - matrix2.M12;

			matrix1.M21 = matrix1.M21 - matrix2.M21;
			matrix1.M22 = matrix1.M22 - matrix2.M22;

			matrix1.M31 = matrix1.M31 - matrix2.M31;
			matrix1.M32 = matrix1.M32 - matrix2.M32;

			return matrix1;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Matrix2D operator *(Matrix2D matrix1, Matrix2D matrix2)
		{
			var m11 = matrix1.M11 * matrix2.M11 + matrix1.M12 * matrix2.M21;
			var m12 = matrix1.M11 * matrix2.M12 + matrix1.M12 * matrix2.M22;

			var m21 = matrix1.M21 * matrix2.M11 + matrix1.M22 * matrix2.M21;
			var m22 = matrix1.M21 * matrix2.M12 + matrix1.M22 * matrix2.M22;

			var m31 = matrix1.M31 * matrix2.M11 + matrix1.M32 * matrix2.M21 + matrix2.M31;
			var m32 = matrix1.M31 * matrix2.M12 + matrix1.M32 * matrix2.M22 + matrix2.M32;

			matrix1.M11 = m11;
			matrix1.M12 = m12;

			matrix1.M21 = m21;
			matrix1.M22 = m22;

			matrix1.M31 = m31;
			matrix1.M32 = m32;

			return matrix1;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Matrix2D operator *(Matrix2D matrix, float scalar)
		{
			matrix.M11 = matrix.M11 * scalar;
			matrix.M12 = matrix.M12 * scalar;

			matrix.M21 = matrix.M21 * scalar;
			matrix.M22 = matrix.M22 * scalar;

			matrix.M31 = matrix.M31 * scalar;
			matrix.M32 = matrix.M32 * scalar;

			return matrix;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Matrix2D operator /(Matrix2D matrix1, Matrix2D matrix2)
		{
			matrix1.M11 = matrix1.M11 / matrix2.M11;
			matrix1.M12 = matrix1.M12 / matrix2.M12;

			matrix1.M21 = matrix1.M21 / matrix2.M21;
			matrix1.M22 = matrix1.M22 / matrix2.M22;

			matrix1.M31 = matrix1.M31 / matrix2.M31;
			matrix1.M32 = matrix1.M32 / matrix2.M32;

			return matrix1;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Matrix2D operator /(Matrix2D matrix, float scalar)
		{
			float inverseScalar = 1f / scalar;
			matrix.M11 = matrix.M11 * inverseScalar;
			matrix.M12 = matrix.M12 * inverseScalar;

			matrix.M21 = matrix.M21 * inverseScalar;
			matrix.M22 = matrix.M22 * inverseScalar;

			matrix.M31 = matrix.M31 * inverseScalar;
			matrix.M32 = matrix.M32 * inverseScalar;

			return matrix;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Matrix2D operator -(Matrix2D matrix)
		{
			matrix.M11 = -matrix.M11;
			matrix.M12 = -matrix.M12;

			matrix.M21 = -matrix.M21;
			matrix.M22 = -matrix.M22;

			matrix.M31 = -matrix.M31;
			matrix.M32 = -matrix.M32;

			return matrix;
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