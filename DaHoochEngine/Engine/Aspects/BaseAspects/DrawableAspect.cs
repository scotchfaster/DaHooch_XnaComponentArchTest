using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectDaHooch
{
	public abstract class DrawableAspect : Aspect, IDaHoochDrawable
	{
		//public string Name
		//{
		//    get { return this.name; }
		//    set { this.name = value; }
		//}
		//public override Entity Owner { get; set; }
		//public bool Active { get; set; }
		//public AspectContainer Aspects { get { return Aspects; } }

		//private AspectContainer aspects;
		//private string name;

		//public DrawableAspect(Entity owner, string name)
		//{
		//    this.Owner = owner;
		//    this.Active = true;
		//    this.name = name;
		//    aspects = new AspectContainer(owner);
		//}

		//public abstract void Initialize();

		//public abstract IAspect Clone(Entity newEntity);

		private bool visible;
		private int drawOrder;
		private CoordinateType coordinateType;

		public DrawableAspect()
		{
			VisibleChanged = new EventHandler(OnVisibleChanged);
			DrawOrderChanged = new EventHandler(OnDrawOrderChanged);
			CoordinateTypeChanged = new EventHandler(OnCoordinateTypeChanged);
			SerializerInfo.AddType(this.GetType());
		}

		protected void OnVisibleChanged(object sender, EventArgs e) { }
		protected void OnDrawOrderChanged(object sender, EventArgs e) { }
		protected void OnCoordinateTypeChanged(object sender, EventArgs e) { }

		#region IDaHoochDrawable Members

		public void Draw(GameTime gameTime)
		{ }

		public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

		public virtual int DrawOrder
		{
			get { return drawOrder; }
			set { drawOrder = value; VisibleChanged(this, new EventArgs()); }
		}

		public event System.EventHandler DrawOrderChanged;

		public bool Visible
		{
			get { return this.visible; }
			set { visible = value; VisibleChanged(this, new EventArgs()); }
		}

		public event System.EventHandler VisibleChanged;

		public CoordinateType CoordinateType
		{
			get { return this.coordinateType; }
			set { coordinateType = value; CoordinateTypeChanged(this, new EventArgs()); }
		}

		public event System.EventHandler CoordinateTypeChanged;

		#endregion

		#region IComparable<IDaHoochDrawable> Members

		public int CompareTo(IDaHoochDrawable other)
		{
			return (this.DrawOrder - other.DrawOrder);
		}

		#endregion
	}
}
