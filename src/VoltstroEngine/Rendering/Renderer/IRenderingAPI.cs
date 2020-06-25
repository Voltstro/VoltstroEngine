namespace VoltstroEngine.Rendering.Renderer
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

		/// <summary>
		/// Sets the viewport size
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public void SetViewport(uint x, uint y, uint width, uint height);
	}
}