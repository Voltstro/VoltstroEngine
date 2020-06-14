﻿using System;
using System.Diagnostics;

namespace VoltstroEngine.Rendering.Buffer
{
	public class BufferElement
	{
		public static uint ShaderTypeSize(ShaderDataType type)
		{
			switch (type)
			{
				case ShaderDataType.Float:
					return 4;
				case ShaderDataType.Float2:
					return 4 * 2;
				case ShaderDataType.Float3:
					return 4 * 3;
				case ShaderDataType.Float4:
					return 4 * 4;
				case ShaderDataType.Mat3:
					return 4 * 3 * 3;
				case ShaderDataType.Mat4:
					return 4 * 4 * 4;
				case ShaderDataType.Int:
					return 4;
				case ShaderDataType.Int2:
					return 4 * 2;
				case ShaderDataType.Int3:
					return 4 * 3;
				case ShaderDataType.Int4:
					return 4 * 4;
				default:
					throw new ArgumentOutOfRangeException(nameof(type), type, null);
			}
		}

		public BufferElement(string name, ShaderDataType type, bool normalized = false)
		{
			Name = name;
			Type = type;
			Size = ShaderTypeSize(type);
			Offset = 0;
			Normalized = normalized;
		}

		public string Name { get; private set; }
		public ShaderDataType Type { get; private set; }
		public uint Size;
		public uint Offset;
		public bool Normalized;

		public uint GetComponentCount()
		{
			switch (Type)
			{
				case ShaderDataType.None:
					return 0;
				case ShaderDataType.Float:
					return 1;
				case ShaderDataType.Float2:
					return 2;
				case ShaderDataType.Float3:
					return 3;
				case ShaderDataType.Float4:
					return 4;
				case ShaderDataType.Mat3:
					return 3 * 3;
				case ShaderDataType.Mat4:
					return 4 * 4;
				case ShaderDataType.Int:
					return 1;
				case ShaderDataType.Int2:
					return 2;
				case ShaderDataType.Int3:
					return 3;
				case ShaderDataType.Int4:
					return 4;
				default:
					throw new ArgumentOutOfRangeException();
			}

			Debug.Assert(false, "Unknown ShaderType!");
			return 0;
		}
	}
}