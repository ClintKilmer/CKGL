namespace CKGL
{
	public class Transform2D
	{
		private Transform2D parent;
		private Vector2 origin = Vector2.Zero;
		private Vector2 position = Vector2.Zero;
		private Rotation rotation = 0f;
		private Vector2 scale = Vector2.One;
		private Vector2 shear = Vector2.Zero;
		private Matrix2D matrix;
		private bool dirty = true;

		public Transform2D Parent
		{
			get { return parent; }
			set
			{
				if (parent != value)
				{
					parent = value;
					dirty = true;
				}
			}
		}

		public Vector2 Origin
		{
			get { return position; }
			set
			{
				if (position != value)
				{
					position = value;
					dirty = true;
				}
			}
		}

		public float OriginX
		{
			get { return origin.X; }
			set
			{
				if (origin.X != value)
				{
					origin.X = value;
					dirty = true;
				}
			}
		}

		public float OriginY
		{
			get { return origin.Y; }
			set
			{
				if (origin.Y != value)
				{
					origin.Y = value;
					dirty = true;
				}
			}
		}

		public Vector2 Position
		{
			get { return position; }
			set
			{
				if (position != value)
				{
					position = value;
					dirty = true;
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
					dirty = true;
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
					dirty = true;
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
					dirty = true;
				}
			}
		}

		public Vector2 Scale
		{
			get { return scale; }
			set
			{
				if (scale != value)
				{
					scale = value;
					dirty = true;
				}
			}
		}

		public float ScaleX
		{
			get { return scale.X; }
			set
			{
				if (scale.X != value)
				{
					scale.X = value;
					dirty = true;
				}
			}
		}

		public float ScaleY
		{
			get { return scale.Y; }
			set
			{
				if (scale.Y != value)
				{
					scale.Y = value;
					dirty = true;
				}
			}
		}

		public Vector2 Shear
		{
			get { return shear; }
			set
			{
				if (shear != value)
				{
					shear = value;
					dirty = true;
				}
			}
		}

		public float ShearX
		{
			get { return shear.X; }
			set
			{
				if (shear.X != value)
				{
					shear.X = value;
					dirty = true;
				}
			}
		}

		public float ShearY
		{
			get { return shear.Y; }
			set
			{
				if (shear.Y != value)
				{
					shear.Y = value;
					dirty = true;
				}
			}
		}

		public Matrix2D Matrix
		{
			get
			{
				if (dirty)
				{
					matrix = Matrix2D.CreateTransform(origin, position, rotation, scale, shear);
					if (parent != null)
						matrix = matrix * parent.Matrix;
					dirty = false;
				}

				return matrix;
			}
		}

		public Vector2 GlobalPosition
		{
			get
			{
				if (parent != null)
					return position * parent.Matrix;
				return position;
			}
		}

		public bool IsAncestorOf(Transform2D t)
		{
			if (t.parent != null)
			{
				var p = t.parent;
				while (p != null)
				{
					if (p == this)
						return true;
					p = p.parent;
				}
			}
			return false;
		}

		public bool IsDescendantOf(Transform2D t)
		{
			return t.IsAncestorOf(this);
		}
	}
}