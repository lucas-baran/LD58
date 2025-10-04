using LD58.Fruits;
using LD58.Taxes;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LD58.Levels
{
    [CreateAssetMenu(fileName = "SO_LevelData", menuName = "LD58/Levels/Level data")]
    public class LevelData : ScriptableObject
    {
        [SerializeField] private List<LevelTax> _taxes = new();
        [SerializeField] private List<FruitPosition> _startingFruits = new();

        public IReadOnlyList<LevelTax> Taxes => _taxes;
        public IReadOnlyList<FruitPosition> StartingFruits => _startingFruits;

        [Serializable]
        public sealed class FruitPosition
        {
            [SerializeField] private Vector2 _position;
            [SerializeField] private FruitData _fruitData;

            public Vector2 Position => _position;
            public FruitData FruitData => _fruitData;
        }
    }
}
