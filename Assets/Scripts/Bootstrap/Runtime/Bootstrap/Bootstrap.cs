using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LucasBaran.Bootstrap
{
    public sealed class Bootstrap : MonoBehaviour
    {
        [SerializeField] private BootstrapSettings _settings;

        private void LoadStartScenario()
        {
#if UNITY_EDITOR
            if (BootstrapEditorPrefs.TryGetScenarioToLoad(out Scenario scenario))
            {
                ScenarioLoader.Instance.LoadAsync(scenario).Forget();
                return;
            }
#endif

            ScenarioLoader.Instance.LoadAsync(_settings.StartScenario).Forget();
        }

        private void Start()
        {
            LoadStartScenario();
        }
    }
}
