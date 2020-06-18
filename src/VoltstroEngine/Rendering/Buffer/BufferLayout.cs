namespace VoltstroEngine.Rendering.Buffer
{
	public class BufferLayout
	{
		public BufferLayout(BufferElement[] elements)
		{
			Elements = elements;
			CalculateOffsetsAndStride();
		}

		public BufferElement[] Elements { get; }
		public uint Stride { get; private set; }

		private void CalculateOffsetsAndStride()
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