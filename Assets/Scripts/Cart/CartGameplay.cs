using Cysharp.Threading.Tasks;
using LD58.Fruits;
using System.Threading;
using UnityEngine;

namespace LD58.Cart
{
    public class CartGameplay : MonoBehaviour
    {
        [SerializeField] private CartControls _cartControls;

        private FruitGrower _fruitGrower;

        private async UniTaskVoid PrepareNextShotAsync(CancellationToken cancellation_token)
        {
            await WaitForMovingFruitsAsync(cancellation_token);
            _fruitGrower.GrowFruits();
        }

        private async UniTask WaitForMovingFruitsAsync(CancellationToken cancellation_token)
        {
            await UniTask.WaitWhile(_fruitGrower, fruit_grower => fruit_grower.IsAnyFruitMoving(), cancellationToken: cancellation_token);
        }

        private void CartControls_OnShot()
        {
            PrepareNextShotAsync(destroyCancellationToken).Forget();
        }

        private void Start()
        {
            _fruitGrower = FindAnyObjectByType<FruitGrower>();

            _cartControls.OnShot += CartControls_OnShot;
        }
    }
}
