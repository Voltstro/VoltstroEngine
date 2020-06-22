using System.Numerics;
using VoltstroEngine.Extensions;

namespace VoltstroEngine.Rendering.Camera
{
	public class OrthographicCamera
	{
		public OrthographicCamera(float left, float right, float bottom, float top)
		{
			ProjectionMatrix = MathExtensions.Ortho(left, right, bottom, top, -1.0f, 1.0f);
			ViewMatrix  = Matrix4x4.Identity;
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
			ProjectionMatrix = Matrix4x4.CreateOrthographicOffCenter(left, right, bottom, top, -1.0f, 1.0f);
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
			Matrix4x4 transform = Matrix4x4.CreateTranslation(Position) *
			                      Matrix4x4.CreateRotationZ(Rotation.ToRadian());

			if (Matrix4x4.Invert(transform, out Matrix4x4 result))
			{
				ViewMatrix = result;
				ViewProjectionMatrix = ProjectionMatrix * result;
			}
		}
	}
}