namespace CKGL
{
	public abstract class Game
	{
		public abstract void Init();
		public abstract void Update();
		public abstract void Draw();
		public abstract void Destroy();

		public virtual void OnFocusGained() { }
		public virtual void OnFocusLost() { }
		public virtual void OnWindowResized() { }
	}
}