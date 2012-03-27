using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System;

namespace ProjectDaHooch
{
	public class RenderingAspect : DrawableAspect
	{
		//Properties
		public int Hash { get { return this.GetHashCode(); } } //TODO Get rid of this hash thing
		public CoordinateType AvailableCoordinateTypes { get { return availableCoordinateTypes; } }

		//Protected and Private Fields - Value Type
		protected CoordinateType availableCoordinateTypes = CoordinateType.None;

		//Protected and Private Fields - Reference Type
		private IRenderingService RenderingService;
		private ISprite[] drawables;

		public RenderingAspect()
		{
			this.Visible = true;
		}

		protected override void InitSubclass()
		{
			RenderingService = (IRenderingService)Owner.Game.Services.GetService(typeof(IRenderingService));
			RenderingService.Register(this);
		}

		protected override void LoadSubclassContent()
		{
			drawables = Array.ConvertAll<ISprite, ISprite>(Owner.Aspects.FindAll<ISprite>(), new Converter<ISprite, ISprite>(
				(sprite) => { return (ISprite)sprite; }));

			for (int i = 0; 
				i < drawables.Length || 
				(availableCoordinateTypes & CoordinateType.Both) == CoordinateType.Both; 
				i++)
			{
				availableCoordinateTypes = availableCoordinateTypes | drawables[i].CoordinateType;
			}


			if ((availableCoordinateTypes & CoordinateType.Screen) == CoordinateType.Screen)
				RenderingService.ScreenDrawStarted += new RenderingServiceNotifier(
					(renderComponent) => CoordinateType = CoordinateType.Screen);

			if ((availableCoordinateTypes & CoordinateType.World) == CoordinateType.World)
			RenderingService.WorldDrawStarted += new RenderingServiceNotifier(
				(renderComponent) => CoordinateType = CoordinateType.World);
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			for (int i = 0; i < drawables.Length; i++)
			{
				bool foo = this.Visible;
				if (drawables[i].CoordinateType == CoordinateType)
					spriteBatch.Draw(drawables[i].Image, drawables[i].Position, drawables[i].ImageFrame, 
						drawables[i].Tint, drawables[i].Rotation, drawables[i].DrawOrigin, drawables[i].Scale, 
						drawables[i].FlipEffects, drawables[i].LayerDepth);
			}
		}

		protected override IAspect CloneSubclass()
		{
			RenderingAspect ra = new RenderingAspect();
			ra.Visible = this.Visible;
			return ra;
		}
	}
}
