using UnityEngine;
using UnityEngine.Events;

namespace LD58.Fruits
{
    public class Fruit : MonoBehaviour, IFruitCollisionTarget
    {
        [SerializeField] private int _startingGrowStep = 0;
        [SerializeField] private FruitData _startingData = null;
        [SerializeField] private Transform _sizeTransform = null;
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
                _data = value;

                if (_data != null)
                {
                    _health = _data.Health;
                    _sizeTransform.localScale = new Vector3(_data.Size, _data.Size, _data.Size);
                    _renderer.sprite = _data.Sprite;
                    _renderer.color = _data.Color;
                    _collider.enabled = _data.HasCollisions;
                }
            }
        }

        public event UnityAction OnFall = null;

        public void Initialize(FruitData data)
        {
            _rigidbody.angularVelocity = 0f;
            _rigidbody.linearVelocity = Vector2.zero;
            _rigidbody.bodyType = RigidbodyType2D.Kinematic;
            Data = data;
        }

        public void Impulse(Vector2 velocity)
        {
            _rigidbody.bodyType = RigidbodyType2D.Dynamic;
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

            _health -= fruit.Data.Damage;

            if (_health <= 0)
            {
                _rigidbody.bodyType = RigidbodyType2D.Dynamic;
                OnFall?.Invoke();
            }
        }

        void IFruitCollisionTarget.OnCollision(Fruit other_fruit)
        {
            other_fruit.OnHit(this);
        }

        private void Awake()
        {
            Data = _startingData;
        }

        private void OnDisable()
        {
            _rigidbody.bodyType = RigidbodyType2D.Kinematic;
        }
    }
}
