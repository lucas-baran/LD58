using LD58.Common;
using System.Collections.Generic;
using UnityEngine;

namespace LD58.Fruits
{
    [CreateAssetMenu(fileName = "SO_FruitDeck", menuName = CreateAssetMenuItems.FRUITS + "Deck")]
    public sealed class FruitDeckSO : ScriptableObject
    {
        [SerializeField] private List<FruitData> _fruits = new();

        public IReadOnlyList<FruitData> Fruits => _fruits;
    }
}
