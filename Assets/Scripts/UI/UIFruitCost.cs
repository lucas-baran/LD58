using LD58.Fruits;
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
            _countText.text = fruit_cost.Quantity.ToString();
            _icon.sprite = fruit_cost.FruitData.Sprite;
        }
    }
}
