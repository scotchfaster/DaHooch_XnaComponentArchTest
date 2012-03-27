using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace ProjectDaHooch
{
	public abstract class UpdateableAspect : Aspect, IUpdateable
	{
		//public string Name
		//{ 
		//    get { return this.name; }
		//    set { this.name = value; }
		//}
		//public Entity Owner { get; set; }
		//public bool Active { get; set; }
		//public AspectContainer Aspects { get { return Aspects; } }

		//private AspectContainer aspects;
		//private string name;

		//public UpdateableAspect(Entity owner, string name)
		//{
		//    this.Owner = owner;
		//    this.Active = true;
		//    this.name = name;
		//    aspects = new AspectContainer(owner);
		//}

		//public abstract void Initialize();

		//public abstract IAspect Clone(Entity newEntity);


		private int updateOrder;
		private bool enabled;

		public bool Enabled
		{
			get { return enabled; }
			set { enabled = value; EnabledChanged(this, new EventArgs()); }
		}

		public int UpdateOrder
		{
			get { return updateOrder; }
			set { updateOrder = value; UpdateOrderChanged(this, new EventArgs()); }
		}


		public event EventHandler UpdateOrderChanged;

		protected virtual void OnEnabledChanged(object sender, EventArgs e)
		{
			Debug.WriteLine("Aspect " + this.AspectID + " = " + enabled);
		}


		public event EventHandler EnabledChanged;

		protected virtual void OnUpdateOrderChanged(object sender, EventArgs e)
		{ }


		public UpdateableAspect()
		{
			EnabledChanged += OnEnabledChanged;
			UpdateOrderChanged += OnUpdateOrderChanged;
			SerializerInfo.AddType(this.GetType());
		}


		public abstract void Update(GameTime gameTime);
	}
}
