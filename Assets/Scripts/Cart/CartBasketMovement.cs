using UnityEngine;

namespace LD58.Cart
{
    public class CartBasketMovement : MonoBehaviour
    {
        [SerializeField] private CartBasketMovementData _data;
        [SerializeField] private Transform _cartTransform;
        [SerializeField] private Rigidbody2D _rigidbody;

        private void FixedUpdate()
        {
            transform.position = new Vector3(_cartTransform.position.x, transform.position.y, transform.position.z);
        }
    }
}
