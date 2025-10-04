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
        private int _shotCount = 0;
        private LevelTax _currentTax;

        public event UnityAction OnShotCountIncreased = null;
        public event UnityAction OnTaxPayed = null;
        public event UnityAction OnLose = null;
        public event UnityAction OnWin = null;

        public int RemainingShotCount => _currentTax.StartsAtShot + _currentTax.ShotCountBeforePaying - _shotCount;
        public LevelTax CurrentTax => _currentTax;

        public void PayTaxes()
        {
            if (!CanPayTax())
            {
                return;
            }

            for (int i = 0; i < _currentTax.FruitCost.FruitCosts.Count; i++)
            {
                SingleFruitCost fruit_cost = _currentTax.FruitCost.FruitCosts[i];
                Player.Instance.Inventory.UnloadFruit(fruit_cost.FruitData, fruit_cost.Quantity);
            }

            _currentTax = null;
            OnTaxPayed?.Invoke();

            if (HasWon())
            {
                Win();
            }
        }

        public bool CanPayTax()
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

            if (HasWon())
            {
                Win();

                return;
            }
            else if (HasLost())
            {
                Lose();

                return;
            }
            else
            {
                // Automatically pay tax if player has no shot left but can pay
                if (TaxIsDue())
                {
                    PayTaxes();
                }

                UpdateCurrentTax();
                OnShotCountIncreased?.Invoke();
            }

            FruitGrower.Instance.GrowFruits();
            _cartControls.Cannon.CanShoot = true;
        }

        private async UniTask WaitForMovingFruitsAsync(CancellationToken cancellation_token)
        {
            await UniTask.WaitWhile(FruitGrower.Instance, fruit_grower => fruit_grower.IsAnyFruitMoving(), cancellationToken: cancellation_token);
        }

        private void Lose()
        {
            OnLose?.Invoke();
        }

        private bool HasLost()
        {
            return !Player.Instance.Inventory.HasFruits()
                || (TaxIsDue() && !CanPayTax());
        }

        private bool TaxIsDue()
        {
            return _currentTax != null && RemainingShotCount <= 0;
        }

        private void Win()
        {
            OnWin?.Invoke();
        }

        private bool HasWon()
        {
            return _currentTax == null
                && !TryGetValidTax(out _);
        }

        private void UpdateCurrentTax()
        {
            if (_currentTax == null)
            {
                TryGetValidTax(out _currentTax);
            }
        }

        private bool TryGetValidTax(out LevelTax tax)
        {
            tax = null;

            for (int i = 0; i < _data.Taxes.Count; i++)
            {
                LevelTax level_tax = _data.Taxes[i];

                if (level_tax.StartsAtShot >= _shotCount)
                {
                    tax = level_tax;

                    return true;
                }
            }

            return false;
        }

        private void CartCannon_OnShot()
        {
            PrepareNextShotAsync(destroyCancellationToken).Forget();
        }

        protected override void Awake()
        {
            base.Awake();
            UpdateCurrentTax();
        }

        private void Start()
        {
            _cartControls = FindAnyObjectByType<CartControls>();

            _cartControls.Cannon.CanShoot = true;
            _cartControls.Cannon.OnShot += CartCannon_OnShot;

            FruitGrower.Instance.SpawnFruits(_data);
        }

        private void OnDestroy()
        {
            _cartControls.Cannon.OnShot -= CartCannon_OnShot;
        }
    }
}
