using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Systems.Camera
{
    public class FollowCam : MonoBehaviour
    {
        [SerializeField] private Vector3 cameraOffset = default;

        [SerializeField] private GameObject parallax = default;

        [SerializeField] private float followSpeed = default;

        [SerializeField] private float rotationSpeed = default;

        [SerializeField] private float zoomDuration = default;

        [SerializeField] private AnimationCurve zoomCurve = default;

        [SerializeField] private List<CameraTarget> Targets = default;

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
            CameraTarget target = Targets.OrderByDescending(t => t.priority).First();

            // Zoom stuff
            float curvePercent = zoomCurve.Evaluate(Mathf.Clamp01(Time.deltaTime / zoomDuration));
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, target.zoomLevel, curvePercent);
            parallax.transform.localScale = Vector3.Lerp(
                parallax.transform.localScale,
                new Vector3(target.zoomLevel, target.zoomLevel, 1),
                curvePercent);

            Vector3 targetCamPos;
            if (target.Interpolate)
            {
                targetCamPos = ((target.target.transform.position + Targets[0].target.transform.position) / 2) +
                               transform.TransformDirection(cameraOffset);
            }
            else
            {
                targetCamPos = target.target.transform.position + transform.TransformDirection(cameraOffset);
            }

            // Position stuff
            transform.position = Vector3.Lerp(
                transform.position,
                targetCamPos, followSpeed * Time.deltaTime);

            // Rotation stuff
            if (target.RotateWithPlayer) target = Targets.OrderByDescending(t => t.priority).Last();

            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                Quaternion.Euler(0, 0, target.target.transform.rotation.eulerAngles.z),
                rotationSpeed * Time.deltaTime);
        }

        [ContextMenu("Snap Camera")]
        private void SnapCam()
        {
            CameraTarget target = Targets.OrderByDescending(t => t.priority).First();

            transform.position = target.target.transform.position + transform.TransformDirection(cameraOffset);
            transform.rotation = Quaternion.Euler(0, 0, target.target.transform.rotation.eulerAngles.z);
        }

        public void AddTarget(CameraTarget target)
        {
            Targets.Add(target);
        }

        public void RemoveTarget(CameraTarget target)
        {
            Targets.Remove(target);
        }
    }
}