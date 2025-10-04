using LD58.Fruits;
using System.Collections.Generic;
using UnityEngine;

namespace LD58.UI
{
    public class UIFruitCostPanel : MonoBehaviour
    {
        [SerializeField] private List<UIFruitCost> _fruitCostUIs = new();

        public void SetCost(FruitCostData fruit_cost_data)
        {
            var fruit_costs = fruit_cost_data.FruitCosts;

            foreach (UIFruitCost fruit_cost_ui in _fruitCostUIs)
            {
                fruit_cost_ui.gameObject.SetActive(false);
            }

            for (int i = 0; i < fruit_costs.Count; i++)
            {
                UIFruitCost fruit_cost_ui = _fruitCostUIs[i];
                fruit_cost_ui.gameObject.SetActive(true);
                fruit_cost_ui.SetCost(fruit_costs[i]);
            }
        }
    }
}
