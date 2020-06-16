using System.Collections.Generic;
using System.Numerics;
using VoltstroEngine;
using VoltstroEngine.Assets;
using VoltstroEngine.Events;
using VoltstroEngine.Extensions;
using VoltstroEngine.Inputs;
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

		private Vector3 cameraPosition = Vector3.Zero;

		private const float MoveSpeed = 0.1f;

		private static readonly Matrix4x4 Scale = Matrix4x4.CreateScale(0.1f);

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
			string triangleVertexSrc = AssetManager.ReadAllText("Shaders/Triangle.vert").Replace("\r\n", "\n");
			string triangleFragmentSrc = AssetManager.ReadAllText("Shaders/Triangle.frag").Replace("\r\n", "\n");

			triangleShader = IShader.Create("Triangle", triangleVertexSrc, triangleFragmentSrc);

			// ----------
			//Square
			// ----------
			squareVertexArray = IVertexArray.Create();

			float[] squareVertices = {
				-0.5f, -0.5f, 0.0f,
				 0.5f, -0.5f, 0.0f,
				 0.5f,  0.5f, 0.0f,
				-0.5f,  0.5f, 0.0f
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
			string squareVertexSrc = AssetManager.ReadAllText("Shaders/Square.vert").Replace("\r\n", "\n");
			string squareFragmentSrc = AssetManager.ReadAllText("Shaders/Square.frag").Replace("\r\n", "\n");
			squareShader = IShader.Create("Square", squareVertexSrc, squareFragmentSrc);
		}

		public void OnAttach()
		{

		}

		public void OnDetach()
		{
			
		}

		public void OnUpdate(TimeStep ts)
		{
			Renderer.SetClearColor(0.2f, 0.2f, 0.2f);
			Renderer.Clear();

			camera.SetPosition(cameraPosition);

			Renderer.BeginScene(camera);
			{
				//Square
				for (int y = 0; y < 10; y++)
				{
					for (int x = 0; x < 10; x++)
					{
						Vector3 pos = new Vector3(x * 1.1f, y * 1.1f, 0);
						Matrix4x4 transform = Matrix4x4.CreateTranslation(pos) * Scale;

						Renderer.Submit(squareShader, squareVertexArray, transform);
					}
				}

				//Triangle
				//Renderer.Submit(triangleShader, triangleVertexArray, Matrix4x4.Identity);
			}
			Renderer.EndScene();
		}

		public void OnEvent(IEvent e)
		{
			EventDispatcher.DispatchEvent<KeyPressedEvent>(e, KeyPressedEvent);
		}

		private void KeyPressedEvent(KeyPressedEvent e)
		{
			//Camera movement
			if (e.KeyCode == KeyCode.A)
				cameraPosition.X -= MoveSpeed;
			if (e.KeyCode == KeyCode.D)
				cameraPosition.X += MoveSpeed;

			if (e.KeyCode == KeyCode.W)
				cameraPosition.Y += MoveSpeed;
			if (e.KeyCode == KeyCode.S)
				cameraPosition.Y -= MoveSpeed;
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