using UnityEngine;
using UnityEngine.Events;

namespace LD58.Enemies
{
    public sealed class EnemyHealth : MonoBehaviour
    {
        [SerializeField] private EnemyHealthConfig _config;
        [SerializeField] private UnityEvent _onHealthChanged = new();
        [SerializeField] private UnityEvent _onDied = new();

        private float _maxHealth;
        private float _health;

        public float MaxHealth => _maxHealth;
        public float Health => _health;
        public float HealthPercent => _health / _maxHealth;
        public bool IsDead => _health <= 0f;

        public UnityEvent OnHealthChanged => _onHealthChanged;
        public UnityEvent OnDied => _onDied;

        public void Initialize()
        {
            _maxHealth = _config.Health;
            _health = _config.Health;
            _onHealthChanged.Invoke();
        }

        public void ChangeHealth(float amount)
        {
            if (IsDead)
            {
                return;
            }

            float previous_health = _health;
            _health = Mathf.Clamp(_health + amount, 0f, _maxHealth);

            if (IsDead)
            {
                _onDied.Invoke();
            }
            else if (_health != previous_health)
            {
                _onHealthChanged.Invoke();
            }
        }
    }
}
