using System;
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

        private Scenario _scenario;
        private List<ScenarioGroup> _allScenarioGroups;
        private List<ScenarioGroup> _includedInScenarioGroups;
        private GUIContent _reorderableListHeader;
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
            EditorGUI.LabelField(element_rect, $"- {scenario_group.Name}");
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

            int scenario_index = 0;
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
