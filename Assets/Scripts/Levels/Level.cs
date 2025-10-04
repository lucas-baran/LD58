using Cysharp.Threading.Tasks;
using LD58.Cart;
using LD58.Fruits;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

namespace LD58.Levels
{
    public class Level : Singleton<Level>
    {
        [SerializeField] private LevelData _data;

        private CartControls _cartControls;
        private FruitGrower _fruitGrower;
        private int _shotCount = 0;

        public event UnityAction OnShot = null;
        public event UnityAction OnLose = null;

        public int ShotCount => _shotCount;

        private async UniTaskVoid PrepareNextShotAsync(CancellationToken cancellation_token)
        {
            _cartControls.Cannon.CanShoot = false;
            await WaitForMovingFruitsAsync(cancellation_token);
            _fruitGrower.GrowFruits();
            _cartControls.Cannon.CanShoot = true;
        }

        private async UniTask WaitForMovingFruitsAsync(CancellationToken cancellation_token)
        {
            await UniTask.WaitWhile(_fruitGrower, fruit_grower => fruit_grower.IsAnyFruitMoving(), cancellationToken: cancellation_token);
        }

        private void Lose()
        {
            OnLose?.Invoke();
        }

        private void CartCannon_OnShot()
        {
            _shotCount++;

            if (HasLost())
            {
                Lose();
            }
            else
            {
                OnShot?.Invoke();
                PrepareNextShotAsync(destroyCancellationToken).Forget();
            }
        }

        private bool HasLost()
        {
            return false;
        }

        private void Cannon_OnHasNoFruitToShoot()
        {
            Lose();
        }

        private void Start()
        {
            _cartControls = FindAnyObjectByType<CartControls>();
            _fruitGrower = FindAnyObjectByType<FruitGrower>();

            _cartControls.Cannon.CanShoot = true;
            _cartControls.Cannon.OnShot += CartCannon_OnShot;
            _cartControls.Cannon.OnHasNoFruitToShoot += Cannon_OnHasNoFruitToShoot;
        }

        private void OnDestroy()
        {
            _cartControls.Cannon.OnShot -= CartCannon_OnShot;
            _cartControls.Cannon.OnHasNoFruitToShoot -= Cannon_OnHasNoFruitToShoot;
        }
    }
}
