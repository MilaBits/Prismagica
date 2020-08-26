using UnityEngine;

namespace Systems.Enemy
{
    [CreateAssetMenu(fileName = "Enemy", menuName = "Prismagica/New Enemy", order = 0)]
    public class Enemy : ScriptableObject
    {
        public Sprite Sprite;

    }
}