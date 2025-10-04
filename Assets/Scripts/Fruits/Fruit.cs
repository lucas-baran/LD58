using UnityEngine;

namespace LD58.Fruits
{
    public class Fruit : MonoBehaviour
    {
        [SerializeField] private FruitData _data = null;
        [SerializeField] private SpriteRenderer _renderer = null;
        [SerializeField] private Rigidbody2D _rigidbody = null;

        public FruitData Data => _data;

        private void OnHit()
        {
            _rigidbody.bodyType = RigidbodyType2D.Dynamic;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        private void OnCollisionEnter2D(
            Collision2D collision
            )
        {
            OnHit();
        }

        private void Awake()
        {
            _renderer.sprite = _data.Sprite;
        }
    }
}
