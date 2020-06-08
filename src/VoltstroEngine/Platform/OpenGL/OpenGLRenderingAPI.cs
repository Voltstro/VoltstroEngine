using System;
using System.Diagnostics;
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
				Debug.Assert(false, $"An error occured while enabling OpenGL!\n{ex}");
#if !DEBUG
				Logger.Log(ex.Message, LogVerbosity.Error);
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

		public RenderingAPI GetAPI()
		{
			return RenderingAPI.OpenGL;
		}
	}
}