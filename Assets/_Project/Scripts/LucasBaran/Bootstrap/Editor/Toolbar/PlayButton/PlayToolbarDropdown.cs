using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace LucasBaran.Bootstrap.Toolbar
{
    internal sealed class PlayToolbarDropdown : AdvancedDropdown
    {
        public PlayToolbarDropdown()
            : base(new AdvancedDropdownState())
        {
        }

        protected override AdvancedDropdownItem BuildRoot()
        {
            AdvancedDropdownItem root = new("Scenarios");
            bool has_scenario_to_load = BootstrapEditorPrefs.TryGetScenarioToLoad(out Scenario scenario_to_load);

            if (!EditorApplication.isPlayingOrWillChangePlaymode)
            {
                root.AddChild(new StandaloneScenesDropdownItem()
                {
                    icon = DropdownUtils.GetSelectionIcon(!has_scenario_to_load),
                });

                root.AddSeparator();
            }

            IEnumerable<Scenario> scenarios = AssetDatabase.FindAssetGUIDs("t:" + nameof(Scenario))
                .Select(guid => AssetDatabase.LoadAssetByGUID<Scenario>(guid));

            foreach (Scenario scenario in scenarios)
            {
                if (scenario.ShowInDebugGui)
                {
                    root.AddChild(new ScenarioDropdownItem(scenario)
                    {
                        icon = DropdownUtils.GetSelectionIcon(scenario == scenario_to_load),
                    });
                }
            }

            return root;
        }

        protected override void ItemSelected(AdvancedDropdownItem item)
        {
            if (item is ScenarioDropdownItem scenario_item)
            {
                PlayToolbarActions.PlayScenario(scenario_item.Scenario);
            }
            else if (item is StandaloneScenesDropdownItem)
            {
                PlayToolbarActions.PlayStandaloneScenes();
            }
        }
    }
}
