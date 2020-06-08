using System;
using System.Diagnostics;
using GLFW;
using VoltstroEngine.Logging;
using VoltstroEngine.Platform.OpenGL;
using VoltstroEngine.Rendering;
using VoltstroEngine.Window;

namespace VoltstroEngine.Platform.Windows
{
	/// <summary>
	/// A window for Windows
	/// </summary>
	public class WindowsWindow : IWindow
	{
		private static bool glfwInitialized;

		private WindowProperties windowProperties;
		private GLFW.Window window;

		private IGraphicsContext context;

		public WindowsWindow(WindowProperties properties)
		{
			Init(properties);
		}

		public void OnUpdate()
		{
			Glfw.PollEvents();
			context.SwapBuffers();
		}

		public int GetWidth()
		{
			return windowProperties.Width;
		}

		public int GetHeight()
		{
			return windowProperties.Height;
		}

		public void SetVSync(bool enable)
		{
			Glfw.SwapInterval(enable ? 1 : 0);
			windowProperties.VSync = enable;
		}

		public bool IsVSync()
		{
			return windowProperties.VSync;
		}

		private void Init(WindowProperties properties)
		{
			Logger.Log("Initializing a window for Windows...", LogVerbosity.Debug);

			//Initialize glfw if it hasn't already
			if (!glfwInitialized)
			{
				bool success = Glfw.Init();
				Debug.Assert(success, "GLFW failed to init!");

				Glfw.SetErrorCallback(ErrorHandler);

				glfwInitialized = true;
			}

			//Set the properties and create the window
			windowProperties = properties;
			window = Glfw.CreateWindow(properties.Width, properties.Height, properties.Title, Monitor.None, GLFW.Window.None);

			//Create context
			context = (Renderer.GetRenderingAPI()) switch
			{
				RenderingAPI.OpenGL => new OpenGLContext(window),
				_ => throw new ArgumentOutOfRangeException(),
			};

			//Init the context
			context.Init();

			SetVSync(properties.VSync);

			Logger.Log($"Created a window for Windows ({properties.Width}x{properties.Height}, {properties.VSync})", LogVerbosity.Debug);
		}

		private static void ErrorHandler(ErrorCode code, IntPtr message)
		{
			Logger.Log($"GLFW Error: {code}:{message}", LogVerbosity.Error);
		}
	}
}