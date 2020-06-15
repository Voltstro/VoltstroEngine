using System.Collections.Generic;
using System.IO;
using VoltstroEngine;
using VoltstroEngine.Events;
using VoltstroEngine.Extensions;
using VoltstroEngine.Layers;
using VoltstroEngine.Rendering;
using VoltstroEngine.Rendering.Buffer;
using VoltstroEngine.Rendering.Camera;
using VoltstroEngine.Rendering.Shaders;

namespace Sandbox
{
	public class ExampleLayer : ILayer
	{
		private readonly IShader triangleShader;
		private readonly IShader squareShader;

		private readonly IVertexArray triangleVertexArray;
		private readonly IVertexArray squareVertexArray;

		private readonly OrthographicCamera camera;

		public ExampleLayer()
		{
			//Create camera
			camera = new OrthographicCamera(-1.6f, 1.6f, -0.9f, 0.9f);

			// ----------
			//Triangle
			// ----------
			triangleVertexArray = IVertexArray.Create();

			float[] triangleVertices = 
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
			
			uint[] triangleIndices = {0, 1, 2};
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

			float[] squareVertices = {
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

			uint[] squareIndices = {0, 1, 2, 2, 3, 0};
			IIndexBuffer squareIndexBuffer = IIndexBuffer.Create(squareIndices, squareIndices.GetBytes() / sizeof(uint));
			squareVertexArray.SetIndexBuffer(squareIndexBuffer);

			//Square shader
			string squareVertexSrc = File.ReadAllText("Shaders/Square.vert").Replace("\r\n", "\n");
			string squareFragmentSrc = File.ReadAllText("Shaders/Square.frag").Replace("\r\n", "\n");
			squareShader = IShader.Create("Square", squareVertexSrc, squareFragmentSrc);
		}

		public void OnAttach()
		{

		}

		public void OnDetach()
		{
			
		}

		public void OnUpdate()
		{
			Renderer.SetClearColor(0.2f, 0.2f, 0.2f);
			Renderer.Clear();

			Renderer.BeginScene(camera);
			{
				//Square
				Renderer.Submit(squareShader, squareVertexArray);

				//Triangle
				Renderer.Submit(triangleShader, triangleVertexArray);
			}
			Renderer.EndScene();
		}

		public void OnEvent(IEvent e)
		{
			
		}
	}

	public class SandboxApp : Application
	{
		public SandboxApp()
		{
			PushOverlay(new ExampleLayer());
		}
	}
}