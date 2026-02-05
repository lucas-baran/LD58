using System.Collections.Generic;
using UnityEngine;

namespace LD58.Fruits
{
    [CreateAssetMenu(fileName = "SO_FruitDeck", menuName = "LD58/Fruits/Deck")]
    public sealed class FruitDeckSO : ScriptableObject
    {
        [SerializeField] private List<FruitData> _fruits = new();

        public IReadOnlyList<FruitData> Fruits => _fruits;
    }
}
