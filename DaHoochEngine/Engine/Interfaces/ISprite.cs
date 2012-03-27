using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectDaHooch
{
	public interface ISprite
	{
		Vector2 Position { get; }
		float Rotation { get; }
		CoordinateType CoordinateType { get; set; }
		Vector2 Scale { get; set; }
		Vector2 DrawOrigin { get; set; }
		float LayerDepth { get; set; }
		Color Tint { get; set; }
		SpriteEffects FlipEffects { get; set; }
		Texture2D Image { get; }
		string ImageFile { get; set; }
		Rectangle ImageFrame { get; set; }

		void FlipLeft();

		void FlipRight();
	}
}
