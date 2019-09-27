namespace CKGL
{
	public class Transform
	{
		private Transform parent;
		private Transform child;
		private Vector3 origin = Vector3.Zero;
		private Vector3 position = Vector3.Zero;
		private Quaternion rotation = Quaternion.Identity;
		private Vector3 scale = Vector3.One;
		private Shear3D shear = Shear3D.Identity;
		private Matrix matrix;
		private bool dirty = true;

		public Transform Parent
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

		public Transform Child
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

		public Vector3 Origin
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

		public float OriginZ
		{
			get { return origin.Z; }
			set
			{
				if (origin.Z != value)
				{
					origin.Z = value;
					SetDirty();
				}
			}
		}

		public Vector3 Position
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

		public float Z
		{
			get { return position.Z; }
			set
			{
				if (position.Z != value)
				{
					position.Z = value;
					SetDirty();
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
					SetDirty();
				}
			}
		}

		public Vector3 Scale
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

		public float ScaleZ
		{
			get { return scale.Z; }
			set
			{
				if (scale.Z != value)
				{
					scale.Z = value;
					SetDirty();
				}
			}
		}

		public Shear3D Shear
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

		public Matrix Matrix
		{
			get
			{
				if (dirty)
				{
					matrix = Matrix.CreateTransform(origin, position, rotation, scale, shear);

					if (parent != null)
						matrix *= parent.Matrix;

					dirty = false;
				}

				return matrix;
			}
		}

		public Vector3 GlobalPosition => position * Matrix;

		public bool IsAncestorOf(Transform t)
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

		public bool IsDescendantOf(Transform t)
		{
			return t.IsAncestorOf(this);
		}

		public Transform Clone()
		{
			return (Transform)MemberwiseClone();
		}

		private void SetDirty()
		{
			dirty = true;
			if (child != null)
				child.SetDirty();
		}
	}
}