using DiscordRPC;
using VoltstroEngine.Core;
using VoltstroEngine.Core.Layers;
using VoltstroEngine.Events;
using VoltstroEngine.Rendering.Renderer;

namespace DiscordRPCExample
{
	public class GameLayer : ILayer
	{
		private DiscordManager manager;

		public void OnAttach()
		{
			manager = new DiscordManager();
			manager.Init();

			manager.SetRichPresence(new RichPresence
			{
				Assets = new Assets
				{
					LargeImageKey = "ve-logo",
					LargeImageText = "VoltstroEngine"
				},
				State = "VoltstroEngine",
				Details = "This is an example."
			});
		}

		public void OnDetach()
		{
			manager.Shutdown();
		}

		public void OnUpdate(TimeStep ts)
		{
			manager.OnUpdate();

			RenderingAPI.SetClearColor(1, 1, 1);
			RenderingAPI.Clear();
		}

		public void OnEvent(IEvent e)
		{

		}
	}

	public class GameApp : Application
	{
		public GameApp()
		{
			PushOverlay(new GameLayer());
		}
	}
}