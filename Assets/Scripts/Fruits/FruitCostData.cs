using System.Collections.Generic;
using UnityEngine;

namespace LD58.Fruits
{
    [CreateAssetMenu(fileName = "SO_FruitCostData", menuName = "LD58/Taxes/Fruit cost data")]
    public class FruitCostData : ScriptableObject
    {
        [SerializeField] private List<SingleFruitCost> _fruitCosts = new();

        public IReadOnlyList<SingleFruitCost> FruitCosts => _fruitCosts;
    }
}
