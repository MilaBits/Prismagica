using UnityEngine;

namespace Systems.Encounter
{
    [CreateAssetMenu(fileName = "Enemy", menuName = "Prismagica/New Enemy", order = 0)]
    public class EnemyInfo : ScriptableObject
    {
        public GameObject AnimatedSprites;
    }
}