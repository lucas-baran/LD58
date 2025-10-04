using UnityEngine;

namespace LD58.Players
{
    public class Player : Singleton<Player>
    {
        [SerializeField] private PlayerInventory _inventory;

        public PlayerInventory Inventory => _inventory;
    }
}
