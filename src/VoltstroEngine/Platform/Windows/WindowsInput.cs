using GLFW;
using VoltstroEngine.Inputs;

namespace VoltstroEngine.Platform.Windows
{
	internal class WindowsInput : IInputImpl
	{
		private readonly GLFW.Window window;

		public WindowsInput(GLFW.Window window)
		{
			this.window = window;
		}

		public bool IsKeyPressed(KeyCode key)
		{
			InputState state = Glfw.GetKey(window, (Keys)key);
			return state == InputState.Press || state == InputState.Repeat;
		}
	}
}