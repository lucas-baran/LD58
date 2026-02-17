using UnityEngine;

namespace LucasBaran.Bootstrap
{
    [CreateAssetMenu(fileName = "SO_BootstrapSettings", menuName = "Lucas Baran/Bootstrap/Settings")]
    public sealed class BootstrapSettings : ScriptableObject
    {
        [SerializeField] private Scenario _startScenario;

        public Scenario StartScenario => _startScenario;
    }
}
