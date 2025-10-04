using LD58.Fruits;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LD58.Players
{
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] private List<StartingFruit> _startingFruits = new();

        private readonly List<CollectedFruit> _collectedFruits = new();

        public IReadOnlyList<CollectedFruit> CollectedFruits => _collectedFruits;

        public void CollectFruit(
            Fruit fruit
            )
        {
            if (fruit != null)
            {
                _collectedFruits.Add(new CollectedFruit(fruit));
            }
        }

        public bool HasFruits()
        {
            return _collectedFruits.Count > 0;
        }

        public void UnloadFruit(CollectedFruit collected_fruit)
        {
            _collectedFruits.Remove(collected_fruit);
        }

        private void Awake()
        {
            foreach (StartingFruit starting_fruit in _startingFruits)
            {
                _collectedFruits.Capacity = _collectedFruits.Capacity + starting_fruit.Count;
                _collectedFruits.Add(new CollectedFruit(starting_fruit.FruitData));
            }
        }

        [Serializable]
        private sealed class StartingFruit
        {
            [SerializeField] private FruitData _fruitData;
            [SerializeField] private int _count;

            public FruitData FruitData => _fruitData;
            public int Count => _count;
        }
    }
}
