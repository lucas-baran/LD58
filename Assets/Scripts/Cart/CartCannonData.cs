using UnityEngine;

namespace LD58.Cart
{
    [CreateAssetMenu(fileName = "SO_CartCannonData", menuName = "LD58/Cart/Cannon Data")]
    public class CartCannonData : ScriptableObject
    {
        [SerializeField] private float _shootForce = 1f;

        public float ShootForce => _shootForce;
    }
}
