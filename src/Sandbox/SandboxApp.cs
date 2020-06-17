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
using VoltstroEngine.Rendering.Texture;

namespace Sandbox
{
	public class ExampleLayer : ILayer
	{
		private readonly IShader triangleShader;
		private readonly IShader squareShader;
		private readonly IShader textureShader;

		private readonly I2DTexture texture;

		private readonly IVertexArray triangleVertexArray;
		private readonly IVertexArray squareVertexArray;

		private readonly OrthographicCamera camera;

		private Vector3 cameraPosition = Vector3.Zero;

		private const float MoveSpeed = 2f;

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
				-0.5f, -0.5f, 0.0f, 0.0f, 0.0f,
				 0.5f, -0.5f, 0.0f, 1.0f, 0.0f,
				 0.5f,  0.5f, 0.0f, 1.0f, 1.0f,
				-0.5f,  0.5f, 0.0f, 0.0f, 1.0f
			};

			IVertexBuffer squareVertexBuffer = IVertexBuffer.Create(squareVertices, triangleVertices.GetBytes());

			BufferLayout squareBufferLayout = new BufferLayout(new List<BufferElement>
			{
				new BufferElement("a_Position", ShaderDataType.Float3),
				new BufferElement("a_TexCoord", ShaderDataType.Float2)
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

			//Texture shader
			string textureVertexSrc = AssetManager.ReadAllText("Shaders/Texture.vert").Replace("\r\n", "\n");
			string textureFragmentSrc = AssetManager.ReadAllText("Shaders/Texture.frag").Replace("\r\n", "\n");
			textureShader = IShader.Create("Texture", textureVertexSrc, textureFragmentSrc);
			
			texture = I2DTexture.Create("Textures/Birdi.png");

			textureShader.Bind();
			textureShader.UploadUniformInt("u_Texture", 0);
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

			//Camera movement
			if (Input.IsKeyPressed(KeyCode.A))
				cameraPosition.X -= MoveSpeed * ts.Seconds;
			if (Input.IsKeyPressed(KeyCode.D))
				cameraPosition.X += MoveSpeed * ts.Seconds;

			if (Input.IsKeyPressed(KeyCode.W))
				cameraPosition.Y += MoveSpeed * ts.Seconds;
			if (Input.IsKeyPressed(KeyCode.S))
				cameraPosition.Y -= MoveSpeed * ts.Seconds;

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

				texture.Bind(0);
				Renderer.Submit(textureShader, squareVertexArray, Matrix4x4.Identity);

				//Triangle
				//Renderer.Submit(triangleShader, triangleVertexArray, Matrix4x4.Identity);
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