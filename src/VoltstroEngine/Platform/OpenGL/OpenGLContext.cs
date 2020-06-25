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

			Logger.Log("OpenGL Info:", LogVerbosity.Debug);
			Logger.Log($"	Vendor: {Gl.GetString(StringName.Vendor)}", LogVerbosity.Debug);
			Logger.Log($"	Renderer: {Gl.GetString(StringName.Renderer)}", LogVerbosity.Debug);
			Logger.Log($"	Version: {Gl.GetString(StringName.Version)}", LogVerbosity.Debug);

			Gl.GetInteger(GetPName.MajorVersion, out int versionMajor);
			Gl.GetInteger(GetPName.MinorVersion, out int versionMinor);

			Logger.Log($"Running OpenGL Version {versionMajor}.{versionMinor}", LogVerbosity.Debug);

			if (versionMajor >= 4 && (versionMajor != 4 || versionMinor >= 5)) return;

			Debug.Assert(false, "Voltstro Engine excepts at least OpenGL version 4.5!");
#if !DEBUG
			Logger.Log("Voltstro Engine excepts at least OpenGL version 4.5!", LogVerbosity.Error);
			System.Environment.Exit(-1);
#endif
		}

		public void SwapBuffers()
		{
			Glfw.SwapBuffers(window);
		}
	}
}