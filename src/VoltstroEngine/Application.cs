﻿using System.Diagnostics;
using System.Reflection;
using VoltstroEngine.Events;
using VoltstroEngine.Layers;
using VoltstroEngine.Logging;
using VoltstroEngine.Rendering;
using VoltstroEngine.Window;

namespace VoltstroEngine
{
	/// <summary>
	/// Main application
	/// </summary>
	public class Application
	{
		private bool isRunning = true;
		private bool minimized;

		private readonly IWindow window;
		private readonly LayerStack layerStack;

		private static Application app;

		/// <summary>
		/// The name and location of the game
		/// <para>E.G: Sandbox</para>
		/// </summary>
		public static string GameName;

		private float lastTime;

		public Application()
		{
			if (app != null)
			{
				Debug.Assert(false, "A running app already exists!");
#if !DEBUG
				Logger.Log("A running app already exists!", LogVerbosity.Error);
				return;
#endif
			}

			app = this;
			GameName = Assembly.GetCallingAssembly().GetName().Name;
			Logger.Log($"Game name is '{GameName}'", LogVerbosity.Debug);

			Renderer.Create();

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

			Renderer.Init();
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
			Renderer.OnWindowResize((uint)e.Width, (uint)e.Height);
		}

		private void OnClose(WindowCloseEvent e)
		{
			isRunning = false;
		}

		/// <summary>
		/// Runs the application
		/// </summary>
		public void Run()
		{
			while (isRunning)
			{
				float time = window.GetTime();
				TimeStep timeStep = new TimeStep(time - lastTime);
				lastTime = time;

				if (!minimized)
				{
					foreach (ILayer layer in layerStack.GetLayers())
						layer.OnUpdate(timeStep);
				}

				window.OnUpdate();
			}

			window.Shutdown();
		}

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