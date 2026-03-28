using LD58.Fruits;
using UnityEngine;

namespace LD58.Players
{
    public class Player : Singleton<Player>
    {
        [SerializeField] private PlayerInventory _inventory;
        [SerializeField] private FruitDeck _fruitDeck;

        public PlayerInventory Inventory => _inventory;
        public FruitDeck FruitDeck => _fruitDeck;
    }
}
