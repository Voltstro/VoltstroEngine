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
			using MagickImage image = new MagickImage(imagePath);

			width = (uint)image.Width;
			height = (uint)image.Height;

			image.Flip();

			textureID = Gl.CreateTexture(TextureTarget.Texture2d);
			Gl.TextureStorage2D(textureID, 1, InternalFormat.Rgb8, image.Width, image.Height);

			Gl.TextureParameteri(textureID, TextureParameterName.TextureMinFilter, Gl.LINEAR);
			Gl.TextureParameteri(textureID, TextureParameterName.TextureMagFilter, Gl.NEAREST);

			IPixelCollection<byte> pixels = image.GetPixels();
			byte[] data = pixels.ToArray();
			Gl.TextureSubImage2D(textureID, 0, 0, 0, image.Width, image.Height, PixelFormat.Rgb, PixelType.UnsignedByte, data);
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