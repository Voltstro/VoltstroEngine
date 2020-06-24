using System.Numerics;
using VoltstroEngine.Extensions;

namespace VoltstroEngine.Rendering.Camera
{
	public class OrthographicCamera
	{
		public OrthographicCamera(float left, float right, float bottom, float top)
		{
			ProjectionMatrix = MathExtensions.Ortho(left, right, bottom, top, -1.0f, 1.0f);
			ViewMatrix = Matrix4x4.Identity;
			Rotation = 0.0f;
			Position = new Vector3(0.0f, 0.0f, 0.0f);

			ViewProjectionMatrix = ProjectionMatrix * ViewMatrix;
		}

		public Matrix4x4 ProjectionMatrix { get; private set; }
		public Matrix4x4 ViewMatrix { get; private set; }
		public Matrix4x4 ViewProjectionMatrix { get; private set; }

		/// <summary>
		/// The current position of the camera
		/// </summary>
		public Vector3 Position { get; private set; }

		/// <summary>
		/// The current rotation of the camera
		/// </summary>
		public float Rotation { get; private set; }

		/// <summary>
		/// Sets the projection
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <param name="bottom"></param>
		/// <param name="top"></param>
		public void SetProjection(float left, float right, float bottom, float top)
		{
			ProjectionMatrix = MathExtensions.Ortho(left, right, bottom, top, -1.0f, 1.0f);
			ViewProjectionMatrix = ProjectionMatrix * ViewMatrix;
		}

		/// <summary>
		/// Sets the position of the camera
		/// </summary>
		/// <param name="position"></param>
		public void SetPosition(Vector3 position)
		{
			Position = position;
			RecalculateViewMatrix();
		}

		/// <summary>
		/// Sets the position of the camera
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		public void SetPosition(float x, float y, float z)
		{
			SetPosition(new Vector3(x, y, z));
		}

		/// <summary>
		/// Sets the rotation of the camera
		/// </summary>
		/// <param name="rotation"></param>
		public void SetRotation(float rotation)
		{
			Rotation = rotation;
			RecalculateViewMatrix();
		}

		private void RecalculateViewMatrix()
		{
			//This is the matrix for our rotation. We're just rotating around the Z axis (anti/clockwise)
			Matrix4x4 rotation = Matrix4x4.CreateRotationZ(Rotation.ToRadian());
			//The matrix for our camera's offset/position
			Matrix4x4 translation = Matrix4x4.CreateTranslation(Position);

			//To transform properly, we do T2 * T1, where T1 is the first 'movement', and T2 is the 2nd.
			//We want to move then rotate, so do 'rotation * translation' (because rotation = T2)
			//However from testing this doesn't appear to make any difference (so far)
			Matrix4x4 transform = rotation * translation;

			//? What is this for?
			if (Matrix4x4.Invert(transform, out Matrix4x4 result))
			{
				ViewMatrix = result;
				//TODO: This only works on opengl, DirectX needs to be multiplied the other way round
				ViewProjectionMatrix = result * ProjectionMatrix;
			}
		}
	}
}