using System;
using OpenGL;
using VoltstroEngine.Rendering.Buffer;

namespace VoltstroEngine.Platform.OpenGL
{
	internal sealed class OpenGLVertexBuffer : IVertexBuffer
	{
		private readonly uint renderID;

		private BufferLayout bufferLayout;

		public OpenGLVertexBuffer(object vertices, uint size)
		{
			renderID = Gl.CreateBuffer();
			Gl.BindBuffer(BufferTarget.ArrayBuffer, renderID);
			Gl.BufferData(BufferTarget.ArrayBuffer, size, vertices, BufferUsage.StaticDraw);
		}

		public void Bind()
		{
			Gl.BindBuffer(BufferTarget.ArrayBuffer, renderID);
		}

		public void UnBind()
		{
			Gl.BindBuffer(BufferTarget.ArrayBuffer, 0);
		}

		public void SetLayout(BufferLayout layout)
		{
			bufferLayout = layout;
		}

		public BufferLayout GetLayout()
		{
			return bufferLayout;
		}

		~OpenGLVertexBuffer()
		{
			ReleaseUnmanagedResources();
		}

		private void ReleaseUnmanagedResources()
		{
			Gl.DeleteBuffers(renderID);
		}

		public void Dispose()
		{
			ReleaseUnmanagedResources();
			GC.SuppressFinalize(this);
		}
	}
}