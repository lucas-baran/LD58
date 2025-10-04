using LD58.Levels;
using UnityEngine;

namespace LD58.UI
{
    public sealed class UIGameplay : MonoBehaviour
    {
        [SerializeField] private UITaxPanel _taxPanelUI;
        [SerializeField] private UIFruitCostPanel _fruitCostUI;

        private void RefreshUI()
        {
            if (Level.Instance.CurrentTax == null)
            {
                _taxPanelUI.gameObject.SetActive(false);
            }
            else
            {
                _taxPanelUI.gameObject.SetActive(true);
                _fruitCostUI.SetCost(Level.Instance.CurrentTax.FruitCost);
                _taxPanelUI.SetRemainingShots(Level.Instance.RemainingShotCount);
                _taxPanelUI.SetButtonEnabled(Level.Instance.CanPayTax());
            }
        }

        private void Level_OnShotCountIncreased()
        {
            RefreshUI();
        }

        private void Level_OnLose()
        {
            _taxPanelUI.gameObject.SetActive(false);
        }

        private void Level_OnTaxPayed()
        {
            RefreshUI();
        }

        private void Start()
        {
            Level.Instance.OnShotCountIncreased += Level_OnShotCountIncreased;
            Level.Instance.OnLose += Level_OnLose;
            Level.Instance.OnTaxPayed += Level_OnTaxPayed;

            RefreshUI();
        }

        private void OnDestroy()
        {
            if (Level.HasInstance)
            {
                Level.Instance.OnShotCountIncreased -= Level_OnShotCountIncreased;
                Level.Instance.OnLose -= Level_OnLose;
                Level.Instance.OnTaxPayed -= Level_OnTaxPayed;
            }
        }
    }
}
