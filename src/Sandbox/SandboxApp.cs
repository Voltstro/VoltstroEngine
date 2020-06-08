using VoltstroEngine;
using VoltstroEngine.Events;
using VoltstroEngine.Layers;

namespace Sandbox
{
	public class ExampleLayer : ILayer
	{
		public void OnAttach()
		{

		}

		public void OnDetach()
		{
			
		}

		public void OnUpdate()
		{
			
		}

		public void OnEvent(IEvent e)
		{
			
		}
	}

	public class SandboxApp : Application
	{
		public SandboxApp()
		{
			PushOverlay(new ExampleLayer());
		}
	}
}