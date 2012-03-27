using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ProjectDaHooch
{
	public class AgeAspect : UpdateableAspect
	{
		public bool KillOwnerUponDeath { get; set; }
		public bool Alive { get; set; }
		public float AgeInSeconds{ get { return age; } }
		private GameTime currentGameTime = new GameTime();
		private bool deathTimerSet = false;
		private float deathTime;
		private float age = 0;
		private EventHandler deathListener;

		public AgeAspect()
		{
			Alive = true;
			KillOwnerUponDeath = true;
			this.Enabled = true; //TODO: Make default for UpdateableAspect Enabled == true
		}

		public override void Update(GameTime gameTime)
		{
			currentGameTime = gameTime;
			age += (float)currentGameTime.ElapsedGameTime.TotalSeconds;

			if (deathTimerSet && age >= deathTime)
			{
				deathListener(this, new EventArgs());
				Alive = false;
				if (KillOwnerUponDeath)
					Owner.Enabled = false;
				deathTimerSet = false;
			}
		}

		public void SetDeathTimer(float deltaTime, EventHandler callBack)
		{
			Alive = true;
			deathTime = age + deltaTime;
			deathListener = callBack;
			deathTimerSet = true;
		}

		public void Reset()
		{
			this.deathTimerSet = false;
			this.age = 0;
		}

		protected override IAspect CloneSubclass()
		{
			AgeAspect ageAspect = new AgeAspect();
			
			ageAspect.KillOwnerUponDeath = this.KillOwnerUponDeath;
			ageAspect.Alive = this.Alive;
			ageAspect.deathTimerSet = this.deathTimerSet;
			ageAspect.deathTime = this.deathTime;
			ageAspect.deathListener = this.deathListener;

			return ageAspect;
		}
	}
}
