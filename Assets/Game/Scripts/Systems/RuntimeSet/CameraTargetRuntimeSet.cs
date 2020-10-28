using System.Linq;
using Systems.Camera;
using UnityEngine;

namespace Game.Scripts.Systems.RuntimeSet
{
    [CreateAssetMenu(menuName = "RuntimeSets/CameraTarget", order = 0)]
    public class CameraTargetRuntimeSet : RuntimeSet<CameraTarget>
    {
        public override CameraTarget GetItemAtIndex(int index)
        {
            return items.OrderByDescending(t => t.priority).ToList()[index];
        }
    }
}