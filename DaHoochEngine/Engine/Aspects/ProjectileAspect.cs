using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FarseerGames.FarseerPhysics.Collisions;
using System.Diagnostics;

namespace ProjectDaHooch
{
	public class ProjectileAspect : UpdateableAspect
	{
		public uint MaxNumberOfProjectiles { get; set; }
		public float CoefficientA { get; set; }
		public float CoefficientB { get; set; }
		public float HorizontalVelocity { get; set; }
		public float ProjectileDuration { get; set; }

		private Entity[] Projectiles;
		private int[] projectileDirections;

		public ProjectileAspect()
		{
			MaxNumberOfProjectiles = 5;
			CoefficientA = 12.5f;
			CoefficientB = -10f;
			HorizontalVelocity = 3f;
			ProjectileDuration = 2f;
		}

		protected override void InitSubclass()
		{
			Projectiles = new Entity[MaxNumberOfProjectiles];
			projectileDirections = new int[MaxNumberOfProjectiles];
		}

		protected override void LoadSubclassContent()
		{
			var template = new Entity()
			{
				IsTemplate = true,
				Game = Owner.Game,
				Name = "PROJETILE",
				Enabled = false
			};

			var projectileSprite = new SpriteAspect()
			{
				ImageFile = "Sprites/hobosquirrel",
				ImageFrame = new Rectangle(0, 154, 19, 12),
				OwnerID = template.EntityID,
				OrientationAspect = "orientation",
				LayerDepth = 0.3f,
				CoordinateType = CoordinateType.World,
				Scale = new Vector2(1.5f, 1.5f)
			};
			template.Aspects.Add(projectileSprite);

			var orientation = new BoxCollisionAspect()
			{
				Name = "orientation",
				OwnerID = template.EntityID,
				Mass = 50f,
				CollisionGroup = EntityTemplates.PlayerCollisionGroup,
				OnCollision = (geom1, geom2, contactList) =>
				{
					Entity e2 = Entity.GetEntityByID(BoxCollisionAspect.FindEntityByGeomID(geom2.Id));
					if (e2 != null && e2.Name == "Enemy1")
					{
						e2.Aspects.FindFirst<RespawnAspect>().Respawn();
					}
					return false;
				}
			};
			template.Aspects.Add(orientation);

			var rotater = new PinwheelRotaterAspect()
			{
				RotationSpeed = .015f,
				OwnerID = template.EntityID
			};
			template.Aspects.Add(rotater);

			var age = new AgeAspect() { OwnerID = template.EntityID };
			template.Aspects.Add(age);

			var render = new RenderingAspect()
			{
				OwnerID = template.EntityID,
				Visible = false
			};
			template.Aspects.Add(render);

			for (int i = 0; i < MaxNumberOfProjectiles; i++)
			{
				Projectiles[i] = (Entity)template.Clone();
				Projectiles[i].Initialize();
				Projectiles[i].LoadContent();
				projectileDirections[i] = 1;
				bool foo = Projectiles[i].Enabled;
			}
		}

		public override void Update(GameTime gameTime)
		{
			for (int i = 0; i < MaxNumberOfProjectiles; i++)
			{
				if (Projectiles[i].Enabled)
				{
					var orientation = Projectiles[i].Aspects.FindFirst<OrientationAspect>();
					var age = Projectiles[i].Aspects.FindFirst<AgeAspect>();

					orientation.Position += new Vector2(HorizontalVelocity * age.AgeInSeconds * projectileDirections[i], 
						CoefficientA * (float)Math.Pow(age.AgeInSeconds, 2) + CoefficientB * age.AgeInSeconds);

					Projectiles[i].Update(gameTime);
				}
			}
		}

		public void FireProjectile(Vector2 position, Direction direction)
		{
			for (int i = 0; i < MaxNumberOfProjectiles; i++)
			{
				if (!Projectiles[i].Enabled)
				{
					if (direction == Direction.Left)
						projectileDirections[i] = -1;
					else
						projectileDirections[i] = 1;
					Projectiles[i].Aspects.FindFirst<OrientationAspect>().Position = position;
					Projectiles[i].Aspects.FindFirst<PinwheelRotaterAspect>().RotationSpeed =
						Math.Abs(Projectiles[i].Aspects.FindFirst<PinwheelRotaterAspect>().RotationSpeed) * projectileDirections[i];
					Projectiles[i].Aspects.FindFirst<AgeAspect>().SetDeathTimer(ProjectileDuration, new EventHandler(ProjectileDeathCallback));
					Projectiles[i].Enabled = true;
					Projectiles[i].Aspects.FindFirst<RenderingAspect>().Visible = true;
					break;
				}
			}
		}

		public void ProjectileDeathCallback(object sender, EventArgs e)
		{
			((AgeAspect)sender).Owner.Enabled = false;
			((AgeAspect)sender).Reset();
			((AgeAspect)sender).Owner.Aspects.FindFirst<RenderingAspect>().Visible = false;
		}

		protected override IAspect CloneSubclass()
		{
#if DEBUG
			throw new NotImplementedException();
#endif
		}
	}
}
