using System.Numerics;
using VoltstroEngine.Core;
using VoltstroEngine.Core.Layers;
using VoltstroEngine.Events;
using VoltstroEngine.Rendering;
using VoltstroEngine.Rendering.Renderer;

namespace Sandbox
{
	public class Sandbox2D : ILayer
	{
		private OrthographicCameraController cameraController;

		public void OnAttach()
		{
			cameraController = new OrthographicCameraController(1280.0f / 720.0f);
		}

		public void OnDetach()
		{
		}

		public void OnUpdate(TimeStep ts)
		{
			//Update
			cameraController.OnUpdate(ts);

			//Render
			Renderer.SetClearColor(0.1f, 0.1f, 0.1f);
			Renderer.Clear();

			Renderer2D.BeginScene(cameraController.GetCamera());
			{
				Renderer2D.DrawQuad(Vector2.Zero, Vector2.One, new Vector4(0.8f, 0.2f, 0.3f, 1.0f));
			}
			Renderer2D.EndScene();
		}

		public void OnEvent(IEvent e)
		{
			cameraController.OnEvent(e);
		}
	}
}