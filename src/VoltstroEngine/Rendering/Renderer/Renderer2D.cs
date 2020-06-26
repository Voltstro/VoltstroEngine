using System.Numerics;
using VoltstroEngine.Extensions;
using VoltstroEngine.Rendering.Buffer;
using VoltstroEngine.Rendering.Camera;
using VoltstroEngine.Rendering.Shaders;
using VoltstroEngine.Rendering.Texture;

namespace VoltstroEngine.Rendering.Renderer
{
	public static class Renderer2D
	{
		private struct Renderer2DStorage
		{
			public IVertexArray QuadVertexArray;
			public IShader FlatColorShader;
			public IShader TextureShader;
		}

		private static Renderer2DStorage rendererData;

		public static void Init()
		{
			rendererData = new Renderer2DStorage
			{
				QuadVertexArray = IVertexArray.Create()
			};

			float[] squareVertices =
			{
				-0.5f, -0.5f, 0.0f, 0.0f, 0.0f,
				 0.5f, -0.5f, 0.0f, 1.0f, 0.0f,
				 0.5f,  0.5f, 0.0f, 1.0f, 1.0f,
				-0.5f,  0.5f, 0.0f, 0.0f, 1.0f
			};

			IVertexBuffer squareVertexBuffer = IVertexBuffer.Create(squareVertices, squareVertices.GetBytes());

			BufferLayout squareBufferLayout = new BufferLayout(new[]
			{
				new BufferElement("a_Position", ShaderDataType.Float3),
				new BufferElement("a_TexCoord", ShaderDataType.Float2)
			});
			squareVertexBuffer.SetLayout(squareBufferLayout);
			rendererData.QuadVertexArray.AddVertexBuffer(squareVertexBuffer);

			uint[] squareIndices = {0, 1, 2, 2, 3, 0};
			IIndexBuffer squareIndexBuffer =
				IIndexBuffer.Create(squareIndices, squareIndices.GetBytes() / sizeof(uint));
			rendererData.QuadVertexArray.SetIndexBuffer(squareIndexBuffer);

			rendererData.FlatColorShader = IShader.Create("Shaders/FlatColor.glsl");

			rendererData.TextureShader = IShader.Create("Shaders/Texture.glsl");
			rendererData.TextureShader.Bind();
			rendererData.TextureShader.SetInt("u_Texture", 0);
		}

		public static void Shutdown()
		{
			//TODO: Implement disposing for VertexBuffer, IndexBuffer, Shader
		}

		public static void BeginScene(OrthographicCamera camera)
		{
			rendererData.FlatColorShader.Bind();
			rendererData.FlatColorShader.SetMat4("u_ViewProjection", camera.ViewProjectionMatrix);

			rendererData.TextureShader.Bind();
			rendererData.TextureShader.SetMat4("u_ViewProjection", camera.ViewProjectionMatrix);
		}

		public static void EndScene()
		{
		}

		//Primitives

		/// <summary>
		/// Draws a quad
		/// </summary>
		/// <param name="position"></param>
		/// <param name="size"></param>
		/// <param name="color"></param>
		public static void DrawQuad(Vector2 position, Vector2 size, Vector4 color)
		{
			DrawQuad(new Vector3(position.X, position.Y, 0.0f), size, color);
		}

		/// <summary>
		/// Draws a quad
		/// </summary>
		/// <param name="position"></param>
		/// <param name="size"></param>
		/// <param name="color"></param>
		public static void DrawQuad(Vector3 position, Vector2 size, Vector4 color)
		{
			rendererData.FlatColorShader.Bind();
			rendererData.FlatColorShader.SetVec4("u_Color", color);

			Matrix4x4 transform = Matrix4x4.CreateTranslation(position) * Matrix4x4.CreateScale(size.X, size.Y, 1.0f);
			rendererData.FlatColorShader.SetMat4("u_Transform", transform);

			rendererData.QuadVertexArray.Bind();
			Renderer.renderingAPI.DrawIndexed(rendererData.QuadVertexArray);
		}

		/// <summary>
		/// Draws a quad
		/// </summary>
		/// <param name="position"></param>
		/// <param name="size"></param>
		/// <param name="texture"></param>
		public static void DrawQuad(Vector2 position, Vector2 size, I2DTexture texture)
		{
			DrawQuad(new Vector3(position.X, position.Y, 0.0f), size, texture);
		}

		/// <summary>
		/// Draws a quad
		/// </summary>
		/// <param name="position"></param>
		/// <param name="size"></param>
		/// <param name="texture"></param>
		public static void DrawQuad(Vector3 position, Vector2 size, I2DTexture texture)
		{
			rendererData.TextureShader.Bind();

			Matrix4x4 transform = Matrix4x4.CreateTranslation(position) * Matrix4x4.CreateScale(size.X, size.Y, 1.0f);
			rendererData.TextureShader.SetMat4("u_Transform", transform);

			texture.Bind();

			rendererData.QuadVertexArray.Bind();
			Renderer.renderingAPI.DrawIndexed(rendererData.QuadVertexArray);
		}
	}
}