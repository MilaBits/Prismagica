using System.Collections;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
#endif

namespace Mechanics
{
    public class DottedLineBorder : MonoBehaviour
    {
        [SerializeField]
        private BoxCollider collisionCollider = null;

        [SerializeField]
        private Collider togglingCollider = null;

        [SerializeField]
        private SpriteRenderer togglingRenderer = null;

        private Vector3 entryPoint;
        private Vector3 playerDirection;

        [SerializeField]
        private float fadeSpeed = 1f;

        private static readonly int Opacity = Shader.PropertyToID("_Opacity");

        private void OnTriggerEnter(Collider other)
        {
            entryPoint = other.transform.position;
        }

        private void OnTriggerExit(Collider other)
        {
            playerDirection = (other.transform.position - entryPoint).normalized;

//        Debug.Log("ExitDirection: " + transform.InverseTransformDirection(playerDirection));
            if (transform.InverseTransformDirection(playerDirection).x < -.8f)
            {
                FadeArtIn();
                togglingCollider.enabled = true;
                return;
            }

            if (transform.InverseTransformDirection(playerDirection).x > .8f)
            {
                FadeArtOut();
                togglingCollider.enabled = false;
            }
        }


        private const float Distance = .2f;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Handles.matrix = transform.localToWorldMatrix;

            Handles.Label(Vector3.left * Distance, "Off");
            Handles.Label(Vector3.right * Distance, "On");


            Gizmos.matrix = transform.localToWorldMatrix;

            Gizmos.color = Color.red;
            Gizmos.DrawRay(Vector3.zero, Vector3.left * Distance);

            Gizmos.color = Color.green;
            Gizmos.DrawRay(Vector3.zero, Vector3.right * Distance);

            Gizmos.color = Color.magenta;
            Gizmos.DrawCube(Vector3.zero, collisionCollider.size);
        }
#endif

        private void FadeArtIn()
        {
//        StartCoroutine(FadeArt(new Color(1, 1, 1, 1), 1f));
            StartCoroutine(FadeArt(1, fadeSpeed));
        }

        private void FadeArtOut()
        {
//        StartCoroutine(FadeArt(new Color(1, 1, 1, 0), 1f));
            StartCoroutine(FadeArt(0, fadeSpeed));
        }

        private IEnumerator FadeArt(float opacity, float time)
        {
            float elapsedTime = 0;

            while (elapsedTime < time)
            {
                togglingRenderer.material.SetFloat(Opacity,
                    Mathf.Lerp(togglingRenderer.material.GetFloat(Opacity), opacity, (elapsedTime / time)));

//            TogglingRenderer.color = Color.Lerp(TogglingRenderer.color, end, (elapsedTime / time));

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            togglingRenderer.material.SetFloat(Opacity, opacity);
//        TogglingRenderer.color = end;

            yield return new WaitForEndOfFrame();
        }
    }
}