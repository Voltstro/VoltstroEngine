using System;
using System.Numerics;

namespace VoltstroEngine.Extensions
{
	public static class MathExtensions
	{
		public static float ToRadian(this float angle)
		{
			return angle / 180.0f * (float) Math.PI;
		}

		public static Matrix4x4 Ortho(float left, float right, float bottom, float top, float near, float far)
		{
			if (Math.Abs(right - left) < float.Epsilon)
				throw new ArgumentException("left/right planes are coincident");
			if (Math.Abs(top - bottom) < float.Epsilon)
				throw new ArgumentException("top/bottom planes are coincident");
			if (Math.Abs(far - near) < float.Epsilon)
				throw new ArgumentException("far/near planes are coincident");

			Matrix4x4 r = new Matrix4x4
			{
				M11 = 2.0f / (right - left),
				M22 = 2.0f / (top - bottom),
				M33 = -2.0f / (far - near),
				M41 = -(right + left) / (right - left),
				M42 = -(top + bottom) / (top - bottom),
				M43 = -(far + near) / (far - near),
				M44 = 1.0f
			};

			return r;
		}
	}
}