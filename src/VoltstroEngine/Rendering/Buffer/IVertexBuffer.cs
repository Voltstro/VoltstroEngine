using System;

namespace VoltstroEngine.Rendering.Buffer
{
	public interface IVertexBuffer
	{
		public static IVertexBuffer Create(object vertices, uint size)
		{
			switch (Renderer.Renderer.GetRenderingAPI())
			{
				case RenderingAPI.OpenGL:
					return new Platform.OpenGL.OpenGLVertexBuffer(vertices, size);
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public void Bind();
		public void UnBind();

		public void SetLayout(BufferLayout layout);
		public BufferLayout GetLayout();
	}
}