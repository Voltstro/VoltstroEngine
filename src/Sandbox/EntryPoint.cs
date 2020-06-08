using VoltstroEngine;
using VoltstroEngine.Logging;

namespace Sandbox
{
	public class SandboxEntry : IEntryPoint
	{
		public Application CreateApplication()
		{
			Logger.Log("Creating new sandbox app...");
			return new SandboxApp();
		}
	}
}