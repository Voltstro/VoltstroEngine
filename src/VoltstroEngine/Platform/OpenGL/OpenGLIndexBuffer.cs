using OpenGL;
using VoltstroEngine.Rendering.Buffer;

namespace VoltstroEngine.Platform.OpenGL
{
	public class OpenGLIndexBuffer : IIndexBuffer
	{
		private readonly uint count;
		private readonly uint renderID;

		public OpenGLIndexBuffer(object indices, uint count)
		{
			this.count = count;
			renderID = Gl.CreateBuffer();
			Gl.BindBuffer(BufferTarget.ElementArrayBuffer, renderID);
			Gl.BufferData(BufferTarget.ElementArrayBuffer, count * sizeof(uint), indices, BufferUsage.StaticDraw);
		}

		public void Bind()
		{
			Gl.BindBuffer(BufferTarget.ElementArrayBuffer, renderID);
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
			Gl.DeleteBuffers(renderID);
		}
	}
}