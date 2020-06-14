namespace VoltstroEngine.Rendering
{
	/// <summary>
	/// Interface for a rending API (Such as OpenGL or Vulkan)
	/// </summary>
	internal interface IRenderingAPI
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

		/// <summary>
		/// Draws something to the screen
		/// </summary>
		/// <param name="vertexArray"></param>
		public void DrawIndexed(IVertexArray vertexArray);

		/// <summary>
		/// Returns what type or rendering API this is
		/// </summary>
		/// <returns></returns>
		public RenderingAPI GetAPI();
	}
}