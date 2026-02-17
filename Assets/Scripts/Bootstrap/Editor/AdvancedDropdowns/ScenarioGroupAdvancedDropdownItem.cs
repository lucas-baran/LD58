using UnityEditor.IMGUI.Controls;

namespace LucasBaran.Bootstrap
{
    public sealed class ScenarioGroupAdvancedDropdownItem : AdvancedDropdownItem
    {
        public ScenarioGroup ScenarioGroup { get; }

        public ScenarioGroupAdvancedDropdownItem(ScenarioGroup scenario_group)
            : base(scenario_group.Name)
        {
            ScenarioGroup = scenario_group;
        }
    }
}
