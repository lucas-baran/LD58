using UnityEngine;

namespace LD58.Fruits
{
    public class Fruit : MonoBehaviour
    {
        [SerializeField] private FruitData _data = null;
        [SerializeField] private Rigidbody2D _rigidbody = null;

        public FruitData Data => _data;

        public void Hit()
        {
            _rigidbody.bodyType = RigidbodyType2D.Dynamic;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}
