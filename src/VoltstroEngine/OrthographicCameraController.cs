using System;
using System.Numerics;
using VoltstroEngine.Events;
using VoltstroEngine.Inputs;
using VoltstroEngine.Rendering.Camera;

namespace VoltstroEngine
{
	public class OrthographicCameraController
	{
		public OrthographicCameraController(float aspectRatio)
		{
			AspectRatio = aspectRatio;
			ZoomLevel = 1.0f;

			CameraPosition = Vector3.Zero;
			CameraTranslationSpeed = 2.0f;

			camera = new OrthographicCamera(-aspectRatio * ZoomLevel, aspectRatio * ZoomLevel, -ZoomLevel, ZoomLevel);
		}

		public float AspectRatio { get; private set; }
		public float ZoomLevel { get; private set; }

		public Vector3 CameraPosition;
		public float CameraTranslationSpeed;

		private readonly OrthographicCamera camera;

		/// <summary>
		/// Gets the <see cref="OrthographicCamera"/>
		/// </summary>
		/// <returns></returns>
		public OrthographicCamera GetCamera()
		{
			return camera;
		}

		public void OnUpdate(TimeStep ts)
		{
			//Camera movement
			if (Input.IsKeyPressed(KeyCode.A))
				CameraPosition.X -= CameraTranslationSpeed * ts.Seconds;
			if (Input.IsKeyPressed(KeyCode.D))
				CameraPosition.X += CameraTranslationSpeed * ts.Seconds;

			if (Input.IsKeyPressed(KeyCode.W))
				CameraPosition.Y += CameraTranslationSpeed * ts.Seconds;
			if (Input.IsKeyPressed(KeyCode.S))
				CameraPosition.Y -= CameraTranslationSpeed * ts.Seconds;

			camera.SetPosition(CameraPosition);

			CameraTranslationSpeed = ZoomLevel;
		}

		public void OnEvent(IEvent e)
		{
			EventDispatcher.DispatchEvent<MouseScrollEvent>(e, OnMouseScrolled);
			EventDispatcher.DispatchEvent<WindowResizedEvent>(e, OnWindowResized);
		}

		private void OnMouseScrolled(MouseScrollEvent e)
		{
			ZoomLevel -= e.OffsetY * 0.25f;
			ZoomLevel = Math.Clamp(ZoomLevel, 0.25f, 1000f);
			camera.SetProjection(-AspectRatio * ZoomLevel, AspectRatio * ZoomLevel, -ZoomLevel, ZoomLevel);
		}

		private void OnWindowResized(WindowResizedEvent e)
		{
			AspectRatio = e.Width / e.Height;
			camera.SetProjection(-AspectRatio * ZoomLevel, AspectRatio * ZoomLevel, -ZoomLevel, ZoomLevel);
		}
	}
}