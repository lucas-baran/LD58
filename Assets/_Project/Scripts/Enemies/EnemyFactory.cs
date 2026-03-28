using UnityEngine;
using UnityEngine.Pool;

namespace LD58.Enemies
{
    public sealed class EnemyFactory
    {
        private readonly Enemy _enemyPrefab;
        private readonly Transform _spawnPoint;
        private readonly InstantiateParameters _instantiateParameters;
        private readonly EnemyUpdater _enemyUpdater;
        private readonly ObjectPool<Enemy> _pool;

        public EnemyFactory(Enemy enemy_prefab, Transform spawn_point, InstantiateParameters instantiate_parameters, EnemyUpdater enemy_updater)
        {
            _enemyPrefab = enemy_prefab;
            _spawnPoint = spawn_point;
            _instantiateParameters = instantiate_parameters;
            _enemyUpdater = enemy_updater;
            _pool = new ObjectPool<Enemy>(InstantiateEnemy, OnGet, OnRelease, OnDestroy, collectionCheck: false);
        }

        public Enemy Get()
        {
            Enemy enemy = _pool.Get();
            _enemyUpdater.AddActiveEnemy(enemy);
            return enemy;
        }

        public void Release(Enemy enemy)
        {
            _pool.Release(enemy);
        }

        private Enemy InstantiateEnemy()
        {
            return Object.Instantiate(_enemyPrefab, _instantiateParameters);
        }

        private void OnGet(Enemy enemy)
        {
            enemy.Initialize(this);
            enemy.transform.position = _spawnPoint.position;
            enemy.gameObject.SetActive(true);
        }

        private void OnRelease(Enemy enemy)
        {
            enemy.gameObject.SetActive(false);
            _enemyUpdater.RemoveActiveEnemy(enemy);
        }

        private void OnDestroy(Enemy enemy)
        {
            _enemyUpdater.RemoveActiveEnemy(enemy);
            Object.Destroy(enemy.gameObject);
        }
    }
}
