using System;
using System.Collections.Generic;
using System.Collections;

namespace ProjectDaHooch
{
	public class AspectContainer
	{
		public List<IAspect> List { get { return (List<IAspect>)aspectList; } }
		private List<IAspect> aspectList = new List<IAspect>();
		private List<IAspect> findResult = new List<IAspect>();

        [Microsoft.Xna.Framework.Content.ContentSerializer(SharedResource=true)]
        public Entity Owner { get { return this.owner; } set { this.owner = value; } }
		
        private List<Aspect> concreteList;
		private bool initialized = false;
        private Entity owner;



		public AspectContainer(Entity owner)
		{
			this.Owner = owner;
		}

        public IAspect[] ToArray
        {
            get { return aspectList.ToArray(); }
        }

		public void InitializeAll()
		{
			for (int i = 0; i < aspectList.Count; i++)
			{
				aspectList[i].Initialize();
			}
			initialized = true;
            //UpdateConcreteList(); // TODO Delete this
		}

		public void LoadAll()
		{
			for (int i = 0; i < aspectList.Count; i++)
			{
				aspectList[i].LoadContent();
			}
		}

		public void Add(IAspect aspect)
		{
            //if (initialized)
                //UpdateConcreteList(); // TODO Delete this

			aspect.Owner = this.Owner;
			aspectList.Add(aspect);
		}

		public void Remove(Aspect aspect)
		{
			aspectList.Remove((IAspect)aspect);
            //if (initialized)
                //UpdateConcreteList(); // TODO Delete this
		}

		public void RemoveByName<T>(string name) where T: IAspect
		{
			IAspect aspect = (IAspect)this.FindByName<T>(name);
			Remove((Aspect)aspect);
		}

		public T[] FindAll<T>()
		{
			findResult = aspectList.FindAll(new Predicate<IAspect>((IAspect) =>
			{
				if (IAspect is T)
					return true;

				return false;
			}));

			if (findResult.Count == 0)
				throw new AspectNotFoundException(Owner, typeof(T));

			return findResult.ConvertAll<T>(new Converter<IAspect, T>((target) =>
			{
				return (T)target;
			})).ToArray();
		}

		public object[] FindAllOfType(Type t)
		{
			findResult = aspectList.FindAll(new Predicate<IAspect>((IAspect) =>
			{
				if (t.IsAssignableFrom(IAspect.GetType()))
					return true;

				return false;
			}));

			if (findResult.Count == 0)
				throw new AspectNotFoundException(Owner, t);

			return (object[])findResult.ToArray();
		}

		public object[] FindAllType(Type t)
		{
			List<object> list = new List<object>();
			foreach (IAspect aspect in aspectList)
			{
				if (t.IsAssignableFrom(aspect.GetType()))
					list.Add(aspect);
			}

			return list.ToArray();
		}

		public T FindFirst<T>()
		{
			T result = (T)aspectList.Find(new Predicate<IAspect>((IAspect) =>
			{
				return (IAspect is T);
			}));

			if (result == null)
				throw new AspectNotFoundException(Owner, typeof(T));

			return result;
		}

		public IAspect FindFirstOfType(Type t)
		{
			foreach (IAspect aspect in aspectList)
			{
				if (t.IsAssignableFrom(aspect.GetType()))
					return aspect;
			}

			return null;
		}

		public T FindByName<T>(string name) where T: IAspect
		{
			IAspect result = aspectList.Find(new Predicate<IAspect>((IAspect) =>
			{
				if (IAspect.Name == null)
					return false;
				return (bool)(IAspect.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && IAspect is T);
			}));

			if (result == null)
				throw new AspectNotFoundException(Owner, typeof(T), name);

			return (T)result;
		}

		public IAspect FindByName(string name)
		{
			IAspect IAspectResult = aspectList.Find(new Predicate<IAspect>((IAspect) =>
			{
				if (IAspect.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
					return true;

				return false;
			}));

			if (IAspectResult == null)
				throw new AspectNotFoundException(Owner, name);

			return IAspectResult;
		}
    }
}
