namespace VoltstroEngine.Core
{
	public interface IEntryPoint
	{
		/// <summary>
		/// Creates a new <see cref="Application"/>
		/// </summary>
		/// <returns></returns>
		public Application CreateApplication();
	}
}