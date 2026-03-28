using Cysharp.Threading.Tasks;
using LucasBaran.Bootstrap;
using UnityEngine;

namespace LD58.Game
{
    public sealed class GameManager : Singleton<GameManager>
    {
        [SerializeField] private ScenarioGroup _mainMenuScenarioGroup;

        private bool _isLoading = false;
        private Scenario _currentLevelScenario;

        public async UniTask LoadLevelAsync(Scenario scenario)
        {
            try
            {
                if (_isLoading)
                {
                    return;
                }

                _isLoading = true;
                _currentLevelScenario = scenario;

                await ScenarioLoader.Instance.UnloadAllFromGroupAsync(_mainMenuScenarioGroup);
                await ScenarioLoader.Instance.LoadAsync(scenario);
            }
            finally
            {
                _isLoading = false;
            }
        }

        public async UniTask ReloadCurrentLevelAsync()
        {
            await ScenarioLoader.Instance.UnloadAllAsync(IsNotMainMenuScenario);
            await ScenarioLoader.Instance.LoadAsync(_currentLevelScenario);
        }

        public async UniTask LoadMainMenuSceneAsync()
        {
            _currentLevelScenario = null;

            await ScenarioLoader.Instance.UnloadAllAsync(IsNotMainMenuScenario);
            await ScenarioLoader.Instance.LoadFromGroupAsync(_mainMenuScenarioGroup);
        }

        private bool IsNotMainMenuScenario(Scenario scenario)
        {
            return !_mainMenuScenarioGroup.Scenarios.Contains(scenario);
        }
    }
}
