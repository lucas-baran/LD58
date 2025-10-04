using System.Collections.Generic;
using UnityEngine;

namespace LD58.Fruits
{
    [CreateAssetMenu(fileName = "SO_FruitData", menuName = "LD58/Fruits/Fruit data")]
    public class FruitData : ScriptableObject
    {
        [SerializeField] private string _name = string.Empty;
        [SerializeField] private int _value = 1;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private bool _hasCollisions;
        [SerializeField] private List<FruitData> _fruitGrowths = new();

        public string Name => _name;
        public bool CanGrow => _fruitGrowths != null && _fruitGrowths.Count > 0;
        public int Value => _value;
        public Sprite Sprite => _sprite;
        public bool HasCollisions => _hasCollisions;

        public FruitData GetRandomFruitGrowth()
        {
            return _fruitGrowths[Random.Range(0, _fruitGrowths.Count)];
        }
    }
}
