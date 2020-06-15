using System;
using System.Numerics;
using OpenGL;

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
		public static IShader Create(string name, string vertexSrc, string fragmentSrc)
		{
			switch (Renderer.GetRenderingAPI())
			{
				case RenderingAPI.OpenGL:
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
		/// Uploads a uniform <see cref="Matrix4x4"/>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="matrix"></param>
		public void UploadUniformMat4(string name, Matrix4x4 matrix);
	}
}