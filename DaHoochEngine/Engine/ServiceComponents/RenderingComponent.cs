using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;

namespace ProjectDaHooch
{
	public class RenderingComponent : DrawableGameComponent, IRenderingService
	{
		private List<ICamera2D> Cameras;
		private SpriteBatch spriteBatch;
		private List<IDaHoochDrawable> drawList = new List<IDaHoochDrawable>();
		private IMessageService MessageService;

        [Microsoft.Xna.Framework.Content.ContentSerializerIgnore]
		public RenderingServiceNotifier WorldDrawStarted { get; set; }
        [Microsoft.Xna.Framework.Content.ContentSerializerIgnore]
		public RenderingServiceNotifier ScreenDrawStarted { get; set; }

		public RenderingComponent(Game game)
			: base(game)
		{
			game.Components.Add(this);
			game.Services.AddService(typeof(IRenderingService), this);
			ScreenDrawStarted = new RenderingServiceNotifier((foo) =>{});
			WorldDrawStarted = new RenderingServiceNotifier((foo)=>{});
		}

		public override void Initialize()
		{
			Cameras = new List<ICamera2D>();			
			MessageService = (IMessageService)Game.Services.GetService(typeof(IMessageService));
			base.Initialize();
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);
			base.LoadContent();
		}

		public override void Update(GameTime gameTime)
		{
			foreach(ICamera2D camera in Cameras)
			{
				camera.Update(gameTime);
			}

			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			WorldDrawStarted(this);
			foreach (ICamera2D camera in Cameras)
			{
				GraphicsDevice.Viewport = camera.Viewport;
				spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.FrontToBack,
					SaveStateMode.None, camera.GlobalTransform);
				
				foreach(IDaHoochDrawable drawable in drawList)
				{
					if ((drawable.Visible) && (drawable.CoordinateType == CoordinateType.World))
						drawable.Draw(gameTime, spriteBatch);
				}

				spriteBatch.End();
			}

			ScreenDrawStarted(this);
			spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.FrontToBack, SaveStateMode.None);
			foreach (IDaHoochDrawable drawable in drawList)
			{
				if (drawable.Visible && drawable.CoordinateType == CoordinateType.Screen)
					drawable.Draw(gameTime, spriteBatch);
			}
			spriteBatch.End();

			base.Draw(gameTime);
		}

		public void AddCamera(ICamera2D camera)
		{
			Cameras.Add(camera);
		}

		public void RemoveCamera(ICamera2D camera)
		{
			Cameras.Remove(camera);
		}

		public ICamera2D FindCamera(string name)
		{
			for (int i = 0; i < Cameras.Count; i++)
			{
				if (Cameras[i].Name == name)
					return Cameras[i];
			}

			throw new CameraNotFoundException(name);
		}

		public void Register(IDaHoochDrawable drawableObject)
		{
			drawList.Add(drawableObject);
			drawList.Sort();
		}

		public void UnRegister(IDaHoochDrawable drawableObject)
		{
			drawList.Remove(drawableObject);
		}
	}
}
