using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LD58.Game
{
    public sealed class GameManager : Singleton<GameManager>
    {
        private SceneReference _currentScene = null;

        public async UniTask LoadSceneAsync(SceneReference scene_reference)
        {
            LoadSceneParameters load_scene_parameters = new()
            {
                loadSceneMode = LoadSceneMode.Single,
                localPhysicsMode = LocalPhysicsMode.Physics2D,
            };

            await SceneManager.LoadSceneAsync(scene_reference.SceneName, load_scene_parameters);
            _currentScene = scene_reference;
        }

        public async UniTask ReloadCurrentSceneAsync()
        {
            await LoadSceneAsync(_currentScene);
        }
    }
}
