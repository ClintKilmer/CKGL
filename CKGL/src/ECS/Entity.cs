namespace CKGL
{
	public abstract class Entity
	{
		public float X
		{
			get { return Position.X; }
			set { Position.X = value; }
		}

		public float Y
		{
			get { return Position.Y; }
			set { Position.Y = value; }
		}

		public Vector2 Position = Vector2.Zero;
		private float _rotation = 0f;
		public float Rotation
		{
			get { return _rotation; }
			set
			{
				_rotation = value;
				if (_rotation > 1f)
					_rotation = _rotation % 1f;
				else if (_rotation < 0f)
					_rotation = _rotation % -1f + 1f;
			}
		}
		public Vector2 Scale { get; set; } = Vector2.One;

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

		public Vector2 Origin { get; set; } = Vector2.Zero;

		public bool Active { get; set; } = true;
		public bool Visible { get; set; } = true;

		public Scene Scene { get; private set; }

		public Entity(Vector2? position = null, Vector2? origin = null, Vector2? scale = null, float rotation = 0f, float depth = 0f)
		{
			Position = position ?? Vector2.Zero;
			Origin = origin ?? Vector2.Zero;
			Scale = scale ?? Vector2.One;
			Rotation = rotation;
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