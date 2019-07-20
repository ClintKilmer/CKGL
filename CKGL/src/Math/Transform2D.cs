namespace CKGL
{
	public class Transform2D
	{
		private Transform2D parent;
		private Transform2D child;
		private Vector2 origin = Vector2.Zero;
		private Vector2 position = Vector2.Zero;
		private Rotation rotation = 0f;
		private Vector2 scale = Vector2.One;
		private Vector2 shear = Vector2.Zero;
		private Matrix2D matrix;
		private bool dirty = true;

		public Vector2 GlobalPosition { get; private set; } = Vector2.Zero;

		public Transform2D Parent
		{
			get { return parent; }
			set
			{
				if (parent != value)
				{
					parent = value;
					parent.child = this;
					SetDirty();
				}
			}
		}

		public Transform2D Child
		{
			get { return child; }
			set
			{
				if (child != value)
				{
					child = value;
					child.parent = this;
					SetDirty();
				}
			}
		}

		public Vector2 Origin
		{
			get { return origin; }
			set
			{
				if (origin != value)
				{
					origin = value;
					SetDirty();
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
					SetDirty();
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
					SetDirty();
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
					SetDirty();
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
					SetDirty();
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
					SetDirty();
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
					SetDirty();
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
					SetDirty();
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
					SetDirty();
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
					SetDirty();
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
					SetDirty();
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
					SetDirty();
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
					SetDirty();
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
					GlobalPosition = position;
					if (parent != null)
					{
						matrix *= parent.Matrix;
						GlobalPosition *= parent.Matrix;
					}
					dirty = false;
				}

				return matrix;
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

		public Transform2D Clone()
		{
			return (Transform2D)MemberwiseClone();
		}

		private void SetDirty()
		{
			dirty = true;
			if (child != null)
				child.SetDirty();
		}
	}
}