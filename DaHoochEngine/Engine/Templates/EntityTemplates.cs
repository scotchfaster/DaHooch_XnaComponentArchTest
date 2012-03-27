using System;
using Microsoft.Xna.Framework;

namespace ProjectDaHooch
{
	public class EntityTemplates
	{
		public static readonly int PlayerCollisionGroup = 42;

		public static Entity MakePlayerEntity(Game game,string name, string image, Vector2 position, Rectangle frame)
		{
			Entity e = MakeCharacterEntity(game, name, image, position, frame);

			e.Aspects.FindFirst<BoxCollisionAspect>().CollisionGroup = PlayerCollisionGroup;

			var input = new PlayerInputAspect()
			{
				Name = "input",
				CollisionAspect = "collider"
			};		
			e.Aspects.Add(input);

			var squirrels = new ProjectileAspect();
			e.Aspects.Add(squirrels);

			return e;
		}

		public static Entity MakeEnemyEntity(Game game, string name, string image, Vector2 position, Rectangle frame)
		{
			Entity e = MakeCharacterEntity(game, name, image, position, frame);

			var ai = new StupidAIAspect();
			e.Aspects.Add(ai);

			return e;
		}

		public static Entity MakeCharacterEntity(Game game, string name, string image, Vector2 position, Rectangle frame)
		{
			var e = new Entity()
			{
				Game = game,
				Name = name
			};

			var renderer = new RenderingAspect()
			{
				Name = "renderer"
			};
			e.Aspects.Add(renderer);

			var sprite = new SpriteAspect()
			{
				Name = "sprite",
				ImageFile = image,
				ImageFrame = frame,
				OrientationAspect = "collider",
				LayerDepth = 0.3f,
				CoordinateType = CoordinateType.World
			};
			e.Aspects.Add(sprite);

			var collider = new BoxCollisionAspect()
			{
				Name = "collider",
				Mass = 100,
				Position = position,
				IsStatic = false,
				Dimensions = frame
			};
			e.Aspects.Add(collider);

			var respawn = new RespawnAspect()
			{
				Bounds = new Rectangle(-512, -512, 2048, 1600),
				RespawnPosition = position
			};
			e.Aspects.Add(respawn);

			return e;
		}
	}
}
