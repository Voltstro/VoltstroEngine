using VoltstroEngine.Events;

namespace VoltstroEngine.Window
{
	/// <summary>
	/// Window interface
	/// </summary>
	public interface IWindow
	{
		/// <summary>
		/// Creates a new window, specific for each platform
		/// </summary>
		/// <param name="properties"></param>
		/// <returns></returns>
		public static IWindow CreateWindow(WindowProperties properties)
		{
#if WINDOWS
			return new Platform.Windows.WindowsWindow(properties);
#else
#error Voltstro Engine Currently only supportes Windows!
#endif
		}

		/// <summary>
		/// Called every frame
		/// </summary>
		public void OnUpdate();

		/// <summary>
		/// Gets the width of the window
		/// </summary>
		/// <returns></returns>
		public int GetWidth();

		/// <summary>
		/// Gets the height of the window
		/// </summary>
		/// <returns></returns>
		public int GetHeight();

		/// <summary>
		/// Enables/disables VSync
		/// </summary>
		/// <param name="enable"></param>
		public void SetVSync(bool enable);

		/// <summary>
		/// Gets if VSync is on or not
		/// </summary>
		/// <returns></returns>
		public bool IsVSync();

		/// <summary>
		/// Shutdown the window
		/// </summary>
		public void Shutdown();

		public delegate void OnEventDelegate(IEvent e);

		public event OnEventDelegate OnEvent;
	}
}