using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using System.Xml.Serialization;


namespace ProjectDaHooch
{
	[XmlInclude(typeof(Texture2D))]
	public class SpriteAspect : Aspect, ISprite
	{
		//TODO: ***URGENT Make sure that these properties don't violate the sequence of Aspect creation/init/load

		//Properties
		public virtual Vector2 Position { get { return orientationAspect.Position; } }
		public virtual float Rotation { get { return orientationAspect.Rotation; } }
		public virtual Vector2 Scale
		{
			get { return scale; }
			set { scale = MathHelper2.Clamp(value, 0f, float.MaxValue); }
		}
		public virtual float LayerDepth
		{
			get { return layerDepth; }
			set { layerDepth = MathHelper.Clamp(value, 0, 1); }
		}
        [XmlIgnore]
        [Microsoft.Xna.Framework.Content.ContentSerializerIgnore]
		public virtual Texture2D Image
		{ 
			get { return image; }
			protected set
			{
				image = value;
				if (!imageFrameInitialized)
					ImageFrame = new Rectangle(0, 0, image.Width, image.Height);
				imageInitialized = true;
			}

		}
		public string ImageFile
		{
			get { return this.imageFile; }
			set
			{
				if (Owner != null)
					Image = Owner.Game.Content.Load<Texture2D>(value);
				this.imageFile = value;

			}
		}
		public string OrientationAspect
		{
			get { return this.orientationAspectName; }
			set
			{
				if (Initialized)
				{
					OrientationAspect oa = Owner.Aspects.FindByName<OrientationAspect>(value);
					if (oa != null)
					{
						this.orientationAspectName = value;
						this.orientationAspect = oa;
					}
				}
				else
					this.orientationAspectName = value;
			}
		}

		//Automatic Properties - Value Type
		public virtual CoordinateType CoordinateType { get; set; }
		public virtual Vector2 DrawOrigin { get; set; }
		public virtual Color Tint { get; set; }
		public virtual SpriteEffects FlipEffects { get; set; }
		public virtual Rectangle ImageFrame
		{
			get { return this.imageFrame; }
			set
			{
				imageFrame = value;
				DrawOrigin = new Vector2(ImageFrame.Width / 2, ImageFrame.Height / 2);
				imageFrameInitialized = true;
			}
		}

		//Protected and Private Fields - Value Type
		protected float layerDepth;
		protected Rectangle imageFrame;
		protected string imageFile;

		private Vector2 scale;
		private Texture2D image;
		private bool imageInitialized = false;
		private bool imageFrameInitialized = false;

		//Protected and Private Fields - Reference Type
		private OrientationAspect orientationAspect;
		private string orientationAspectName;


		public SpriteAspect()
		{
			this.Scale = new Vector2(1, 1);
			this.DrawOrigin = Vector2.Zero;
			this.LayerDepth = 0;
			this.Tint = Color.White;
			this.FlipEffects = SpriteEffects.None;
		}

		protected override void InitSubclass()
		{
			if (!imageInitialized)
			{
				Image = Owner.Game.Content.Load<Texture2D>(imageFile);
			}
			this.orientationAspect = (OrientationAspect)Owner.Aspects.FindByName<OrientationAspect>(orientationAspectName);
		}

		protected override void LoadSubclassContent()
		{
			orientationAspect.Dimensions = new Rectangle((int)DrawOrigin.X, (int)DrawOrigin.Y, imageFrame.Width, imageFrame.Height);
		}

		public void FlipLeft()
		{
			this.FlipEffects = SpriteEffects.FlipHorizontally;
		}


		public void FlipRight()
		{
			this.FlipEffects = SpriteEffects.None;
		}

		protected override IAspect CloneSubclass()
		{
			SpriteAspect sa = new SpriteAspect();
			
			sa.orientationAspectName = this.orientationAspectName;
			sa.Scale = this.scale;
			sa.LayerDepth = this.layerDepth;
			sa.ImageFile = this.imageFile;
			sa.CoordinateType = this.CoordinateType;
			sa.DrawOrigin = this.DrawOrigin;
			sa.Tint = this.Tint;
			sa.FlipEffects = this.FlipEffects;
			sa.ImageFrame = this.ImageFrame;

			return sa;
		}
	}
}
