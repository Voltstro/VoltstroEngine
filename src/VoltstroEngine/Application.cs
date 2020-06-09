using System.IO;
using OpenGL;
using VoltstroEngine.Events;
using VoltstroEngine.Layers;
using VoltstroEngine.Rendering;
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

		//TODO: Completely remove all OpenGL stuff here
		private uint vertexArray;
		private uint vertexBuffer;

		private uint indexBuffer;

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

			vertexBuffer = Gl.GenBuffer();
			Gl.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);

			float[] vertices = new float[3 * 3]
			{
				-0.5f, -0.5f, 0.0f,
				0.5f, -0.5f, 0.0f,
				0.0f, 0.5f, 0.0f
			};

			Gl.BufferData(BufferTarget.ArrayBuffer, (uint)Buffer.ByteLength(vertices), vertices, BufferUsage.StaticDraw);

			Gl.EnableVertexAttribArray(0);
			Gl.VertexAttribPointer(0, 3, VertexAttribType.Float, false, 3 * sizeof(float), null);

			indexBuffer = Gl.GenBuffer();
			Gl.BindBuffer(BufferTarget.ElementArrayBuffer, indexBuffer);

			int[] indices = new int[3]{0, 1, 2};
			Gl.BufferData(BufferTarget.ElementArrayBuffer, (uint)Buffer.ByteLength(indices), indices, BufferUsage.StaticDraw);

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
				Gl.DrawElements(PrimitiveType.Triangles, 3, DrawElementsType.UnsignedInt, null);

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