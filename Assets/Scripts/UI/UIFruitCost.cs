using LD58.Fruits;
using LD58.Players;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LD58.UI
{
    public class UIFruitCost : MonoBehaviour
    {
        [SerializeField] private TMP_Text _countText;
        [SerializeField] private Image _icon;

        public void SetCost(SingleFruitCost fruit_cost)
        {
            _countText.text = FormatFruitQuantity(fruit_cost);
            _icon.sprite = fruit_cost.FruitData.Sprite;
            _icon.color = fruit_cost.FruitData.Color;
        }

        private string FormatFruitQuantity(SingleFruitCost fruit_cost)
        {
            return $"{Player.Instance.Inventory.GetFruitCount(fruit_cost.FruitData)}/{fruit_cost.Quantity}";
        }
    }
}
