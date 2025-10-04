using LD58.Taxes;
using System.Collections.Generic;
using UnityEngine;

namespace LD58.Levels
{
    [CreateAssetMenu(fileName = "SO_LevelData", menuName = "LD58/Levels/Level data")]
    public class LevelData : ScriptableObject
    {
        // TODO spawn disposition
        [SerializeField] private List<LevelTax> _taxes = new();

        public IReadOnlyList<LevelTax> Taxes => _taxes;
    }
}
