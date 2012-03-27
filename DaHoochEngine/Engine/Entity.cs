using System;
using Microsoft.Xna.Framework;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Diagnostics;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace ProjectDaHooch
{
	public class Entity : UpdateableBase, ICloneable
	{
		//Static Fields/Properties
		private static List<Entity> masterEntityList = new List<Entity>();
		static Entity()
		{
			masterEntityList.Add(null);
		}

		protected override void OnEnabledChanged(object sender, EventArgs e)
		{
			Debug.WriteLine("Entity#" + this.entityID + " Name: '" + this.Name + "' Enabled = " + this.Enabled);
			base.OnEnabledChanged(sender, e);
		}


		//Properties
		public string Name { get; set; }
        [Microsoft.Xna.Framework.Content.ContentSerializerIgnore]
        public AspectContainer Aspects { get { return aspects; } }
		public virtual bool Initialized { get { return initialized; } }
		public virtual bool Loaded { get { return loaded; } }
		public int EntityID { get { return entityID; } }

        //[XmlIgnore] // FIXME Just delete this attribute, we're using IntermediateSerializer
        [Microsoft.Xna.Framework.Content.ContentSerializerIgnore]
		public virtual Game Game
		{ 
			get { return this.game; }
			set
			{
				if (this.initialized)
				{
					this.Enabled = false;
					this.initialized = false;
				}
				this.game = value;
			}
		}
		public int Hash { get { return this.GetHashCode(); } }

		//Automatic Properties
		public bool IsTemplate { get; set; }

		//Private and Protected Members - Value Type
		protected bool initialized = false;
		protected bool loaded = false;
		private int entityID = -1;

		//Private and Protected Members - Reference Type
        [Microsoft.Xna.Framework.Content.ContentSerializer(ElementName = "AspectsInstance")]
		private AspectContainer aspects;
        [Microsoft.Xna.Framework.Content.ContentSerializerIgnore]
		private Game game = null;


		public Entity()
		{
			aspects = new AspectContainer(this);
			this.entityID = GetID(this);
			this.Enabled = true;
		}

		public virtual void Initialize()
		{
			if (this.IsTemplate)
			{
				throw new InvalidOperationException("Entity Templates are not meant to be Initialized and Loaded. Clone it!");
			}
			Aspects.InitializeAll();
			this.initialized = true;
		}

		public virtual void LoadContent()
		{
			if (!this.initialized)
			{
				throw new InvalidOperationException("Must call entity.Initialize() before entity.LoadContent!!!");
			}
			Aspects.LoadAll();
			this.loaded = true;
		}

		public override void Update(GameTime gameTime)
		{
			IAspect[] aspectArray = Aspects.ToArray;
			for (int i = 0; i < aspectArray.Length; i++)
			{
				if (aspectArray[i] is IUpdateable)
					((IUpdateable)aspectArray[i]).Update(gameTime);
			}
		}


		//Static Methods
		private static int GetID(Entity entity)
		{
			masterEntityList.Add(entity);
			return masterEntityList.IndexOf(entity);
		}

		public static Entity GetEntityByID(int id)
		{
			if (id < masterEntityList.Count)
				return masterEntityList[id];
			else
				return null;
		}
		

		//Interface implementations
		#region ICloneable Members

		public virtual object Clone()
		{
			Entity e = new Entity();
			e.game = this.game;
			e.Name = this.Name;
			e.Enabled = this.Enabled;

			foreach (Aspect a in aspects.List)
			{
				e.aspects.Add(a.Clone(e.EntityID));
			}

			if (this.initialized)
				e.Initialize();

			return e;
		}

		#endregion
	}
}
