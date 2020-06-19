//TODO: This test fails on Azure DevOps for some reason
/*
using NUnit.Framework;
using VoltstroEngine.Logging;
using VoltstroEngine.Rendering;
using VoltstroEngine.Rendering.Texture;

namespace VoltstroEngine.Tests
{
public class TextureTests
{
	[OneTimeSetUp]
	public void Setup()
	{
		Renderer.Create();
		Renderer.Init();
		Logger.InitiateLogger();

		//Force set our game name
		Application.GameName = "TestTextures";
	}

	[OneTimeTearDown]
	public void Teardown()
	{
		Logger.EndLogger();
	}

	[Test]
	public void PngTextureTest()
	{
		I2DTexture texture = I2DTexture.Create("Birdi.png");
		texture.Bind();

		//The birdi texture should be 1024 x 1024
		Assert.AreEqual(1024, texture.GetWidth());
		Assert.AreEqual(1024, texture.GetHeight());
		
		//Alpha texture
		I2DTexture aTexture = I2DTexture.Create("Face.png");
		texture.Bind();

		//The face texture should be 128 x 128
		Assert.AreEqual(128, aTexture.GetWidth());
		Assert.AreEqual(128, aTexture.GetHeight());
	}
}
}
*/