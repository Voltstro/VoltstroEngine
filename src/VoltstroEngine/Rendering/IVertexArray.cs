﻿using System;
using VoltstroEngine.Rendering.Buffer;

namespace VoltstroEngine.Rendering
{
	public interface IVertexArray
	{
		public static IVertexArray Create()
		{
			switch (Renderer.GetRenderingAPI())
			{
				case RenderingAPI.OpenGL:
					return new Platform.OpenGL.OpenGLVertexArray();
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public void Bind();
		public void UnBind();

		public void AddVertexBuffer(IVertexBuffer vertexBuffer);
		public void SetIndexBuffer(IIndexBuffer indexBuffer);

		public IVertexBuffer[] GetVertexBuffers();
		public IIndexBuffer GetIndexBuffer();
	}
}