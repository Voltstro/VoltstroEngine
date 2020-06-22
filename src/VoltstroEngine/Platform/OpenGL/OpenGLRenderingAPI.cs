using System;
using System.Diagnostics;
using OpenGL;
using VoltstroEngine.Rendering;

namespace VoltstroEngine.Platform.OpenGL
{
	/// <summary>
	/// The OpenGL rendering API
	/// </summary>
	public class OpenGLRenderingAPI : IRenderingAPI
	{
		public void Init()
		{
			try
			{
				Gl.Enable(EnableCap.Blend);
				Gl.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
			}
			catch (Exception ex)
			{
				Debug.Assert(false, $"An error occured while enabling OpenGL!\n{ex}");
#if !DEBUG
				Logging.Logger.Log(ex.Message, Logging.LogVerbosity.Error);
#endif
			}
		}

		public void SetClearColor(float red, float green, float blue, float alpha)
		{
			Gl.ClearColor(red, green, blue, alpha);
		}

		public void Clear()
		{
			Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
		}

		public void DrawIndexed(IVertexArray vertexArray)
		{
			Gl.DrawElements(PrimitiveType.Triangles, (int)vertexArray.GetIndexBuffer().GetCount(), DrawElementsType.UnsignedInt, null);
		}

		public RenderingAPI GetAPI()
		{
			return RenderingAPI.OpenGL;
		}

		public void SetViewport(uint x, uint y, uint width, uint height)
		{
			Gl.Viewport((int)x, (int)y, (int)width, (int)height);
		}
	}
}