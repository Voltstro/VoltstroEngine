namespace VoltstroEngine.Core
{
	public interface IEntryPoint
	{
		/// <summary>
		/// Creates a new <see cref="Application"/>
		/// </summary>
		/// <returns></returns>
		public Application CreateApplication();

		/// <summary>
		/// Gets the game name
		/// </summary>
		/// <returns></returns>
		public string GetGameName();
	}
}