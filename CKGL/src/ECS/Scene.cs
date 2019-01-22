namespace CKGL
{
	public class Scene
	{
		public EntityList Entities;

		public Colour clearColour = Colour.Grey;

		public Scene(Colour? clearColour = null)
		{
			Entities = new EntityList(this);
			this.clearColour = clearColour ?? Colour.Grey;
		}

		public virtual void Begin()
		{
			foreach (var entity in Entities)
				entity.SceneBegin();
		}

		public virtual void End()
		{
			foreach (var entity in Entities)
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