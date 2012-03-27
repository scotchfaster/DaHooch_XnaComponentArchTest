using Microsoft.Xna.Framework;

namespace ProjectDaHooch
{
	public delegate void RenderingServiceNotifier(IRenderingService RenderingService);

	public interface IRenderingService : IGameComponent
	{
		void AddCamera(ICamera2D camera);

		void RemoveCamera(ICamera2D camera);

		ICamera2D FindCamera(string name);

		void Register(IDaHoochDrawable drawableObject);

		void UnRegister(IDaHoochDrawable drawableObject);

        [Microsoft.Xna.Framework.Content.ContentSerializerIgnore]
		RenderingServiceNotifier WorldDrawStarted { get; set; }
        [Microsoft.Xna.Framework.Content.ContentSerializerIgnore]
		RenderingServiceNotifier ScreenDrawStarted { get; set; }
	}
}
