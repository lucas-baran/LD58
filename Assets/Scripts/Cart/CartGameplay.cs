using LD58.Fruits;
using UnityEngine;

namespace LD58.Cart
{
    public class CartGameplay : MonoBehaviour
    {
        [SerializeField] private CartControls _cartControls;

        private FruitGrower _fruitGrower;

        private void CartControls_OnShot()
        {
            _fruitGrower.GrowFruits();
        }

        private void Start()
        {
            _fruitGrower = FindAnyObjectByType<FruitGrower>();

            _cartControls.OnShot += CartControls_OnShot;
        }
    }
}
