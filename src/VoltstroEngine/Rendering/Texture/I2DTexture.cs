using System;
using VoltstroEngine.Rendering.Renderer;

namespace VoltstroEngine.Rendering.Texture
{
	/// <summary>
	/// A 2D Texture
	/// </summary>
	public interface I2DTexture : ITexture, IDisposable
	{
		/// <summary>
		/// Creates a new texture
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <returns></returns>
		public static I2DTexture Create(uint width, uint height)
		{
			switch (RenderingAPI.GetRenderingAPI())
			{
				case RenderingAPIType.OpenGL:
					return new Platform.OpenGL.OpenGL2DTexture(width, height);
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		/// <summary>
		/// Creates a new texture
		/// </summary>
		/// <param name="texturePath"></param>
		/// <returns></returns>
		public static I2DTexture Create(string texturePath)
		{
			switch (RenderingAPI.GetRenderingAPI())
			{
				case RenderingAPIType.OpenGL:
					return new Platform.OpenGL.OpenGL2DTexture(texturePath);
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}