using UnityEngine;

namespace Game.Scripts.Systems.Collectables
{
    [CreateAssetMenu(fileName = "New Lore Collectable", menuName = "Collectables/Lore")]
    public class CollectableLore : Collectable
    {
        private string description;
    }
}