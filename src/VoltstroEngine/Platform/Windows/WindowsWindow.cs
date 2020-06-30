﻿using System;
using System.Diagnostics;
using GLFW;
using VoltstroEngine.Core.Inputs;
using VoltstroEngine.Core.Logging;
using VoltstroEngine.Core.Window;
using VoltstroEngine.DebugTools;
using VoltstroEngine.Events;
using VoltstroEngine.Platform.OpenGL;
using VoltstroEngine.Rendering.Renderer;
using Exception = System.Exception;

namespace VoltstroEngine.Platform.Windows
{
	/// <summary>
	/// A window for Windows
	/// </summary>
	public class WindowsWindow : IWindow
	{
		private static uint windowCount;

		private IGraphicsContext context;
		private NativeWindow window;

		private WindowProperties windowProperties;

		public WindowsWindow(WindowProperties properties)
		{
			Init(properties);
		}

		public void OnUpdate()
		{
			if (Glfw.WindowShouldClose(window)) return;
			try
			{
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
			InstrumentationTimer glfwShutdownTimer = InstrumentationTimer.Create("WindowsWindow.Shutdown");

			//For what ever reason this is causing an System.AccessViolationException
			//Glfw.DestroyWindow(window);
			windowCount--;

			if(windowCount == 0)
				Glfw.Terminate();

			glfwShutdownTimer.Stop();
		}

		public float GetTime()
		{
			return (float) Glfw.Time;
		}

		public event IWindow.OnEventDelegate OnEvent;

		private void Init(WindowProperties properties)
		{
			InstrumentationTimer winInitTimer = InstrumentationTimer.Create("Windows Window init.");
			Logger.Log("Initializing a window for Windows...", LogVerbosity.Debug);

			//Initialize glfw if it hasn't already
			if (windowCount == 0)
			{
				bool success = Glfw.Init();
				Debug.Assert(success, "GLFW failed to init!");

				Glfw.SetErrorCallback(ErrorHandler);
			}

#if DEBUG
			if(RenderingAPI.GetRenderingAPI() == RenderingAPIType.OpenGL)
				Glfw.WindowHint(Hint.OpenglDebugContext, true);
#endif

			//Set the properties and create the window
			windowProperties = properties;
			{
				InstrumentationTimer glfwCreateWindowTimer = InstrumentationTimer.Create("Glfw.CreateWindow");
				window = new NativeWindow(properties.Width, properties.Height, properties.Title);
				windowCount++;
				glfwCreateWindowTimer.Stop();
			}

			//Create context
			context = RenderingAPI.GetRenderingAPI() switch
			{
				RenderingAPIType.OpenGL => new OpenGLContext(window),
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
						OnEvent?.Invoke(new KeyReleasedEvent((KeyCode) args.Key));
						break;
					case InputState.Press:
						OnEvent?.Invoke(new KeyPressedEvent((KeyCode) args.Key));
						break;
					case InputState.Repeat:
						OnEvent?.Invoke(new KeyPressedEvent((KeyCode) args.Key, 1));
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
						OnEvent?.Invoke(new MouseButtonPressedEvent((int) args.Button));
						break;
					case InputState.Release:
						OnEvent?.Invoke(new MouseButtonReleasedEvent((int) args.Button));
						break;
				}
			};

			window.MouseScroll += (sender, args) =>
				OnEvent?.Invoke(new MouseScrollEvent((float) args.X, (float) args.Y));

			window.MouseMoved += (sender, args) => OnEvent?.Invoke(new MouseMovedEvent((float) args.X, (float) args.Y));

			Logger.Log($"Created a window for Windows ({properties.Width}x{properties.Height}, {properties.VSync})",
				LogVerbosity.Debug);
			winInitTimer.Stop();
		}

		private static void ErrorHandler(ErrorCode code, IntPtr message)
		{
			Logger.Log($"GLFW Error: {code}:{message}", LogVerbosity.Error);
		}
	}
}