using UnityEngine;

namespace Systems.Player
{
    public class FlipCollider : MonoBehaviour
    {
        [SerializeField]
        private LayerMask collisions = default;

        public bool CanFlip => _canFlip;
        private bool _canFlip = true;

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