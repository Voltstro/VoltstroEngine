using System.Diagnostics;
using OpenGL;
using VoltstroEngine.Imaging;
using VoltstroEngine.Rendering.Texture;

namespace VoltstroEngine.Platform.OpenGL
{
	internal class OpenGL2DTexture : I2DTexture
	{
		private readonly uint textureID;

		private readonly uint width, height;

		public OpenGL2DTexture(string imagePath)
		{
			Image image = new Image($"{Application.GameName}/{imagePath}", true);

			InternalFormat internalFormat = 0;
			PixelFormat dataFormat = 0;
			if (image.Channels == 4)
			{
				internalFormat = InternalFormat.Rgba8;
				dataFormat = PixelFormat.Rgba;
			}
			else if (image.Channels == 3)
			{
				internalFormat = InternalFormat.Rgb8;
				dataFormat = PixelFormat.Rgb;
			}

			Debug.Assert(internalFormat != 0, "Format not supported!");

			width = (uint)image.Width;
			height = (uint)image.Height;

			textureID = Gl.CreateTexture(TextureTarget.Texture2d);
			Gl.TextureStorage2D(textureID, 1, internalFormat, image.Width, image.Height);

			Gl.TextureParameteri(textureID, TextureParameterName.TextureMinFilter, Gl.LINEAR);
			Gl.TextureParameteri(textureID, TextureParameterName.TextureMagFilter, Gl.NEAREST);

			Gl.TextureSubImage2D(textureID, 0, 0, 0, image.Width, image.Height, dataFormat, PixelType.UnsignedByte, image.Data);
		}

		~OpenGL2DTexture()
		{
			Gl.DeleteTextures(textureID);
		}

		public uint GetWidth()
		{
			return width;
		}

		public uint GetHeight()
		{
			return height;
		}

		public void Bind(uint slot = 0)
		{
			Gl.BindTextureUnit(slot, textureID);
		}
	}
}