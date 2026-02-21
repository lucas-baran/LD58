using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LucasBaran.Bootstrap
{
    public sealed class Bootstrap : MonoBehaviour
    {
        [SerializeField] private BootstrapSettings _settings;

        private async UniTaskVoid LoadStartScenario()
        {
            await ScenarioLoader.Instance.LoadFromGroupsAsync(_settings.StartGroups);

#if UNITY_EDITOR
            if (BootstrapEditorPrefs.TryGetScenarioToLoad(out Scenario scenario))
            {
                await ScenarioLoader.Instance.LoadAsync(scenario);
                return;
            }
#endif

            await ScenarioLoader.Instance.LoadAsync(_settings.StartScenario);
        }

        private void Start()
        {
            LoadStartScenario().Forget();
        }
    }
}
