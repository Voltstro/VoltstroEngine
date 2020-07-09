using System.Diagnostics;
using GLFW;
using OpenGL;
using VoltstroEngine.Core.Logging;
using VoltstroEngine.DebugTools;
using VoltstroEngine.Rendering.Renderer;
using Exception = System.Exception;

namespace VoltstroEngine.Platform.OpenGL
{
	internal sealed class OpenGLContext : IGraphicsContext
	{
		private readonly Window window;

		public OpenGLContext(Window window)
		{
			this.window = window;
		}

		public void Init()
		{
			ProfilerTimer.Profile(() =>
			{
				Logger.Info("Initializing OpenGL context...");
				try
				{
					Gl.Initialize();
					Glfw.MakeContextCurrent(window);
				}
				catch (Exception ex)
				{
					Debug.Assert(false, $"Some error occured while initializing OpenGL for GLFW!\n{ex}");
#if !DEBUG
					Logger.Error("An error occured while initializing OpenGL for GLFW!\n{@Message}", ex);
#endif
				}
			});
			Logger.Info("OpenGL context initialized!");

			Logger.Info("OpenGL Info:");
			Logger.Info("	Vendor: {@Vendor}", Gl.GetString(StringName.Vendor));
			Logger.Info("	Renderer: {@Renderer}", Gl.GetString(StringName.Renderer));
			Logger.Info("	Version: {@Version}", Gl.GetString(StringName.Version));

			Gl.GetInteger(GetPName.MajorVersion, out int versionMajor);
			Gl.GetInteger(GetPName.MinorVersion, out int versionMinor);

			Logger.Info("Running OpenGL Version {@VersionMajor}.{@VersionMinor}", versionMajor, versionMinor);

			if (versionMajor >= 4 && (versionMajor != 4 || versionMinor >= 5)) return;

			Debug.Assert(false, "Voltstro Engine excepts at least OpenGL version 4.5!");
#if !DEBUG
			Logger.Error("Voltstro Engine excepts at least OpenGL version 4.5!");
			System.Environment.Exit(-1);
#endif
		}

		public void SwapBuffers()
		{
			ProfilerTimer.Profile(() =>
			{
				Glfw.PollEvents();
				Glfw.SwapBuffers(window);
			});
		}
	}
}