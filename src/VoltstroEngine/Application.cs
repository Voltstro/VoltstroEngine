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
		private bool isRunning = true;
		private readonly IWindow window;
		private readonly LayerStack layerStack;

		private IShader shader;
		private IVertexArray vertexArray;

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

			vertexArray = IVertexArray.Create();

			float[] vertices = new float[3 * 7]
			{
				-0.5f, -0.5f, 0.0f, 0.8f, 0.2f, 0.8f, 1.0f,
				0.5f, -0.5f, 0.0f, 0.2f, 0.3f, 0.8f, 1.0f,
				0.0f,  0.5f, 0.0f, 0.8f, 0.8f, 0.2f, 1.0f
			};

			IVertexBuffer vertexBuffer = IVertexBuffer.Create(vertices, vertices.GetBytes());
			BufferLayout layout = new BufferLayout(new List<BufferElement>
			{
				new BufferElement(ShaderDataType.Float3, "a_Position"),
				new BufferElement(ShaderDataType.Float4, "a_Color")
			});
			vertexBuffer.SetLayout(layout);
			vertexArray.AddVertexBuffer(vertexBuffer);

			uint[] indices = new uint[3]{0, 1, 2};
			IIndexBuffer indexBuffer = IIndexBuffer.Create(indices, indices.GetBytes() / sizeof(uint));
			vertexArray.SetIndexBuffer(indexBuffer);

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

				vertexArray.Bind();
				Gl.DrawElements(PrimitiveType.Triangles, (int)vertexArray.GetIndexBuffer().GetCount(), DrawElementsType.UnsignedInt, null);

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