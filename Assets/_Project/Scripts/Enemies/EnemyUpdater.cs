using Cysharp.Threading.Tasks;
using LucasBaran.GameTime;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace LD58.Enemies
{
    public sealed class EnemyUpdater : MonoBehaviour
    {
        [SerializeField] private float _movementDuration = 1f;

        private readonly HashSet<Enemy> _activeEnemies = new();

        public bool TryGetFirstEnemy(out Enemy enemy)
        {
            foreach (Enemy first_enemy in _activeEnemies)
            {
                enemy = first_enemy;
                return true;
            }

            enemy = null;
            return false;
        }

        public async UniTask MoveEnemiesAsync(CancellationToken cancellation_token)
        {
            float elapsed_time = 0f;

            while (elapsed_time < _movementDuration)
            {
                await UniTask.NextFrame(cancellation_token);

                float delta_time = GameplayTime.DeltaTime;
                elapsed_time += delta_time;
                float movement_delta_time = delta_time / _movementDuration;

                foreach (Enemy enemy in _activeEnemies)
                {
                    enemy.Movement.Tick(movement_delta_time);
                }
            }
        }

        internal bool AddActiveEnemy(Enemy enemy)
        {
            return _activeEnemies.Add(enemy);
        }

        internal bool RemoveActiveEnemy(Enemy enemy)
        {
            return _activeEnemies.Remove(enemy);
        }
    }
}
