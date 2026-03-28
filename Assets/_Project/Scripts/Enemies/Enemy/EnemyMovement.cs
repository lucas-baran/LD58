using UnityEngine;

namespace LD58.Enemies
{
    public sealed class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private EnemyMovementConfig _config;

        internal void Tick(float delta_time)
        {
            Vector3 direction = Vector3.left;
            transform.position += delta_time * _config.Speed * direction;
        }
    }
}
