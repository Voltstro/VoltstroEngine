using NUnit.Framework;
using VoltstroEngine.Rendering.Buffer;

namespace VoltstroEngine.Tests
{
	public class BufferTests
	{
		[Test]
		public void Float3BufferTest()
		{
			BufferElement element = new BufferElement("a_Position", ShaderDataType.Float3);

			Assert.IsTrue(element.Type == ShaderDataType.Float3, "element.Type == ShaderDataType.Float3");
			Assert.AreEqual(4 * 3, element.Size);
			Assert.AreEqual(3, element.GetComponentCount());
		}

		[Test]
		public void Float4BufferTest()
		{
			BufferElement element = new BufferElement("a_Color", ShaderDataType.Float4);

			Assert.IsTrue(element.Type == ShaderDataType.Float4, "element.Type == ShaderDataType.Float4");
			Assert.AreEqual(4 * 4, element.Size);
			Assert.AreEqual(4, element.GetComponentCount());
		}

		[Test]
		public void Mat4BufferTest()
		{
			BufferElement element = new BufferElement("u_ViewProjection", ShaderDataType.Mat4);

			Assert.IsTrue(element.Type == ShaderDataType.Mat4, "element.Type == ShaderDataType.Mat4");
			Assert.AreEqual(4 * 4 * 4, element.Size);
			Assert.AreEqual(4 * 4, element.GetComponentCount());
		}

		[Test]
		public void BufferLayoutTest()
		{
			BufferLayout layout = new BufferLayout(new []
			{
				new BufferElement("a_Position", ShaderDataType.Float3),
				new BufferElement("a_Color", ShaderDataType.Float4)
			});

			Assert.AreEqual(2, layout.Elements.Length);
			Assert.AreEqual(28, layout.Stride);

			Assert.IsTrue(layout.Elements[0].Type == ShaderDataType.Float3);
			Assert.AreEqual(12, layout.Elements[0].Size);
			Assert.AreEqual(0, layout.Elements[0].Offset);

			Assert.IsTrue(layout.Elements[1].Type == ShaderDataType.Float4);
			Assert.AreEqual(16, layout.Elements[1].Size);
			Assert.AreEqual(12, layout.Elements[1].Offset);
		}
	}
}