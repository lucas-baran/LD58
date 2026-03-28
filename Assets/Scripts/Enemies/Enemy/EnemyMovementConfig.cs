using LD58.Common;
using UnityEngine;

namespace LD58.Enemies
{
    [CreateAssetMenu(fileName = "SO_EnemyMovement", menuName = CreateAssetMenuItems.ENEMIES + "Movement")]
    public sealed class EnemyMovementConfig : ScriptableObject
    {
        [SerializeField] private float _speed = 1f;

        public float Speed => _speed;
    }
}
