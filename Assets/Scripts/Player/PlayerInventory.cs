using LD58.Fruits;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LD58.Players
{
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] private List<StartingFruit> _startingFruits = new();

        private readonly Dictionary<FruitData, int> _collectedFruits = new();

        public void CollectFruit(
            FruitData fruit_data
            )
        {
            if (fruit_data != null)
            {
                _collectedFruits.TryAdd(fruit_data, 0);
                _collectedFruits[fruit_data]++;
            }
        }

        public bool HasFruits()
        {
            foreach (int count in _collectedFruits.Values)
            {
                if (count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public void UnloadFruit(FruitData fruit_data)
        {
            if (_collectedFruits.ContainsKey(fruit_data))
            {
                _collectedFruits[fruit_data]--;
            }
        }

        private void Awake()
        {
            foreach (StartingFruit starting_fruit in _startingFruits)
            {
                _collectedFruits.TryAdd(starting_fruit.FruitData, 0);
                _collectedFruits[starting_fruit.FruitData] += starting_fruit.Count;
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
