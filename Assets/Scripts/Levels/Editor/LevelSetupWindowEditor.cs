using UnityEditor;
using UnityEngine;

namespace LD58.Levels
{
    [CustomEditor(typeof(LevelSetupWindow))]
    public sealed class LevelSetupWindowEditor : Editor
    {
        private static readonly string[] EXCLUDE_PROPERTIES = new string[]
        {
            "m_Script",
            LevelSetupWindow.LEVEL_DATA_PROPERTY,
        };

        private SerializedProperty _levelDataProperty;
        private Editor _levelDataEditor;
        private bool _levelDataExpanded = false;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawPropertiesExcluding(serializedObject, EXCLUDE_PROPERTIES);

            using (new EditorGUILayout.HorizontalScope())
            {
                _levelDataExpanded = _levelDataProperty.objectReferenceValue != null & EditorGUILayout.Foldout(_levelDataExpanded, _levelDataProperty.displayName, toggleOnLabelClick: true);
                EditorGUILayout.PropertyField(_levelDataProperty, GUIContent.none, includeChildren: true);
            }

            using (new EditorGUI.IndentLevelScope())
            {
                if (_levelDataExpanded)
                {
                    CreateCachedEditor(_levelDataProperty.objectReferenceValue, editorType: null, ref _levelDataEditor);
                    _levelDataEditor.OnInspectorGUI();
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void OnEnable()
        {
            _levelDataProperty = serializedObject.FindProperty(LevelSetupWindow.LEVEL_DATA_PROPERTY);
        }

        private void OnDisable()
        {
            _levelDataProperty = null;

            if (_levelDataEditor != null)
            {
                DestroyImmediate(_levelDataEditor);
                _levelDataEditor = null;
            }
        }
    }
}
