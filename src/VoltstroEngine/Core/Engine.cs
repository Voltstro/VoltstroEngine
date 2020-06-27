using VoltstroEngine.Core.Logging;
using VoltstroEngine.Rendering.Renderer;

namespace VoltstroEngine.Core
{
	public static class Engine
	{
		/// <summary>
		/// The name and location of the game
		/// <para>E.G: Sandbox</para>
		/// </summary>
		public static string GameName { get; private set; }

		/// <summary>
		/// Init, and runs the game
		/// </summary>
		/// <param name="entry"></param>
		/// <param name="noWindow"></param>
		public static void Init(IEntryPoint entry, bool noWindow = false)
		{
			RenderingAPI.Create();

			GameName = entry.GetGameName();
			Logger.Log($"Game name is {GameName}");

			if (!noWindow)
			{
				Application app = entry.CreateApplication();
				Renderer.Init();
				app.Run();
			}
			else
				Renderer.Init();
		}
	}
}