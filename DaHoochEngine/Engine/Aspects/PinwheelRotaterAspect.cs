using System;
using Microsoft.Xna.Framework;

namespace ProjectDaHooch
{
	public class PinwheelRotaterAspect : UpdateableAspect
	{
		public float RotationSpeed { get; set; }

		private float timer = 0;


		private OrientationAspect orientation;

		protected override void InitSubclass()
		{
			orientation = Owner.Aspects.FindFirst<OrientationAspect>();
			this.Enabled = true;
		}

		public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
		{
			timer = MathHelper.WrapAngle(timer + RotationSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds);
			orientation.Rotation = timer;
		}

		protected override IAspect CloneSubclass()
		{
			PinwheelRotaterAspect pw = new PinwheelRotaterAspect();
			pw.RotationSpeed = this.RotationSpeed;
			return pw;
		}
	}
}
