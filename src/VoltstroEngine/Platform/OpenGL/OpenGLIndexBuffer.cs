using OpenGL;
using VoltstroEngine.Rendering.Buffer;

namespace VoltstroEngine.Platform.OpenGL
{
	public class OpenGLIndexBuffer : IIndexBuffer
	{
		private readonly uint renderID;
		private readonly uint count;

		public OpenGLIndexBuffer(object indices, uint count)
		{
			this.count = count;
			renderID = Gl.CreateBuffer();
			Gl.BindBuffer(BufferTarget.ElementArrayBuffer, renderID);
			Gl.BufferData(BufferTarget.ElementArrayBuffer, count * sizeof(uint), indices, BufferUsage.StaticDraw);
		}

		~OpenGLIndexBuffer()
		{
			Gl.DeleteBuffers(renderID);
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
	}
}