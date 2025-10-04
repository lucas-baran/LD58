using Cysharp.Threading.Tasks;
using LD58.Fruits;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

namespace LD58.Cart
{
    public class CartGameplay : MonoBehaviour
    {
        [SerializeField] private CartControls _cartControls;

        private FruitGrower _fruitGrower;

        public event UnityAction OnLose = null;

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
            PrepareNextShotAsync(destroyCancellationToken).Forget();
        }

        private void Cannon_OnHasNoFruitToShoot()
        {
            Lose();
        }

        private void Start()
        {
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
