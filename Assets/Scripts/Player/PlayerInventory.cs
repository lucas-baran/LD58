using LD58.Fruits;
using System.Collections.Generic;
using UnityEngine;

namespace LD58.Players
{
    public class PlayerInventory : MonoBehaviour
    {
        private readonly List<CollectedFruit> _collectedFruits = new();

        public void CollectFruit(
            Fruit fruit
            )
        {
            if (fruit != null)
            {
                _collectedFruits.Add(fruit.GetCollectedFruit());
            }
        }
    }
}
