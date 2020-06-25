using System.Numerics;
using VoltstroEngine.Core;
using VoltstroEngine.Core.Layers;
using VoltstroEngine.Events;
using VoltstroEngine.Extensions;
using VoltstroEngine.Rendering;
using VoltstroEngine.Rendering.Buffer;
using VoltstroEngine.Rendering.Shaders;

namespace Sandbox
{
	public class Sandbox2D : ILayer
	{
		private IVertexArray squareVA;
		private IShader flatColorShader;

		private OrthographicCameraController cameraController;

		private Vector4 squareColor;

		public void OnAttach()
		{
			cameraController = new OrthographicCameraController(1280.0f / 720.0f);
			squareColor = new Vector4(0.2f, 0.3f, 0.8f, 1.0f);

			squareVA = IVertexArray.Create();

			float[] squareVertices =
			{
				-0.5f, -0.5f, 0.0f,
				 0.5f, -0.5f, 0.0f,
				 0.5f,  0.5f, 0.0f,
				-0.5f,  0.5f, 0.0f
			};

			IVertexBuffer squareVertexBuffer = IVertexBuffer.Create(squareVertices, squareVertices.GetBytes());

			BufferLayout squareBufferLayout = new BufferLayout(new[]
			{
				new BufferElement("a_Position", ShaderDataType.Float3)
			});
			squareVertexBuffer.SetLayout(squareBufferLayout);
			squareVA.AddVertexBuffer(squareVertexBuffer);

			uint[] squareIndices = {0, 1, 2, 2, 3, 0};
			IIndexBuffer squareIndexBuffer =
				IIndexBuffer.Create(squareIndices, squareIndices.GetBytes() / sizeof(uint));
			squareVA.SetIndexBuffer(squareIndexBuffer);

			flatColorShader = IShader.Create("Shaders/FlatColor.glsl");
		}

		public void OnDetach()
		{
		}

		public void OnUpdate(TimeStep ts)
		{
			//Update
			cameraController.OnUpdate(ts);

			//Render
			Renderer.SetClearColor(0.1f, 0.1f, 0.1f);
			Renderer.Clear();

			Renderer.BeginScene(cameraController.GetCamera());
			{
				flatColorShader.Bind();
				flatColorShader.UploadUniformFloat4("u_Color", squareColor);

				Renderer.Submit(flatColorShader, squareVA, Matrix4x4.CreateScale(1.0f));
			}
			Renderer.EndScene();
		}

		public void OnEvent(IEvent e)
		{
			cameraController.OnEvent(e);
		}
	}
}