using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LucasBaran.Bootstrap
{
    public sealed class Bootstrap : MonoBehaviour
    {
        [SerializeField] private BootstrapSettings _settings;

        private void LoadStartScenario()
        {
            ScenarioLoader.Instance.LoadAsync(_settings.StartScenario).Forget();
        }

        private void Start()
        {
            LoadStartScenario();
        }
    }
}
