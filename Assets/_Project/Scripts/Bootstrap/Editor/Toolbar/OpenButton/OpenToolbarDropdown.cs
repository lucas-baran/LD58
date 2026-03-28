using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace LucasBaran.Bootstrap.Toolbar
{
    internal sealed class OpenToolbarDropdown : AdvancedDropdown
    {
        public OpenToolbarDropdown() : base(new AdvancedDropdownState()) { }

        protected override AdvancedDropdownItem BuildRoot()
        {
            AdvancedDropdownItem root = new("Scenarios");

            root.AddChild(new BootstrapDropdownItem());
            root.AddSeparator();

            IEnumerable<Scenario> scenarios = AssetDatabase.FindAssetGUIDs("t:" + nameof(Scenario))
                .Select(guid => AssetDatabase.LoadAssetByGUID<Scenario>(guid));

            foreach (Scenario scenario in scenarios)
            {
                if (scenario.ShowInDebugGui)
                {
                    root.AddChild(new ScenarioDropdownItem(scenario));
                }
            }

            root.AddSeparator();

            AdvancedDropdownItem hidden_scenarios_root = new("Hidden scenarios");
            root.AddChild(hidden_scenarios_root);

            foreach (Scenario scenario in scenarios)
            {
                if (!scenario.ShowInDebugGui)
                {
                    hidden_scenarios_root.AddChild(new ScenarioDropdownItem(scenario));
                }
            }

            AddOptionItems(hidden_scenarios_root);
            AddOptionItems(root);

            return root;
        }

        private void AddOptionItems(AdvancedDropdownItem root)
        {
            root.AddSeparator();
            root.AddChild(new AdvancedDropdownItem("Shift - load additively") { enabled = false });
            root.AddChild(new AdvancedDropdownItem("Ctrl - unload") { enabled = false });
            root.AddChild(new AdvancedDropdownItem("Alt - locate") { enabled = false });
        }

        protected override void ItemSelected(AdvancedDropdownItem item)
        {
            Event current_event = Event.current;
            bool load_additively = current_event.shift;
            bool unload = current_event.control;
            bool locate = current_event.alt;

            if (item is ScenarioDropdownItem scenario_item)
            {
                Scenario scenario = scenario_item.Scenario;

                if (locate)
                {
                    OpenToolbarActions.LocateScenario(scenario);
                }
                else if (unload)
                {
                    OpenToolbarActions.CloseScenario(scenario);
                }
                else
                {
                    OpenToolbarActions.OpenScenario(scenario, load_additively);
                }
            }
            else if (item is BootstrapDropdownItem)
            {
                if (locate)
                {
                    OpenToolbarActions.LocateBootstrapScene();
                }
                else if (unload)
                {
                    OpenToolbarActions.CloseBootstrapScene();
                }
                else
                {
                    OpenToolbarActions.OpenBootstrapScene(load_additively);
                }
            }
        }
    }
}
