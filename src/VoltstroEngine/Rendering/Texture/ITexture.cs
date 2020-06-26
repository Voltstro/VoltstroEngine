namespace VoltstroEngine.Rendering.Texture
{
	/// <summary>
	/// Base of a texture
	/// </summary>
	public interface ITexture
	{
		/// <summary>
		/// Gets the width of this texture
		/// </summary>
		/// <returns></returns>
		public uint GetWidth();

		/// <summary>
		/// Gets the height of this texture
		/// </summary>
		/// <returns></returns>
		public uint GetHeight();

		/// <summary>
		/// Sets data
		/// </summary>
		/// <param name="data"></param>
		/// <param name="size"></param>
		public void SetData(object data, uint size);

		/// <summary>
		/// Bind this texture
		/// </summary>
		/// <param name="slot"></param>
		/// <returns></returns>
		public void Bind(uint slot = 0);
	}
}