using System;

namespace VoltstroEngine.Extensions
{
	public static class MathExtensions
	{
		public static float ToRadian(this float angle)
		{
			return angle / 180.0f * (float) Math.PI;
		}
	}
}