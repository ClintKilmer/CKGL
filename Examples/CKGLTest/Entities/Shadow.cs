using CKGL;

namespace CKGLTest
{
	public class Shadow : Entity
	{
		public Colour ShadowColourNear = Colour.Black;
		public Colour ShadowColourFar = Colour.Black;//.Alpha(0);

		private RenderTarget _surface;
		private RenderTarget Surface
		{
			get
			{
				if (_surface == null || _surface.Width != Resolution.Width || _surface.Height != Resolution.Height)
					_surface = new RenderTarget(Resolution.Width, Resolution.Height, 1, TextureFormat.RGBA8);

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
			RenderTarget originalRenderTarget = RenderTarget.Current;
			RenderTarget.Bind(Surface);
			Graphics.Clear(Colour.Black);

			// Shadow Casters
			Vector2 caster = occlusion.player.Position;
			//Vector2 caster = Input.Mouse.Position.World.round();
			//Vector2 caster = Vector2.Zero;

			// Falloff Shadow
			BlendState originalBlendState = Graphics.State.BlendState;
			Graphics.State.SetBlendState(BlendState.Subtractive);
			Renderer.Draw.Circle(caster, Resolution.Height * 2f, Colour.White, Colour.White.Alpha(0), 32);
			Graphics.State.SetBlendState(originalBlendState);

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
			RenderTarget.Bind(originalRenderTarget);
			//Graphics.State.SetBlendState(BlendState.Additive);
			Renderer.Draw.RenderTarget(Surface, Camera.Position.X - Resolution.Half.Width / Camera.Zoom, Camera.Position.Y - Resolution.Half.Height / Camera.Zoom, 1f / Camera.Zoom, Camera.Rotation, Camera.Position, Colour.White);
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