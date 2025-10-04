using LD58.Fruits;
using LD58.Players;
using UnityEngine;

namespace LD58.Cart
{
    public class CartBasketCollector : MonoBehaviour, IFruitCollisionTarget
    {
        void IFruitCollisionTarget.OnTrigger(Fruit fruit)
        {
            Player.Instance.Inventory.CollectFruit(fruit.Data);
            FruitGrower.Instance.Destroy(fruit);
        }
    }
}
