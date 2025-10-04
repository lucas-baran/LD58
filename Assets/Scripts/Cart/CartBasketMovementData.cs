using UnityEngine;

namespace LD58.Cart
{
    [CreateAssetMenu(fileName = "SO_CartBasketMovementData", menuName = "LD58/Cart/Basket Movement Data")]
    public class CartBasketMovementData : ScriptableObject
    {
        [SerializeField] private float _acceleration = 1f;
        [SerializeField] private float _accelerationThreshold = 0.0001f;
        [SerializeField] private float _velocityThreshold = 0.0001f;

        public float Acceleration => _acceleration;
        public float AccelerationThreshold => _accelerationThreshold;
        public float VelocityThreshold => _velocityThreshold;
    }
}
