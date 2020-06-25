namespace VoltstroEngine.Rendering.Renderer
{
	public interface IGraphicsContext
	{
		/// <summary>
		/// Initializes the graphics context
		/// </summary>
		public void Init();

		/// <summary>
		/// Swaps the buffers
		/// </summary>
		public void SwapBuffers();
	}
}