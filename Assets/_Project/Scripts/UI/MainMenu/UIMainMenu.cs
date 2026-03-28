using Cysharp.Threading.Tasks;
using LD58.Game;
using LD58.Inputs;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace LD58.UI
{
    public sealed class UIMainMenu : MonoBehaviour
    {
        [SerializeField] private UIScenarioPanel _scenarioPanel;
        [SerializeField] private Button _previousScenarioButton;
        [SerializeField] private Button _nextScenarioButton;
        [SerializeField] private Button _launchScenarioButton;
        [SerializeField] private Button _quitGameButton;
        [SerializeField] private List<LevelDescription> _scenarioDatas;

        private int _currentScenarioIndex = 0;

        private void RefreshScenario()
        {
            _scenarioPanel.SetScenarioData(_scenarioDatas[_currentScenarioIndex]);
        }

        private void PreviousScenarioButton_OnClick()
        {
            _currentScenarioIndex--;

            if (_currentScenarioIndex < 0)
            {
                _currentScenarioIndex = _scenarioDatas.Count - 1;
            }

            RefreshScenario();
        }

        private void NextScenarioButton_OnClick()
        {
            _currentScenarioIndex++;

            if (_currentScenarioIndex >= _scenarioDatas.Count)
            {
                _currentScenarioIndex = 0;
            }

            RefreshScenario();
        }

        private void LaunchScenarioButton_OnClick()
        {
            GameManager.Instance.LoadLevelAsync(_scenarioDatas[_currentScenarioIndex].Scenario).Forget();
        }

        private void QuitGameButton_OnClick()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }

        private void TriggerNavigation_performed(InputAction.CallbackContext context)
        {
            float navigation = context.ReadValue<float>();

            if (navigation < 0f)
            {
                PreviousScenarioButton_OnClick();
            }
            else
            {
                NextScenarioButton_OnClick();
            }
        }

        private void Submit_performed(InputAction.CallbackContext context)
        {
            LaunchScenarioButton_OnClick();
        }

        private void Cancel_performed(InputAction.CallbackContext context)
        {
            QuitGameButton_OnClick();
        }

        private void Start()
        {
            RefreshScenario();

            InputManager.Instance.UI.TriggerNavigation.performed += TriggerNavigation_performed;
            InputManager.Instance.UI.Submit.performed += Submit_performed;
            InputManager.Instance.UI.Cancel.performed += Cancel_performed;

            _previousScenarioButton.onClick.AddListener(PreviousScenarioButton_OnClick);
            _nextScenarioButton.onClick.AddListener(NextScenarioButton_OnClick);
            _launchScenarioButton.onClick.AddListener(LaunchScenarioButton_OnClick);
            _quitGameButton.onClick.AddListener(QuitGameButton_OnClick);
        }

        private void OnDestroy()
        {
            if (InputManager.HasInstance)
            {
                InputManager.Instance.UI.TriggerNavigation.performed -= TriggerNavigation_performed;
                InputManager.Instance.UI.Submit.performed -= Submit_performed;
                InputManager.Instance.UI.Cancel.performed -= Cancel_performed;
            }

            _previousScenarioButton.onClick.RemoveListener(PreviousScenarioButton_OnClick);
            _nextScenarioButton.onClick.RemoveListener(NextScenarioButton_OnClick);
            _launchScenarioButton.onClick.RemoveListener(LaunchScenarioButton_OnClick);
            _quitGameButton.onClick.RemoveListener(QuitGameButton_OnClick);
        }
    }
}
