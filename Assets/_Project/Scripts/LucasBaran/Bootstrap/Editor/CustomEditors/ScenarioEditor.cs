using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace LucasBaran.Bootstrap
{
    [CustomEditor(typeof(Scenario))]
    public sealed class ScenarioEditor : Editor
    {
        private const string SCENARIOS_PROPERTY = "_scenarios";
        private const int SCENARIO_FIELD_CONTROL_ID = -6003;

        private Scenario _scenario;
        private List<ScenarioGroup> _allScenarioGroups;
        private List<ScenarioGroup> _includedInScenarioGroups;
        private GUIContent _reorderableListHeader;
        private Texture _pingButtonTexture;
        private ReorderableList _reorderableList;
        private ScenarioGroupAdvancedDropdown _scenarioGroupDropdown;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            _reorderableList.DoLayoutList();
        }

        private void DrawScenarioGroupListHeader(Rect header_rect)
        {
            EditorGUI.LabelField(header_rect, _reorderableListHeader);
        }

        private void DrawScenarioGroupListElement(Rect element_rect, int index, bool is_active, bool is_focused)
        {
            ScenarioGroup scenario_group = _includedInScenarioGroups[index];
            Event current_event = Event.current;
            EventType event_type = current_event.type;
            int control_id = GUIUtility.GetControlID(SCENARIO_FIELD_CONTROL_ID, FocusType.Keyboard, element_rect);
            bool contains_mouse = element_rect.Contains(current_event.mousePosition);

            switch (event_type)
            {
                case EventType.Repaint:
                {
                    GUIContent content = new(scenario_group.Name, AssetPreview.GetMiniThumbnail(scenario_group));
                    EditorStyles.objectField.Draw(element_rect, content, control_id, on: false, hover: contains_mouse);

                    break;
                }
                case EventType.MouseDown:
                {
                    if (!contains_mouse || Event.current.button != 0)
                    {
                        break;
                    }

                    if (Event.current.clickCount == 1)
                    {
                        EditorGUIUtility.PingObject(scenario_group);
                        current_event.Use();
                    }
                    else if (Event.current.clickCount == 2 && scenario_group != null)
                    {
                        AssetDatabase.OpenAsset(scenario_group);
                        current_event.Use();
                        GUIUtility.ExitGUI();
                    }

                    break;
                }
                default:
                    break;
            }
        }

        private void AddToScenarioDropdownButton(Rect button_rect, ReorderableList list)
        {
            _scenarioGroupDropdown.Show(button_rect);
        }

        private void OnScenarioGroupSelected(ScenarioGroup scenario_group)
        {
            using SerializedObject scenario_group_serialized_object = new(scenario_group);
            using SerializedProperty scenarios_property = scenario_group_serialized_object.FindProperty(SCENARIOS_PROPERTY);

            _includedInScenarioGroups.Add(scenario_group);

            scenario_group_serialized_object.Update();

            int scenario_index = scenarios_property.arraySize;
            scenarios_property.InsertArrayElementAtIndex(scenario_index);
            using SerializedProperty scenario_property = scenarios_property.GetArrayElementAtIndex(scenario_index);
            scenario_property.objectReferenceValue = _scenario;

            scenario_group_serialized_object.ApplyModifiedProperties();
        }

        private void RemoveFromScenarioGroup(ReorderableList list)
        {
            ReadOnlyCollection<int> selected_indices = list.selectedIndices;
            int remove_index = selected_indices.Count == 0
                ? _includedInScenarioGroups.Count - 1
                : selected_indices[0];

            RemoveFromScenarioGroupAt(remove_index);
        }

        private void RemoveFromScenarioGroupAt(int scenario_group_index)
        {
            ScenarioGroup scenario_group = _includedInScenarioGroups[scenario_group_index];
            _includedInScenarioGroups.RemoveAt(scenario_group_index);

            using SerializedObject scenario_group_serialized_object = new(scenario_group);
            using SerializedProperty scenarios_property = scenario_group_serialized_object.FindProperty(SCENARIOS_PROPERTY);

            scenario_group_serialized_object.Update();
            int scenario_count = scenarios_property.arraySize;

            for (int scenario_index = 0; scenario_index < scenario_count; scenario_index++)
            {
                using SerializedProperty scenario_property = scenarios_property.GetArrayElementAtIndex(scenario_index);

                if (scenario_property.objectReferenceValue == _scenario)
                {
                    scenarios_property.DeleteArrayElementAtIndex(scenario_index);

                    break;
                }
            }

            scenario_group_serialized_object.ApplyModifiedProperties();
        }

        private void InitializeScenarioGroups()
        {
            _allScenarioGroups = AssetDatabase.FindAssetGUIDs("t:" + nameof(ScenarioGroup))
                .Select(guid => AssetDatabase.LoadAssetByGUID<ScenarioGroup>(guid))
                .ToList();

            _includedInScenarioGroups = _allScenarioGroups.Where(scenario_group => scenario_group.Scenarios.Contains(_scenario)).ToList();
        }

        private void OnEnable()
        {
            _scenario = (Scenario)target;
            InitializeScenarioGroups();

            _reorderableListHeader = new GUIContent("Include in scenario groups");
            _pingButtonTexture = EditorGUIUtility.IconContent("d_scenepicking_pickable_hover").image;
            _scenarioGroupDropdown = new ScenarioGroupAdvancedDropdown(_allScenarioGroups, OnScenarioGroupSelected);
            _reorderableList = new ReorderableList(_includedInScenarioGroups, typeof(ScenarioGroup))
            {
                draggable = false,
                multiSelect = false,
                drawHeaderCallback = DrawScenarioGroupListHeader,
                drawElementCallback = DrawScenarioGroupListElement,
                onAddDropdownCallback = AddToScenarioDropdownButton,
                onRemoveCallback = RemoveFromScenarioGroup,
            };
        }
    }
}
