using Systems;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts.Systems.HealthSystem
{
    public class Health : MonoBehaviour
    {
        [VectorLabels("Max", "Current")] [SerializeField]
        private Vector2Int _health = new Vector2Int(100, 100);

        public UnityEvent<int> HealthChanged = new UnityEvent<int>();
        public UnityEvent Death = new UnityEvent();

        public int MaxHealth => _health.x;
        public int CurrentHealth => _health.y;
        public void Init(int max, int current) => _health = new Vector2Int(max, current);

        private void OnEnable() => _health.y = _health.x;

        public void TakeDamage(int damage)
        {
            _health.y = Mathf.Clamp(CurrentHealth - damage, 0, MaxHealth);

            HealthChanged.Invoke(CurrentHealth);
            if (CurrentHealth <= 0) Death.Invoke();

            //TODO: Add visual feedback to health loss other than losing a petal (screen flash/shake or something)?
        }
    }
}