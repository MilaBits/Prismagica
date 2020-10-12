using System;
using UnityEngine;

namespace Systems.Encounter
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private EnemyInfo info;

        private void Start() => Instantiate(info.AnimatedSprites, transform);
    }
}