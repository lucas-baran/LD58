using LD58.Fruits;
using System;
using UnityEngine;

namespace LD58.Taxes
{
    [Serializable]
    public sealed class LevelTax
    {
        [SerializeField] private FruitCostData _fruitCost;
        [SerializeField] private int _startsAtShot = 0;
        [SerializeField] private int _shotCountBeforePaying = 5;

        public FruitCostData FruitCost => _fruitCost;
        public int StartsAtShot => _startsAtShot;
        public int ShotCountBeforePaying => _shotCountBeforePaying;
    }
}
