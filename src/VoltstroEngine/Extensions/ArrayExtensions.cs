using System;

namespace VoltstroEngine.Extensions
{
	public static class ArrayExtensions
	{
		public static uint GetBytes(this Array array)
		{
			return (uint)Buffer.ByteLength(array);
		}
	}
}