namespace CKGL
{
	public class Camera2D
	{
		private Vector2 position = Vector2.Zero;
		private Rotation rotation = 0f;
		private float scale = 1f;
		private bool viewDirty = true;
		private Matrix viewMatrix;

		private int width = 100;
		private int height = 100;
		private float znear = -10000f;
		private float zfar = 10000f;
		private bool projectionDirty = true;
		private Matrix projectionMatrix;

		public Vector2 Position
		{
			get { return position; }
			set
			{
				if (position != value)
				{
					position = value;
					viewDirty = true;
				}
			}
		}

		public float X
		{
			get { return position.X; }
			set
			{
				if (position.X != value)
				{
					position.X = value;
					viewDirty = true;
				}
			}
		}

		public float Y
		{
			get { return position.Y; }
			set
			{
				if (position.Y != value)
				{
					position.Y = value;
					viewDirty = true;
				}
			}
		}

		public Rotation Rotation
		{
			get { return rotation; }
			set
			{
				if (rotation != value)
				{
					rotation = value;
					viewDirty = true;
				}
			}
		}

		public float Scale
		{
			get { return scale; }
			set
			{
				if (scale != value)
				{
					scale = Math.Max(value, 0f);
					viewDirty = true;
				}
			}
		}

		public Matrix ViewMatrix
		{
			get
			{
				if (viewDirty)
				{
					// Flip Right Handedness to Left Handedness
					// Unity - Camera.worldToCameraMatrix
					// https://docs.unity3d.com/ScriptReference/Camera-worldToCameraMatrix.html
					// https://forum.unity.com/threads/reproducing-cameras-worldtocameramatrix.365645/
					viewMatrix = Matrix2D.CreateScale(scale) * Matrix2D.CreateRotationZ(rotation) * Matrix2D.CreateTranslation(position);
					viewMatrix = viewMatrix.Invert();
					viewMatrix.M13 *= -1f;
					viewMatrix.M23 *= -1f;
					viewMatrix.M33 *= -1f;
					viewMatrix.M43 *= -1f;
					viewDirty = false;
				}

				return viewMatrix;
			}
		}

		public int Width
		{
			get { return width; }
			set
			{
				if (width != value)
				{
					width = Math.Max(value, 0);
					projectionDirty = true;
				}
			}
		}

		public int Height
		{
			get { return height; }
			set
			{
				if (height != value)
				{
					height = Math.Max(value, 0);
					projectionDirty = true;
				}
			}
		}

		public float zNear
		{
			get { return znear; }
			set
			{
				if (znear != value)
				{
					znear = Math.Max(value, 0f);
					projectionDirty = true;
				}
			}
		}

		public float zFar
		{
			get { return zfar; }
			set
			{
				if (zfar != value)
				{
					zfar = Math.Max(value, 0f);
					projectionDirty = true;
				}
			}
		}

		public Matrix ProjectionMatrix
		{
			get
			{
				if (projectionDirty)
				{
					projectionMatrix = Matrix.CreateOrthographicOffCenter(0, width, 0, height, znear, zfar);
					projectionDirty = false;
				}

				return projectionMatrix;
			}
		}

		public Matrix Matrix => ViewMatrix * ProjectionMatrix;
	}
}