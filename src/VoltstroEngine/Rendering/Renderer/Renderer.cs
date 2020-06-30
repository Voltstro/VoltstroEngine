using System.Numerics;
using VoltstroEngine.Core.Logging;
using VoltstroEngine.DebugTools;
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
		internal static void Init()
		{
			InstrumentationTimer rendererTimer = InstrumentationTimer.Create("Renderer Init");
			if (initialized)
			{
				Logger.Log("The rendering api is already initialized!", LogVerbosity.Error);
				rendererTimer.Stop();
				return;
			}

			RenderingAPI.Init();
			Renderer2D.Init();

			initialized = true;
			rendererTimer.Stop();
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