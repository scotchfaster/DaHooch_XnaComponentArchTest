using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using FarseerGames.FarseerPhysics.Collisions;

namespace ProjectDaHooch
{
	public enum Direction
	{
		Left,
		Right
	}

	public class PlayerInputAspect : UpdateableAspect
	{
		public float MoveSpeed { get; set; }
		public string CollisionAspect
		{
			get { return this.boxColliderName; }
			set
			{
				if (Initialized)
				{
					BoxCollisionAspect oa = Owner.Aspects.FindByName<BoxCollisionAspect>(value);
					if (oa != null)
					{
						this.boxColliderName = value;
						this.orientation = oa;
					}
				}
				else
					this.boxColliderName = value;
			}
		}
		public bool HasCollision { get { return HasCollision; } }
		public Direction MoveDirection { get { return this.moveDirection; } }
		private bool hasCollision = false;
		protected KeyboardState keyboard;
		protected BoxCollisionAspect orientation;
		protected string boxColliderName;
		private GameTime currentGameTime;
		private bool OnTheGround = false;
		private const float normalJumpMargin = 0.01f;
		const int jumpInterval = 100;
		private double jumpTimer = 0;
		private bool CanJump;
		private bool IsJumping;
		private uint collisionCount = 0;
		private float jumpImpulse = 3500f;
		private float MaximumXVelocity = 200f;
		private float shootInterval = 0.25f;
		private float shootTimer = 0;
		private Direction moveDirection = Direction.Right;
		private ISprite sprite;

		private ProjectileAspect projectileAttack;

		protected override void InitSubclass()
		{
			orientation = Owner.Aspects.FindByName<BoxCollisionAspect>(boxColliderName);
			MoveSpeed = 500;

			sprite = Owner.Aspects.FindFirst<ISprite>();

			projectileAttack = Owner.Aspects.FindFirst<ProjectileAspect>();
		}

		protected override void LoadSubclassContent()
		{
			orientation.geom.OnCollision += HandleCollision;
			orientation.body.Mass = 25f;
			orientation.geom.FrictionCoefficient = 0.5f;
		}

		public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
		{
			int direction = moveDirection == Direction.Left ? -1 : 1;
			keyboard = Keyboard.GetState();
			Vector2 move = new Vector2(0);
			currentGameTime = gameTime;

			if (keyboard.IsKeyDown(Keys.Up))
				move.Y -= MoveSpeed;
			//if (keyboard.IsKeyDown(Keys.Down))
			//    move.Y += MoveSpeed;
			//if (keyboard.IsKeyDown(Keys.Left))
			//    move.X -= MoveSpeed;
			//if (keyboard.IsKeyDown(Keys.Right))
			//    move.X += MoveSpeed;

			if (keyboard.IsKeyDown(Keys.Z))
			{
				moveDirection = Direction.Left;
				move.X -= MoveSpeed;
				sprite.FlipLeft();
			}
			if (keyboard.IsKeyDown(Keys.C))
			{
				moveDirection = Direction.Right;
				move.X += MoveSpeed;
				sprite.FlipRight();
			}
			if (keyboard.IsKeyDown(Keys.X))
			{
				if (currentGameTime.TotalGameTime.TotalSeconds - shootTimer > shootInterval)
				{
					shootTimer = (float)currentGameTime.TotalGameTime.TotalSeconds;
					projectileAttack.FireProjectile(orientation.Position, moveDirection);
				}
			}

			if (keyboard.IsKeyDown(Keys.Space) && this.CanJump)
			{
				this.IsJumping = true;
				this.CanJump = false;
				this.OnTheGround = false;
				move.Y = -jumpImpulse;
			}

			orientation.body.ApplyImpulse(ref move);
			orientation.body.LinearVelocity.X = MathHelper.Clamp(orientation.body.LinearVelocity.X, -MaximumXVelocity, MaximumXVelocity);
		}



		private bool HandleCollision(Geom geom1, Geom geom2, ContactList contactList)
		{
			collisionCount++;
			this.hasCollision = true;

			if (!this.OnTheGround)
			{
				//Console.WriteLine("***WE GOT A COLLISION Time="+currentGameTime.TotalGameTime.TotalMilliseconds+
				//    "\n\tJumpTimer="+jumpTimer.ToString());
				int contactCount = 0;
				Vector2 contactSum = Vector2.Zero;
				foreach (Contact contact in contactList)
				{
					if (Math.Abs(contact.Normal.X) > Math.Abs(contact.Normal.Y))
						return true;
					contactSum += contact.Normal;
					contactCount++;
				}

				contactSum /= contactCount;
				float angleOfCollision = Math.Abs((float)Math.Atan2(contactSum.X, contactSum.Y));

				if (Math.Abs(MathHelper.Pi - angleOfCollision) < normalJumpMargin &&
					currentGameTime.TotalGameTime.TotalMilliseconds - jumpTimer > jumpInterval)
				{
					//Console.WriteLine("Okay to Jump!"+jumpTimer.ToString()+
					//    "\n\tCollision Vector:("+((double)contactSum.X).ToString()+","+contactSum.Y+")"+
					//    "\n\tCollision Angle:"+((double)angleOfCollision).ToString());
					this.OnTheGround = true;
					this.IsJumping = false;
					this.CanJump = true;
				}
			}
			return true;
		}


		private void HandleSeparation(Geom geom1, Geom geom2)
		{
			collisionCount--;

			if (collisionCount <= 0)
			{
				hasCollision = false;
				this.OnTheGround = false;
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
