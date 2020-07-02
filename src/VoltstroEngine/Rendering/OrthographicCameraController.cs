using System;
using System.Numerics;
using VoltstroEngine.Core;
using VoltstroEngine.Core.Inputs;
using VoltstroEngine.Events;
using VoltstroEngine.Rendering.Camera;

namespace VoltstroEngine.Rendering
{
	/// <summary>
	/// Handles stuff such as movement, zoom, camera setup and window resizing for a <see cref="OrthographicCamera"/>
	/// </summary>
	public class OrthographicCameraController
	{
		/// <summary>
		/// Creates a new <see cref="OrthographicCameraController"/>
		/// </summary>
		/// <param name="aspectRatio"></param>
		/// <param name="enableRotationControls"></param>
		public OrthographicCameraController(float aspectRatio, bool enableRotationControls = true)
		{
			AspectRatio = aspectRatio;
			ZoomLevel = 1.0f;
			EnableRotationControls = enableRotationControls;

			cameraPosition = Vector3.Zero;
			CameraTranslationSpeed = 2.0f;
			CameraRotationSpeed = 25.0f;

			EnableMovementControls = true;
			EnableZoomControls = true;

			camera = new OrthographicCamera(-aspectRatio * ZoomLevel, aspectRatio * ZoomLevel, -ZoomLevel, ZoomLevel);
		}

		/// <summary>
		/// The <see cref="OrthographicCamera"/>
		/// </summary>
		private readonly OrthographicCamera camera;

		/// <summary>
		/// The aspect ratio of the camera
		/// </summary>
		public float AspectRatio { get; private set; }

		/// <summary>
		/// Are rotation controls enabled
		/// </summary>
		public bool EnableRotationControls { get; }

		/// <summary>
		/// Are movement controls enabled?
		/// </summary>
		public bool EnableMovementControls { get; set; }

		/// <summary>
		/// Are zoom controls enabled?
		/// </summary>
		public bool EnableZoomControls { get; set; }

		/// <summary>
		/// Gets the <see cref="OrthographicCamera"/>
		/// </summary>
		/// <returns></returns>
		public OrthographicCamera GetCamera()
		{
			return camera;
		}

		#region Camera Zoom

		/// <summary>
		/// The zoom level of this camera
		/// </summary>
		public float ZoomLevel { get; private set; }

		/// <summary>
		/// Sets the camera's zoom level
		/// </summary>
		/// <param name="zoom"></param>
		public void SetZoomLevel(float zoom)
		{
			ZoomLevel = zoom;
			ZoomLevel = Math.Clamp(ZoomLevel, 0.25f, 1000f);
			camera.SetProjection(-AspectRatio * ZoomLevel, AspectRatio * ZoomLevel, -ZoomLevel, ZoomLevel);
		}

		#endregion

		#region Camera Position

		/// <summary>
		/// The camera's current position
		/// </summary>
		private Vector3 cameraPosition;

		/// <summary>
		/// Sets the camera's position
		/// </summary>
		/// <param name="position"></param>
		public void SetCameraPosition(Vector3 position)
		{
			cameraPosition = position;
			camera.SetPosition(position);
		}

		/// <summary>
		/// Gets the camera's position
		/// </summary>
		/// <returns></returns>
		public Vector3 GetCameraPosition()
		{
			return cameraPosition;
		}

		/// <summary>
		/// The speed at which the camera moves
		/// </summary>
		public float CameraTranslationSpeed;

		#endregion

		#region Camera Rotation

		/// <summary>
		/// The rotation of the camera
		/// </summary>
		private float cameraRotation;

		/// <summary>
		/// Sets the camera's rotation
		/// </summary>
		/// <param name="rotation"></param>
		public void SetCameraRotation(float rotation)
		{
			cameraRotation = rotation;
			camera.SetRotation(rotation);
		}

		/// <summary>
		/// Gets the camera's rotation
		/// </summary>
		/// <returns></returns>
		public float GetCameraRotation()
		{
			return cameraRotation;
		}

		/// <summary>
		/// The speed at which the camera rotates
		/// </summary>
		public float CameraRotationSpeed { get; set; }

		#endregion

		/// <summary>
		/// Call this on every update
		/// </summary>
		/// <param name="ts"></param>
		public void OnUpdate(TimeStep ts)
		{
			if(!EnableMovementControls)
				return;

			//Camera movement controls
			if (Input.IsKeyPressed(KeyCode.A))
				cameraPosition.X -= CameraTranslationSpeed * ts.Seconds;
			else if (Input.IsKeyPressed(KeyCode.D))
				cameraPosition.X += CameraTranslationSpeed * ts.Seconds;

			if (Input.IsKeyPressed(KeyCode.W))
				cameraPosition.Y += CameraTranslationSpeed * ts.Seconds;
			else if (Input.IsKeyPressed(KeyCode.S))
				cameraPosition.Y -= CameraTranslationSpeed * ts.Seconds;

			//Camera rotation controls
			if (EnableRotationControls)
			{
				if (Input.IsKeyPressed(KeyCode.E))
					cameraRotation += CameraRotationSpeed * ts.Seconds;
				else if (Input.IsKeyPressed(KeyCode.Q))
					cameraRotation -= CameraRotationSpeed * ts.Seconds;

				camera.SetRotation(cameraRotation);
			}

			camera.SetPosition(cameraPosition);

			CameraTranslationSpeed = ZoomLevel;
		}

		/// <summary>
		/// Call this on an event
		/// </summary>
		/// <param name="e"></param>
		public void OnEvent(IEvent e)
		{
			EventDispatcher.DispatchEvent<MouseScrollEvent>(e, OnMouseScrolled);
			EventDispatcher.DispatchEvent<WindowResizedEvent>(e, OnWindowResized);
		}

		private void OnMouseScrolled(MouseScrollEvent e)
		{
			//Camera zoom controls
			if(!EnableZoomControls)
				return;

			ZoomLevel -= e.OffsetY * 0.25f;
			ZoomLevel = Math.Clamp(ZoomLevel, 0.25f, 1000f);
			camera.SetProjection(-AspectRatio * ZoomLevel, AspectRatio * ZoomLevel, -ZoomLevel, ZoomLevel);
		}

		private void OnWindowResized(WindowResizedEvent e)
		{
			if (e.Width == 0 || e.Height == 0)
				return;

			AspectRatio = (float) e.Width / e.Height;
			camera.SetProjection(-AspectRatio * ZoomLevel, AspectRatio * ZoomLevel, -ZoomLevel, ZoomLevel);
		}
	}
}