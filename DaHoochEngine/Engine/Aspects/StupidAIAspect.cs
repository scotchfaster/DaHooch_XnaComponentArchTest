using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ProjectDaHooch
{
	public class StupidAIAspect : UpdateableAspect
	{
		private BoxCollisionAspect player;
		private BoxCollisionAspect collider;
		private ISprite sprite;

		private int moveDirection = 0;
		public float MoveSpeed = 300f;
		public float MaxSpeed = 100f;

		protected override void InitSubclass()
		{
			collider = Owner.Aspects.FindFirst<BoxCollisionAspect>();
			player = Entity.GetEntityByID(FirstLevel.PlayerID).Aspects.FindFirst <BoxCollisionAspect>();
			sprite = Owner.Aspects.FindFirst<SpriteAspect>();
		}

		public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
		{
			moveDirection = (int)((player.Position.X - collider.Position.X) / Math.Abs((player.Position.X - collider.Position.X)));

			collider.body.ApplyImpulse(new Vector2(MoveSpeed * moveDirection, 0));

			collider.body.LinearVelocity.X = Math.Abs(collider.body.LinearVelocity.X) > MaxSpeed ? 
				MaxSpeed*moveDirection : collider.body.LinearVelocity.X;

			switch (moveDirection)
			{
				case -1:
					sprite.FlipRight();
					break;
				case 1:
					sprite.FlipLeft();
					break;
			}

		}

		protected override IAspect CloneSubclass()
		{
#if DEBUG
			throw new NotImplementedException();
#endif
		}
	}
}
