using UnityEngine;

namespace LD58.Enemies
{
    public sealed class Enemy : MonoBehaviour
    {
        [SerializeField] private EnemyHealth _health;
        [SerializeField] private EnemyMovement _movement;

        public EnemyHealth Health => _health;
        public EnemyMovement Movement => _movement;
    }
}
