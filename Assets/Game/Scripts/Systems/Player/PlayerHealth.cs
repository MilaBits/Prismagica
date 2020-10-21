using Systems;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [VectorLabels("Max", "Current"), SerializeField]
    private Vector2Int Health;

    public UnityEvent<int> HealthChanged = new UnityEvent<int>();


    private void OnEnable()
    {
        Health.y = Health.x;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Hurts"))
        {
            int cachedHealth = Health.y;

            Health.y -= Mathf.Clamp(other.gameObject.GetComponent<Damager>().Damage, 0, Health.x);

            //TODO: Add visual feedback to health loss other than losing a petal (screen flash/shake or something)

            if (Health.y != cachedHealth) HealthChanged.Invoke(Health.y);
        }
    }
}