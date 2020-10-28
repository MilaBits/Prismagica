using UnityEngine;

namespace Game.Scripts.Systems.RuntimeSet
{
    [CreateAssetMenu(menuName = "RuntimeSets/GameObject", order = 0)]
    public class GameObjectRuntimeSet : RuntimeSet<GameObject>
    {
        public void SetActive(bool active)
        {
            for (var i = 0; i < items.Count; i++)
            {
                GameObject item = items[i];
                item.SetActive(active);
            }
        }
    }
}