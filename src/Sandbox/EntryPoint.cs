using VoltstroEngine.Core;
using VoltstroEngine.Core.Logging;

namespace Sandbox
{
	public class SandboxEntry : IEntryPoint
	{
		public Application CreateApplication()
		{
			Logger.Log("Creating new sandbox app...");
			return new SandboxApp();
		}

		public string GetGameName()
		{
			return "Sandbox";
		}
	}
}