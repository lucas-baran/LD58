using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace LucasBaran.Bootstrap
{
    [CustomPropertyDrawer(typeof(Scenario.Module))]
    public sealed class ScenarioModuleDrawer : PropertyDrawer
    {
        private const string ID = "_id";
        private const string ASSET_REFERENCE = "_assetReference";

        private string[] _moduleNames;
        private int[] _moduleIds;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty asset_reference_property = property.FindPropertyRelative(ASSET_REFERENCE);

            return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing
                + EditorGUI.GetPropertyHeight(asset_reference_property);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            FindModules();

            Rect id_rect = position;
            id_rect.height = EditorGUIUtility.singleLineHeight;
            SerializedProperty id_property = property.FindPropertyRelative(ID);
            id_property.intValue = EditorGUI.IntPopup(id_rect, id_property.displayName, id_property.intValue, _moduleNames, _moduleIds);

            float offset = id_rect.height + EditorGUIUtility.standardVerticalSpacing;
            Rect asset_reference_rect = position;
            asset_reference_rect.y += offset;
            asset_reference_rect.height -= offset;
            SerializedProperty asset_reference_property = property.FindPropertyRelative(ASSET_REFERENCE);
            EditorGUI.PropertyField(asset_reference_rect, asset_reference_property);
        }

        private void FindModules()
        {
            if (_moduleNames != null)
            {
                return;
            }

            TypeCache.FieldInfoCollection fields = TypeCache.GetFieldsWithAttribute(typeof(ScenarioModuleAttribute));
            int field_index = 0;
            int field_count = fields.Count;
            _moduleNames = new string[field_count];
            _moduleIds = new int[field_count];

            foreach (FieldInfo field_info in fields)
            {
                _moduleNames[field_index] = field_info.Name;
                _moduleIds[field_index] = (int)field_info.GetRawConstantValue();
                field_index++;
            }
        }
    }
}
