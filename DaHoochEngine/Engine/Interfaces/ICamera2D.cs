using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectDaHooch
{
	public interface ICamera2D
	{
		string Name { get; set; }

		Viewport Viewport { get; set; }

		Vector2 Origin { get; }

		Vector2 Position { get; set; }

		Rectangle WorldBoundaries { get; set; }

		float MoveSpeed { get; set; }

		float Rotation { get; set; }

		float Zoom { get; set; }

		bool HasSpriteInView(ISprite sprite);

		void MoveTo(Vector2 point);

		void FollowSprite(ISprite sprite);

		bool EnableSpriteFollow { get; set; }

		Matrix GlobalTransform { get; }

		void Update(GameTime gameTime);
	}
}
