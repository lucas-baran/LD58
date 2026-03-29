using LucasBaran.GameTime;
using UnityEngine;

namespace LD58.Enemies
{
    public sealed class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private EnemyMovementConfig _config;

        private void Update()
        {
            Vector3 direction = Vector3.left;
            transform.position += EnemyMovementTime.DeltaTime * _config.Speed * direction;
        }
    }
}
