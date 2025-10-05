using LD58.Fruits;
using LD58.Players;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LD58.Cart
{
    public class CartCannon : MonoBehaviour
    {
        [SerializeField] private CartCannonData _data;
        [SerializeField] private Transform _fruitParent;

        private Fruit _fruitToShoot;
        private bool _canShoot;
        private int _fruitIndex;
        private readonly IComparer<FruitData> _fruitShootValueComparer = new FruitShootValueComparer();
        private readonly List<FruitData> _fruitsInInventory = new();

        public Vector3 ShootPosition => _fruitParent.transform.position;
        public Vector3 ShootVelocity => _data.ShootForce * -_fruitParent.up;
        public Fruit CurrentFruit => _fruitToShoot;
        public bool CanShoot
        {
            get => _canShoot;
            set
            {
                if (value != _canShoot)
                {
                    _canShoot = value;
                    RefreshCannon();
                }
            }
        }

        public event UnityAction OnShot = null;

        public void Shoot()
        {
            if (_canShoot && _fruitToShoot != null)
            {
                Player.Instance.Inventory.UnloadFruit(_fruitToShoot.Data);
                _fruitToShoot.Impulse(ShootVelocity);
                _fruitToShoot.EnableCollisions();
                _fruitToShoot.transform.parent = null;
                _fruitToShoot.transform.localScale = Vector3.one;
                _fruitToShoot = null;

                OnShot?.Invoke();
            }
        }

        public void NextFruit()
        {
            if (_fruitsInInventory.Count <= 1)
            {
                return;
            }

            _fruitIndex++;

            if (_fruitIndex >= _fruitsInInventory.Count)
            {
                _fruitIndex = 0;
            }

            RefreshFruitToShoot();
        }

        public void PreviousFruit()
        {
            if (_fruitsInInventory.Count <= 1)
            {
                return;
            }

            _fruitIndex--;

            if (_fruitIndex < 0)
            {
                _fruitIndex = _fruitsInInventory.Count - 1;
            }

            RefreshFruitToShoot();
        }

        private void RefreshCannon()
        {
            if (_canShoot)
            {
                LoadFruitToShoot();
            }
            else
            {
                HideFruitToShoot();
            }
        }

        private void LoadFruitToShoot()
        {
            if (_fruitToShoot != null)
            {
                return;
            }

            if (Player.Instance.Inventory.HasFruits())
            {
                FruitData fruit_data = Player.Instance.Inventory.GetBestFruit(_fruitShootValueComparer);
                Player.Instance.Inventory.GetFruitsInInventory(_fruitsInInventory);
                _fruitIndex = _fruitsInInventory.IndexOf(fruit_data);
                RefreshFruitToShoot();
            }
        }

        private void RefreshFruitToShoot()
        {
            if (_fruitToShoot != null)
            {
                FruitGrower.Instance.Destroy(_fruitToShoot);
                _fruitToShoot = null;
            }

            FruitData fruit_data = _fruitsInInventory[_fruitIndex];
            _fruitToShoot = FruitGrower.Instance.GetFruit(fruit_data);
            _fruitToShoot.transform.localScale = Vector3.one;
            _fruitToShoot.transform.SetParent(_fruitParent, worldPositionStays: true);
            _fruitToShoot.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }

        private void HideFruitToShoot()
        {
            if (_fruitToShoot != null)
            {
                FruitGrower.Instance.Destroy(_fruitToShoot);
                _fruitToShoot = null;
            }
        }
    }
}
