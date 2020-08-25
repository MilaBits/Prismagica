using System;
using UnityEngine;

namespace DefaultNamespace
{
    [Serializable]
    public struct PositionedEnemy
    {
        public Vector2 Position;
        public Enemy Enemy;
    }
}