using System.IO;
using OpenGL;
using VoltstroEngine.Events;
using VoltstroEngine.Extensions;
using VoltstroEngine.Layers;
using VoltstroEngine.Rendering;
using VoltstroEngine.Rendering.Buffer;
using VoltstroEngine.Rendering.Shaders;
using VoltstroEngine.Window;
using Buffer = System.Buffer;

namespace VoltstroEngine
{
	public class Application
	{
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

			float[] vertices = new float[3 * 3]
			{
				-0.5f, -0.5f, 0.0f,
				0.5f, -0.5f, 0.0f,
				0.0f, 0.5f, 0.0f
			};

			vertexBuffer = IVertexBuffer.Create(vertices, vertices.GetBytes());

			Gl.BufferData(BufferTarget.ArrayBuffer, (uint)Buffer.ByteLength(vertices), vertices, BufferUsage.StaticDraw);

			Gl.EnableVertexAttribArray(0);
			Gl.VertexAttribPointer(0, 3, VertexAttribType.Float, false, 3 * sizeof(float), null);

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