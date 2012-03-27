using Microsoft.Xna.Framework;

namespace ProjectDaHooch
{
	public class OrientationAspect : Aspect
	{
		public virtual Vector2 Position { get; set; }
		public virtual float Rotation { get; set; }
		public virtual Rectangle Dimensions { get; set; }

		protected override IAspect CloneSubclass()
		{
			OrientationAspect oa = new OrientationAspect();

			oa.Position = this.Position;
			oa.Rotation = this.Rotation;

			return oa;
		}
	}
}
