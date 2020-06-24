using System;
using System.Diagnostics;
using GLFW;
using VoltstroEngine.Core.Inputs;
using VoltstroEngine.Core.Logging;
using VoltstroEngine.Core.Window;
using VoltstroEngine.Events;
using VoltstroEngine.Platform.OpenGL;
using VoltstroEngine.Rendering;
using Exception = System.Exception;

namespace VoltstroEngine.Platform.Windows
{
	/// <summary>
	/// A window for Windows
	/// </summary>
	public class WindowsWindow : IWindow
	{
		private static bool glfwInitialized;

		private WindowProperties windowProperties;
		private NativeWindow window;

		private IGraphicsContext context;

		public WindowsWindow(WindowProperties properties)
		{
			Init(properties);
		}

		public void OnUpdate()
		{
			if(Glfw.WindowShouldClose(window)) return;
			try
			{
				Glfw.PollEvents();
				context.SwapBuffers();
			}
			catch (Exception ex)
			{
				Logger.Log(ex.ToString());
			}
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

		public void Shutdown()
		{
			//Glfw.DestroyWindow(window);
		}

		public float GetTime()
		{
			return (float)Glfw.Time;
		}

		public event IWindow.OnEventDelegate OnEvent;

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
			window = new NativeWindow(properties.Width, properties.Height, properties.Title);

			//Create context
			context = Renderer.GetRenderingAPI() switch
			{
				RenderingAPI.OpenGL => new OpenGLContext(window),
				_ => throw new ArgumentOutOfRangeException()
			};

			//Init the context
			context.Init();

			SetVSync(properties.VSync);

			//Setup input
			Input.KeyInputImpl = new WindowsInput(window);

			//GLFW callbacks
			window.Closed += (sender, args) => OnEvent?.Invoke(new WindowCloseEvent());

			window.SizeChanged += delegate(object sender, SizeChangeEventArgs args)
			{
				windowProperties.Width = args.Size.Width;
				windowProperties.Height = args.Size.Height;

				OnEvent?.Invoke(new WindowResizedEvent(args.Size.Width, args.Size.Height));
			};

			window.KeyAction += delegate(object sender, KeyEventArgs args)
			{
				switch (args.State)
				{
					case InputState.Release:
						OnEvent?.Invoke(new KeyReleasedEvent((KeyCode)args.Key));
						break;
					case InputState.Press:
						OnEvent?.Invoke(new KeyPressedEvent((KeyCode)args.Key));
						break;
					case InputState.Repeat:
						OnEvent?.Invoke(new KeyPressedEvent((KeyCode)args.Key, 1));
						break;
					default:
						throw new ArgumentOutOfRangeException(nameof(args.State), args.State, null);
				}
			};

			window.MouseButton += delegate(object sender, MouseButtonEventArgs args)
			{
				switch (args.Action)
				{
					case InputState.Press:
						OnEvent?.Invoke(new MouseButtonPressedEvent((int)args.Button));
						break;
					case InputState.Release:
						OnEvent?.Invoke(new MouseButtonReleasedEvent((int)args.Button));
						break;
				}
			};

			window.MouseScroll += (sender, args) => OnEvent?.Invoke(new MouseScrollEvent((float) args.X, (float) args.Y));

			window.MouseMoved += (sender, args) => OnEvent?.Invoke(new MouseMovedEvent((float) args.X, (float) args.Y));

			Logger.Log($"Created a window for Windows ({properties.Width}x{properties.Height}, {properties.VSync})", LogVerbosity.Debug);
		}

		private static void ErrorHandler(ErrorCode code, IntPtr message)
		{
			Logger.Log($"GLFW Error: {code}:{message}", LogVerbosity.Error);
		}
	}
}