using LD58.Fruits;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LD58.Players
{
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] private List<StartingFruit> _startingFruits = new();

        private readonly List<FruitData> _collectedFruits = new();

        public void CollectFruit(
            Fruit fruit
            )
        {
            if (fruit != null)
            {
                _collectedFruits.Add(fruit.Data);
            }
        }

        public bool HasFruits()
        {
            return _collectedFruits.Count > 0;
        }

        public void UnloadFruit(FruitData fruit_data)
        {
            _collectedFruits.Remove(fruit_data);
        }

        private void Awake()
        {
            foreach (StartingFruit starting_fruit in _startingFruits)
            {
                _collectedFruits.Capacity = _collectedFruits.Capacity + starting_fruit.Count;
                _collectedFruits.Add(starting_fruit.FruitData);
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
