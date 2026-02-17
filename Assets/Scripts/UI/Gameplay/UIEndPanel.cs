using Cysharp.Threading.Tasks;
using LD58.Game;
using LD58.Inputs;
using LD58.Levels;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace LD58.UI
{
    public sealed class UIEndPanel : MonoBehaviour
    {
        [SerializeField] private EndText _winText;
        [SerializeField] private EndText _loseText;
        [SerializeField] private TMP_Text _mainText;
        [SerializeField] private TMP_Text _quoteText;
        [SerializeField] private Button _replayButton;
        [SerializeField] private Button _quitToMenuButton;

        public bool IsActive => gameObject.activeSelf;

        private void Show(EndText end_text)
        {
            gameObject.SetActive(true);
            _mainText.text = end_text.Main;
            _quoteText.text = end_text.Quote;
        }

        private void Level_OnLose()
        {
            Show(_loseText);
        }

        private void Level_OnWin()
        {
            Show(_winText);
        }

        private void ReplayButton_OnClick()
        {
            if (IsActive)
            {
                GameManager.Instance.ReloadCurrentLevelAsync().Forget();
            }
        }

        private void QuitToMenuButton_OnClick()
        {
            if (IsActive)
            {
                GameManager.Instance.LoadMainMenuSceneAsync().Forget();
            }
        }

        private void Submit_performed(InputAction.CallbackContext context)
        {
            ReplayButton_OnClick();
        }

        private void Cancel_performed(InputAction.CallbackContext context)
        {
            QuitToMenuButton_OnClick();
        }

        private void Start()
        {
            Level.Instance.OnLose += Level_OnLose;
            Level.Instance.OnWin += Level_OnWin;

            InputManager.Instance.UI.Submit.performed += Submit_performed;
            InputManager.Instance.UI.Cancel.performed += Cancel_performed;

            _replayButton.onClick.AddListener(ReplayButton_OnClick);
            _quitToMenuButton.onClick.AddListener(QuitToMenuButton_OnClick);
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            if (InputManager.HasInstance)
            {
                InputManager.Instance.UI.Submit.performed -= Submit_performed;
                InputManager.Instance.UI.Cancel.performed -= Cancel_performed;
            }

            _replayButton.onClick.RemoveListener(ReplayButton_OnClick);
            _quitToMenuButton.onClick.RemoveListener(QuitToMenuButton_OnClick);
        }

        [Serializable]
        private sealed class EndText
        {
            [SerializeField] private string _main = string.Empty;
            [SerializeField] private string _quote = string.Empty;

            public string Main => _main;
            public string Quote => _quote;
        }
    }
}
