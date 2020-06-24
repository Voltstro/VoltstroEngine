using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Text;
using OpenGL;
using VoltstroEngine.Core;
using VoltstroEngine.Core.Logging;
using VoltstroEngine.Rendering.Shaders;

namespace VoltstroEngine.Platform.OpenGL
{
	public class OpenGLShader : IShader
	{
		private static readonly string[] ShaderTypes = {"vert", "frag"};

		public readonly string ShaderName;
		private uint program;
		private List<uint> shaderIDs;

		public OpenGLShader(string shaderPath)
		{
			//Read shader file, we remove \r\n otherwise it fucks up OpenGL, this is a Windows only issue, of course...
			string shaderFileText = File.ReadAllText($"{Application.GameName}/{shaderPath}").Replace("\r\n", "\n");

			//Split the shader
			string[] sources = shaderFileText.Split("#type ", StringSplitOptions.RemoveEmptyEntries);
			Dictionary<ShaderType, string> shaderSources = new Dictionary<ShaderType, string>();
			foreach (string source in sources)
			foreach (string shaderType in ShaderTypes)
			{
				if (!source.StartsWith(shaderType)) continue;

				shaderSources.Add(GetShaderTypeFromString(shaderType), source.Replace(shaderType, ""));
				break;
			}

			Compile(shaderSources);
			ShaderName = Path.GetFileNameWithoutExtension($"{Application.GameName}/{shaderPath}");
		}

		public OpenGLShader(string name, string vertexSrc, string fragmentSrc)
		{
			ShaderName = name;
			Compile(new Dictionary<ShaderType, string>
			{
				[ShaderType.VertexShader] = vertexSrc,
				[ShaderType.FragmentShader] = fragmentSrc
			});
		}

		public void Bind()
		{
			Gl.UseProgram(program);
		}

		public void UnBind()
		{
			Gl.UseProgram(0);
		}

		public void UploadUniformMat4(string name, Matrix4x4 matrix)
		{
			int location = Gl.GetUniformLocation(program, name);
			Gl.UniformMatrix4f(location, 1, false, matrix);
		}

		public void UploadUniformInt(string name, int value)
		{
			int location = Gl.GetUniformLocation(program, name);
			Gl.Uniform1i(location, 1, value);
		}

		public string GetShaderName()
		{
			return ShaderName;
		}

		~OpenGLShader()
		{
			Gl.DeleteProgram(program);
		}

		private ShaderType GetShaderTypeFromString(string type)
		{
			switch (type)
			{
				case "vert":
					return ShaderType.VertexShader;
				case "frag":
					return ShaderType.FragmentShader;
				default:
					Debug.Assert(false, "Unknown shader type!");
					break;
			}

			return 0;
		}

		/// <summary>
		/// Compiles the shader
		/// </summary>
		private void Compile(Dictionary<ShaderType, string> shaderSources)
		{
			program = Gl.CreateProgram();
			shaderIDs = new List<uint>();

			foreach (KeyValuePair<ShaderType, string> shaderSource in shaderSources)
			{
				ShaderType type = shaderSource.Key;
				string source = shaderSource.Value;

				//Create the shader
				uint shader = Gl.CreateShader(type);

				//Upload the shader source
				Gl.ShaderSource(shader, source.Split(new[] {Environment.NewLine}, StringSplitOptions.None));

				//Compile the shader
				Gl.CompileShader(shader);

				Gl.GetShader(shader, ShaderParameterName.CompileStatus, out int compiled);

				//Shader failed to compile
				if (compiled == 0)
				{
					const int logMaxLength = 1024;

					StringBuilder infoLog = new StringBuilder(logMaxLength);
					Gl.GetShaderInfoLog(shader, logMaxLength, out int infoLogLength, infoLog);

					Logger.Log($"The shader failed to compile!\n{infoLog}", LogVerbosity.Error);
					Debug.Assert(false, $"The shader failed to compile!\n{infoLog}");
#if !DEBUG
					return;
#endif
				}

				Gl.AttachShader(program, shader);
				shaderIDs.Add(shader);
			}

			//Link our program object
			Gl.LinkProgram(program);

			Gl.GetProgram(program, ProgramProperty.LinkStatus, out int linked);
			if (linked == 0)
			{
				const int logMaxLength = 1024;

				StringBuilder infoLog = new StringBuilder(logMaxLength);

				Gl.GetProgramInfoLog(program, 1024, out int infoLogLength, infoLog);

				Logger.Log($"The program failed to link!\n{infoLog}", LogVerbosity.Error);
				Debug.Assert(false, $"The program failed to link!\n{infoLog}");
#if !DEBUG
					return;
#endif
			}

			foreach (uint shader in shaderIDs) Gl.DetachShader(program, shader);
		}
	}
}