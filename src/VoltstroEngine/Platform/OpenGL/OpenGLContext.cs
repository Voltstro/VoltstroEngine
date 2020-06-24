using System.Diagnostics;
using GLFW;
using OpenGL;
using VoltstroEngine.Core.Logging;
using VoltstroEngine.Rendering;
using Exception = System.Exception;

namespace VoltstroEngine.Platform.OpenGL
{
	public class OpenGLContext : IGraphicsContext
	{
		private readonly Window window;

		public OpenGLContext(Window window)
		{
			this.window = window;
		}

		public void Init()
		{
			Logger.Log("Initializing OpenGL...", LogVerbosity.Debug);
			try
			{
				Gl.Initialize();
				Glfw.MakeContextCurrent(window);
			}
			catch (Exception ex)
			{
				Debug.Assert(false, $"Some error occured while initializing OpenGL for GLFW!\n{ex}");
#if !DEBUG
				Logger.Log(ex.Message, LogVerbosity.Error);
#endif
			}

			Logger.Log("OpenGL Initialized", LogVerbosity.Debug);

			Logger.Log("OpenGL Info:");
			Logger.Log($"	Vendor: {Gl.GetString(StringName.Vendor)}");
			Logger.Log($"	Renderer: {Gl.GetString(StringName.Renderer)}");
			Logger.Log($"	Version: {Gl.GetString(StringName.Version)}");
		}

		public void SwapBuffers()
		{
			Glfw.SwapBuffers(window);
		}
	}
}