using Microsoft.Xna.Framework;
using FarseerGames.FarseerPhysics;

namespace ProjectDaHooch
{
	public interface IPhysicsService : IGameComponent
	{
		PhysicsSimulator Simulator { get; }
	}
}
