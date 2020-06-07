using VoltstroEngine.Logging;
using VoltstroEngine.Platform.OpenGL;

namespace VoltstroEngine.Rendering
{
	public static class Renderer
	{
		private static bool initialized;

		private static IRenderingAPI renderingAPI;

		/// <summary>
		/// Gets the currently in use rendering API
		/// </summary>
		/// <returns></returns>
		public static RenderingAPI GetRenderingAPI()
		{
			//For now, we only have one...
			return RenderingAPI.OpenGL;
		}

		/// <summary>
		/// Initializes the rendering system
		/// </summary>
		public static void Init()
		{
			if (initialized)
			{
				Logger.Log("The rendering api is already initialized!", LogVerbosity.Error);
				return;
			}

			renderingAPI = new OpenGLRenderingAPI();
			renderingAPI.Init();
			initialized = true;
		}
	}
}