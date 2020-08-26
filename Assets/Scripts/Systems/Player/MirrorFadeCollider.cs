using Extensions;
using UnityEngine;

namespace Systems.Player
{
    public class MirrorFadeCollider : MonoBehaviour
    {
        [SerializeField]
        private LayerMask collisions = default;

        public float colliderDistance;

        private void OnTriggerEnter2D(Collider2D other) => colliderDistance =
            Vector3.Distance(transform.position, other.ClosestPoint(transform.position.AsVector2()));

        private void OnTriggerStay2D(Collider2D other) => colliderDistance =
            Vector3.Distance(transform.position, other.ClosestPoint(transform.position.AsVector2()));

        private void OnTriggerExit2D(Collider2D other) => colliderDistance =
            Vector3.Distance(transform.position, other.ClosestPoint(transform.position.AsVector2()));
    }
}