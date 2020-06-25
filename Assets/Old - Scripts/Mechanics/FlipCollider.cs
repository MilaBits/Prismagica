using UnityEngine;

namespace Mechanics
{
    public class FlipCollider : MonoBehaviour
    {
        private CapsuleCollider flipCollider;

        public bool canFlip;

        private void Start()
        {
            flipCollider = GetComponent<CapsuleCollider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            canFlip = false;
        }

        private void OnTriggerStay(Collider other)
        {
            canFlip = false;
        }

        private void OnTriggerExit(Collider other)
        {
            canFlip = true;
        }
    }
}