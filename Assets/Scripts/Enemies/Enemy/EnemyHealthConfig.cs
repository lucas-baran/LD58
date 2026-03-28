using LD58.Common;
using UnityEngine;

namespace LD58.Enemies
{
    [CreateAssetMenu(fileName = "SO_EnemyHealth", menuName = CreateAssetMenuItems.ENEMIES + "Health")]
    public sealed class EnemyHealthConfig : ScriptableObject
    {
        [SerializeField] private int _health = 10;

        public int Health => _health;
    }
}
