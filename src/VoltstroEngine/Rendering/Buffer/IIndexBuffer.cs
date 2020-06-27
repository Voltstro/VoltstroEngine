using System;
using VoltstroEngine.Rendering.Renderer;

namespace VoltstroEngine.Rendering.Buffer
{
	public interface IIndexBuffer
	{
		public static IIndexBuffer Create(object indices, uint count)
		{
			switch (Renderer.RenderingAPI.GetRenderingAPI())
			{
				case RenderingAPIType.OpenGL:
					return new Platform.OpenGL.OpenGLIndexBuffer(indices, count);
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public void Bind();

		public void UnBind();

		public uint GetCount();
	}
}