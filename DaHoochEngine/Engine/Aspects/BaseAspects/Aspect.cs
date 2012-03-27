using Microsoft.Xna.Framework;
using System.Xml.Serialization;

namespace ProjectDaHooch
{
	public abstract class Aspect : IAspect
	{
		private static int idCounter = 0;

		public int AspectID { get { return aspectID; } }
		public string Name
		{
			get { return this.name; }
			set { this.name = value; }
		}
		public bool Initialized { get { return initialized; } }
		public bool Loaded { get { return loaded; } }
		public AspectContainer Aspects { get { return aspects; } }
		[XmlIgnore]
        [Microsoft.Xna.Framework.Content.ContentSerializerIgnore]
		public Entity Owner 
		{
			get { return this.owner; }
			set
			{
				this.owner = value;
				OwnerID = this.owner.EntityID;
			}
		}
		[XmlIgnore]
		public int OwnerID
		{ 
			get { return this.ownerID; }
			set
			{
				this.ownerID = value;
				this.ownerSet = true;
			}
		}

		private AspectContainer aspects;
		private string name;
		private int aspectID;
		private int ownerID = 0;
		private bool ownerSet = false;
		private bool initialized = false;
		private bool loaded = false;
		private Entity owner;

		public Aspect()
		{
			aspectID = GetID();
			SerializerInfo.AddType(this.GetType());
		}

		public void Initialize()
		{
			if (!ownerSet)
				throw new OrphanedAspectException(this);
			Owner = Entity.GetEntityByID(this.ownerID);

			InitSubclass();
			initialized = true;
		}

		/// <summary>
		/// It's not necessary to call Base.InitSubclass()
		/// </summary>
		protected virtual void InitSubclass()
		{
		}

		public void LoadContent()
		{
			LoadSubclassContent();
			this.loaded = true;
		}

		/// <summary>
		/// It's not necessary to call Base.LoadSubclassContent()
		/// </summary>
		protected virtual void LoadSubclassContent()
		{
		}

		public IAspect Clone(int NewOwnerID)
		{
			IAspect a = CloneSubclass();
			a.Name = this.name;
			a.OwnerID = NewOwnerID;

			return a;
		}

		protected abstract IAspect CloneSubclass();

		private static int GetID()
		{
			return ++idCounter;
		}
	}
}