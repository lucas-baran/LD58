using UnityEngine;

namespace LD58.Fruits
{
    public class Fruit : MonoBehaviour
    {
        [SerializeField] private int _startingGrowStep = 0;
        [SerializeField] private FruitData _data = null;
        [SerializeField] private SpriteRenderer _renderer = null;
        [SerializeField] private Collider2D _collider = null;
        [SerializeField] private Rigidbody2D _rigidbody = null;

        private int _currentGrowIndex = 0;

        public FruitData Data => _data;
        public bool IsMoving => _rigidbody.bodyType == RigidbodyType2D.Dynamic;
        public int GrowStep => _currentGrowIndex;

        public void Initialize(int grown_step)
        {
            _rigidbody.angularVelocity = 0f;
            _rigidbody.linearVelocity = Vector2.zero;
            _rigidbody.bodyType = RigidbodyType2D.Kinematic;
            SetGrowStep(grown_step);
        }

        public void OnDestroyed()
        {
            SetGrowStep(0);
        }

        public void Impulse(Vector2 velocity)
        {
            _rigidbody.AddForce(velocity, ForceMode2D.Impulse);
        }

        public void Grow()
        {
            if (!IsFullyGrown())
            {
                SetGrowStep(_currentGrowIndex + 1);
            }
        }

        public bool IsFullyGrown()
        {
            return _currentGrowIndex >= _data.MaxGrow;
        }

        private void SetGrowStep(
            int grow_step_index
            )
        {
            _currentGrowIndex = grow_step_index;
            FruitData.GrowStep grow_step = _data.GrowSteps[_currentGrowIndex];
            _renderer.sprite = grow_step.Sprite;
            _collider.enabled = grow_step.HasCollisions;
        }

        private void OnHit()
        {
            _rigidbody.bodyType = RigidbodyType2D.Dynamic;
        }

        private void OnCollisionEnter2D(
            Collision2D collision
            )
        {
            OnHit();
        }

        private void Awake()
        {
            SetGrowStep(_startingGrowStep);
        }
    }
}
