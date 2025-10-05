using System.Collections.Generic;
using UnityEngine;

namespace LD58.Fruits
{
    [CreateAssetMenu(fileName = "SO_FruitData", menuName = "LD58/Fruits/Fruit data")]
    public class FruitData : ScriptableObject
    {
        [SerializeField] private string _name = string.Empty;
        [SerializeField] private int _value = 1;
        [SerializeField] private int _shootValue = 1;
        [SerializeField] private int _health = 1;
        [SerializeField] private int _damage = 1;
        [SerializeField] private float _spriteSize = 0.3f;
        [SerializeField] private float _colliderSize = 0.3f;
        [SerializeField] private int _sortingOrder = 0;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private Color _color = Color.white;
        [SerializeField] private bool _hasCollisions;
        [SerializeField] private List<FruitData> _fruitGrowths = new();

        public string Name => _name;
        public bool CanGrow => _fruitGrowths != null && _fruitGrowths.Count > 0;
        public int Value => _value;
        public int ShootValue => _shootValue;
        public int Health => _health;
        public int Damage => _damage;
        public float SpriteSize => _spriteSize;
        public float ColliderSize => _colliderSize;
        public int SortingOrder => _sortingOrder;
        public Sprite Sprite => _sprite;
        public Color Color => _color;
        public bool HasCollisions => _hasCollisions;

        public FruitData GetRandomFruitGrowth()
        {
            return _fruitGrowths[Random.Range(0, _fruitGrowths.Count)];
        }
    }
}
