using System.Collections.Generic;
using UnityEngine;

namespace LD58.Fruits
{
    public sealed class LevelFruitDeck
    {
        private readonly List<FruitData> _fruits;

        public LevelFruitDeck(IReadOnlyList<FruitData> fruits)
        {
            _fruits = fruits.Clone();
        }

        public bool TryGetRandomFruit(out FruitData fruit)
        {
            if (_fruits.Count == 0)
            {
                fruit = null;

                return false;
            }

            int random = Random.Range(0, _fruits.Count);
            fruit = _fruits[random];
            _fruits.RemoveAt(random);

            return true;
        }

        public void Return(FruitData fruit)
        {
            _fruits.Add(fruit);
        }
    }
}
