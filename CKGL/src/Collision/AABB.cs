namespace CKGL
{
	public class AABB
	{
		public Vector2 Position { get; set; } = Vector2.Zero;
		public float X
		{
			get { return Position.X; }
			set { Position = new Vector2(value, Y); }
		}
		public float Y
		{
			get { return Position.Y; }
			set { Position = new Vector2(X, value); }
		}

		public Vector2 Size { get; set; } = Vector2.Zero;
		public float Width
		{
			get { return Size.X; }
			set { Size = new Vector2(value, Height); }
		}
		public float Height
		{
			get { return Size.Y; }
			set { Size = new Vector2(Width, value); }
		}

		public float Top { get { return Position.Y; } }
		public float Bottom { get { return Position.Y + Size.Y; } }
		public float Left { get { return Position.X; } }
		public float Right { get { return Position.X + Size.X; } }

		public AABB(float x, float y, float width, float height) : this(new Vector2(x, y), new Vector2(width, height)) { }
		public AABB(Vector2 position, Vector2 size)
		{
			Position = position;
			Size = size;
		}

		public bool Intersects(AABB aabb)
		{
			return Right > aabb.Left &&
				   Left < aabb.Right &&
				   Bottom > aabb.Top &&
				   Top < aabb.Bottom;
		}
	}
}