using System.Collections.Generic;
using UnityEngine;

namespace LucasBaran.Bootstrap
{
    [CreateAssetMenu(fileName = "SO_ScenarioGroup", menuName = "Lucas Baran/Bootstrap/Scenario group")]
    public sealed class ScenarioGroup : ScriptableObject
    {
        [SerializeReference] private string _name;
        [SerializeReference] private Scenario[] _scenarios;

        public string Name => string.IsNullOrEmpty(_name) ? name : _name;
        public IReadOnlyList<Scenario> Scenarios => _scenarios;
    }
}
