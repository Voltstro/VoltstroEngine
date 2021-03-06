﻿using GLFW;
using VoltstroEngine.Core.Inputs;

namespace VoltstroEngine.Platform.Windows
{
	internal class WindowsInput : IInputImpl
	{
		private readonly Window window;

		public WindowsInput(Window window)
		{
			this.window = window;
		}

		public bool IsKeyPressed(KeyCode key)
		{
			InputState state = Glfw.GetKey(window, (Keys) key);
			return state == InputState.Press || state == InputState.Repeat;
		}
	}
}