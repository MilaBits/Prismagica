using UnityEngine;

namespace Extensions
{
    public static class Vector3Extensions
    {
        public static Vector2 AsVector2(this Vector3 vector)
        {
            return new Vector2(vector.x, vector.y);
        }
    }
}