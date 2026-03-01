using Cysharp.Threading.Tasks;
using LD58.Cart;
using LD58.Fruits;
using System.Threading;
using UnityEngine;

namespace LD58.Levels
{
    public class Level : Singleton<Level>
    {
        [SerializeField] private LevelData _data;
        [SerializeField] private LevelIntro _intro;

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
        }

        private async UniTaskVoid PrepareNextShotAsync(CancellationToken cancellation_token)
        {
            _cartControls.Cannon.CanShoot = false;
            await WaitForMovingFruitsAsync(cancellation_token);

            FruitGrower.Instance.GrowFruits();
            _cartControls.Cannon.CanShoot = true;
        }

        private async UniTask WaitForMovingFruitsAsync(CancellationToken cancellation_token)
        {
            await UniTask.WaitWhile(FruitGrower.Instance, fruit_grower => fruit_grower.IsAnyFruitMoving(), cancellationToken: cancellation_token);
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
