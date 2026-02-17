using System;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;

namespace LucasBaran.Bootstrap
{
    public sealed class ScenarioGroupAdvancedDropdown : AdvancedDropdown
    {
        private readonly IEnumerable<ScenarioGroup> _scenarioGroups;
        private readonly Action<ScenarioGroup> OnSelectAction;

        public ScenarioGroupAdvancedDropdown(IEnumerable<ScenarioGroup> scenario_groups, Action<ScenarioGroup> on_select_action)
            : base(new AdvancedDropdownState())
        {
            _scenarioGroups = scenario_groups;
            OnSelectAction = on_select_action;
        }

        protected override AdvancedDropdownItem BuildRoot()
        {
            AdvancedDropdownItem root = new("Groups");

            foreach (ScenarioGroup scenario_group in _scenarioGroups)
            {
                ScenarioGroupAdvancedDropdownItem scenario_group_item = new(scenario_group);
                root.AddChild(scenario_group_item);
            }

            return root;
        }

        protected override void ItemSelected(AdvancedDropdownItem item)
        {
            if (item is ScenarioGroupAdvancedDropdownItem scenario_group_item)
            {
                OnSelectAction?.Invoke(scenario_group_item.ScenarioGroup);
            }
        }
    }
}
