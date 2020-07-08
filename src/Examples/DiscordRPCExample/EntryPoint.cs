using VoltstroEngine.Core;

namespace DiscordRPCExample
{
	public class EntryPoint : IEntryPoint
	{
		public Application CreateApplication()
		{
			return new GameApp();
		}

		public string GetGameName()
		{
			return "DiscordRPCExample";
		}
	}
}