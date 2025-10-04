using UnityEngine;

namespace LD58.Fruits
{
    public class Fruit : MonoBehaviour
    {
        [SerializeField] private int _startingGrowStep = 0;
        [SerializeField] private FruitData _startingData = null;
        [SerializeField] private SpriteRenderer _renderer = null;
        [SerializeField] private Collider2D _collider = null;
        [SerializeField] private Rigidbody2D _rigidbody = null;

        private FruitData _data;
        private int _health;

        public bool IsMoving => _rigidbody.bodyType == RigidbodyType2D.Dynamic;
        public FruitData Data
        {
            get => _data;
            private set
            {
                if (value != _data)
                {
                    _data = value;
                    _health = _data.Health;
                    _renderer.sprite = _data.Sprite;
                    _collider.enabled = _data.HasCollisions;
                }
            }
        }

        public void Initialize(FruitData data)
        {
            _rigidbody.angularVelocity = 0f;
            _rigidbody.linearVelocity = Vector2.zero;
            _rigidbody.bodyType = RigidbodyType2D.Kinematic;
            Data = data;
        }

        public void Impulse(Vector2 velocity)
        {
            _rigidbody.AddForce(velocity, ForceMode2D.Impulse);
        }

        public void EnableCollisions()
        {
            _collider.enabled = true;
        }

        public void Grow()
        {
            if (_data.CanGrow)
            {
                Data = _data.GetRandomFruitGrowth();
            }
        }

        private void OnHit(Fruit fruit)
        {
            if (IsMoving)
            {
                return;
            }

            if (_health <= 0)
            {
                _health -= fruit.Data.Damage;
                _rigidbody.bodyType = RigidbodyType2D.Dynamic;
            }
        }

        private void OnCollisionEnter2D(
            Collision2D collision
            )
        {
            if (collision.collider.TryGetComponent(out Fruit other_fruit))
            {
                other_fruit.OnHit(this);
            }
        }

        private void Awake()
        {
            Data = _startingData;
        }
    }
}
