using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Systems.Collectables
{
    [CreateAssetMenu(fileName = "Collectable Collection", menuName = "Collectables/Collection", order = 0)]
    public class CollectableCollection : ScriptableObject
    {
        [SerializeField] private List<Collectable> collected = default;

        public void AddCollectable(Collectable collectable)
        {
            if (collected.Contains(collectable)) return;

            collected.Add(collectable);
        }
    }
}