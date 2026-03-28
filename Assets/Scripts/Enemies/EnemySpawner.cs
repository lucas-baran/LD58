using UnityEngine;

namespace LD58.Enemies
{
    public sealed class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private EnemySpawnerConfig _config;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private EnemyUpdater _enemyUpdater;

        private EnemyFactory _enemyFactory;
        private int _spawnTick = 0;
        private int _nextSpawnTick = 0;

        public static EnemySpawner Instance { get; private set; }

        public void DamageFirstEnemy()
        {
            if (_enemyUpdater.TryGetFirstEnemy(out Enemy enemy))
            {
                enemy.Health.ChangeHealth(-1f);
            }
        }

        public void SpawnEnemies()
        {
            _spawnTick++;

            if (_spawnTick >= _nextSpawnTick)
            {
                SpawnEnemy();
                _nextSpawnTick += _config.SpawnTick;
            }
        }

        private void SpawnEnemy()
        {
            Enemy enemy = _enemyFactory.Get();
        }

        private void Awake()
        {
            Instance = this;
            _enemyFactory = new EnemyFactory(_config.EnemyPrefab, _spawnPoint, new InstantiateParameters() { scene = gameObject.scene, originalImmutable = true }, _enemyUpdater);
        }
    }
}
