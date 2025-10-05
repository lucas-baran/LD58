using Cysharp.Threading.Tasks;
using LD58.Game;
using LD58.Inputs;
using LD58.Levels;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace LD58.UI
{
    public sealed class UIGameplay : MonoBehaviour
    {
        [SerializeField] private UITaxPanel _taxPanelUI;
        [SerializeField] private UIFruitCostPanel _fruitCostUI;
        [SerializeField] private Button _quitToMenuButton;

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

        private void QuitToMenuButton_OnClick()
        {
            if (Level.HasInstance && Level.Instance.IsPlaying)
            {
                GameManager.Instance.LoadMainMenuSceneAsync().Forget();
            }
        }

        private void Cancel_performed(InputAction.CallbackContext context)
        {
            QuitToMenuButton_OnClick();
        }

        private void Start()
        {
            InputManager.Instance.UI.Cancel.performed += Cancel_performed;
            Level.Instance.OnShotCountIncreased += Level_OnShotCountIncreased;
            Level.Instance.OnLose += Level_OnLose;
            Level.Instance.OnTaxPayed += Level_OnTaxPayed;

            _quitToMenuButton.onClick.AddListener(QuitToMenuButton_OnClick);

            RefreshUI();
        }

        private void OnDestroy()
        {
            if (InputManager.HasInstance)
            {
                InputManager.Instance.UI.Cancel.performed -= Cancel_performed;
            }

            if (Level.HasInstance)
            {
                Level.Instance.OnShotCountIncreased -= Level_OnShotCountIncreased;
                Level.Instance.OnLose -= Level_OnLose;
                Level.Instance.OnTaxPayed -= Level_OnTaxPayed;
            }

            _quitToMenuButton.onClick.RemoveListener(QuitToMenuButton_OnClick);
        }
    }
}
