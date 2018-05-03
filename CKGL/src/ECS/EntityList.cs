using System;
using System.Collections;
using System.Collections.Generic;

namespace CKGL
{
	public sealed class EntityList : IEnumerable<Entity>, IEnumerable
	{
		public Scene Scene;

		private List<Entity> entities;
		private List<Entity> toAdd;
		private List<Entity> toAwake;
		private List<Entity> toRemove;
		private bool unsorted;

		public EntityList(Scene scene)
		{
			Scene = scene;

			entities = new List<Entity>();
			toAdd = new List<Entity>();
			toAwake = new List<Entity>();
			toRemove = new List<Entity>();
		}

		public void SetUnsorted()
		{
			unsorted = true;
		}

		public void UpdateLists()
		{
			if (toAdd.Count > 0)
			{
				foreach (var entity in toAdd)
				{
					if (!entities.Contains(entity))
					{
						entities.Add(entity);
						if (Scene != null)
						{
							entity.Added(Scene);
						}
					}
				}

				unsorted = true;
			}

			if (toRemove.Count > 0)
			{
				foreach (var entity in toRemove)
				{
					if (entities.Contains(entity))
					{
						entities.Remove(entity);
						if (Scene != null)
						{
							entity.Removed();
						}
					}
				}

				toRemove.Clear();
			}

			if (unsorted)
			{
				unsorted = false;
				entities.Sort(CompareDepth);
			}

			if (toAdd.Count > 0)
			{
				toAwake.AddRange(toAdd);
				toAdd.Clear();

				foreach (var entity in toAwake)
					entity.Awake();
				toAwake.Clear();
			}
		}

		public void Add(Entity entity)
		{
			if (!toAdd.Contains(entity) && !entities.Contains(entity))
				toAdd.Add(entity);
		}

		public void Remove(Entity entity)
		{
			if (!toRemove.Contains(entity) && entities.Contains(entity))
				toRemove.Add(entity);
		}

		public void Add(IEnumerable<Entity> entities)
		{
			foreach (var entity in entities)
				Add(entity);
		}

		public void Remove(IEnumerable<Entity> entities)
		{
			foreach (var entity in entities)
				Remove(entity);
		}

		public void Add(params Entity[] entities)
		{
			for (int i = 0; i < entities.Length; i++)
				Add(entities[i]);
		}

		public void Remove(params Entity[] entities)
		{
			for (int i = 0; i < entities.Length; i++)
				Remove(entities[i]);
		}

		public int Count
		{
			get
			{
				return entities.Count;
			}
		}

		public Entity this[int index]
		{
			get
			{
				if (index < 0 || index >= entities.Count)
					throw new IndexOutOfRangeException();
				else
					return entities[index];
			}
		}

		public T FindFirst<T>() where T : Entity
		{
			foreach (var e in entities)
				if (e is T)
					return e as T;

			return null;
		}

		public List<T> FindAll<T>() where T : Entity
		{
			List<T> list = new List<T>();

			foreach (var e in entities)
				if (e is T)
					list.Add(e as T);

			return list;
		}

		public void With<T>(Action<T> action) where T : Entity
		{
			foreach (var e in entities)
				if (e is T)
					action(e as T);
		}

		public IEnumerator<Entity> GetEnumerator()
		{
			return entities.GetEnumerator();
		}

		IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public Entity[] ToArray()
		{
			return entities.ToArray();
		}

		public void Update()
		{
			foreach (var entity in entities)
				if (entity.Active)
					entity.Update();
		}

		public void Draw()
		{
			foreach (var entity in entities)
				if (entity.Visible)
					entity.Draw();
		}

		static public Comparison<Entity> CompareDepth = (a, b) => { return Math.Sign(b.Depth - a.Depth); };
	}
}