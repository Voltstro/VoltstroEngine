using System;
using System.Numerics;
using VoltstroEngine.Rendering.Renderer;

namespace VoltstroEngine.Rendering.Shaders
{
	/// <summary>
	/// A shader... what did you except?
	/// </summary>
	public interface IShader
	{
		/// <summary>
		/// Creates a new shader
		/// </summary>
		/// <returns></returns>
		public static IShader Create(string shaderPath)
		{
			switch (Renderer.RenderingAPI.GetRenderingAPI())
			{
				case RenderingAPIType.OpenGL:
					return new Platform.OpenGL.OpenGLShader(shaderPath);
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		/// <summary>
		/// Creates a new shader
		/// </summary>
		/// <returns></returns>
		public static IShader Create(string name, string vertexSrc, string fragmentSrc)
		{
			switch (Renderer.RenderingAPI.GetRenderingAPI())
			{
				case RenderingAPIType.OpenGL:
					return new Platform.OpenGL.OpenGLShader(name, vertexSrc, fragmentSrc);
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		/// <summary>
		/// Binds the shader
		/// </summary>
		public void Bind();

		/// <summary>
		/// Un-binds the shader
		/// </summary>
		public void UnBind();

		/// <summary>
		/// Sets a <see cref="int"/> in the shader
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public void SetInt(string name, int value);

		/// <summary>
		/// Sets a <see cref="Vector3"/> in the shader
		/// </summary>
		/// <param name="name"></param>
		/// <param name="vector"></param>
		public void SetVec3(string name, Vector3 vector);

		/// <summary>
		/// Sets a <see cref="Vector4"/> in the shader
		/// </summary>
		/// <param name="name"></param>
		/// <param name="vector"></param>
		public void SetVec4(string name, Vector4 vector);

		/// <summary>
		/// Sets a <see cref="Matrix4x4"/> in the shader
		/// </summary>
		/// <param name="name"></param>
		/// <param name="matrix"></param>
		public void SetMat4(string name, Matrix4x4 matrix);

		/// <summary>
		/// Get this shader's name
		/// </summary>
		/// <returns></returns>
		public string GetShaderName();
	}
}