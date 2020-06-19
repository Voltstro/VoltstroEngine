using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace VoltstroEngine.Rendering.Shaders
{
	public class ShaderLibrary
	{
		private readonly List<IShader> shaders;

		public ShaderLibrary()
		{
			shaders = new List<IShader>();
		}

		/// <summary>
		/// Adds a shader to the library
		/// </summary>
		/// <param name="name"></param>
		/// <param name="shader"></param>
		public void AddShader(string name, IShader shader)
		{
			Debug.Assert(!ShaderExist(name), "Shader exist in library!");

			shaders.Add(shader);
		}

		/// <summary>
		/// Adds a shader to the library
		/// </summary>
		/// <param name="shader"></param>
		public void AddShader(IShader shader)
		{
			AddShader(shader.GetShaderName(), shader);
		}

		/// <summary>
		/// Loads and adds a shader to the library
		/// </summary>
		/// <param name="filePath"></param>
		public void LoadAndAddShader(string filePath)
		{
			IShader shader = IShader.Create(filePath);
			AddShader(shader);
		}

		/// <summary>
		/// Gets a shader
		/// </summary>
		/// <param name="name"></param>
		public IShader GetShader(string name)
		{
			Debug.Assert(ShaderExist(name), "Shader doesn't exist!");
			return shaders.FirstOrDefault(x => x.GetShaderName() == name);
		}

		private bool ShaderExist(string name)
		{
			return shaders.FirstOrDefault(x => x.GetShaderName() == name) != null;
		}
	}
}