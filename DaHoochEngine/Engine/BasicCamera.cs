using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ProjectDaHooch
{
	public class BasicCamera : ICamera2D
	{
		//Properties
		public string Name { get; set; }
		public bool EnableSpriteFollow { get; set; }
		public Viewport Viewport { get; set; }
		public Vector2 Origin
		{
			get { return new Vector2(origin.X, origin.Y); }
		}
		public Vector2 Position
		{
			get { return new Vector2(position.X, position.Y); }
			set { position.X = value.X; position.Y = value.Y; }
		}
		public Rectangle WorldBoundaries { get; set; }
		public float MoveSpeed { get; set; }
		public float Rotation
		{
			get { return rotation; }
			set { rotation = MathHelper.WrapAngle(value); }
		}
		public float Zoom
		{
			get { return zoom; }
			set { zoom = MathHelper.Clamp(value, 0.0f, float.MaxValue); }
		}
		public Matrix GlobalTransform { get { return this.globalTransform; } }

		//Private Fields
		private Matrix globalTransform;
		private Vector3 origin;
		private Vector3 position;
		private float rotation;
		private float zoom;
		private ISprite spriteToFollow;
		private Vector3 velocity;


		public BasicCamera(Viewport viewport)
		{
			Initialize(viewport);
		}

		public BasicCamera(Viewport viewport, string cameraName)
		{
			this.Name = cameraName;
			Initialize(viewport);
		}

		private void Initialize(Viewport viewport)
		{
			this.Viewport = viewport;
			origin.X = viewport.X + (viewport.Width / 2);
			origin.Y = viewport.Y + (viewport.Height / 2);
			position = new Vector3(0);
			rotation = 0f;
			zoom = 1.0f;
		}

		public bool HasSpriteInView(ISprite sprite)
		{
			throw new System.NotImplementedException("Soon, but not now!");
		}

		public void MoveTo(Vector2 point)
		{
			throw new System.NotImplementedException("This will take a little while, got other things on my plate!");
		}

		public void FollowSprite(ISprite sprite)
		{
			spriteToFollow = sprite;
			EnableSpriteFollow = true;
		}

		public void Update(GameTime gameTime)
		{
			if (EnableSpriteFollow)
			{
				Vector3 viewPosition = position - origin;

				viewPosition.X = MathHelper.Lerp(viewPosition.X, -spriteToFollow.Position.X, 0.1f);
				viewPosition.Y = MathHelper.Lerp(viewPosition.Y, -spriteToFollow.Position.Y, 0.1f);

				position = viewPosition + origin;
			}

			globalTransform = Matrix.CreateTranslation(-(origin - position))
				* Matrix.CreateScale(Zoom, Zoom, 0)
				* Matrix.CreateRotationZ(Rotation)
				* Matrix.CreateTranslation((origin - position))
				* Matrix.CreateTranslation(position);
		}
	}
}
