using System.Numerics;
using VoltstroEngine.Core;
using VoltstroEngine.Core.Layers;
using VoltstroEngine.Events;
using VoltstroEngine.Extensions;
using VoltstroEngine.Rendering;
using VoltstroEngine.Rendering.Buffer;
using VoltstroEngine.Rendering.Shaders;
using VoltstroEngine.Rendering.Texture;

namespace Sandbox
{
	public class ExampleLayer : ILayer
	{
		private static readonly Matrix4x4 Scale = Matrix4x4.CreateScale(0.1f);

		private readonly I2DTexture birdiTexture, faceTexture;

		private readonly OrthographicCameraController cameraController;
		private readonly ShaderLibrary shaderLibrary;

		private readonly IVertexArray squareVertexArray;

		public ExampleLayer()
		{
			//Create camera
			cameraController = new OrthographicCameraController(1280.0f / 720.0f);

			//Shader library
			shaderLibrary = new ShaderLibrary();

			// ----------
			//Square
			// ----------
			squareVertexArray = IVertexArray.Create();

			float[] squareVertices =
			{
				-0.5f, -0.5f, 0.0f, 0.0f, 0.0f,
				0.5f, -0.5f, 0.0f, 1.0f, 0.0f,
				0.5f, 0.5f, 0.0f, 1.0f, 1.0f,
				-0.5f, 0.5f, 0.0f, 0.0f, 1.0f
			};

			IVertexBuffer squareVertexBuffer = IVertexBuffer.Create(squareVertices, squareVertices.GetBytes());

			BufferLayout squareBufferLayout = new BufferLayout(new[]
			{
				new BufferElement("a_Position", ShaderDataType.Float3),
				new BufferElement("a_TexCoord", ShaderDataType.Float2)
			});
			squareVertexBuffer.SetLayout(squareBufferLayout);
			squareVertexArray.AddVertexBuffer(squareVertexBuffer);

			uint[] squareIndices = {0, 1, 2, 2, 3, 0};
			IIndexBuffer squareIndexBuffer =
				IIndexBuffer.Create(squareIndices, squareIndices.GetBytes() / sizeof(uint));
			squareVertexArray.SetIndexBuffer(squareIndexBuffer);

			//Square shader
			shaderLibrary.LoadAndAddShader("Shaders/Square.glsl");

			//Texture shader
			IShader textureShader = IShader.Create("Shaders/Texture.glsl");
			shaderLibrary.AddShader(textureShader);

			birdiTexture = I2DTexture.Create("Textures/Birdi.png");
			faceTexture = I2DTexture.Create("Textures/Face.png");

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
			cameraController.OnUpdate(ts);

			//Render
			Renderer.SetClearColor(0.2f, 0.2f, 0.2f);
			Renderer.Clear();

			Renderer.BeginScene(cameraController.GetCamera());
			{
				//Square
				for (int y = 0; y < 10; y++)
				for (int x = 0; x < 10; x++)
				{
					Vector3 pos = new Vector3(x * 1.1f, y * 1.1f, 0);
					Matrix4x4 transform = Matrix4x4.CreateTranslation(pos) * Scale;

					Renderer.Submit(shaderLibrary.GetShader("Square"), squareVertexArray, transform);
				}

				birdiTexture.Bind();
				Renderer.Submit(shaderLibrary.GetShader("Texture"), squareVertexArray, Matrix4x4.Identity);

				faceTexture.Bind();
				Renderer.Submit(shaderLibrary.GetShader("Texture"), squareVertexArray, Matrix4x4.Identity);
			}
			Renderer.EndScene();
		}

		public void OnEvent(IEvent e)
		{
			cameraController.OnEvent(e);
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