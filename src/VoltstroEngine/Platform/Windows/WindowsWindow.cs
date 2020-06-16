using System;
using System.Diagnostics;
using GLFW;
using VoltstroEngine.Events;
using VoltstroEngine.Inputs;
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

		public void Shutdown()
		{
			Glfw.DestroyWindow(window);
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

			//GLFW callbacks
			Glfw.SetWindowSizeCallback(window, (outWindow, width, height) => OnEvent?.Invoke(new WindowResizedEvent(width, height)));
			Glfw.SetCloseCallback(window, outWindow => OnEvent?.Invoke(new WindowCloseEvent()));

			Glfw.SetKeyCallback(window,
				delegate(GLFW.Window outWindow, Keys key, int code, InputState state, ModifierKeys mods)
				{
					switch (state)
					{
						case InputState.Release:
							OnEvent?.Invoke(new KeyReleasedEvent((KeyCode)key));
							break;
						case InputState.Press:
							OnEvent?.Invoke(new KeyPressedEvent((KeyCode)key));
							break;
						case InputState.Repeat:
							OnEvent?.Invoke(new KeyPressedEvent((KeyCode)key, 1));
							break;
						default:
							throw new ArgumentOutOfRangeException(nameof(state), state, null);
					}
				});

			Glfw.SetCharCallback(window, (outWindow, key) => OnEvent?.Invoke(new KeyTypedEvent((KeyCode)key)));

			Glfw.SetMouseButtonCallback(window,
				delegate(GLFW.Window outWindow, MouseButton button, InputState state, ModifierKeys modifiers)
				{
					switch (state)
					{
						case InputState.Release:
							OnEvent?.Invoke(new MouseButtonPressedEvent((int)button));
							break;
						case InputState.Press:
							OnEvent?.Invoke(new MouseButtonReleasedEvent((int)button));
							break;
					}
				});

			Glfw.SetScrollCallback(window, (outWindow, xOffset, yOffset) => OnEvent?.Invoke(new MouseScrollEvent((float)xOffset, (float)yOffset)));

			Glfw.SetCursorPositionCallback(window, (outWindow, xPos, yPos) => OnEvent?.Invoke(new MouseMovedEvent((float)xPos, (float)yPos)));

			Logger.Log($"Created a window for Windows ({properties.Width}x{properties.Height}, {properties.VSync})", LogVerbosity.Debug);
		}

		private static void ErrorHandler(ErrorCode code, IntPtr message)
		{
			Logger.Log($"GLFW Error: {code}:{message}", LogVerbosity.Error);
		}
	}
}