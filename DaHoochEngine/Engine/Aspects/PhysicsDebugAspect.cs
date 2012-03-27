using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectDaHooch
{
	public class PhysicsDebugAspect : UpdateAndDrawableAspect
	{
		private DemoBaseXNA.PhysicsSimulatorView farseerDebugView;
		private IPhysicsService PhysicsService;
		private IRenderingService RenderingService;
		private float preLayer;

		private bool keyDown = false;

		public float LayerDepth
		{
			get
			{
				if (Initialized)
					return farseerDebugView.LayerDepth;
				else
					return preLayer;
			}
			set
			{
				if (Initialized)
					farseerDebugView.LayerDepth = value;
				else
					preLayer = value;
			}
		}

		protected override void InitSubclass()
		{
			this.CoordinateType = CoordinateType.World;
			PhysicsService = (IPhysicsService)Owner.Game.Services.GetService(typeof(IPhysicsService));
			farseerDebugView = new DemoBaseXNA.PhysicsSimulatorView(PhysicsService.Simulator);
			RenderingService = (IRenderingService)Owner.Game.Services.GetService(typeof(IRenderingService));
			RenderingService.Register(this);
			this.DrawOrder = 0;
			this.Visible = true;
			farseerDebugView.LoadContent(Owner.Game.GraphicsDevice, Owner.Game.Content);
			farseerDebugView.EnablePerformancePanelBodyCount = true;
			farseerDebugView.LayerDepth = preLayer;
		}

		public override void Update(GameTime gameTime)
		{
			KeyboardState keystate = Keyboard.GetState();
			if (keystate.IsKeyDown(Keys.P))
				keyDown = true;
			else if (keyDown && keystate.IsKeyUp(Keys.P))
			{
				keyDown = false;
				this.Visible = !this.Visible;
			}
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			switch (CoordinateType)
			{
				case CoordinateType.World:
					farseerDebugView.EnableAllNonPanelObjects = true;
					farseerDebugView.EnablePerformancePanelView = false;
					CoordinateType = CoordinateType.Screen;
					break;
				case CoordinateType.Screen:
					farseerDebugView.EnableAllNonPanelObjects = false;
					farseerDebugView.EnablePerformancePanelView = true;
					CoordinateType = CoordinateType.World;
					break;
			}

			farseerDebugView.Draw(spriteBatch);
		}

		protected override IAspect CloneSubclass()
		{
			PhysicsDebugAspect pa = new PhysicsDebugAspect();
			return pa;
		}
	}
}
