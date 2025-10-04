using LD58.Fruits;
using LD58.Players;
using UnityEngine;

namespace LD58.Cart
{
    public class CartBasketCollector : MonoBehaviour
    {
        private void OnTriggerEnter2D(
            Collider2D collider
            )
        {
            if (collider.TryGetComponent(out Fruit fruit))
            {
                Player.Instance.Inventory.CollectFruit(fruit.Data);
                FruitGrower.Instance.Destroy(fruit);
            }
        }
    }
}
