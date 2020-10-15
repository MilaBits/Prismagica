using System;
using UnityEngine;

namespace Systems.Encounter
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private EnemyInfo info = default;

        private void Start() => Instantiate(info.AnimatedSprites, transform);
    }
}