using System;
using System.Collections.Generic;
using System.Diagnostics;
using OpenGL;
using VoltstroEngine.Rendering;
using VoltstroEngine.Rendering.Buffer;

namespace VoltstroEngine.Platform.OpenGL
{
	public class OpenGLVertexArray : IVertexArray
	{
		private IIndexBuffer indexBuffer;
		private readonly List<IVertexBuffer> vertexBuffers;

		private readonly uint rendererID;

		public OpenGLVertexArray()
		{
			vertexBuffers = new List<IVertexBuffer>();
			rendererID = Gl.CreateVertexArray();
		}

		~OpenGLVertexArray()
		{
			Gl.DeleteVertexArrays(rendererID);
		}

		public void Bind()
		{
			Gl.BindVertexArray(rendererID);
		}

		public void UnBind()
		{
			Gl.BindVertexArray(0);
		}

		public void AddVertexBuffer(IVertexBuffer vertexBuffer)
		{
			Debug.Assert(vertexBuffer.GetLayout().Elements.Count != 0, "Vertex buffer has no layout!");

			Gl.BindVertexArray(rendererID);
			vertexBuffer.Bind();

			uint index = 0;
			foreach (BufferElement element in vertexBuffer.GetLayout().Elements)
			{
				Gl.EnableVertexAttribArray(index);
				Gl.VertexAttribPointer(index, 
					(int)element.GetComponentCount(), 
					ShaderDataTypeToOpenGLBaseType(element.Type), 
					element.Normalized, 
					(int)vertexBuffer.GetLayout().Stride, 
					(IntPtr)element.Offset);
				index++;
			}

			vertexBuffers.Add(vertexBuffer);
		}

		public void SetIndexBuffer(IIndexBuffer buffer)
		{
			Gl.BindVertexArray(rendererID);

			buffer.Bind();
			indexBuffer = buffer;
		}

		public IVertexBuffer[] GetVertexBuffers()
		{
			return vertexBuffers.ToArray();
		}

		public IIndexBuffer GetIndexBuffer()
		{
			return indexBuffer;
		}

		private VertexAttribType ShaderDataTypeToOpenGLBaseType(ShaderDataType type)
		{
			switch (type)
			{
				case ShaderDataType.None:
					return 0;

				case ShaderDataType.Float:
				case ShaderDataType.Float2:
				case ShaderDataType.Float3:
				case ShaderDataType.Float4:
				case ShaderDataType.Mat3:
				case ShaderDataType.Mat4:
					return VertexAttribType.Float;

				case ShaderDataType.Int:
				case ShaderDataType.Int2:
				case ShaderDataType.Int3:
				case ShaderDataType.Int4:
					return VertexAttribType.Int;
				default:
					throw new ArgumentOutOfRangeException(nameof(type), type, null);
			}
		}
	}
}