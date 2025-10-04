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
        private readonly IComparer<FruitData> _fruitShootValueComparer = new FruitShootValueComparer();

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

        public event UnityAction OnHasNoFruitToShoot = null;
        public event UnityAction OnShot = null;

        public void Shoot()
        {
            if (_canShoot && _fruitToShoot != null)
            {
                Vector3 shoot_velocity = _data.ShootForce * -_fruitParent.up;
                _fruitToShoot.Impulse(shoot_velocity);
                _fruitToShoot.EnableCollisions();
                _fruitToShoot.transform.parent = null;
                _fruitToShoot.transform.localScale = Vector3.one;
                _fruitToShoot = null;

                OnShot?.Invoke();
            }
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

            if (!Player.Instance.Inventory.HasFruits())
            {
                OnHasNoFruitToShoot?.Invoke();

                return;
            }

            FruitData fruit_data = Player.Instance.Inventory.GetBestFruit(_fruitShootValueComparer);
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
