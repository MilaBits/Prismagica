using UnityEngine;

namespace Mechanics
{
    public class FlipCollider : MonoBehaviour
    {
        private CapsuleCollider flipCollider;

        [SerializeField]
        private LayerMask collisions;

        public bool CanFlip => _canFlip;
        private bool _canFlip = true;

        private void Start()
        {
            flipCollider = GetComponent<CapsuleCollider>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            _canFlip = false;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            _canFlip = false;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            _canFlip = true;
        }
    }
}