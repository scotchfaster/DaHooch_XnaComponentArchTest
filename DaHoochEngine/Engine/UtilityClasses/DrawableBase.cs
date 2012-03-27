using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectDaHooch
{
	public abstract class DrawableBase : IDaHoochDrawable
	{
		private bool visible;
		private int drawOrder;
		private CoordinateType coordinateType;

		public virtual Entity Owner { get; set; }

		public DrawableBase()
		{
			VisibleChanged = new EventHandler(OnVisibleChanged);
			DrawOrderChanged = new EventHandler(OnDrawOrderChanged);
			CoordinateTypeChanged = new EventHandler(OnCoordinateTypeChanged);
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
