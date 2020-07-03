using System.Numerics;
using VoltstroEngine.DebugTools;
using VoltstroEngine.Extensions;
using VoltstroEngine.Rendering.Buffer;
using VoltstroEngine.Rendering.Camera;
using VoltstroEngine.Rendering.Shaders;
using VoltstroEngine.Rendering.Texture;
using VoltstroEngine.Types;

namespace VoltstroEngine.Rendering.Renderer
{
	public static class Renderer2D
	{
		private struct Renderer2DStorage
		{
			public IVertexArray QuadVertexArray;
			public IShader TextureShader;
			public I2DTexture WhiteTexture;
		}

		private static Renderer2DStorage rendererData;

		public static void Init()
		{
			ProfilerTimer.Profile(() =>
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

				rendererData.WhiteTexture = I2DTexture.Create(1, 1);
				uint whiteTextureData = 0xffffffff;
				rendererData.WhiteTexture.SetData(whiteTextureData, sizeof(uint));

				rendererData.TextureShader = IShader.Create("Shaders/Texture.glsl");
				rendererData.TextureShader.Bind();
				rendererData.TextureShader.SetInt("u_Texture", 0);
			});
		}

		public static void Shutdown()
		{
			//TODO: Implement disposing for VertexBuffer, IndexBuffer, Shader
		}

		public static void BeginScene(OrthographicCamera camera)
		{
			rendererData.TextureShader.Bind();
			rendererData.TextureShader.SetMat4("u_ViewProjection", camera.ViewProjectionMatrix);
		}

		public static void EndScene()
		{
		}

		//Primitives

		#region DrawQuads

		/// <summary>
		/// Draws a quad using a color
		/// </summary>
		/// <param name="transform"></param>
		/// <param name="color"></param>
		public static void DrawQuad(Transform transform, Vector4 color)
		{
			rendererData.TextureShader.SetVec4("u_Color", color);
			rendererData.TextureShader.SetFloat("u_TilingFactor", 1.0f);
			rendererData.WhiteTexture.Bind();

			Matrix4x4 shaderTransform = Matrix4x4.CreateTranslation(transform.Position) * Matrix4x4.CreateScale(transform.Scale.X, transform.Scale.Y, 1.0f);
			rendererData.TextureShader.SetMat4("u_Transform", shaderTransform);

			rendererData.QuadVertexArray.Bind();
			RenderingAPI.DrawIndexed(rendererData.QuadVertexArray);
		}

		/// <summary>
		/// Draws a quad using a texture
		/// </summary>
		/// <param name="transform"></param>
		/// <param name="texture"></param>
		/// <param name="tintColor"></param>
		/// <param name="tillingFactor"></param>
		public static void DrawQuad(Transform transform, I2DTexture texture, Vector4 tintColor, float tillingFactor = 1.0f)
		{
			rendererData.TextureShader.SetVec4("u_Color", tintColor);
			rendererData.TextureShader.SetFloat("u_TilingFactor", tillingFactor);
			texture.Bind();

			Matrix4x4 shaderTransform = Matrix4x4.CreateTranslation(transform.Position) * Matrix4x4.CreateScale(transform.Scale.X, transform.Scale.Y, 1.0f);
			rendererData.TextureShader.SetMat4("u_Transform", shaderTransform);

			rendererData.QuadVertexArray.Bind();
			RenderingAPI.DrawIndexed(rendererData.QuadVertexArray);
		}
		
		#endregion

		#region DrawQuads Rotated

		/// <summary>
		/// Draws a quad rotated using color
		/// </summary>
		/// <param name="transform"></param>
		/// <param name="color"></param>
		public static void DrawRotatedQuad(Transform transform, Vector4 color)
		{
			//If the rotation is only 0, we will call the regular draw quad, to save performance on the rotation calculations
			if (transform.Rotation == 0.0f)
			{
				DrawQuad(transform, color);
				return;
			}

			rendererData.TextureShader.SetVec4("u_Color", color);
			rendererData.TextureShader.SetFloat("u_TilingFactor", 1.0f);
			rendererData.WhiteTexture.Bind();

			Matrix4x4 shaderTransform = Matrix4x4.CreateTranslation(transform.Position) 
								  * Matrix4x4.CreateRotationZ(transform.Rotation.ToRadian())
			                      * Matrix4x4.CreateScale(transform.Scale.X, transform.Scale.Y, 1.0f);
			rendererData.TextureShader.SetMat4("u_Transform", shaderTransform);

			rendererData.QuadVertexArray.Bind();
			RenderingAPI.DrawIndexed(rendererData.QuadVertexArray);
		}

		/// <summary>
		/// Draws a quad rotated
		/// </summary>
		/// <param name="transform"></param>
		/// <param name="tintColor"></param>
		/// <param name="texture"></param>
		/// <param name="tillingFactor"></param>
		public static void DrawRotatedQuad(Transform transform, Vector4 tintColor, I2DTexture texture, float tillingFactor = 1.0f)
		{
			//If the rotation is only 0, we will call the regular draw quad, to save performance on the rotation calculations
			if (transform.Rotation == 0.0f)
			{
				DrawQuad(transform, texture, tintColor, tillingFactor);
				return;
			}

			rendererData.TextureShader.SetVec4("u_Color", tintColor);
			rendererData.TextureShader.SetFloat("u_TilingFactor", tillingFactor);
			texture.Bind();

			Matrix4x4 shaderTransform = Matrix4x4.CreateTranslation(transform.Position) 
			                      * Matrix4x4.CreateRotationZ(transform.Rotation.ToRadian()) 
			                      * Matrix4x4.CreateScale(transform.Scale.X, transform.Scale.Y, 1.0f);

			rendererData.TextureShader.SetMat4("u_Transform", shaderTransform);

			rendererData.QuadVertexArray.Bind();
			RenderingAPI.DrawIndexed(rendererData.QuadVertexArray);
		}

		#endregion
	}
}