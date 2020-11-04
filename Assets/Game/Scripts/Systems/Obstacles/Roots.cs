using Game.Scripts.Systems.HealthSystem;
using UnityEngine;

public class Roots : MonoBehaviour
{
    [SerializeField] private Health health;

    private void OnEnable() => health.Death.AddListener(delegate { Death(); });
    private void OnDisable() => health.Death.RemoveListener(delegate { Death(); });
    private void Death() => Destroy(gameObject);
}