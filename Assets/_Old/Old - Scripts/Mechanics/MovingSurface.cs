using UnityEngine;

namespace Mechanics
{
    public class MovingSurface : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            Movement player = other.gameObject.GetComponentInParent<Movement>();
            if (player) other.transform.SetParent(transform);
        }

        private void OnCollisionExit(Collision other)
        {
            Movement player = other.gameObject.GetComponentInParent<Movement>();
            if (player) other.transform.SetParent(null);
        }
    }
}