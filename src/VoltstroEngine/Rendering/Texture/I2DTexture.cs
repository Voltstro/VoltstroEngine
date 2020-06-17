using System;

namespace VoltstroEngine.Rendering.Texture
{
	/// <summary>
	/// A 2D Texture
	/// </summary>
	public interface I2DTexture : ITexture
	{
		/// <summary>
		/// Creates a new texture
		/// </summary>
		/// <param name="texturePath"></param>
		/// <returns></returns>
		public static I2DTexture Create(string texturePath)
		{
			switch (Renderer.GetRenderingAPI())
			{
				case RenderingAPI.OpenGL:
					return new Platform.OpenGL.OpenGL2DTexture(texturePath);
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}