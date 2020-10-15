using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Systems.Camera
{
    public class FollowCam : MonoBehaviour
    {
        // [SerializeField] private Vector3 cameraOffset = default;

        [SerializeField] private GameObject parallax = default;

        [SerializeField] private float followSpeed = default;

        [SerializeField] private float rotationSpeed = default;

        [SerializeField] private float zoomDuration = default;

        [SerializeField] private AnimationCurve zoomCurve = default;

        [SerializeField] private List<CameraTarget> Targets = default;
        [SerializeField] private CameraTarget topTarget = default;
        [SerializeField] private CameraTarget playerTarget = default;

        private UnityEngine.Camera cam;

        private Vector3 startPos;

        public Transform MainTarget => Targets[0].target;

        private void Start()
        {
            cam = GetComponent<UnityEngine.Camera>();
            SnapCam();
        }

        private void Update()
        {
            UpdatePosition();
            transform.GetChild(0).localScale = new Vector3(cam.orthographicSize * .5f, cam.orthographicSize * .5f, 1);
        }

        private void UpdatePosition()
        {
            CameraTarget currentTarget;
            if (Targets.Count > 0)
                currentTarget = playerTarget.priority > topTarget.priority ? playerTarget : topTarget;
            else
                currentTarget = playerTarget;

            // Zoom stuff
            float curvePercent = zoomCurve.Evaluate(Mathf.Clamp01(Time.deltaTime / zoomDuration));
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, currentTarget.zoomLevel, curvePercent);
            parallax.transform.localScale = Vector3.Lerp(
                parallax.transform.localScale,
                new Vector3(currentTarget.zoomLevel, currentTarget.zoomLevel, 1),
                curvePercent);

            Vector3 targetCamPos;
            if (currentTarget.Interpolate)
                targetCamPos = ((currentTarget.target.transform.position + Targets[0].target.transform.position) / 2) +
                               transform.TransformDirection(currentTarget.offset);
            else
                targetCamPos = currentTarget.target.transform.position +
                               transform.TransformDirection(currentTarget.offset);

            // Position stuff
            transform.position = Vector3.Lerp(
                transform.position,
                targetCamPos, followSpeed * Time.deltaTime);

            // Rotation stuff
            if (currentTarget.RotateWithPlayer)
                transform.rotation = Quaternion.Lerp(transform.rotation,
                    Quaternion.Euler(0, 0, playerTarget.target.transform.rotation.eulerAngles.z),
                    rotationSpeed * Time.deltaTime);
            else
                transform.rotation = Quaternion.Lerp(transform.rotation,
                    Quaternion.Euler(0, 0, currentTarget.target.transform.rotation.eulerAngles.z),
                    rotationSpeed * Time.deltaTime);
        }

        [ContextMenu("Snap Camera")]
        private void SnapCam()
        {
            CameraTarget target = GetTopTarget();

            transform.position = target.target.transform.position + transform.TransformDirection(target.offset);
            transform.rotation = Quaternion.Euler(0, 0, target.target.transform.rotation.eulerAngles.z);
        }

        private CameraTarget GetTopTarget()
        {
            CameraTarget target;
            if (Targets.Count > 0)
            {
                topTarget = Targets.OrderByDescending(t => t.priority).First();
                target = playerTarget.priority > topTarget.priority ? playerTarget : topTarget;
            }
            else
            {
                target = playerTarget;
            }

            return target;
        }

        public void AddTarget(CameraTarget target)
        {
            Targets.Add(target);
            topTarget = GetTopTarget();
        }

        public void RemoveTarget(CameraTarget target)
        {
            Targets.Remove(target);
            topTarget = GetTopTarget();
        }
    }
}