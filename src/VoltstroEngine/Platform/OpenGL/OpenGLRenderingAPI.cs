using System;
using OpenGL;
using VoltstroEngine.Logging;
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
				Logger.Log(ex.ToString(), LogVerbosity.Error);
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
	}
}