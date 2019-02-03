namespace CKGL
{
	public abstract class Entity : Transform2D
	{
		#region Transform2D - Commented out
		//Transform2D Transform2D = new Transform2D();

		//public Vector2 Position
		//{
		//	get { return Transform2D.Position; }
		//	set { Transform2D.Position = value; }
		//}

		//public float X
		//{
		//	get { return Transform2D.X; }
		//	set { Transform2D.X = value; }
		//}

		//public float Y
		//{
		//	get { return Transform2D.Y; }
		//	set { Transform2D.Y = value; }
		//}

		//public Rotation Rotation
		//{
		//	get { return Transform2D.Rotation; }
		//	set { Transform2D.Rotation = value; }
		//}

		//public Vector2 Scale
		//{
		//	get { return Transform2D.Scale; }
		//	set { Transform2D.Scale = value; }
		//}

		//public float ScaleX
		//{
		//	get { return Transform2D.ScaleX; }
		//	set { Transform2D.ScaleX = value; }
		//}

		//public float ScaleY
		//{
		//	get { return Transform2D.ScaleY; }
		//	set { Transform2D.ScaleY = value; }
		//}

		//public Vector2 Shear
		//{
		//	get { return Transform2D.Shear; }
		//	set { Transform2D.Shear = value; }
		//}

		//public float ShearX
		//{
		//	get { return Transform2D.ShearX; }
		//	set { Transform2D.ShearX = value; }
		//}

		//public float ShearY
		//{
		//	get { return Transform2D.ShearY; }
		//	set { Transform2D.ShearY = value; }
		//} 
		#endregion

		private float depth;
		public float Depth
		{
			get { return depth; }
			set
			{
				if (depth != value)
				{
					depth = value;
					if (Scene != null)
						Scene.Entities.SetUnsorted();
				}
			}
		}

		public bool Active { get; set; } = true;
		public bool Visible { get; set; } = true;

		public Scene Scene { get; private set; }

		public Entity(Vector2? position = null, Vector2? origin = null, Vector2? scale = null, Rotation? rotation = null, float depth = 0f)
		{
			Position = position ?? Vector2.Zero;
			Origin = origin ?? Vector2.Zero;
			Scale = scale ?? Vector2.One;
			Rotation = rotation ?? Rotation.Zero;
			Depth = depth;

			Game.Scene.Entities.Add(this);
		}

		public virtual void SceneBegin()
		{
		}

		public virtual void SceneEnd()
		{
		}

		public virtual void Awake()
		{
		}

		public virtual void Added(Scene scene)
		{
			Scene = scene;
		}
		public virtual void Removed()
		{
			Scene = null;
		}

		public virtual void Update() { }

		public virtual void Draw() { }
	}
}