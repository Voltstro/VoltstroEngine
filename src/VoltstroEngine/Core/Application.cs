using System.Diagnostics;
using VoltstroEngine.Core.Layers;
using VoltstroEngine.Core.Window;
using VoltstroEngine.DebugTools;
using VoltstroEngine.Events;
using VoltstroEngine.Rendering.Renderer;

namespace VoltstroEngine.Core
{
	/// <summary>
	/// Main application
	/// </summary>
	public class Application
	{
		private static Application app;

		private readonly LayerStack layerStack;

		private readonly IWindow window;
		private bool isRunning = true;

		private float lastTime;
		private bool minimized;

		public Application()
		{
			InstrumentationTimer appTimer = InstrumentationTimer.Create("Application Create");

			if (app != null)
			{
				Debug.Assert(false, "A running app already exists!");
#if !DEBUG
				Logging.Logger.Log("A running app already exists!", Logging.LogVerbosity.Error);
				return;
#endif
			}

			app = this;

			//Creates a new window
			window = IWindow.CreateWindow(new WindowProperties
			{
				Title = "Voltstro Engine",
				Width = 1280,
				Height = 720,
				VSync = true
			});
			window.OnEvent += WindowOnOnEvent;

			layerStack = new LayerStack();
			appTimer.Stop();
		}

		private void WindowOnOnEvent(IEvent e)
		{
			EventDispatcher.DispatchEvent<WindowCloseEvent>(e, OnClose);
			EventDispatcher.DispatchEvent<WindowResizedEvent>(e, WindowResize);

			foreach (ILayer layer in layerStack.GetLayers())
				layer.OnEvent(e);
		}

		private void WindowResize(WindowResizedEvent e)
		{
			if (e.Width == 0 || e.Height == 0)
			{
				minimized = true;
				return;
			}

			minimized = false;
			RenderingAPI.OnWindowResize((uint) e.Width, (uint) e.Height);
		}

		private void OnClose(WindowCloseEvent e)
		{
			isRunning = false;
		}

		/// <summary>
		/// Runs the main application loop
		/// </summary>
		public void Run()
		{
			while (isRunning)
			{
				InstrumentationTimer runLoopTimer = InstrumentationTimer.Create("Run Loop");
				float time = window.GetTime();
				TimeStep timeStep = new TimeStep(time - lastTime);
				lastTime = time;

				//LayerStack update
				{
					InstrumentationTimer layerStackTimer = InstrumentationTimer.Create("LayerStack.OnUpdate");
					if (!minimized)
					{
						foreach (ILayer layer in layerStack.GetLayers())
							layer.OnUpdate(timeStep);
					}
					layerStackTimer.Stop();
				}

				//Window update
				{
					InstrumentationTimer windowUpdateTimer = InstrumentationTimer.Create("Window.OnUpdate");
					window.OnUpdate();
					windowUpdateTimer.Stop();
				}
				
				runLoopTimer.Stop();
			}
		}

		internal static void Shutdown()
		{
			app.window.Shutdown();
		}

		/// <summary>
		/// Adds a layer
		/// </summary>
		/// <param name="layer"></param>
		public void PushLayer(ILayer layer)
		{
			layerStack.PushLayer(layer);
			layer.OnAttach();
		}

		public void PushOverlay(ILayer layer)
		{
			layerStack.PushOverlay(layer);
			layer.OnAttach();
		}
	}
}