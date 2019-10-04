using CKGL;

namespace CKGLTest2D
{
	public class Shadow : Entity
	{
		public Colour ShadowColourNear = Colour.Black;
		public Colour ShadowColourFar = Colour.Black;//.Alpha(0);

		private Framebuffer _surface;
		private Framebuffer Surface
		{
			get
			{
				if (_surface == null || _surface.Width != CKGLTest2D.Camera.Width || _surface.Height != CKGLTest2D.Camera.Height)
					_surface = Framebuffer.Create(CKGLTest2D.Camera.Width, CKGLTest2D.Camera.Height, 1, TextureFormat.RGBA8);

				return _surface;
			}
		}

		public Shadow()
		{
			Depth = Layer.Shadow;
		}

		public override void Update()
		{
		}

		public override void Draw()
		{
			Framebuffer originalFramebuffer = Framebuffer.Current;
			Surface.Bind();
			Graphics.Clear(Colour.Black);

			// Shadow Casters
			Vector2 caster = CKGLTest2D.Player.Position.Floor();
			//Vector2 caster = Input.Mouse.Position.World.round();
			//Vector2 caster = Vector2.Zero;

			// Falloff Shadow
			BlendState originalBlendState = BlendState.Current;
			BlendState.Subtractive.Set();
			Renderer.Draw.Circle(caster, CKGLTest2D.Camera.Height * 2f, Colour.White, Colour.White.Alpha(0), 32);
			originalBlendState.Set();

			//Renderer.SetBlendState(nsBlendState.Subtractive);
			foreach (Box box in Scene.Entities.FindAll<Box>())
			{
				var block_l = box.X - box.Origin.X;
				var block_r = box.X - box.Origin.X + box.Size.X;
				var block_t = box.Y - box.Origin.Y;
				var block_b = box.Y - box.Origin.Y + box.Size.Y;

				if (caster.Y > block_t)
					DrawShadow(new Vector2(block_l, block_t), new Vector2(block_r, block_t), caster, 700);
				if (caster.Y < block_b)
					DrawShadow(new Vector2(block_l, block_b), new Vector2(block_r, block_b), caster, 700);
				if (caster.X > block_l)
					DrawShadow(new Vector2(block_l, block_t), new Vector2(block_l, block_b), caster, 700);
				if (caster.X < block_r)
					DrawShadow(new Vector2(block_r, block_t), new Vector2(block_r, block_b), caster, 700);
			}
			originalFramebuffer.Bind();
			//Graphics.State.SetBlendState(BlendState.Additive);
			Renderer.Draw.Framebuffer(Surface, TextureAttachment.Colour0, CKGLTest2D.Camera.Position.X, CKGLTest2D.Camera.Position.Y, Colour.White);
			//Renderer.ResetBlendState();
		}

		private void DrawShadow(Vector2 A, Vector2 B, Vector2 O, float dist)
		{
			Vector2 Adir = (A - O);
			Adir.Normalize();
			Vector2 Ad = A + Adir * dist;

			Vector2 Bdir = (B - O);
			Bdir.Normalize();
			Vector2 Bd = B + Bdir * dist;

			Renderer.Draw.Triangle(A, B, Ad, ShadowColourNear, ShadowColourNear, ShadowColourFar);
			Renderer.Draw.Triangle(B, Ad, Bd, ShadowColourNear, ShadowColourFar, ShadowColourFar);

			//Renderer.Draw.TriangleStrip.Begin();
			//Renderer.Draw.TriangleStrip.AddVertex(A, ShadowColourNear);
			//Renderer.Draw.TriangleStrip.AddVertex(B, ShadowColourNear);
			//Renderer.Draw.TriangleStrip.AddVertex(Ad, ShadowColourFar);
			//Renderer.Draw.TriangleStrip.AddVertex(Bd, ShadowColourFar);
			//Renderer.Draw.TriangleStrip.End();
		}
	}
}