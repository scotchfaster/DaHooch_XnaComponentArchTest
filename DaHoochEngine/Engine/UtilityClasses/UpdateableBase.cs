using Microsoft.Xna.Framework;
using System;

namespace ProjectDaHooch
{
	public abstract class UpdateableBase : IUpdateable
	{
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


		public UpdateableBase()
		{
			EnabledChanged += OnEnabledChanged;
			UpdateOrderChanged += OnUpdateOrderChanged;
		}


		public abstract void Update(GameTime gameTime);
	}
}
