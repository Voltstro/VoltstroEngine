using System;
using System.Collections.Generic;
using System.IO;
using OpenGL;
using VoltstroEngine.Events;
using VoltstroEngine.Extensions;
using VoltstroEngine.Layers;
using VoltstroEngine.Rendering;
using VoltstroEngine.Rendering.Buffer;
using VoltstroEngine.Rendering.Shaders;
using VoltstroEngine.Window;

namespace VoltstroEngine
{
	public class Application
	{
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

		private bool isRunning = true;
		private readonly IWindow window;
		private readonly LayerStack layerStack;

		private IShader shader;
		private IIndexBuffer indexBuffer;
		private IVertexBuffer vertexBuffer;

		//TODO: Completely remove all OpenGL stuff here
		private uint vertexArray;

		public Application()
		{
			Renderer.Init();

			//Creates a new window
			window = IWindow.CreateWindow(new WindowProperties
			{
				Title = "Voltstro Engine",
				Width = 1280,
				Height = 720,
				VSync = true
			});
			window.OnEvent += WindowOnOnEvent;

			layerStack = new LayerStack();

			vertexArray = Gl.GenVertexArray();
			Gl.BindVertexArray(vertexArray);

			float[] vertices = new float[3 * 7]
			{
				-0.5f, -0.5f, 0.0f, 0.8f, 0.2f, 0.8f, 1.0f,
				 0.5f, -0.5f, 0.0f, 0.2f, 0.3f, 0.8f, 1.0f,
				 0.0f,  0.5f, 0.0f, 0.8f, 0.8f, 0.2f, 1.0f,
			};

			vertexBuffer = IVertexBuffer.Create(vertices, vertices.GetBytes());
			
			BufferLayout layout = new BufferLayout(new List<BufferElement>
			{
				new BufferElement("a_Position", ShaderDataType.Float3),
				new BufferElement("a_Color", ShaderDataType.Float4)
			});
			
			vertexBuffer.SetLayout(layout);

			uint index = 0;
			foreach (BufferElement element in layout.Elements)
			{
				Gl.EnableVertexAttribArray(index);
				Gl.VertexAttribPointer(index, 
					(int)element.GetComponentCount(), 
					ShaderDataTypeToOpenGLBaseType(element.Type), 
					element.Normalized, 
					(int)layout.Stride, 
					(IntPtr)element.Offset);
				index++;
			}

			uint[] indices = new uint[3]{0, 1, 2};
			indexBuffer = IIndexBuffer.Create(indices, indices.GetBytes() / sizeof(uint));

			//TODO: Asset managing
			string vertexSrc = File.ReadAllText("Shaders/Triangle.vert").Replace("\r\n", "\n");
			string fragmentSrc = File.ReadAllText("Shaders/Triangle.frag").Replace("\r\n", "\n");

			shader = IShader.Create("Triangle", vertexSrc, fragmentSrc);
		}

		private void WindowOnOnEvent(IEvent e)
		{
			EventDispatcher eventDispatcher = new EventDispatcher();
			eventDispatcher.DispatchEvent<WindowCloseEvent>(e, OnClose);

			foreach (ILayer layer in layerStack.GetLayers())
				layer.OnEvent(e);
		}

		private void OnClose()
		{
			isRunning = false;
		}

		/// <summary>
		/// Runs the application
		/// </summary>
		public void Run()
		{
			while (isRunning)
			{
				Renderer.SetClearColor(0.2f, 0.2f, 0.2f);
				Renderer.Clear();

				shader.Bind();

				Gl.BindVertexArray(vertexArray);
				Gl.DrawElements(PrimitiveType.Triangles, (int)indexBuffer.GetCount(), DrawElementsType.UnsignedInt, null);

				foreach (ILayer layer in layerStack.GetLayers())
					layer.OnUpdate();

				window.OnUpdate();
			}
		}

		public void PushLayer(ILayer layer)
		{
			layerStack.PushLayer(layer);
			layer.OnAttach();
		}

		public void PushOverlay(ILayer layer)
		{
			layerStack.PushOverlay(layer);
			layer.OnAttach();
		}
	}
}