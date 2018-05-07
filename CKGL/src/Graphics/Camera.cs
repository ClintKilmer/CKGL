namespace CKGL
{
	public static class Camera
	{
		public static float X
		{
			get { return Position.X; }
			set
			{
				_postition.X = value;
				_translationMatrix = UpdateTranslationMatrix();
				_worldMatrix = UpdateWorldMatrix();
			}
		}

		public static float Y
		{
			get { return Position.Y; }
			set
			{
				_postition.Y = value;
				_translationMatrix = UpdateTranslationMatrix();
				_worldMatrix = UpdateWorldMatrix();
			}
		}

		private static Vector2 _postition = Vector2.Zero;
		public static Vector2 Position
		{
			get { return _postition; }
			set
			{
				_postition = value;
				_translationMatrix = UpdateTranslationMatrix();
				_worldMatrix = UpdateWorldMatrix();
			}
		}

		private static float _rotation = 0f;
		public static float Rotation
		{
			get { return _rotation; }
			set
			{
				_rotation = value;
				if (_rotation > 1f)
					_rotation = _rotation % 1f;
				else if (_rotation < 0f)
					_rotation = _rotation % -1f + 1f;

				_rotationMatrix = UpdateRotationMatrix();
				_worldMatrix = UpdateWorldMatrix();
			}
		}
		private static float zoom = 1f;
		public static float Zoom
		{
			get { return zoom; }
			set
			{
				zoom = Math.Max(value, 0f);

				_scaleMatrix = UpdateScaleMatrix();
			}
		}
		public static float zNearClip = -10000f;
		public static float zFarClip = 10000f;

		private static Matrix _translationMatrix = UpdateTranslationMatrix();
		public static Matrix UpdateTranslationMatrix()
		{
			return Matrix.CreateTranslation(-Position.X, -Position.Y, 0f);
		}

		private static Matrix _rotationMatrix = UpdateRotationMatrix();
		public static Matrix UpdateRotationMatrix()
		{
			return Matrix.CreateRotationZ(-Rotation);
		}

		private static Matrix _scaleMatrix = UpdateScaleMatrix();
		public static Matrix UpdateScaleMatrix()
		{
			return Matrix.CreateScale(Zoom, Zoom, 1f);
		}

		private static Matrix _worldMatrix = UpdateWorldMatrix();
		public static Matrix UpdateWorldMatrix()
		{
			return _translationMatrix * _rotationMatrix;
		}
		public static Matrix WorldMatrix
		{
			get
			{
				return _worldMatrix;
			}
		}

		public static Matrix ViewMatrix { get; } = Matrix.CreateLookAt(Vector3.Zero, new Vector3(0f, 0f, 1f), new Vector3(0f, -1f, 0f));

		public static Matrix ProjectionMatrix
		{
			get
			{
				return
					   // Half Pixel Offset
#if LINUX
					   Matrix.CreateOrthographicOffCenter(0f - Resolution.Half.Width + 0.5f / Zoom, Resolution.Width - Resolution.Half.Width + 0.5f / Zoom, 0f - Resolution.Half.Height - 0.5f / Zoom, Resolution.Height - Resolution.Half.Height - 0.5f / Zoom, zNearClip, zFarClip) *
					   //Matrix.CreateOrthographic(Resolution.Width, Resolution.Height, zNearClip, zFarClip) *
#else
					   Matrix.CreateOrthographic(Resolution.Width, Resolution.Height, zNearClip, zFarClip) *
#endif
					   _scaleMatrix;
			}
		}
	}
}