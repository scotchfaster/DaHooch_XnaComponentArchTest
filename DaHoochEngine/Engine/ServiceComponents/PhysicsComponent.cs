using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerGames.FarseerPhysics;

namespace ProjectDaHooch
{
	public class PhysicsComponent : GameComponent, IPhysicsService
	{
		public PhysicsSimulator Simulator { get { return simulator; } }
		public Vector2 Gravity { get; set; }

		private PhysicsSimulator simulator;

		public PhysicsComponent(Game game)
			: base(game)
		{
			Gravity = new Vector2(0, 100);
			simulator = new PhysicsSimulator(this.Gravity);
			this.Game.Services.AddService(typeof(IPhysicsService), this);
			this.Game.Components.Add(this);
		}

		public override void Initialize()
		{
			base.Initialize();
		}

		public override void Update(GameTime gameTime)
		{
			simulator.Update((float)gameTime.ElapsedGameTime.TotalSeconds, (float)gameTime.ElapsedRealTime.TotalSeconds);
			base.Update(gameTime);
		}
	}
}
