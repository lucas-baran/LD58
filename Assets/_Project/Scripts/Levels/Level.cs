using Cysharp.Threading.Tasks;
using LD58.Cart;
using LD58.Enemies;
using LD58.Fruits;
using System.Threading;
using UnityEngine;

namespace LD58.Levels
{
    public class Level : Singleton<Level>
    {
        [SerializeField] private LevelData _data;
        [SerializeField] private LevelIntro _intro;
        [SerializeField] private EnemyUpdater _enemyUpdater;
        [SerializeField] private EnemySpawner _enemySpawner;

        private CartControls _cartControls;

        public bool IsPlaying { get; private set; }
        public LevelData Data => _data;

        private async UniTaskVoid PlayIntroAsync(CancellationToken cancellation_token)
        {
            if (_intro != null)
            {
                await _intro.PlayAsync(cancellation_token);
            }

            IsPlaying = true;
            _cartControls.SetEnabled(true);
            _enemySpawner.SpawnEnemies();
        }

        private async UniTaskVoid PrepareNextShotAsync(CancellationToken cancellation_token)
        {
            _cartControls.Cannon.CanShoot = false;

            await FruitGrower.Instance.WaitForMovingFruitsAsync(cancellation_token);
            await _enemyUpdater.MoveEnemiesAsync(cancellation_token);
            _enemySpawner.SpawnEnemies();

            FruitGrower.Instance.GrowFruits();
            _cartControls.Cannon.CanShoot = true;
        }

        private void CartCannon_OnShot()
        {
            PrepareNextShotAsync(destroyCancellationToken).Forget();
        }

        private void Start()
        {
            _cartControls = FindAnyObjectByType<CartControls>();
            _cartControls.Cannon.OnShot += CartCannon_OnShot;

            FruitGrower.Instance.Initialize(_data);
            _cartControls.SetEnabled(false);
            IsPlaying = false;

            PlayIntroAsync(destroyCancellationToken).Forget();
        }

        private void OnDestroy()
        {
            _cartControls.Cannon.OnShot -= CartCannon_OnShot;
        }
    }
}
