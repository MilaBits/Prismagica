using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts.Systems.LineToggle
{
    public class LineToggleTrigger : MonoBehaviour
    {
        public UnityEvent entered;
        public UnityEvent exited;
        public BoxCollider2D triggerCollider;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player")) entered.Invoke();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player")) exited.Invoke();
        }
    }
}