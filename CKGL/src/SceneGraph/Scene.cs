namespace CKGL
{
	public class Scene
	{
		public static Scene Current = new Scene();

		public EntityList Entities;

		public Colour ClearColour = Colour.Grey;

		public Scene(Colour? clearColour = null)
		{
			Entities = new EntityList(this);
			ClearColour = clearColour ?? Colour.Grey;
		}

		public virtual void Begin()
		{
			foreach (Entity entity in Entities)
				entity.SceneBegin();
		}

		public virtual void End()
		{
			foreach (Entity entity in Entities)
				entity.SceneEnd();
		}

		public virtual void BeforeUpdate()
		{
		}

		public void Update()
		{
			BeforeUpdate();
			Entities.Update();
			AfterUpdate();
		}

		public virtual void AfterUpdate()
		{
		}

		public void Draw()
		{
			Entities.Draw();
		}
	}
}