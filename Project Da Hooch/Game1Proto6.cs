using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System.Diagnostics;
using System;
#if EDITOR
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
#endif

namespace ProjectDaHooch
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1Proto6 : Microsoft.Xna.Framework.Game
	{
		const float camMoveSpeed = 5.0f;
		const float camRotateSpeed = 0.01f;
		const float camZoomSpeed = 0.01f;

		private GraphicsDeviceManager graphics;
		private ContentManager contentManager;
		private KeyboardState keyboardState;
		private GamePadState gamePadState;
		private GameTime currentGameTime;

		private IMessageService MessageSerivce;
		private IRenderingService RenderingService;
		private IPhysicsService PhysicsService;
		private List<Entity> EntityList = new List<Entity>();
		private Level level;
		private BasicCamera camera;
#if EDITOR
		const int SAVE_MODE = 1;
		const int OPEN_MODE = 2;

        private int levelMode = SAVE_MODE;
        //private int levelMode = OPEN_MODE;
#endif
        public Game1Proto6()
		{
			graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
			//contentManager = new ContentManager(this.Services);

			PhysicsService = (IPhysicsService)new PhysicsComponent(this);
			PhysicsService.Simulator.Gravity.Y = 500;
			MessageSerivce = (IMessageService)new MessageDispatcher(this);
			RenderingService = (IRenderingService)new RenderingComponent(this);
		}




		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{

			base.Initialize();
		}




		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			camera = new BasicCamera(GraphicsDevice.Viewport);
			RenderingService.AddCamera(camera);
			camera.MoveSpeed = 5f;

#if EDITOR
			switch (levelMode)
			{
				case SAVE_MODE:
                    level = new Level();
					FirstLevel.LoadFirstLevel(level, this);
					break;
				case OPEN_MODE:
                    //Level level2 = new Level();
                    //level2.game = this;
                    //FirstLevel.LoadFirstLevel(level2, this); // HACK stupid hack to read in Aspect types, won't need in the future
                    //FileStream file = new FileStream("../../../xmlTest.xml", FileMode.Open);
                    //XmlSerializer xml = new XmlSerializer(typeof(Level), SerializerInfo.GetTypes());
                    //level = (Level)xml.Deserialize(file);
                    level = Content.Load<Level>("xnaXmlTest");
					break;
			}
#else
            level = Content.Load<Level>("xnaXmlTest2");
#endif
			level.Load(this);
						
			Entity player = level.GetFirstEntityByName("PLAYER");
			camera.FollowSprite(player.Aspects.FindFirst<SpriteAspect>());
		}




		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}




		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			currentGameTime = gameTime;
			keyboardState = Keyboard.GetState();

			foreach (Entity e in EntityList)
				e.Update(gameTime);

			Vector2 camMove = new Vector2(0);

			if (keyboardState.IsKeyDown(Keys.W))
				camMove.Y += camMoveSpeed;
			if (keyboardState.IsKeyDown(Keys.S))
				camMove.Y -= camMoveSpeed;
			if (keyboardState.IsKeyDown(Keys.A))
				camMove.X += camMoveSpeed;
			if (keyboardState.IsKeyDown(Keys.D))
				camMove.X -= camMoveSpeed;
			if (keyboardState.IsKeyDown(Keys.OemPlus))
				camera.Zoom += camZoomSpeed;
			if (keyboardState.IsKeyDown(Keys.OemMinus))
				camera.Zoom -= camZoomSpeed;
			if (keyboardState.IsKeyDown(Keys.E))
				camera.Rotation += camRotateSpeed;
			if (keyboardState.IsKeyDown(Keys.Q))
				camera.Rotation -= camRotateSpeed;

			camera.Position += camMove;

			if (keyboardState.IsKeyDown(Keys.Escape))
			{
#if EDITOR
				if (levelMode == SAVE_MODE)
				{
                   Entity player = Entity.GetEntityByID(FirstLevel.PlayerID);
                   string filePath = Content.RootDirectory + @"\..\..\..\..\Content\xnaXmlTest2.xml";
                   FileStream file = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite);
                   IntermediateSerializer.Serialize<Level>(XmlWriter.Create(file), level, filePath);
				}
#endif
                this.Exit();
			}

			level.Update(gameTime);

			base.Update(gameTime);
		}




		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			base.Draw(gameTime);
		}
	}
}
