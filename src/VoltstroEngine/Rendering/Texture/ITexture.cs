﻿namespace VoltstroEngine.Rendering.Texture
{
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
		/// Bind this texture
		/// </summary>
		/// <param name="slot"></param>
		/// <returns></returns>
		public void Bind(uint slot = 0);
	}
}