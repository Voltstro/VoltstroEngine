using System;
using OpenGL;
using VoltstroEngine.Rendering.Buffer;

namespace VoltstroEngine.Platform.OpenGL
{
	internal sealed class OpenGLIndexBuffer : IIndexBuffer
	{
		private readonly uint count;
		private readonly uint bufferID;

		public OpenGLIndexBuffer(object indices, uint count)
		{
			this.count = count;
			bufferID = Gl.CreateBuffer();
			Gl.BindBuffer(BufferTarget.ElementArrayBuffer, bufferID);
			Gl.BufferData(BufferTarget.ElementArrayBuffer, count * sizeof(uint), indices, BufferUsage.StaticDraw);
		}

		public void Bind()
		{
			Gl.BindBuffer(BufferTarget.ElementArrayBuffer, bufferID);
		}

		public void UnBind()
		{
			Gl.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
		}

		public uint GetCount()
		{
			return count;
		}

		~OpenGLIndexBuffer()
		{
			ReleaseUnmanagedResources();
		}

		private void ReleaseUnmanagedResources()
		{
			Gl.DeleteBuffers(bufferID);
		}

		public void Dispose()
		{
			ReleaseUnmanagedResources();
			GC.SuppressFinalize(this);
		}
	}
}