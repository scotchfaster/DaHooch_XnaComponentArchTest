using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ProjectDaHooch
{
	public interface IDaHoochDrawable : IDrawable, IComparable<IDaHoochDrawable>
	{
		Entity Owner { get; set; }
		CoordinateType CoordinateType { get; }
		event System.EventHandler CoordinateTypeChanged;

		void Draw(GameTime gameTime, SpriteBatch spriteBatch);
	}
}
