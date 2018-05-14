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
		private float zNearClip = -10000f;
		private float zFarClip = 10000f;
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
					viewMatrix = Matrix2D.CreateScale(scale) * Matrix2D.CreateTranslation(-position) * Matrix2D.CreateRotationZ(-rotation);
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

		public float ZNearClip
		{
			get { return zNearClip; }
			set
			{
				if (zNearClip != value)
				{
					zNearClip = Math.Max(value, 0f);
					projectionDirty = true;
				}
			}
		}

		public float ZFarClip
		{
			get { return zFarClip; }
			set
			{
				if (zFarClip != value)
				{
					zFarClip = Math.Max(value, 0f);
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
					//projectionMatrix = Matrix.CreateOrthographic(width, height, zNearClip, zFarClip);
					projectionMatrix = Matrix.CreateOrthographicOffCenter(0, width, 0, height, zNearClip, zFarClip);
					projectionDirty = false;
				}

				return projectionMatrix;
			}
		}

		public Matrix Matrix => ViewMatrix * ProjectionMatrix;
	}
}