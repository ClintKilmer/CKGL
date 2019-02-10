using CKGL;

namespace CKGLTest2D
{
	class Debug : Entity
	{
		private bool enabled = true;

		public Debug()
		{
			Depth = Layer.Debug;
		}

		public override void Update()
		{
			if (Input.Keyboard.KeyDown(Keys.F3))
				enabled = !enabled;
		}

		public override void Draw()
		{
			if (enabled)
			{
				Renderer.Draw.Text(SpriteFonts.Font,
					"|:shadow=0,1,0,0,0,0.5:|"
					+ $"FPS: {Metrics.FPS.ToString("N1").PadLeft(2, '0')}\n"
					//+ $"UFPS: {Metrics.UpdateFPS.ToString("N1").PadLeft(2, '0')}\n"
#if DEBUG
					+ $"Mem: {Metrics.Memory.ToString("F")} MB\n"
					+ $"GC: {Metrics.GCMemory.ToString("F")} MB\n"
#endif
					+ $"Draw Calls: {Metrics.DrawCalls}\n"
					+ $"Entities: {Scene.Entities.Count}\n"
					, Camera.Position - Resolution.Half.Vector2 + new Vector2(1f, 0), Vector2.One, Color.White);

				Renderer.Draw.Pixel(Input.Mouse.Position.World, Color.White);
			}
		}
	}
}