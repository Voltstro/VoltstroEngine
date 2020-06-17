using System.Diagnostics;
using ImageMagick;
using OpenGL;
using VoltstroEngine.Rendering.Texture;

namespace VoltstroEngine.Platform.OpenGL
{
	internal class OpenGL2DTexture : I2DTexture
	{
		private readonly uint textureID;

		private readonly uint width, height;

		public OpenGL2DTexture(string imagePath)
		{
			using MagickImage image = new MagickImage($"{Application.GameName}/{imagePath}");

			InternalFormat internalFormat = 0;
			PixelFormat dataFormat = 0;
			if (image.ChannelCount == 4)
			{
				internalFormat = InternalFormat.Rgba8;
				dataFormat = PixelFormat.Rgba;
			}
			else if (image.ChannelCount == 3)
			{
				internalFormat = InternalFormat.Rgb8;
				dataFormat = PixelFormat.Rgb;
			}

			Debug.Assert(internalFormat != 0, "Format not supported!");

			width = (uint)image.Width;
			height = (uint)image.Height;

			image.Flip();

			textureID = Gl.CreateTexture(TextureTarget.Texture2d);
			Gl.TextureStorage2D(textureID, 1, internalFormat, image.Width, image.Height);

			Gl.TextureParameteri(textureID, TextureParameterName.TextureMinFilter, Gl.LINEAR);
			Gl.TextureParameteri(textureID, TextureParameterName.TextureMagFilter, Gl.NEAREST);

			IPixelCollection<byte> pixels = image.GetPixels();
			byte[] data = pixels.ToArray();
			Gl.TextureSubImage2D(textureID, 0, 0, 0, image.Width, image.Height, dataFormat, PixelType.UnsignedByte, data);
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