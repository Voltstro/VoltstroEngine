using System.Collections.Generic;
using System.IO;
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

		private readonly IShader triangleShader;
		private readonly IShader squareShader;

		private readonly IVertexArray triangleVertexArray;
		private readonly IVertexArray squareVertexArray;

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

			// ----------
			//Triangle
			// ----------
			triangleVertexArray = IVertexArray.Create();

			float[] triangleVertices = new float[3 * 7]
			{
				-0.5f, -0.5f, 0.0f, 0.8f, 0.2f, 0.8f, 1.0f,
				 0.5f, -0.5f, 0.0f, 0.2f, 0.3f, 0.8f, 1.0f,
				 0.0f,  0.5f, 0.0f, 0.8f, 0.8f, 0.2f, 1.0f,
			};

			IVertexBuffer triangleVertexBuffer = IVertexBuffer.Create(triangleVertices, triangleVertices.GetBytes());
			
			BufferLayout triangleBufferLayout = new BufferLayout(new List<BufferElement>
			{
				new BufferElement("a_Position", ShaderDataType.Float3),
				new BufferElement("a_Color", ShaderDataType.Float4)
			});
			
			triangleVertexBuffer.SetLayout(triangleBufferLayout);
			triangleVertexArray.AddVertexBuffer(triangleVertexBuffer);
			
			uint[] triangleIndices = new uint[3]{0, 1, 2};
			IIndexBuffer triangleIndexBuffer = IIndexBuffer.Create(triangleIndices, triangleIndices.GetBytes() / sizeof(uint));
			triangleVertexArray.SetIndexBuffer(triangleIndexBuffer);

			//Triangle Shader
			//TODO: Asset managing
			string triangleVertexSrc = File.ReadAllText("Shaders/Triangle.vert").Replace("\r\n", "\n");
			string triangleFragmentSrc = File.ReadAllText("Shaders/Triangle.frag").Replace("\r\n", "\n");

			triangleShader = IShader.Create("Triangle", triangleVertexSrc, triangleFragmentSrc);

			// ----------
			//Square
			// ----------
			squareVertexArray = IVertexArray.Create();

			float[] squareVertices = new float[3 * 4]
			{
				-0.75f, -0.75f, 0.0f,
				 0.75f, -0.75f, 0.0f,
				 0.75f,  0.75f, 0.0f,
				-0.75f,  0.75f, 0.0f
			};

			IVertexBuffer squareVertexBuffer = IVertexBuffer.Create(squareVertices, triangleVertices.GetBytes());

			BufferLayout squareBufferLayout = new BufferLayout(new List<BufferElement>
			{
				new BufferElement("a_Position", ShaderDataType.Float3)
			});
			squareVertexBuffer.SetLayout(squareBufferLayout);
			squareVertexArray.AddVertexBuffer(squareVertexBuffer);

			uint[] squareIndices = new uint[6]{0, 1, 2, 2, 3, 0};
			IIndexBuffer squareIndexBuffer = IIndexBuffer.Create(squareIndices, squareIndices.GetBytes() / sizeof(uint));
			squareVertexArray.SetIndexBuffer(squareIndexBuffer);

			//Square shader
			string squareVertexSrc = File.ReadAllText("Shaders/Square.vert").Replace("\r\n", "\n");
			string squareFragmentSrc = File.ReadAllText("Shaders/Square.frag").Replace("\r\n", "\n");
			squareShader = IShader.Create("Square", squareVertexSrc, squareFragmentSrc);
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

				Renderer.BeginScene();
				{
					//Square
					squareShader.Bind();
					Renderer.Submit(squareVertexArray);

					//Triangle
					triangleShader.Bind();
					Renderer.Submit(triangleVertexArray);
				}
				Renderer.EndScene();

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