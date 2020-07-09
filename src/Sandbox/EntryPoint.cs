using VoltstroEngine.Core;

namespace Sandbox
{
	public class SandboxEntry : IEntryPoint
	{
		public Application CreateApplication()
		{
			return new SandboxApp();
		}

		public string GetGameName()
		{
			return "Sandbox";
		}
	}
}