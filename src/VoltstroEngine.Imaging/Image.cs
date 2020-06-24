using System.IO;
using StbImageSharp;

namespace VoltstroEngine.Imaging
{
	/// <summary>
	/// Represents an image
	/// </summary>
	public class Image
	{
		/// <summary>
		/// Creates and loads and new image
		/// </summary>
		/// <param name="fileLocation">Location of the image</param>
		/// <param name="filp">Filp the image on load</param>
		/// <exception cref="FileNotFoundException"></exception>
		public Image(string fileLocation, bool filp)
		{
			if(!File.Exists(fileLocation))
				throw new FileNotFoundException("Image doesn't exist!", fileLocation);

			//Filp image on load, if we want to filp it
			if(filp)
				StbImage.stbi_set_flip_vertically_on_load(1);

			//Open stream
			FileStream imageStream = File.OpenRead(fileLocation);

			//Create image
			ImageResult image = ImageResult.FromStream(imageStream);
			Data = image.Data;
			Width = image.Width;
			Height = image.Height;

			//IDK if this was purposely done, but the enum number matches to the channels count of what the enum is
			Channels = (int)image.SourceComp;
		}

		/// <summary>
		/// The width of this image
		/// </summary>
		public readonly int Width;

		/// <summary>
		/// The height of this image
		/// </summary>
		public readonly int Height;

		/// <summary>
		/// How many channels does this image have?
		/// </summary>
		public readonly int Channels;

		/// <summary>
		/// The data of this image
		/// </summary>
		public readonly byte[] Data;
	}
}