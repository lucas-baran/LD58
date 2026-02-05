using System.Collections.Generic;
using UnityEngine;

namespace LD58.Fruits
{
    public sealed class FruitDeck : MonoBehaviour
    {
        [SerializeField] private FruitDeckSO _startingDeck = null;

        private readonly List<FruitData> _fruits = new();

        public IReadOnlyList<FruitData> Fruits => _fruits;

        public void AddFruit(FruitData data)
        {
            AddFruitInternal(data, trigger_effects: true);
        }

        private void AddFruitInternal(FruitData data, bool trigger_effects)
        {
            _fruits.Add(data);
        }

        private void Awake()
        {
            for (int fruit_index = 0; fruit_index < _startingDeck.Fruits.Count; fruit_index++)
            {
                AddFruitInternal(_startingDeck.Fruits[fruit_index], trigger_effects: false);
            }
        }
    }
}
