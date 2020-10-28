using System;
using UnityEngine;

namespace Game.Scripts.Systems.Collectables
{
    public class CollectableCollector : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Collectable"))
            {
                CollectableObject collectable = other.gameObject.GetComponent<CollectableObject>();
            }
        }
    }
}