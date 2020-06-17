namespace VoltstroEngine.Inputs
{
	/// <summary>
	/// Input stuff
	/// </summary>
	public static class Input
	{
		internal static IInputImpl KeyInputImpl;

		/// <summary>
		/// Check if a key is pressed or not
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static bool IsKeyPressed(KeyCode key)
		{
			return KeyInputImpl.IsKeyPressed(key);
		}
	}
}