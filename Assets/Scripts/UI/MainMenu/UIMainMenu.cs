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
        [SerializeField] private ScenarioData _defaultScenarioData;
        [SerializeField] private List<ScenarioData> _scenarioDatas;

        private int _currentScenarioIndex;

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
            GameManager.Instance.LoadSceneAsync(_scenarioDatas[_currentScenarioIndex].SceneReference).Forget();
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

        private void Start()
        {
            _currentScenarioIndex = _scenarioDatas.IndexOf(_defaultScenarioData);
            RefreshScenario();

            InputManager.Instance.UI.TriggerNavigation.performed += TriggerNavigation_performed;
            InputManager.Instance.UI.Submit.performed += Submit_performed;

            _previousScenarioButton.onClick.AddListener(PreviousScenarioButton_OnClick);
            _nextScenarioButton.onClick.AddListener(NextScenarioButton_OnClick);
            _launchScenarioButton.onClick.AddListener(LaunchScenarioButton_OnClick);
        }

        private void OnDestroy()
        {
            if (InputManager.HasInstance)
            {
                InputManager.Instance.UI.TriggerNavigation.performed -= TriggerNavigation_performed;
                InputManager.Instance.UI.Submit.performed -= Submit_performed;
            }

            _previousScenarioButton.onClick.RemoveListener(PreviousScenarioButton_OnClick);
            _nextScenarioButton.onClick.RemoveListener(NextScenarioButton_OnClick);
            _launchScenarioButton.onClick.RemoveListener(LaunchScenarioButton_OnClick);
        }
    }
}
