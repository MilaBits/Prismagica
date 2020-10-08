using Systems.Player;
using UnityEngine;

namespace Systems.Camera
{
    public class CameraFocusZone : MonoBehaviour
    {
        private FollowCam followCam;

        [SerializeField] private CameraTarget cameraTarget = default;

        private void Start()
        {
            followCam = UnityEngine.Camera.main.GetComponent<FollowCam>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Player") followCam.AddTarget(cameraTarget);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.tag == "Player") followCam.RemoveTarget(cameraTarget);
        }
    }
}