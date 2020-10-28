using Game.Scripts.Systems.RuntimeSet;
using UnityEngine;

namespace Systems.Camera
{
    public class CameraFocusZone : MonoBehaviour
    {
        [SerializeField] private CameraTargetRuntimeSet cameraTargetRuntimeSet = default;
        [SerializeField] private CameraTarget cameraTarget = default;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Player") cameraTargetRuntimeSet.AddToSet(cameraTarget);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.tag == "Player") cameraTargetRuntimeSet.RemoveFromSet(cameraTarget);
        }
    }
}