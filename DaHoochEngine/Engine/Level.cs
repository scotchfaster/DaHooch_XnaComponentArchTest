using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ProjectDaHooch
{
	public class Level
	{
		public List<Entity> Entities;
        [XmlIgnore]
        [Microsoft.Xna.Framework.Content.ContentSerializerIgnore]
		public Game game;

		public Level()
		{
			Entities = new List<Entity>();
		}

		public void Load(Game game)
		{
			this.game = game;	
			for (int i = 0; i < Entities.Count; i++)
			{
				Entities[i].Game = game;
				Entities[i].Initialize();
				Entities[i].LoadContent();
			}
		}

		public void Update(GameTime gameTime)
		{
			for (int i = 0; i < Entities.Count; i++)
			{
				if (Entities[i].Enabled)
					Entities[i].Update(gameTime);
				else
					Debug.WriteLine("WARNING Entity:" + Entities[i].Name + " is disabled!");
			}
		}

		public Entity GetEntityByID(uint id)
		{
			for (int i = 0; i < Entities.Count; i++)
			{
				if (Entities[i].EntityID == id)
					return Entities[i];
			}
			return null;
		}

		public Entity GetFirstEntityByName(string name)
		{
			for (int i = 0; i < Entities.Count; i++)
			{
				if (Entities[i].Name == name)
					return Entities[i];
			}
			return null;
		}
	}
}
