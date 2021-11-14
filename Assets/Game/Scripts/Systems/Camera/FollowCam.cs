using Flask;
using Game.Scripts.Systems.RuntimeSet;
using Unity.Mathematics;
using UnityEngine;

namespace Systems.Camera
{
    public class FollowCam : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private GameObject parallax = default;

        private CameraTarget topTarget = default;

        [Space] [SerializeField] private GameObjectRuntimeSet PlayerRuntimeSet = default;
        [SerializeField] private CameraTargetRuntimeSet CameraTargetRuntimeSet = default;
        private UnityEngine.Camera cam;
        private Vector3 startPos;

        [Header("Settings")] [SerializeField] private float followOmega = 1;
        [SerializeField] private float zoomOmega = 1;
        [SerializeField] private float rotationOmega = 1;

        [Space] [SerializeField] private float flipDelay = 1f;
        private float flipTime;
        [Space] [SerializeField] private CameraTarget playerTarget = default;


        private DTween _zoom = new DTween(1, 1);
        private DTweenVector3 _position = new DTweenVector3(Vector3.zero, 1);
        private DTweenQuaternion _rotation = new DTweenQuaternion(quaternion.identity, 1);

        public Transform MainTarget => CameraTargetRuntimeSet.GetItemAtIndex(0).target;

        private void Start()
        {
            CameraTargetRuntimeSet.Initialize();
            cam = GetComponent<UnityEngine.Camera>();

            SnapCam();
        }

        private void OnEnable() => CameraTargetRuntimeSet.ItemsChanged.AddListener(delegate { GetTopTarget(); });
        private void OnDisable() => CameraTargetRuntimeSet.ItemsChanged.RemoveListener(delegate { GetTopTarget(); });
        private void Update()
        {
            UpdatePosition();
            transform.GetChild(0).localScale = new Vector3(cam.orthographicSize * .5f, cam.orthographicSize * .5f, 1);
        }

        private void UpdatePosition()
        {
            // Determine target
            CameraTarget currentTarget;
            if (CameraTargetRuntimeSet.Count > 0)
                currentTarget = playerTarget.priority > topTarget.priority ? playerTarget : topTarget;
            else
                currentTarget = playerTarget;

            // Zoom stuff
            _zoom.omega = zoomOmega;
            _zoom.Step(currentTarget.zoomLevel);
            cam.orthographicSize = _zoom;

            Vector3 targetCamPos;

            if (currentTarget.Interpolate)
                targetCamPos = ((currentTarget.target.transform.position +
                                 CameraTargetRuntimeSet.GetItemAtIndex(0).target.transform.position) / 2) +
                               transform.TransformDirection(currentTarget.offset);
            else
                targetCamPos = currentTarget.target.transform.position +
                               transform.TransformDirection(currentTarget.offset);

            // Position stuff
            _position.omega = followOmega;
            _position.Step(targetCamPos);
            transform.position = _position;

            quaternion targetRotation = currentTarget.RotateWithPlayer
                ? playerTarget.target.transform.rotation
                : currentTarget.target.transform.rotation;

            _rotation.omega = rotationOmega;
            _rotation.Step(targetRotation);
            transform.rotation = _rotation;
        }

        private CameraTarget GetTopTarget()
        {
            CameraTarget target;
            if (CameraTargetRuntimeSet.Count > 0)
            {
                topTarget = CameraTargetRuntimeSet.GetItemAtIndex(0);
                target = playerTarget.priority > topTarget.priority ? playerTarget : topTarget;
            }
            else
            {
                target = playerTarget;
            }

            return target;
        }
        
        [ContextMenu("Snap Camera")]
        private void SnapCam()
        {
            CameraTarget target = GetTopTarget();

            transform.position = target.target.transform.position + transform.TransformDirection(target.offset);
            transform.rotation = Quaternion.Euler(0, 0, target.target.transform.rotation.eulerAngles.z);

            _zoom = new DTween(target.zoomLevel, 1);
            _position = new DTweenVector3(transform.position, 1);
            _rotation = new DTweenQuaternion(transform.rotation, 1);
        }
    }
}