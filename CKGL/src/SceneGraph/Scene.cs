namespace CKGL
{
	public class Scene
	{
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
			Entities.UpdateLists();
		}

		public virtual void Update()
		{
			Entities.Update();
		}

		public virtual void AfterUpdate()
		{
		}

		public virtual void Draw()
		{
			Entities.Draw();
		}
	}
}