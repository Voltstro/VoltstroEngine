using System.Diagnostics;

namespace VoltstroEngine.Rendering.Buffer
{
	public class BufferElement
	{
		public BufferElement(ShaderDataType type, string name)
		{
			Type = type;
			Name = name;
		}

		public string Name;
		public ShaderDataType Type;
		public uint Size;
		public uint Offset;
		public bool Normalized;

		public uint GetComponentCount()
		{
			switch (Type)
			{
				case ShaderDataType.Float:   return 1;
				case ShaderDataType.Float2:  return 2;
				case ShaderDataType.Float3:  return 3;
				case ShaderDataType.Float4:  return 4;
				case ShaderDataType.Mat3:    return 3 * 3;
				case ShaderDataType.Mat4:    return 4 * 4;
				case ShaderDataType.Int:     return 1;
				case ShaderDataType.Int2:    return 2;
				case ShaderDataType.Int3:    return 3;
				case ShaderDataType.Int4:    return 4;
				case ShaderDataType.Bool:    return 1;
			}

			Debug.Assert(false, "ShaderDataType doesn't exist!");
			return 0;
		}
	}
}
