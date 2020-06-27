using System.IO;
using VoltstroEngine.Core;

namespace VoltstroEngine.Assets
{
	public static class AssetManager
	{
		private static string GameAssetsPath => Engine.GameName;

		/// <summary>
		/// Reads all text from a file in the game's directory
		/// </summary>
		/// <param name="file"></param>
		/// <returns></returns>
		public static string ReadAllText(string file)
		{
			return File.ReadAllText($"{GameAssetsPath}/{file}");
		}
	}
}