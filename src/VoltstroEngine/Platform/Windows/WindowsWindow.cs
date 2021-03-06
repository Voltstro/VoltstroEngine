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
	internal sealed class WindowsWindow : IWindow
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
				Logger.Error("An error occured while swapping buffers!", ex);
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
			ProfilerTimer.Profile(() =>
			{
				window.Dispose();
				windowCount--;

				if(windowCount == 0)
					Glfw.Terminate();
			});
		}

		public float GetTime()
		{
			return (float) Glfw.Time;
		}

		public event IWindow.OnEventDelegate OnEvent;

		private void Init(WindowProperties properties)
		{
			ProfilerTimer.Profile(() =>
			{
				Logger.Info("Initializing a window for Windows...");

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
					ProfilerTimer.Profile("GLFW Create Window", () =>
					{
						window = new NativeWindow(properties.Width, properties.Height, properties.Title);
						windowCount++;
					});
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

				Logger.Debug("Created a window for Windows ({@Width}x{@Height}, {@VSync})", properties.Width, properties.Height, properties.VSync);
			});
		}

		private static void ErrorHandler(ErrorCode code, IntPtr message)
		{
			Logger.Error("GLFW Error: {@Code}:{@Message}", code, message);
		}
	}
}