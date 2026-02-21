using Cysharp.Threading.Tasks;
using UnityEditor;

namespace LucasBaran.Bootstrap.Toolbar
{
    public static class PlayToolbarActions
    {
        public static void PlayScenario(Scenario scenario)
        {
            if (EditorApplication.isPlaying)
            {
                LoadScenarioAsync(scenario).Forget();
                return;
            }

            BootstrapEditorPrefs.EnableBootstrapAtPlayMode = true;
            BootstrapEditorPrefs.SetScenarioToLoad(scenario);
            EditorApplication.EnterPlaymode();
        }

        public static void PlayStandaloneScenes()
        {
            BootstrapEditorPrefs.EnableBootstrapAtPlayMode = false;
            BootstrapEditorPrefs.DeleteScenarioToLoad();
            EditorApplication.EnterPlaymode();
        }

        private static async UniTaskVoid LoadScenarioAsync(Scenario scenario)
        {
            await ScenarioLoader.Instance.UnloadAllAsync();
            ScenarioLoader.Instance.LoadAsync(scenario).Forget();
        }
    }
}
