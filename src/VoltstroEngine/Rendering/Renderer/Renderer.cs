using System.Numerics;
using VoltstroEngine.Core.Logging;
using VoltstroEngine.Platform.OpenGL;
using VoltstroEngine.Rendering.Camera;
using VoltstroEngine.Rendering.Shaders;

namespace VoltstroEngine.Rendering.Renderer
{
	public static class Renderer
	{
		private static bool initialized;

		public static IRenderingAPI renderingAPI;

		private static SceneData sceneData;

		/// <summary>
		/// Gets the currently in use rendering API
		/// </summary>
		/// <returns></returns>
		public static RenderingAPI GetRenderingAPI()
		{
			return renderingAPI.GetAPI();
		}

		/// <summary>
		/// Creates the rendering system
		/// </summary>
		public static void Create()
		{
			renderingAPI = new OpenGLRenderingAPI();
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

			Renderer2D.Init();

			renderingAPI.Init();
			initialized = true;
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
		/// Call on window resize
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		internal static void OnWindowResize(uint width, uint height)
		{
			renderingAPI.SetViewport(0, 0, width, height);
		}

		private struct SceneData
		{
			public Matrix4x4 ViewProjectionMatrix;
		}

		#region Rendering Stuff to Screen

		/// <summary>
		/// Starts a new scene
		/// </summary>
		public static void BeginScene(OrthographicCamera camera)
		{
			sceneData.ViewProjectionMatrix = camera.ViewProjectionMatrix;
		}

		/// <summary>
		/// Ends the current scene
		/// </summary>
		public static void EndScene()
		{
		}

		/// <summary>
		/// Submit something to be drawn
		/// </summary>
		/// <param name="shader"></param>
		/// <param name="vertexArray"></param>
		/// <param name="transform"></param>
		public static void Submit(IShader shader, IVertexArray vertexArray, Matrix4x4 transform)
		{
			shader.Bind();
			shader.UploadUniformMat4("u_ViewProjection", sceneData.ViewProjectionMatrix);
			shader.UploadUniformMat4("u_Transform", transform);

			vertexArray.Bind();
			renderingAPI.DrawIndexed(vertexArray);
		}

		#endregion
	}
}