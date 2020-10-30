using UnityEngine;

namespace Game.Scripts.Systems.HealthSystem
{
    [RequireComponent(typeof(Health))]
    public class DamageOnCollision : MonoBehaviour
    {
        [SerializeField] private Health health;

        private void Awake()
        {
            if (!health && GetComponentInChildren<Health>())
            {
                health = GetComponentInChildren<Health>();
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Hurts"))
            {
                health.TakeDamage(other.gameObject.GetComponent<Damager>().Damage);
            }
        }
    }
}