using Cysharp.Threading.Tasks;
using LD58.Cart;
using LD58.Fruits;
using LD58.Players;
using LD58.Taxes;
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
        private LevelTax _currentTax;

        public event UnityAction OnShotCountIncreased = null;
        public event UnityAction OnLose = null;

        public int RemainingShotCount => _currentTax.StartsAtShot + _currentTax.ShotCountBeforePaying - _shotCount;
        public LevelTax CurrentTax => _currentTax;

        public void PayTaxes()
        {
            if (_currentTax == null)
            {
                return;
            }

            for (int i = 0; i < _currentTax.FruitCost.FruitCosts.Count; i++)
            {
                SingleFruitCost fruit_cost = _currentTax.FruitCost.FruitCosts[i];
                Player.Instance.Inventory.UnloadFruit(fruit_cost.FruitData, fruit_cost.Quantity);
            }
        }

        public bool CanPayTaxes()
        {
            if (_currentTax == null)
            {
                return false;
            }

            for (int i = 0; i < _currentTax.FruitCost.FruitCosts.Count; i++)
            {
                SingleFruitCost fruit_cost = _currentTax.FruitCost.FruitCosts[i];

                if (Player.Instance.Inventory.GetFruitCount(fruit_cost.FruitData) < fruit_cost.Quantity)
                {
                    return false;
                }
            }

            return true;
        }

        private async UniTaskVoid PrepareNextShotAsync(CancellationToken cancellation_token)
        {
            _cartControls.Cannon.CanShoot = false;
            await WaitForMovingFruitsAsync(cancellation_token);
            _shotCount++;

            if (HasLost())
            {
                Lose();

                return;
            }
            else
            {
                UpdateCurrentTax();
                OnShotCountIncreased?.Invoke();
            }

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

        private void UpdateCurrentTax()
        {
            if (_currentTax != null)
            {
                return;
            }

            for (int i = 0; i < _data.Taxes.Count; i++)
            {
                LevelTax level_tax = _data.Taxes[i];

                if (_shotCount >= level_tax.StartsAtShot)
                {
                    _currentTax = level_tax;

                    return;
                }
            }
        }

        private void CartCannon_OnShot()
        {
            PrepareNextShotAsync(destroyCancellationToken).Forget();
        }

        private bool HasLost()
        {
            return !Player.Instance.Inventory.HasFruits()
                || (_currentTax != null && _shotCount > _currentTax.StartsAtShot + _currentTax.ShotCountBeforePaying);
        }

        protected override void Awake()
        {
            base.Awake();
            UpdateCurrentTax();
        }

        private void Start()
        {
            _cartControls = FindAnyObjectByType<CartControls>();
            _fruitGrower = FindAnyObjectByType<FruitGrower>();

            _cartControls.Cannon.CanShoot = true;
            _cartControls.Cannon.OnShot += CartCannon_OnShot;
        }

        private void OnDestroy()
        {
            _cartControls.Cannon.OnShot -= CartCannon_OnShot;
        }
    }
}
