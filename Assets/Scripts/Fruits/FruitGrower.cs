using System.Collections.Generic;
using UnityEngine;

namespace LD58.Fruits
{
    public class FruitGrower : MonoBehaviour
    {
        private readonly List<Fruit> _fruits = new();

        public void GrowFruits()
        {
            foreach (Fruit fruit in _fruits)
            {
                fruit.Grow();
            }
        }

        private void Start()
        {
            _fruits.AddRange(FindObjectsByType<Fruit>(FindObjectsSortMode.None));
        }
    }
}
