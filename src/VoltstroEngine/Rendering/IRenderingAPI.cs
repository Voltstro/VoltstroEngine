namespace VoltstroEngine.Rendering
{
	/// <summary>
	/// Interface for a rending API (Such as OpenGL or Vulkan)
	/// </summary>
	public interface IRenderingAPI
	{
		/// <summary>
		/// Initializes the rendering API
		/// </summary>
		public void Init();

		/// <summary>
		/// Sets the clear color
		/// </summary>
		/// <param name="red"></param>
		/// <param name="green"></param>
		/// <param name="blue"></param>
		/// <param name="alpha"></param>
		public void SetClearColor(float red, float green, float blue, float alpha);

		/// <summary>
		/// Clears the buffer
		/// </summary>
		public void Clear();
	}
}