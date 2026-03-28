using UnityEditor;
using UnityEngine;

namespace LD58.Levels
{
    public sealed class LevelSetupWindow : EditorWindow
    {
        public const string LEVEL_DATA_PROPERTY = nameof(_levelData);
        private const int FIRST_CONTROL_ID = -626867;
        private const int ADD_CONTROL_ID = FIRST_CONTROL_ID - 1;

        [SerializeField] private float _handleSize = 0.1f;
        [SerializeField] private LevelData _levelData;

        private SerializedObject _serializedObject;
        private Editor _editor;
        private bool _isEditing = false;

        private void OnSceneGUI(SceneView scene_view)
        {
            if (_levelData == null || !_isEditing)
            {
                return;
            }

            HandleUtility.AddDefaultControl(ADD_CONTROL_ID);

            using SerializedObject level_data_serialized_object = new(_levelData);
            level_data_serialized_object.Update();
            SerializedProperty grow_spots_property = level_data_serialized_object.FindProperty(Properties.GROW_SPOTS);
            int position_count = grow_spots_property.arraySize;
            Vector3 snap = EditorSnapSettings.move;

            for (int grow_spot_index = 0; grow_spot_index < position_count; grow_spot_index++)
            {
                int control_id = FIRST_CONTROL_ID + grow_spot_index;
                using SerializedProperty grow_spot_property = grow_spots_property.GetArrayElementAtIndex(grow_spot_index);
                using SerializedProperty position_property = grow_spot_property.FindPropertyRelative(Properties.POSITION);
                position_property.vector2Value = Handles.FreeMoveHandle(control_id, position_property.vector2Value, _handleSize, snap, Handles.DotHandleCap);
            }

            if (TryGetMouseWorldPosition(out Vector2 mouse_position))
            {
                HandleAddEvent(grow_spots_property, mouse_position);
            }

            HandleRemoveEvent(grow_spots_property);
            level_data_serialized_object.ApplyModifiedProperties();
        }

        private bool TryGetMouseWorldPosition(out Vector2 mouse_position)
        {
            Plane plane = new(Vector3.forward, d: 0f);
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            bool has_hit = plane.Raycast(ray, out float distance);
            mouse_position = ray.GetPoint(distance);

            return has_hit;
        }

        private void HandleAddEvent(SerializedProperty grow_spots_property, Vector2 mouse_position)
        {
            Event evt = Event.current;

            if (evt.type == EventType.MouseDown && evt.button == 0
                && HandleUtility.nearestControl == ADD_CONTROL_ID
                )
            {
                int index = grow_spots_property.arraySize;
                grow_spots_property.InsertArrayElementAtIndex(index);
                using SerializedProperty grow_spot_property = grow_spots_property.GetArrayElementAtIndex(index);
                using SerializedProperty position_property = grow_spot_property.FindPropertyRelative(Properties.POSITION);
                position_property.vector2Value = mouse_position;

                GUIUtility.hotControl = FIRST_CONTROL_ID + index;
                evt.Use();
            }
        }

        private void HandleRemoveEvent(SerializedProperty grow_spots_property)
        {
            Event evt = Event.current;
            int nearest_control_id = HandleUtility.nearestControl;
            int max_control_id = FIRST_CONTROL_ID + grow_spots_property.arraySize;

            if (evt.type == EventType.MouseDown && evt.button == 1
                && nearest_control_id >= FIRST_CONTROL_ID && nearest_control_id < max_control_id
                )
            {
                int index = nearest_control_id - FIRST_CONTROL_ID;
                grow_spots_property.DeleteArrayElementAtIndex(index);
                evt.Use();
            }
        }

        private void OnGUI()
        {
            if (GUILayout.Button(GetEditingButtonText()))
            {
                ToggleEditing();
            }

            Editor.CreateCachedEditor(this, editorType: typeof(LevelSetupWindowEditor), ref _editor);
            _editor.OnInspectorGUI();
        }

        private void ToggleEditing()
        {
            if (_isEditing)
            {
                StopEditing();
            }
            else
            {
                StartEditing();
            }
        }

        private void StartEditing()
        {
            if (!_isEditing)
            {
                _isEditing = true;
            }
        }

        private void StopEditing()
        {
            if (_isEditing)
            {
                _isEditing = false;

                if (_levelData != null)
                {
                    AssetDatabase.SaveAssetIfDirty(_levelData);
                }
            }
        }

        private string GetEditingButtonText()
        {
            return _isEditing ? "Stop editing" : "Start editing";
        }

        private void OnInspectorUpdate()
        {
            Repaint();
        }

        private void OnEnable()
        {
            _serializedObject = new SerializedObject(this);
            SceneView.duringSceneGui += OnSceneGUI;

            Level level = FindAnyObjectByType<Level>();

            if (level != null)
            {
                _levelData = level.Data;
            }
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
            _serializedObject.Dispose();
            _serializedObject = null;

            if (_editor != null)
            {
                DestroyImmediate(_editor);
                _editor = null;
            }

            StopEditing();
        }

        [MenuItem("LD58/Level Editor")]
        private static void OpenWindow()
        {
            GetWindow<LevelSetupWindow>();
        }

        private static class Properties
        {
            public const string GROW_SPOTS = "_growSpots";
            public const string POSITION = "_position";
        }
    }
}
