using System;
using Microsoft.Xna.Framework;

namespace ProjectDaHooch
{
	public class RespawnAspect : UpdateableAspect
	{
		public Rectangle Bounds { get; set; }
		public Vector2 RespawnPosition { get; set; }

		BoxCollisionAspect collider;

		protected override void InitSubclass()
		{
			collider = Owner.Aspects.FindFirst<BoxCollisionAspect>();
		} 

		public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
		{
			if (collider.Position.X < Bounds.Left ||	//TODO: Fix this stupid ugly if statement
				collider.Position.X > Bounds.Right ||
				collider.Position.Y < Bounds.Top ||
				collider.Position.Y > Bounds.Bottom)
				Respawn();
		}

		public void Respawn()
		{
			collider.Position = RespawnPosition;
		}

		protected override IAspect CloneSubclass()
		{
			RespawnAspect ra = new RespawnAspect();
			
			ra.Bounds = this.Bounds;
			ra.RespawnPosition = this.RespawnPosition;

			return ra;
		}
	}
}
