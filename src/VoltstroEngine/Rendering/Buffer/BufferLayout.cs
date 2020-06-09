using System.Collections.Generic;

namespace VoltstroEngine.Rendering.Buffer
{
	public class BufferLayout
	{
		public BufferLayout(List<BufferElement> elements)
		{
			Elements = elements;
			CalculateStrideAndOffset();
		}

		public uint Stride { get; private set; }
		public List<BufferElement> Elements { get; }

		private void CalculateStrideAndOffset()
		{
			uint offset = 0;
			Stride = 0;
			foreach (BufferElement element in Elements)
			{
				element.Offset = offset;
				offset += element.Size;
				Stride += element.Size;
			}
		}
	}
}