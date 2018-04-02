using System;
using System.Runtime.CompilerServices;

namespace CKGL
{
	class Matrix3D
	{
		public float M11;
		public float M12;
		public float M13;
		public float M14;
		public float M21;
		public float M22;
		public float M23;
		public float M24;
		public float M31;
		public float M32;
		public float M33;
		public float M34;
		public float M41;
		public float M42;
		public float M43;
		public float M44;

		#region Constructors
		public Matrix3D(float m11, float m12, float m13, float m14,
						float m21, float m22, float m23, float m24,
						float m31, float m32, float m33, float m34,
						float m41, float m42, float m43, float m44)
		{
			M11 = m11; M12 = m12; M13 = m13; M14 = m14;
			M21 = m21; M22 = m22; M23 = m23; M24 = m24;
			M31 = m31; M32 = m32; M33 = m33; M34 = m34;
			M41 = m41; M42 = m42; M43 = m43; M44 = m44;
		}
		#endregion

		#region Static Constructors
		public static readonly Matrix3D Identity = new Matrix3D(1f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 1f);
		#endregion

		#region Properties
		public Vector3 Position
		{
			get { return new Vector3(M41, M42, M43); }
		}

		public Vector3 Right
		{
			get { return new Vector3(M11, M12, M13); }
		}

		public Vector3 Left
		{
			get { return new Vector3(-M11, -M12, -M13); }
		}

		public Vector3 Up
		{
			get { return new Vector3(M21, M22, M23); }
		}

		public Vector3 Down
		{
			get { return new Vector3(-M21, -M22, -M23); }
		}

		public Vector3 Forward
		{
			get { return new Vector3(-M31, -M32, -M33); }
		}

		public Vector3 Backward
		{
			get { return new Vector3(M31, M32, M33); }
		}

		// TODO
		//public Matrix3D Inverse
		//{
		//	get { return Invert(); }
		//}
		#endregion

		// TODO

		#region Methods
		#endregion

		#region Static Methods
		#endregion

		#region Overrides
		#endregion

		#region Operators
		#endregion

		#region Implicit Convertion Operators
		public static implicit operator Matrix3D(Matrix2D matrix2D)
		{
			return matrix2D.ToMatrix3D();
		}
		#endregion
	}
}