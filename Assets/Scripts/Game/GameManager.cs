using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LD58.Game
{
    public sealed class GameManager : Singleton<GameManager>
    {
        [SerializeField] private SceneReference _mainMenuScene;

        private bool _isLoading = false;
        private SceneReference _currentScene = null;

        public async UniTask LoadSceneAsync(SceneReference scene_reference)
        {
            try
            {
                if (_isLoading)
                {
                    return;
                }

                _isLoading = true;
                LoadSceneParameters load_scene_parameters = new()
                {
                    loadSceneMode = LoadSceneMode.Single,
                    localPhysicsMode = LocalPhysicsMode.Physics2D,
                };

                await SceneManager.LoadSceneAsync(scene_reference.SceneName, load_scene_parameters);
                _currentScene = scene_reference;
            }
            finally
            {
                _isLoading = false;
            }
        }

        public async UniTask ReloadCurrentSceneAsync()
        {
            await LoadSceneAsync(_currentScene);
        }

        public async UniTask LoadMainMenuSceneAsync()
        {
            await LoadSceneAsync(_mainMenuScene);
        }
    }
}
