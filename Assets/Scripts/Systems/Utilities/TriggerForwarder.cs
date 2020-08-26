using UnityEngine;
using UnityEngine.Events;

namespace Systems.Utilities
{
    public class TriggerForwarder : MonoBehaviour
    {
        public UnityEvent<Collider2D> enterEvent;
        public UnityEvent<Collider2D> stayEvent;
        public UnityEvent<Collider2D> exitEvent;

        private void OnTriggerEnter2D(Collider2D other) => enterEvent.Invoke(other);
        private void OnTriggerStay2D(Collider2D other) => stayEvent.Invoke(other);
        private void OnTriggerExit2D(Collider2D other) => exitEvent.Invoke(other);
    }
}