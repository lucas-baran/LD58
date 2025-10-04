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

        public FruitData GetBestFruit(IComparer<FruitData> fruit_comparer)
        {
            FruitData best_fruit_data = null;

            foreach ((FruitData fruit_data, int count) in _collectedFruits)
            {
                if (count == 0)
                {
                    continue;
                }

                if (best_fruit_data == null)
                {
                    best_fruit_data = fruit_data;
                }
                else if (fruit_comparer.Compare(fruit_data, best_fruit_data) > 0)
                {
                    best_fruit_data = fruit_data;
                }
            }

            if (best_fruit_data != null)
            {
                UnloadFruit(best_fruit_data);
            }
            else
            {
                Debug.LogError("No fruit in inventory!");
            }

            return best_fruit_data;
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
