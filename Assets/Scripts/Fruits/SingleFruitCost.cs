using System;
using UnityEngine;

namespace LD58.Fruits
{
    [Serializable]
    public sealed class SingleFruitCost
    {
        [SerializeField] private FruitData _fruitData;
        [SerializeField] private int _quantity;

        public FruitData FruitData => _fruitData;
        public int Quantity => _quantity;
    }
}
