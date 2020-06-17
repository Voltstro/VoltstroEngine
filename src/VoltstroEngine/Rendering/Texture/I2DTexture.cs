using System;

namespace VoltstroEngine.Rendering.Texture
{
	public interface I2DTexture : ITexture
	{
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