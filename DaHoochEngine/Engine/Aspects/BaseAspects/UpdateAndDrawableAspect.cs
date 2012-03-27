using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectDaHooch
{
	public abstract class UpdateAndDrawableAspect : Aspect, IUpdateable, IDaHoochDrawable
	{
		public UpdateAndDrawableAspect()
		{
			EnabledChanged += OnEnabledChanged;
			UpdateOrderChanged += OnUpdateOrderChanged;
			VisibleChanged = new EventHandler(OnVisibleChanged);
			DrawOrderChanged = new EventHandler(OnDrawOrderChanged);
			CoordinateTypeChanged = new EventHandler(OnCoordinateTypeChanged);
		}

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
		{ }


		public event EventHandler EnabledChanged;

		protected virtual void OnUpdateOrderChanged(object sender, EventArgs e)
		{ }


		public abstract void Update(GameTime gameTime);

		private bool visible;
		private int drawOrder;
		private CoordinateType coordinateType;

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
