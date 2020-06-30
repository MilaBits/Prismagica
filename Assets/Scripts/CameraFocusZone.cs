using UnityEngine;

namespace UI
{
    public class CameraFocusZone : MonoBehaviour
    {
        private FollowCam followCam;

        [SerializeField]
        private CameraTarget cameraTarget = default;

        private void Start()
        {
            followCam = Camera.main.GetComponent<FollowCam>();
        }

        private void OnTriggerEnter(Collider other)
        {
            followCam.AddTarget(cameraTarget);
        }

        private void OnTriggerExit(Collider other)
        {
            followCam.RemoveTarget(cameraTarget);
        }

//    private IEnumerator SmoothZoom(Transform target, float targetZoom, float time)
//    {
//        running = true;
//        float elapsedTime = 0;
//
//        while (elapsedTime < time)
//        {
//            elapsedTime += Time.deltaTime;
//            float percent = Mathf.Clamp01(elapsedTime / time);
//            float curvePercent = zoomCurve.Evaluate(percent);
//
//            GetComponent<Camera>().transform.position = Vector3.LerpUnclamped(GetComponent<Camera>().transform.position, target.position, curvePercent);
//            GetComponent<Camera>().orthographicSize = Mathf.Lerp(GetComponent<Camera>().orthographicSize, targetZoom, curvePercent);
//
//            yield return null;
//        }
//
//        transform.position = target.position;
//        GetComponent<Camera>().orthographicSize = targetZoom;
//        running = false;
//
//        yield return new WaitForEndOfFrame();
//    }
    }
}