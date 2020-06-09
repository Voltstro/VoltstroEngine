using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using OpenGL;
using VoltstroEngine.Logging;
using VoltstroEngine.Rendering.Shaders;

namespace VoltstroEngine.Platform.OpenGL
{
	public class OpenGLShader : IShader
	{
		public readonly string ShaderName;
		private List<uint> shaderIDs;
		private uint program;

		public OpenGLShader(string name, string vertexSrc, string fragmentSrc)
		{
			ShaderName = name;
			Compile(new Dictionary<ShaderType, string>
			{
				[ShaderType.VertexShader] = vertexSrc,
				[ShaderType.FragmentShader] = fragmentSrc
			});
		}

		~OpenGLShader()
		{
			Gl.DeleteProgram(program);
		}

		public void Bind()
		{
			Gl.UseProgram(program);
		}

		public void UnBind()
		{
			Gl.UseProgram(0);
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
				Gl.ShaderSource(shader, source.Split(new []{Environment.NewLine}, StringSplitOptions.None));

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

			foreach (uint shader in shaderIDs)
			{
				Gl.DetachShader(program, shader);
			}
		}
	}
}