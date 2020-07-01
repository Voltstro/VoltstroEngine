using System.Numerics;

namespace VoltstroEngine.Types
{
	/// <summary>
	/// Contains an object's position, scale and rotation
	/// </summary>
	public class Transform
	{
		/// <summary>
		/// The position of this object
		/// </summary>
		public Vector3 Position { get; set; }

		/// <summary>
		/// The scale of this object
		/// </summary>
		public Vector2 Scale { get; set; }

		/// <summary>
		/// The rotation of this object (in degrees)
		/// </summary>
		public float Rotation { get; set; }
	}
}