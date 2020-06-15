﻿using System.Numerics;
using VoltstroEngine.Logging;
using VoltstroEngine.Platform.OpenGL;
using VoltstroEngine.Rendering.Camera;
using VoltstroEngine.Rendering.Shaders;

namespace VoltstroEngine.Rendering
{
	public static class Renderer
	{
		private static bool initialized;

		private static IRenderingAPI renderingAPI;

		private static SceneData sceneData = new SceneData();

		/// <summary>
		/// Gets the currently in use rendering API
		/// </summary>
		/// <returns></returns>
		public static RenderingAPI GetRenderingAPI()
		{
			return renderingAPI.GetAPI();
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
		public static void Submit(IShader shader, IVertexArray vertexArray)
		{
			shader.Bind();
			shader.UploadUniformMat4("u_ViewProjection", sceneData.ViewProjectionMatrix);

			vertexArray.Bind();
			renderingAPI.DrawIndexed(vertexArray);
		}
		
		#endregion

		/// <summary>
		/// Initializes the rendering system
		/// </summary>
		internal static void Init()
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

		/// <summary>
		/// Clears the buffer
		/// </summary>
		internal static void Clear()
		{
			renderingAPI.Clear();
		}

		/// <summary>
		/// Sets the clear color
		/// </summary>
		/// <param name="red"></param>
		/// <param name="green"></param>
		/// <param name="blue"></param>
		internal static void SetClearColor(float red, float green, float blue)
		{
			renderingAPI.SetClearColor(red, green, blue, 1.0f);
		}

		private struct SceneData
		{
			public Matrix4x4 ViewProjectionMatrix;
		}
	}
}