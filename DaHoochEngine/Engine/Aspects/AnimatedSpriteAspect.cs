using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

namespace ProjectDaHooch
{
	public class AnimatedSpriteAspect : SpriteAspect, IUpdateable
	{
		//Properties
		public override Vector2 DrawOrigin
		{
			get { return this.currentAnimation.DrawOrigin; }
			set
			{
				if (this.currentAnimation != null)
					this.currentAnimation.DrawOrigin = value;
			}
		}
		public bool IsPlaying { get { return playing; } }
		public int AnimationCount { get { return animations.Count; } }
		public string CurrentAnimation
		{
			get
			{
				if (this.currentAnimation != null)
				{
					return currentAnimationName;
				}
				else
					return string.Empty;
			}
		}
		public override Texture2D Image
		{
			get { return currentAnimation.Texture; }
			protected set { throw new InvalidOperationException("You're trying to set texture of an AnimatedSpriteAspect. DON'T DO IT." + 
				"Instead, use some of the animation control methods."); }
		}
		public override Rectangle ImageFrame
		{
			get { return currentAnimation.DrawFrame; }
			set { throw new InvalidOperationException("You're trying to set the image frame rectangle of an AnimatedSpriteAspect. DON'T DO IT." +
				"Instead, use some of the animation control methods."); }
		}

		//Automatic Properties
		public bool IsLooping { get; set; }
		
		//Protected and Private Members
		private Dictionary<string, Animation2D> animations;
		private Animation2D currentAnimation;
		protected bool playing = false;
		private string currentAnimationName;



		public AnimatedSpriteAspect()
		{
			this.animations = new Dictionary<string, Animation2D>();
		}

		public virtual void AddAnimation(string animationName, Animation2D animation)
		{
			this.animations.Add(animationName, animation);
			if (this.currentAnimation == null)
			{
				this.currentAnimation = animation;
				this.currentAnimationName = animationName;
			}
		}

		public virtual void ChangeCurrentAnimation(string animationName)
		{
			Animation2D tempAnim;
			this.animations.TryGetValue(animationName, out tempAnim);
			if (this.currentAnimation != tempAnim)
			{
				if (!this.animations.TryGetValue(animationName, out currentAnimation))
				{
					throw new ArgumentOutOfRangeException("string animationName",
						"That animation does not exist in this Sprite's list of animations.");
				}
				this.currentAnimationName = animationName;
			}
		}

		public virtual void AddNewAnimation(string name, Texture2D loadedTexture, Point frameOrigin, Point frameSize, uint frameCount, uint framesPerRow)
		{
			Animation2D newAnim = new Animation2D(loadedTexture, frameSize, frameCount, framesPerRow);
			newAnim.FrameOrigin = frameOrigin;
			this.AddAnimation(name, newAnim);
		}


		public virtual void Update(GameTime gameTime)
		{
			currentAnimation.IsPlaying = this.playing;
			currentAnimation.LoopIt = this.IsLooping;
			currentAnimation.Update((float)gameTime.ElapsedGameTime.Milliseconds / 1000.0f);
		}


		public Animation2D GetAnimation(string name)
		{
			Animation2D anim = null;
			this.animations.TryGetValue(name, out anim);
			return anim;
		}


		public virtual void Play()
		{
			playing = true;
		}


		public virtual void Play(uint frame)
		{
			this.currentAnimation.CurrentFrame = frame;
			playing = true;
		}


		public virtual void Pause()
		{
			playing = false;
		}

		public virtual void Stop()
		{
			if (!this.currentAnimation.PlayItBackwards)
				this.currentAnimation.CurrentFrame = 0;
			else if (this.currentAnimation.PlayItBackwards)
				this.currentAnimation.CurrentFrame = this.currentAnimation.FrameCount - 1;

			this.currentAnimation.Update(0);
			playing = false;
		}

		public void SetAnimationOriginsToCenter()
		{
			foreach (Animation2D anim in animations.Values)
			{
				anim.DrawOrigin = new Vector2(anim.FrameSize.X / 2.0f, anim.FrameSize.Y / 2.0f);
			}
		}


		public void SetAnimationOriginsToTopLeft()
		{
			foreach (Animation2D anim in animations.Values)
			{
				anim.DrawOrigin = new Vector2(0, 0);
			}
		}

		protected override IAspect CloneSubclass()
		{
			AnimatedSpriteAspect asa = new AnimatedSpriteAspect();

			asa.OrientationAspect = this.OrientationAspect;
			asa.Scale = this.Scale;
			asa.LayerDepth = this.layerDepth;
			asa.CoordinateType = this.CoordinateType;
			asa.Tint = this.Tint;
			asa.FlipEffects = this.FlipEffects;
			asa.IsLooping = this.IsLooping;
			asa.animations = new Dictionary<string,Animation2D>(this.animations);
			if (this.currentAnimation != null)
				asa.ChangeCurrentAnimation(this.currentAnimationName);
			asa.playing = this.playing;

			return asa;
		}

		#region IUpdateable Members

		private int updateOrder;
		private bool enabled;

		public bool Enabled
		{
			get { return enabled; }
			protected set { enabled = value; EnabledChanged(this, new EventArgs()); }
		}

		public event System.EventHandler EnabledChanged;

		public int UpdateOrder
		{
			get { return updateOrder; }
			protected set { updateOrder = value; UpdateOrderChanged(this, new EventArgs()); }
		}

		public event System.EventHandler UpdateOrderChanged;

		#endregion
	}
}
