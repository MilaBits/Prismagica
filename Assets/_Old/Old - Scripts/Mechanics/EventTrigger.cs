using Enums;
using UnityEngine;
using UnityEngine.Events;

namespace Mechanics
{
    public class EventTrigger : MonoBehaviour
    {
        [SerializeField]
        private bool runOnce = false;

        [SerializeField]
        private bool enabled = false;

        public UnityEvent trigger;

        public LayerMask layers;

        [SerializeField]
        private bool isPixi = false;

        public Power power;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != layers) return;

            if (enabled)
            {
                trigger.Invoke();
                return;
            }

            if (runOnce) enabled = false;
        }
    }
}