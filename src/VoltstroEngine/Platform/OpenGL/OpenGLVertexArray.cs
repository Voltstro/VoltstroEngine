using System.Collections.Generic;
using System.Diagnostics;
using OpenGL;
using VoltstroEngine.Rendering;
using VoltstroEngine.Rendering.Buffer;

namespace VoltstroEngine.Platform.OpenGL
{
	public class OpenGLVertexArray : IVertexArray
	{
		private readonly uint renderID;
		private readonly List<IVertexBuffer> buffers;
		private IIndexBuffer indexBuffer;

		public OpenGLVertexArray()
		{
			renderID = Gl.CreateVertexArray();
			buffers = new List<IVertexBuffer>();
		}

		~OpenGLVertexArray()
		{
			Gl.DeleteVertexArrays(renderID);
		}

		public void Bind()
		{
			Gl.BindVertexArray(renderID);
		}

		public void UnBind()
		{
			Gl.BindVertexArray(0);
		}

		public void AddVertexBuffer(IVertexBuffer buffer)
		{
			BufferLayout layout = buffer.GetLayout();
			Debug.Assert(layout.Elements.Count != 0, "Vertex Buffer has no layout!");

			Gl.BindVertexArray(renderID);
			buffer.Bind();

			uint index = 0;
			foreach (BufferElement element in layout.Elements)
			{
				Gl.EnableVertexAttribArray(index);
				Gl.VertexAttribPointer(index, (int)element.GetComponentCount(), ShaderDataTypeToOpenGLBaseType(element.Type), element.Normalized, (int)layout.Stride, element.Offset);
				index++;
			}

			buffers.Add(buffer);
		}

		public void SetIndexBuffer(IIndexBuffer buffer)
		{
			Gl.BindVertexArray(renderID);
			buffer.Bind();

			indexBuffer = buffer;
		}

		public IVertexBuffer[] GetVertexBuffers()
		{
			return buffers.ToArray();
		}

		public IIndexBuffer GetIndexBuffer()
		{
			return indexBuffer;
		}

		private VertexAttribType ShaderDataTypeToOpenGLBaseType(ShaderDataType type)
		{
			switch (type)
			{
				case ShaderDataType.Float:    return VertexAttribType.Float;
				case ShaderDataType.Float2:   return VertexAttribType.Float;
				case ShaderDataType.Float3:   return VertexAttribType.Float;
				case ShaderDataType.Float4:   return VertexAttribType.Float;
				case ShaderDataType.Mat3:     return VertexAttribType.Float;
				case ShaderDataType.Mat4:     return VertexAttribType.Float;
				case ShaderDataType.Int:      return VertexAttribType.Int;
				case ShaderDataType.Int2:     return VertexAttribType.Int;
				case ShaderDataType.Int3:     return VertexAttribType.Int;
				case ShaderDataType.Int4:     return VertexAttribType.Int;
			}

			Debug.Assert(false, "ShaderDataType to OpenGL doesn't exist!");
			return 0;
		}
	}
}