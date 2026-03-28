using UnityEngine;

namespace LD58.Fruits
{
    public class FruitCollisions : MonoBehaviour
    {
        [SerializeField] private Fruit _fruit;

        public Fruit Fruit => _fruit;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.otherRigidbody.TryGetComponent(out IFruitCollisionTarget target))
            {
                target.OnCollision(_fruit);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IFruitCollisionTarget target))
            {
                target.OnTrigger(_fruit);
            }
        }
    }
}
