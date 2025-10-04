using LD58.Taxes;
using System.Collections.Generic;
using UnityEngine;

namespace LD58.Levels
{
    [CreateAssetMenu(fileName = "SO_FruitCostData", menuName = "LD58/Taxes/Fruit cost data")]
    public class LevelData : ScriptableObject
    {
        // TODO spawn disposition
        [SerializeField] private List<LevelTax> _taxes = new();

        public IReadOnlyList<LevelTax> Taxes => _taxes;
    }
}
