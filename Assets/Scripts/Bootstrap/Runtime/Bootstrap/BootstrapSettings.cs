using System.Collections.Generic;
using UnityEngine;

namespace LucasBaran.Bootstrap
{
    [CreateAssetMenu(fileName = "SO_BootstrapSettings", menuName = "Lucas Baran/Bootstrap/Settings")]
    public sealed class BootstrapSettings : ScriptableObject
    {
        [SerializeField] private Scenario _startScenario;
        [SerializeField] private List<ScenarioGroup> _startGroups;

        public Scenario StartScenario => _startScenario;
        public IReadOnlyList<ScenarioGroup> StartGroups => _startGroups;
    }
}
