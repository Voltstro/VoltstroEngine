using VoltstroEngine.Events;
using VoltstroEngine.Rendering;
using VoltstroEngine.Window;

namespace VoltstroEngine
{
	public class Application
	{
		private bool isRunning = true;
		private readonly IWindow window;

		public Application()
		{
			Renderer.Init();

			//Creates a new window
			window = IWindow.CreateWindow(new WindowProperties
			{
				Title = "Voltstro Engine",
				Width = 1280,
				Height = 720,
				VSync = true
			});
			window.OnEvent += WindowOnOnEvent;
		}

		private void WindowOnOnEvent(IEvent e)
		{
			EventDispatcher eventDispatcher = new EventDispatcher();
			eventDispatcher.DispatchEvent<WindowCloseEvent>(e, OnClose);
		}

		private void OnClose()
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
				Renderer.SetClearColor(0.2f, 0.2f, 0.2f);
				Renderer.Clear();

				window.OnUpdate();
			}
		}
	}
}