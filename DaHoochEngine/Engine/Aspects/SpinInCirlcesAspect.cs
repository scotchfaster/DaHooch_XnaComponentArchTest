using Microsoft.Xna.Framework;
using System;

namespace ProjectDaHooch
{
	class SpinInCirlcesAspect : UpdateableAspect
	{
		public float Radius { get; set; }
		public float Speed { get; set; }
		public string OrientationAspect
		{
			get { return this.orientationAspectName; }
			set
			{
				if (Initialized)
				{
					OrientationAspect oa = Owner.Aspects.FindByName<OrientationAspect>(value);
					if (oa != null)
					{
						this.orientationAspectName = value;
						this.orientationAspect = oa;
					}
				}
				else
					this.orientationAspectName = value;
			}
		}

		private OrientationAspect orientationAspect;
		private string orientationAspectName;
		private float time;

		protected override void InitSubclass()
		{
			Radius = 3;
			Speed = 15f;
			this.orientationAspect = Owner.Aspects.FindByName<OrientationAspect>(orientationAspectName);
		}

		public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
		{
			time = MathHelper.WrapAngle((time + Speed*(float)gameTime.ElapsedGameTime.TotalSeconds));
			Vector2 newVector = new Vector2((float)Math.Cos(time), (float)Math.Sin(time));
			newVector.Normalize();
			newVector *= Radius;
			orientationAspect.Position += newVector;
			newVector = orientationAspect.Position;
		}

		protected override IAspect CloneSubclass()
		{
			SpinInCirlcesAspect spinner = new SpinInCirlcesAspect();
			
			spinner.Radius = this.Radius;
			spinner.Speed = this.Speed;
			spinner.orientationAspectName = this.orientationAspectName;

			return spinner;
		}
	}
}
