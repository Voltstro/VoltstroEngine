using System;
using OpenGL;
using VoltstroEngine.Rendering.Texture;

namespace VoltstroEngine.Platform.OpenGL
{
	internal class OpenGL2DTexture : I2DTexture
	{
		private readonly uint textureID;

		public OpenGL2DTexture(string imagePath)
		{
			//TODO: Add image loading

			/*
			textureID = Gl.CreateTexture(TextureTarget.Texture2d);
			Gl.TextureStorage2D(textureID, 1, InternalFormat.Rgb8, image.Width, image.Height);

			Gl.TextureParameteri(textureID, TextureParameterName.TextureMinFilter, Gl.LINEAR);
			Gl.TextureParameteri(textureID, TextureParameterName.TextureMagFilter, Gl.NEAREST);

			Gl.TextureSubImage2D(textureID, 0, 0, 0, image.Width, image.Height, PixelFormat.Rgb, PixelType.UnsignedByte, image);
			*/
		}

		public uint GetWidth()
		{
			throw new NotImplementedException();
		}

		public uint GetHeight()
		{
			throw new NotImplementedException();
		}

		public void Bind(uint slot = 0)
		{
			Gl.BindTextureUnit(slot, textureID);
		}
	}
}