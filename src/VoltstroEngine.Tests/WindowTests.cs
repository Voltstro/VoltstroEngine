using NUnit.Framework;
using VoltstroEngine.Logging;
using VoltstroEngine.Rendering;
using VoltstroEngine.Window;

namespace VoltstroEngine.Tests
{
	public class WindowTests
	{
		[OneTimeSetUp]
		public void Setup()
		{
			Logger.InitiateLogger();
			Renderer.Create();
			Renderer.Init();
		}

		[OneTimeTearDown]
		public void Teardown()
		{
			Logger.EndLogger();
		}

		[Test]
		public void CheckRenderingAPI()
		{
			Assert.IsTrue(Renderer.GetRenderingAPI() == RenderingAPI.OpenGL, "The rendering API wasn't OpenGL!");
		}

		//This doesn't work on Azure build agents (Or well at least the hosted ones from Microsoft)
		//[Test, Order(2)]
		public void CreateWindowTest()
		{
			//Create window
			IWindow window = IWindow.CreateWindow(new WindowProperties
			{
				Title = "Voltstro Engine",
				Width = 1280,
				Height = 720,
				VSync = true
			});

			Assert.IsNotNull(window, "Window fail to create!");

			Assert.IsTrue(window.GetWidth() == 1280, "Incorrect window size!");
			Assert.IsTrue(window.GetHeight() == 720, "Incorrect window size!");
			Assert.IsTrue(window.IsVSync(), "Window isn't VSync!");

			window.SetVSync(false);

			Assert.IsFalse(window.IsVSync(), "Window didn't change to VSync off!");

			window.Shutdown();
		}
	}
}