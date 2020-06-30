using System.Numerics;
using VoltstroEngine.Core;
using VoltstroEngine.Core.Layers;
using VoltstroEngine.DebugTools;
using VoltstroEngine.EtoForms;
using VoltstroEngine.Events;
using VoltstroEngine.Rendering;
using VoltstroEngine.Rendering.Renderer;
using VoltstroEngine.Rendering.Texture;

namespace Sandbox
{
	public class Sandbox2D : ILayer
	{
		private OrthographicCameraController cameraController;
		private I2DTexture birdiTexture;

		private EtoTestForm testForm;

		public void OnAttach()
		{
			cameraController = new OrthographicCameraController(1280.0f / 720.0f);
			birdiTexture = I2DTexture.Create("Textures/Birdi.png");

			testForm = new EtoTestForm();
			EtoFormsSystem.AddForm(testForm);
		}

		public void OnDetach()
		{
		}

		public void OnUpdate(TimeStep ts)
		{
			InstrumentationTimer mainTimer = new InstrumentationTimer("Sandbox2D.OnUpdate");

			//Update
			{
				InstrumentationTimer update = new InstrumentationTimer("CameraController.OnUpdate");
				cameraController.OnUpdate(ts);
				update.Stop();
			}

			//Render
			{
				InstrumentationTimer renderPrep = new InstrumentationTimer("Renderer Prep");

				RenderingAPI.SetClearColor(0.1f, 0.1f, 0.1f);
				RenderingAPI.Clear();

				renderPrep.Stop();
			}

			{
				InstrumentationTimer renderer = new InstrumentationTimer("Renderer Draw");

				Renderer2D.BeginScene(cameraController.GetCamera());
				{
					Renderer2D.DrawQuad(new Vector2(-1.0f, 0.0f), new Vector2(0.8f, 0.8f), new Vector4(0.8f, 0.2f, 0.3f, 1.0f));

					Renderer2D.DrawQuad(new Vector2(0.5f, -0.5f), new Vector2(0.5f, 0.75f), new Vector4(0.2f, 0.3f, 0.8f, 1.0f));

					Renderer2D.DrawQuad(new Vector3(0f, 0f, -0.1f), new Vector2(10f, 10f), birdiTexture);
				}
				Renderer2D.EndScene();
				renderer.Stop();
			}
			
			mainTimer.Stop();
		}

		public void OnEvent(IEvent e)
		{
			cameraController.OnEvent(e);
		}
	}
}