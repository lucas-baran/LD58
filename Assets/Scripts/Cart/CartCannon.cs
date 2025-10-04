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

        public event UnityAction OnHasNoFruitToShoot = null;

        public void Shoot()
        {
            if (_fruitToShoot != null)
            {
                Vector3 shoot_velocity = _data.ShootForce * -_fruitParent.up;
                _fruitToShoot.Impulse(shoot_velocity);
                _fruitToShoot.transform.parent = null;
            }
        }

        public void SetCanShoot(bool can_shoot)
        {
            if (can_shoot)
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
