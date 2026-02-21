using UnityEditor.IMGUI.Controls;

namespace LucasBaran.Bootstrap
{
    internal sealed class ScenarioDropdownItem : AdvancedDropdownItem
    {
        public Scenario Scenario { get; }

        public ScenarioDropdownItem(Scenario scenario)
            : base(scenario.Name)
        {
            Scenario = scenario;
        }
    }
}
