using System.Diagnostics;
using OpenGL;
using VoltstroEngine.Core;
using VoltstroEngine.Imaging;
using VoltstroEngine.Rendering.Texture;

namespace VoltstroEngine.Platform.OpenGL
{
	internal class OpenGL2DTexture : I2DTexture
	{
		private readonly uint textureID;

		private readonly uint width, height;

		private readonly PixelFormat dataFormat;

		public OpenGL2DTexture(uint width, uint height)
		{
			InternalFormat internalFormat = InternalFormat.Rgba8;
			dataFormat = PixelFormat.Rgba;

			this.width = width;
			this.height = height;

			textureID = Gl.CreateTexture(TextureTarget.Texture2d);
			Gl.TextureStorage2D(textureID, 1, internalFormat, (int)width, (int)height);

			Gl.TextureParameteri(textureID, TextureParameterName.TextureMinFilter, Gl.LINEAR);
			Gl.TextureParameteri(textureID, TextureParameterName.TextureMagFilter, Gl.NEAREST);

			Gl.TextureParameterIi(textureID, TextureParameterName.TextureWrapS, Gl.REPEAT);
			Gl.TextureParameterIi(textureID, TextureParameterName.TextureWrapT, Gl.REPEAT);
		}

		public OpenGL2DTexture(string imagePath)
		{
			Image image = new Image($"{Engine.GameName}/{imagePath}", true);

			InternalFormat internalFormat = 0;
			dataFormat = 0;
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

			width = (uint) image.Width;
			height = (uint) image.Height;

			textureID = Gl.CreateTexture(TextureTarget.Texture2d);
			Gl.TextureStorage2D(textureID, 1, internalFormat, image.Width, image.Height);

			Gl.TextureParameteri(textureID, TextureParameterName.TextureMinFilter, Gl.LINEAR);
			Gl.TextureParameteri(textureID, TextureParameterName.TextureMagFilter, Gl.NEAREST);

			Gl.TextureParameterIi(textureID, TextureParameterName.TextureWrapS, Gl.REPEAT);
			Gl.TextureParameterIi(textureID, TextureParameterName.TextureWrapT, Gl.REPEAT);

			Gl.TextureSubImage2D(textureID, 0, 0, 0, image.Width, image.Height, dataFormat, PixelType.UnsignedByte,
				image.Data);
		}

		public uint GetWidth()
		{
			return width;
		}

		public uint GetHeight()
		{
			return height;
		}

		public void SetData(object data, uint size)
		{
#if DEBUG
			uint bpp = dataFormat == PixelFormat.Rgba ? 4 : (uint)3;
			Debug.Assert(bpp == width * height * bpp, "Data must be entire texture!");
#endif

			Gl.TextureSubImage2D(textureID, 0, 0, 0, (int)width, (int)height, dataFormat, PixelType.UnsignedByte, data);
		}

		public void Bind(uint slot = 0)
		{
			Gl.BindTextureUnit(slot, textureID);
		}

		~OpenGL2DTexture()
		{
			Gl.DeleteTextures(textureID);
		}
	}
}