using System;
using VoltstroEngine;

namespace Sandbox
{
	public class SandboxApp : Application
	{

	}

	public class SandboxEntry : IEntryPoint
	{
		public Application CreateApplication()
		{
			Console.WriteLine("Creating new SandBox application...");
			return new SandboxApp();
		}
	}
}