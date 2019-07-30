namespace CKGL
{
	public enum Projection
	{
		Orthographic,
		Perspective
	}

	public class Camera
	{
		private Vector3 position = Vector3.Zero;
		private Quaternion rotation = Quaternion.Identity;
		private bool viewDirty = true;
		private Matrix viewMatrix;

		private Projection projection = Projection.Perspective;
		// Orthographic
		private int width = 100;
		private int height = 100;
		// Perspective
		private float fov = 75f;
		private float aspectRatio = 1f;
		// Both
		private float znear = 0.01f;
		private float zfar = 1000f;
		private bool projectionDirty = true;
		private Matrix projectionMatrix;

		public Vector3 Position
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

		public float Z
		{
			get { return position.Z; }
			set
			{
				if (position.Z != value)
				{
					position.Z = value;
					viewDirty = true;
				}
			}
		}

		public Quaternion Rotation
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
					viewMatrix = rotation.Matrix * Matrix.CreateTranslation(position);
					viewMatrix = viewMatrix.Inverse();
					viewMatrix.M13 *= -1f;
					viewMatrix.M23 *= -1f;
					viewMatrix.M33 *= -1f;
					viewMatrix.M43 *= -1f;
					viewDirty = false;
				}

				return viewMatrix;
			}
		}

		public Projection Projection
		{
			get { return projection; }
			set
			{
				if (projection != value)
				{
					projection = value;
					projectionDirty = true;
				}
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
					projectionDirty = projection == Projection.Orthographic;
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
					projectionDirty = projection == Projection.Orthographic;
				}
			}
		}

		public float FoV
		{
			get { return fov; }
			set
			{
				if (fov != value)
				{
					fov = Math.Clamp(value, 1f, 179f);
					projectionDirty = projection == Projection.Perspective;
				}
			}
		}

		public float AspectRatio
		{
			get { return aspectRatio; }
			set
			{
				if (aspectRatio != value)
				{
					aspectRatio = value;
					projectionDirty = projection == Projection.Perspective;
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
					if (projection == Projection.Orthographic)
						projectionMatrix = Matrix.CreateOrthographic(width, height, znear, zfar);
					else if (projection == Projection.Perspective)
						projectionMatrix = Matrix.CreatePerspectiveFieldOfView(Math.DegreesToRadians(FoV), aspectRatio, znear, zfar);
					projectionDirty = false;
				}

				return projectionMatrix;
			}
		}

		public Matrix Matrix => ViewMatrix * ProjectionMatrix;
	}
}