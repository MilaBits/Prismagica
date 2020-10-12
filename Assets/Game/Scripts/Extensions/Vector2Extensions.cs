using UnityEngine;

namespace Extensions
{
	public static class Vector2Extensions
	{
		public static float Angle(this Vector2 vector2)
		{
			float value = (Mathf.Atan2(vector2.y, vector2.x) / Mathf.PI) * 180f;
			if (value < 0) value += 360f;

			return value;
		}
	}
}