﻿using System;
using UnityEngine;

namespace Systems.Enemy
{
    [Serializable]
    public struct PositionedEnemy
    {
        public Vector2 Position;
        public Enemy Enemy;
    }
}