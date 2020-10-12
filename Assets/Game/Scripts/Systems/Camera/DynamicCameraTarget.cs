using UnityEngine;

namespace Systems.Camera
{
    
    //TODO: This can certainly be optimized to only work when it's needed.
    public class DynamicCameraTarget : MonoBehaviour
    {
        public Transform[] targets;
        private void Update() => transform.position = Average();

        private Vector3 Average()
        {
            Vector3 total = Vector3.zero;
            for (int i = 0; i < targets.Length; i++) total += targets[i].position;
            return total /= targets.Length;
        }
    }
}