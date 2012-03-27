using System;

namespace ProjectDaHooch
{
	class AspectNotFoundException : Exception
	{
		public Type AspectType { get; set; }
		public Entity Entity { get; set; }
		public string AspectName { get; set; }

		public AspectNotFoundException(Entity entity, Type type)
			: base("Looking for aspect of type " + type.ToString() + " in entity(ID#" + entity.EntityID + "). None found.")
		{
			this.AspectType = type;
			this.Entity =  entity;
		}

		public AspectNotFoundException(Entity entity, string aspectName)
			: base("Looking for aspect named " + aspectName + " in entity(ID#" + entity.EntityID + "). None found.")
		{
			this.AspectName = aspectName;
			this.Entity = entity;
		}

		public AspectNotFoundException(Entity entity, Type type, string name)
			: base("Looking for aspect of type " + type.ToString() + " named " + name + ". None found.")
		{
			this.Entity = entity;
			this.AspectType = type;
			this.AspectName = name;
		}
	}
}
