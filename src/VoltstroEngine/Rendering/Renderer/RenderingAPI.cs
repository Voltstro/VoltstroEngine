using VoltstroEngine.Platform.OpenGL;

namespace VoltstroEngine.Rendering.Renderer
{
	public static class RenderingAPI
	{
		private static IRenderingAPI renderingAPI;

		/// <summary>
		/// Creates the rendering API (E.G: OpenGl, Vulkan)
		/// </summary>
		internal static void Create()
		{
			renderingAPI = new OpenGLRenderingAPI();
		}

		/// <summary>
		/// Inits the rendering API
		/// </summary>
		internal static void Init()
		{
			renderingAPI.Init();
		}

		/// <summary>
		/// Gets the currently in use rendering API
		/// </summary>
		/// <returns></returns>
		public static RenderingAPIType GetRenderingAPI()
		{
			return renderingAPI.GetAPI();
		}

		/// <summary>
		/// Call on window resize
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		internal static void OnWindowResize(uint width, uint height)
		{
			renderingAPI.SetViewport(0, 0, width, height);
		}

		/// <summary>
		/// Clears the buffer
		/// </summary>
		public static void Clear()
		{
			renderingAPI.Clear();
		}

		/// <summary>
		/// Sets the clear color
		/// </summary>
		/// <param name="red"></param>
		/// <param name="green"></param>
		/// <param name="blue"></param>
		public static void SetClearColor(float red, float green, float blue)
		{
			renderingAPI.SetClearColor(red, green, blue, 1.0f);
		}

		/// <summary>
		/// Draws something to the screen
		/// </summary>
		/// <param name="vertexArray"></param>
		public static void DrawIndexed(IVertexArray vertexArray)
		{
			renderingAPI.DrawIndexed(vertexArray);
		}
	}
}