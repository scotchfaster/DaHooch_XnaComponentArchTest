using Microsoft.Xna.Framework;

namespace ProjectDaHooch
{
	public static class MathHelper2
	{
		public static Vector2 Clamp(Vector2 value, float min, float max)
		{
			value.X = MathHelper.Clamp(value.X, min, max);
			value.Y = MathHelper.Clamp(value.Y, min, max);
			return value;
		}
	}
}
