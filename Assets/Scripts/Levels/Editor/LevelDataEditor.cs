using UnityEditor;
using UnityEngine;

namespace LD58.Levels
{
    [CustomEditor(typeof(LevelData))]
    public sealed class LevelDataEditor : Editor
    {
        private SerializedProperty _growSpotsProperty;

        private void SceneGUI(SceneView scene_view)
        {
            bool has_changed = false;
            serializedObject.Update();

            for (int i = 0; i < _growSpotsProperty.arraySize; i++)
            {
                SerializedProperty starting_fruit_property = _growSpotsProperty.GetArrayElementAtIndex(i);
                SerializedProperty position_property = starting_fruit_property.FindPropertyRelative("_position");

                Vector2 old_position = position_property.vector2Value;
                Vector2 new_position = Handles.DoPositionHandle(old_position, Quaternion.identity);
                position_property.vector2Value = new_position;
                has_changed |= !new_position.Equals(old_position);
            }

            if (has_changed)
            {
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void OnEnable()
        {
            _growSpotsProperty = serializedObject.FindProperty("_growSpots");
            SceneView.duringSceneGui += SceneGUI;
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= SceneGUI;
        }
    }
}
