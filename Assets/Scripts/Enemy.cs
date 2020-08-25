using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "Enemy", menuName = "Prismagica/New Enemy", order = 0)]
    public class Enemy : ScriptableObject
    {
        public Sprite Sprite;

    }
}