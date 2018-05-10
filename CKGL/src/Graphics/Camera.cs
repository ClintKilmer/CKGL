namespace CKGL
{
	public class Camera
	{
		private Vector3 position = Vector3.Zero;
		private float scale = 1f;
		private Vector3 rotation = Vector3.Zero;
		private bool viewDirty = true;
		private Matrix viewMatrix;

		private float fov = 75f;
		public float AspectRatio = 1f;
		public float zNearClip = 0.01f;
		public float zFarClip = 1000f;
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

		public float Scale
		{
			get { return scale; }
			set
			{
				if (scale != value)
				{
					scale = value;
					viewDirty = true;
				}
			}
		}

		public Vector3 Rotation
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

		public float RotationX
		{
			get { return rotation.X; }
			set
			{
				if (rotation.X != value)
				{
					rotation.X = value;
					viewDirty = true;
				}
			}
		}

		public float RotationY
		{
			get { return rotation.Y; }
			set
			{
				if (rotation.Y != value)
				{
					rotation.Y = value;
					viewDirty = true;
				}
			}
		}

		public float RotationZ
		{
			get { return rotation.Z; }
			set
			{
				if (rotation.Z != value)
				{
					rotation.Z = value;
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
					viewMatrix = Matrix.CreateLookAt(position, position + Vector3.Forward * (Matrix.CreateRotationX(rotation.X) * Matrix.CreateRotationY(rotation.Y)), Vector3.Up);
					viewDirty = false;
				}

				return viewMatrix;
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
					projectionMatrix = Matrix.CreatePerspectiveFieldOfView(Math.DegreesToRadians(FoV), AspectRatio, zNearClip, zFarClip);
					projectionDirty = false;
				}

				return projectionMatrix;
			}
		}

		public Matrix CombinedMatrix => ViewMatrix * ProjectionMatrix;
	}
}