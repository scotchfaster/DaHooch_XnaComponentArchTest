using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectDaHooch
{
	public static class FirstLevel
	{

		public static int PlayerID = 0;
		public static int backdropID = 0;

		public static void LoadFirstLevel(Level level, Game game)
		{
			//Using prefab entity creation methods
			Entity platform = NewStaticObjectEntity(game, "platform1", "Sprites/platform",
				new Vector2(300, 350), 0.2f, CoordinateType.World);

			Entity platform2 = NewStaticObjectEntity(game, "platform2", "Sprites/platform",
				new Vector2(864, 400), 0.2f, CoordinateType.World);

			Entity platform3 = NewStaticObjectEntity(game, "platform3", "Sprites/crate",
				new Vector2(1408, 420), 0.2f, CoordinateType.World);

			Entity player = EntityTemplates.MakePlayerEntity(game, "PLAYER", "Sprites/hobosquirrel",
				new Vector2(60, 10), new Rectangle(0, 0, 30, 56));

			PlayerID = player.EntityID;
			//Constructing backdrop entity
			Entity backdrop = new Entity();
			backdrop.Game = game;
			backdropID = backdrop.EntityID;

			SpriteAspect sprite = new SpriteAspect();
			sprite.OwnerID = backdrop.EntityID;
			sprite.Name = "sprite";
			sprite.OrientationAspect = "orientation";
			backdrop.Aspects.Add(sprite);

			OrientationAspect orientation = new OrientationAspect();
			orientation.OwnerID = backdrop.EntityID;
			orientation.Name = "orientation";
			backdrop.Aspects.Add(orientation);

			RenderingAspect render = new RenderingAspect();
			render.OwnerID = backdrop.EntityID;
			render.Name = "renderer";
			backdrop.Aspects.Add(render);

			backdrop.Aspects.FindFirst<SpriteAspect>().CoordinateType = CoordinateType.World;
			backdrop.Aspects.FindFirst<SpriteAspect>().ImageFile = "Sprites/Blue hills";
			backdrop.Aspects.FindFirst<SpriteAspect>().Scale = new Vector2(2.0f, 2.0f);
			backdrop.Aspects.FindFirst<SpriteAspect>().LayerDepth = 0.1f;

			//Constructing physics debug view entity
			Entity physicsDebugger = new Entity();
			physicsDebugger.Game = game;
			PhysicsDebugAspect phydebug = new PhysicsDebugAspect();
			phydebug.OwnerID = physicsDebugger.EntityID;
			phydebug.Name = "farseer";
			phydebug.LayerDepth = 1;
			physicsDebugger.Aspects.Add(phydebug);
			physicsDebugger.Name = "PHYDEBUGGER";
			level.Entities.Add(physicsDebugger);

			//Add Entities to level
			platform.Name = "PLATFORM1";
			platform2.Name = "PLATFORM2";
			platform3.Name = "PLATFORM3";
			backdrop.Name = "BACKDROP";

			Entity enemy = EntityTemplates.MakeEnemyEntity(game, "Enemy1", "Sprites/funnycaveman", 
				new Vector2(350, 60), new Rectangle(0, 0, 70, 46));
			level.Entities.Add(enemy);

			level.Entities.Add(platform);
			level.Entities.Add(platform2);
			level.Entities.Add(platform3);
			level.Entities.Add(player);
			level.Entities.Add(backdrop);

			//Load level (initialize entities)
			//level.Load(); //DO IT YOURSELF


			//Ignore this crap, I was playing with sprite ordering
			//((RenderingAspect)player.Aspects.FindFirst<RenderingAspect>()).Visible = false;
			//((RenderingAspect)platform.Aspects.FindFirst<RenderingAspect>()).Visible = false;
			//((RenderingAspect)backdrop.Aspects.FindFirst<RenderingAspect>()).Visible = false;

			//((RenderingAspect)player.Aspects.FindFirst<RenderingAspect>()).DrawOrder = 3;
			//((RenderingAspect)platform.Aspects.FindFirst<RenderingAspect>()).DrawOrder = 2;
			//((RenderingAspect)backdrop.Aspects.FindFirst<RenderingAspect>()).DrawOrder = 1;
		}

		public static Entity NewStaticObjectEntity(Game game, string name, string image,
			Vector2 position, float layerDepth, CoordinateType coordinateType)
		{
			Entity e = new Entity();
			e.Game = game;
			e.Name = name;

			RenderingAspect renderer = new RenderingAspect();
			renderer.Name = "renderer";
			e.Aspects.Add(renderer);

			SpriteAspect sprite = new SpriteAspect();
			sprite.Name = "sprite";
			sprite.ImageFile = image;
			sprite.OrientationAspect = "collider";
			sprite.LayerDepth = layerDepth;
			sprite.CoordinateType = CoordinateType.World;
			e.Aspects.Add(sprite);

			BoxCollisionAspect collider = new BoxCollisionAspect();
			collider.Dimensions = sprite.ImageFrame;
			collider.Name = "collider";
			collider.Mass = 100;
			collider.Position = position;
			collider.IsStatic = true;
			e.Aspects.Add(collider);

			return e;
		}

		public static Entity NewPlayerEntity(Game game, string image, Vector2 position, Rectangle frame)
		{
			Entity e = NewStaticObjectEntity(game, "PLAYER", image, position, 0.3f, CoordinateType.World);

			e.Aspects.FindFirst<SpriteAspect>().ImageFrame = frame;
			e.Aspects.FindFirst<BoxCollisionAspect>().Dimensions = frame;
			e.Aspects.FindFirst<BoxCollisionAspect>().IsStatic = false;

			PlayerInputAspect input = new PlayerInputAspect();
			input.Name = "input";
			input.CollisionAspect = "collider";
			e.Aspects.Add(input);

			ProjectileAspect squirrels = new ProjectileAspect();
			e.Aspects.Add(squirrels);

			return e;
		}
	}
}
