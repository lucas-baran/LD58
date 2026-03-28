using UnityEngine;

namespace LD58.Enemies
{
    public sealed class Enemy : MonoBehaviour
    {
        [SerializeField] private EnemyHealth _health;
        [SerializeField] private EnemyMovement _movement;

        private EnemyFactory _enemyFactory;

        public EnemyHealth Health => _health;
        public EnemyMovement Movement => _movement;

        public void Initialize(EnemyFactory enemy_factory)
        {
            _enemyFactory = enemy_factory;
            Health.Initialize();
        }

        public void Despawn()
        {
            _enemyFactory.Release(this);
        }

        private void EnemyHealth_OnDied()
        {
            Despawn();
        }

        private void Awake()
        {
            _health.OnDied.AddListener(EnemyHealth_OnDied);
        }
    }
}
