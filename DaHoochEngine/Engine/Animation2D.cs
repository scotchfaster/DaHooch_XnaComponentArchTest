using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectDaHooch
{
	public class Animation2D
	{
		public uint CurrentFrame
		{
			get { return (uint)this.currentFrame; }
			set { this.currentFrame = MathHelper.Clamp(value, 0, FrameCount); }
		}
		public float FramesPerSecond { get; set; }
		public bool PlayItBackwards { get; set; }
		///<summary>Gets/sets size of animation frame in texels</summary>
		public Point FrameSize { get; set; }
		public uint FramesPerRow { get; set; }
		public Point FrameOrigin { get; set; }
		public Vector2 DrawOrigin { get; set; }
		public Rectangle DrawFrame
		{
			get { return drawFrame; }
			protected set { drawFrame = value; }
		}
		public Texture2D Texture
		{
			get { return texture; }
		}
		public bool LoopIt { get; set; }
		public bool IsPlaying { get; set; }

		public uint FrameCount;
		protected Texture2D texture;
		protected Rectangle drawFrame;
		protected float currentFrame;
		protected uint wholeFrame;

		public Animation2D(Texture2D loadedTexture, Point frameSize, uint frameCount, uint framesPerRow)
		{
			this.texture = loadedTexture;
			this.FrameOrigin = new Point(0, 0);
			this.FrameSize = frameSize;
			this.FrameCount = frameCount;
			this.FramesPerRow = framesPerRow;
			this.CurrentFrame = 0;
			this.wholeFrame = frameCount + 1;
			this.DrawFrame = new Rectangle(FrameOrigin.X, FrameOrigin.Y, FrameSize.X, FrameSize.Y);
			this.FramesPerSecond = 24.0f;
		}

		public virtual void Update(float deltaSec)
		{
			if (FrameCount > 1 && this.IsPlaying)
			{
				if (!this.PlayItBackwards)
				{
					this.currentFrame += this.FramesPerSecond * deltaSec;
					if (currentFrame >= this.FrameCount)
					{
						if (this.LoopIt)
							this.currentFrame = 0;
						else
						{
							this.currentFrame = this.FrameCount - 1;
							this.IsPlaying = false;
						}
					}
					else if (currentFrame < 0)
					{
						currentFrame = 0;
					}
				}
				else
				{
					this.currentFrame -= this.FramesPerSecond * deltaSec;
					if (currentFrame < 0)
					{
						if (this.LoopIt)
							this.currentFrame = this.FrameCount;
						else
						{
							this.currentFrame = 0;
							this.IsPlaying = false;
						}
					}
					else if (currentFrame >= this.FrameCount)
					{
						currentFrame = this.FrameCount - 1;
					}
				}

				if ((uint)this.currentFrame != this.wholeFrame)
				{
					this.wholeFrame = (uint)this.currentFrame;
					Point frame = new Point();
					frame.Y = (int)(this.currentFrame / (this.FramesPerRow+1));
					frame.Y = !this.PlayItBackwards ? frame.Y : -frame.Y;
					frame.X = (int)(this.currentFrame % this.FramesPerRow);

					this.drawFrame.X = this.FrameOrigin.X + (int)frame.X * this.FrameSize.X;
					this.drawFrame.Y = this.FrameOrigin.Y + (int)frame.Y * this.FrameSize.Y;
				}
			}
		}


		public Animation2D CopyOf()
		{
			return (Animation2D)this.MemberwiseClone();
		}
	}
}
