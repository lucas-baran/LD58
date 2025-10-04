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

        public CollectedFruit GetCollectedFruit()
        {
            return new CollectedFruit(_data, _currentGrowIndex);
        }

        public void Grow()
        {
            if (!IsFullyGrown())
            {
                _currentGrowIndex++;
                RefreshGrowState();
            }
        }

        public void Destroy()
        {
            _currentGrowIndex = 0;
            RefreshGrowState();
        }

        public bool IsFullyGrown()
        {
            return _currentGrowIndex >= _data.MaxGrow;
        }

        private void RefreshGrowState()
        {
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
            _currentGrowIndex = _startingGrowStep;
            RefreshGrowState();
        }
    }
}
