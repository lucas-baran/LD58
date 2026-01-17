using UnityEngine;
using UnityEngine.Events;

namespace LD58.Fruits
{
    public class Fruit : MonoBehaviour, IFruitCollisionTarget
    {
        [SerializeField] private int _startingGrowStep = 0;
        [SerializeField] private int _sortingOrder = 0;
        [SerializeField] private FruitData _startingData = null;
        [SerializeField] private Transform _spriteSizeTransform = null;
        [SerializeField] private Transform _colliderSizeTransform = null;
        [SerializeField] private SpriteRenderer _renderer = null;
        [SerializeField] private Collider2D _collider = null;
        [SerializeField] private Rigidbody2D _rigidbody = null;

        private FruitData _data;
        private int _health;
        private int _lastDamageFrame;

        public bool IsMoving => _rigidbody.bodyType == RigidbodyType2D.Dynamic;
        public float GravityScale => _rigidbody.gravityScale;
        public int SortingOrder
        {
            get => _renderer.sortingOrder;
            set => _renderer.sortingOrder = value;
        }
        public FruitData Data
        {
            get => _data;
            private set
            {
                _data = value;

                if (_data != null)
                {
                    _health = _data.Health;
                    _spriteSizeTransform.localScale = new Vector3(_data.SpriteSize, _data.SpriteSize, _data.SpriteSize);
                    _colliderSizeTransform.localScale = new Vector3(_data.ColliderSize, _data.ColliderSize, _data.ColliderSize);
                    _renderer.sprite = _data.Sprite;
                    _renderer.color = _data.Color;
                    SortingOrder = _data.SortingOrder;
                    _collider.enabled = _data.HasCollisions;
                }
            }
        }

        public event UnityAction OnDetach = null;

        public void Initialize(FruitData data)
        {
            transform.parent = null;
            transform.localScale = Vector3.one;
            _rigidbody.angularVelocity = 0f;
            _rigidbody.linearVelocity = Vector2.zero;
            _rigidbody.bodyType = RigidbodyType2D.Kinematic;
            SortingOrder = _sortingOrder;
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
            if (IsMoving
                || _lastDamageFrame == Time.frameCount
                )
            {
                return;
            }

            _lastDamageFrame = Time.frameCount;
            _health -= fruit.Data.Damage;

            if (_health <= 0)
            {
                _rigidbody.bodyType = RigidbodyType2D.Dynamic;
                OnDetach?.Invoke();
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
