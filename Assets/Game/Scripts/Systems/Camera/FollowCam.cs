using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Systems.RuntimeSet;
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
        [SerializeField] private CameraTarget topTarget = default;
        [SerializeField] private CameraTarget playerTarget = default;
        [SerializeField] private GameObjectRuntimeSet PlayerRuntimeSet = default;
        [SerializeField] private CameraTargetRuntimeSet CameraTargetRuntimeSet = default;
        private UnityEngine.Camera cam;
        private Vector3 startPos;

        public Transform MainTarget => CameraTargetRuntimeSet.GetItemAtIndex(0).target;

        private void Start()
        {
            CameraTargetRuntimeSet.Initialize();
            cam = GetComponent<UnityEngine.Camera>();
            StartCoroutine(WaitForPlayer());
        }

        private void OnEnable() => CameraTargetRuntimeSet.ItemsChanged.AddListener(delegate { GetTopTarget(); });
        private void OnDisable() => CameraTargetRuntimeSet.ItemsChanged.RemoveListener(delegate { GetTopTarget(); });

        private IEnumerator WaitForPlayer()
        {
            while (!playerTarget.target)
            {
                yield return new WaitForSeconds(.05f);
                playerTarget.target = PlayerRuntimeSet.GetItemAtIndex(0).transform;
            }

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
            if (CameraTargetRuntimeSet.Count > 0)
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
                targetCamPos = ((currentTarget.target.transform.position +
                                 CameraTargetRuntimeSet.GetItemAtIndex(0).target.transform.position) / 2) +
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
    }
}