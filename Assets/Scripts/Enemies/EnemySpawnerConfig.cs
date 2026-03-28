using LD58.Common;
using UnityEngine;

namespace LD58.Enemies
{
    [CreateAssetMenu(fileName = "SO_EnemySpawnerConfig", menuName = CreateAssetMenuItems.ENEMIES + "Spawner config")]
    public sealed class EnemySpawnerConfig : ScriptableObject
    {
        [SerializeField] private Enemy _enemyPrefab;
        [SerializeField] private int _spawnTick = 2;

        public Enemy EnemyPrefab => _enemyPrefab;
        public int SpawnTick => _spawnTick;
    }
}
