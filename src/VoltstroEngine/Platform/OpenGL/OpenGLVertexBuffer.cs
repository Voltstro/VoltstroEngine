﻿using OpenGL;
using VoltstroEngine.Rendering.Buffer;

namespace VoltstroEngine.Platform.OpenGL
{
	public class OpenGLVertexBuffer : IVertexBuffer
	{
		private readonly uint renderID;

		public OpenGLVertexBuffer(object vertices, uint size)
		{
			renderID = Gl.CreateBuffer();
			Gl.BindBuffer(BufferTarget.ArrayBuffer, renderID);
			Gl.BufferData(BufferTarget.ArrayBuffer, size, vertices, BufferUsage.StaticDraw);
		}

		~OpenGLVertexBuffer()
		{
			Gl.DeleteBuffers(renderID);
		}

		public void Bind()
		{
			Gl.BindBuffer(BufferTarget.ArrayBuffer, renderID);
		}

		public void UnBind()
		{
			Gl.BindBuffer(BufferTarget.ArrayBuffer, 0);
		}
	}
}