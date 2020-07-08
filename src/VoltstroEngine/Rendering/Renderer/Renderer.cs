using System.Numerics;
using VoltstroEngine.DebugTools;
using VoltstroEngine.Exceptions;
using VoltstroEngine.Rendering.Camera;
using VoltstroEngine.Rendering.Shaders;

namespace VoltstroEngine.Rendering.Renderer
{
	public static class Renderer
	{
		private static bool initialized;
		private static SceneData sceneData;

		/// <summary>
		/// Initializes the rendering system
		/// </summary>
		/// <exception cref="InitializationException"></exception>
		internal static void Init()
		{
			ProfilerTimer.Profile(() =>
			{
				if (initialized)
					throw new InitializationException("The rendering system is already initialized!");

				RenderingAPI.Init();
				Renderer2D.Init();

				initialized = true;
			});
		}

		/// <summary>
		/// Shuts down the rendering system
		/// </summary>
		/// <exception cref="InitializationException"></exception>
		internal static void Shutdown()
		{
			if (!initialized)
				throw new InitializationException("The rendering system is not initialized!");

			Renderer2D.Shutdown();
			initialized = false;
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
			shader.SetMat4("u_ViewProjection", sceneData.ViewProjectionMatrix);
			shader.SetMat4("u_Transform", transform);

			vertexArray.Bind();
			RenderingAPI.DrawIndexed(vertexArray);
		}

		#endregion
	}
}