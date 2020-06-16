namespace VoltstroEngine.Inputs
{
	public static class Input
	{
		internal static IInputImpl KeyInputImpl;

		public static bool IsKeyPressed(KeyCode key)
		{
			return KeyInputImpl.IsKeyPressed(key);
		}
	}
}