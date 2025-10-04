using LD58.Fruits;
using LD58.Players;
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

            if (!HasFruitInInventory())
            {
                OnHasNoFruitToShoot?.Invoke();

                return;
            }

            FruitData fruit_data = GetFruitFromInventory();
            _fruitToShoot = FruitGrower.Instance.GetFruit(fruit_data);
            _fruitToShoot.transform.parent = _fruitParent;
            _fruitToShoot.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }

        private bool HasFruitInInventory()
        {
            return Player.Instance.Inventory.HasFruits();
        }

        private FruitData GetFruitFromInventory()
        {
            throw new System.NotImplementedException();
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
