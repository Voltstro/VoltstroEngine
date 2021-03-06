﻿using System;
using VoltstroEngine.Rendering.Renderer;

namespace VoltstroEngine.Rendering.Buffer
{
	public interface IIndexBuffer : IDisposable
	{
		public static IIndexBuffer Create(object indices, uint count)
		{
			switch (RenderingAPI.GetRenderingAPI())
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