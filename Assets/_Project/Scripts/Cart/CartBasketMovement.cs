using UnityEngine;

namespace LD58.Cart
{
    public class CartBasketMovement : MonoBehaviour
    {
        [SerializeField] private bool _noPhysics = true;
        [SerializeField] private bool _useSignOnly = false;
        [SerializeField] private CartBasketMovementData _data;
        [SerializeField] private Transform _cartTransform;
        [SerializeField] private Rigidbody2D _rigidbody;

        private void FixedUpdate()
        {
            if (_noPhysics)
            {
                transform.position = new Vector3(_cartTransform.position.x, transform.position.y, transform.position.z);

                return;
            }

            float difference = _cartTransform.position.x - transform.position.x;

            if (Mathf.Abs(difference) < _data.AccelerationThreshold
                && Mathf.Abs(_rigidbody.linearVelocityX) < _data.VelocityThreshold
                )
            {
                transform.position = new Vector3(_cartTransform.position.x, transform.position.y, transform.position.z);

                return;
            }

            _rigidbody.AddForce(new Vector2(_data.Acceleration * (_useSignOnly ? Mathf.Sign(difference) : difference), 0f), ForceMode2D.Impulse);
        }
    }
}
