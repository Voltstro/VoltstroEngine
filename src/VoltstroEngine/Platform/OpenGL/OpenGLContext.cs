using System;
using GLFW;
using OpenGL;
using VoltstroEngine.Logging;
using VoltstroEngine.Rendering;

namespace VoltstroEngine.Platform.OpenGL
{
	public class OpenGLContext : IGraphicsContext
	{
		private readonly GLFW.Window window;

		public OpenGLContext(GLFW.Window window)
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
			catch (InvalidOperationException ex)
			{
				Console.WriteLine(ex);
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