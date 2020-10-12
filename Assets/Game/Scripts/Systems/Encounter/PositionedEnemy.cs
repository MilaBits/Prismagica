using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Systems.Encounter
{
    [Serializable]
    public struct PositionedEnemy
    {
        public Vector2Int Position;
        [FormerlySerializedAs("Enemy")] public Enemy enemy;
    }
}